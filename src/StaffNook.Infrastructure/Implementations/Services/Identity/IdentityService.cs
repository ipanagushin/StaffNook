using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using StaffNook.Domain.Claims;
using StaffNook.Domain.Dtos.Identity;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Repositories;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Configuration;
using StaffNook.Infrastructure.Exceptions;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Implementations.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly IClaimsRolesRepository _claimsRolesRepository;
    private readonly ICurrentUserService _currentUserService;

    public IdentityService(
        IUserRepository userRepository,
        IHashService hashService,
        IClaimsRolesRepository claimsRolesRepository,
        ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _hashService = hashService;
        _claimsRolesRepository = claimsRolesRepository;
        _currentUserService = currentUserService;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        var userEntity = await _userRepository.GetByLogin(requestDto.Login, cancellationToken);
        if (userEntity is null)
        {
            // Для безопасности возвращаем не NotFoundException
            throw new AuthorizationException("Ошибка авторизации. Неверные учетные данные.");
        }

        var isValid = _hashService.VerifyPassword(requestDto.Password, userEntity.Hash, userEntity.Salt);
        if (!isValid)
        {
            throw new AuthorizationException("Ошибка авторизации. Неверные учетные данные.");
        }

        var roleClaims = await _claimsRolesRepository.GetByRoleId(userEntity.RoleId, cancellationToken);

        var token = GetAuthToken(userEntity, roleClaims);

        return new LoginResponseDto
        {
            Token = token,
            ExpiresIn = AppConfiguration.JwtConfiguration.LifeTime
        };
    }

    public async Task ChangePassword(ChangePasswordRequestDto requestDto)
    {
        if (requestDto.UserId != _currentUserService.User.Id && !_currentUserService.User.IsAdmin)
        {
            throw new ForbiddenException();
        }
        
        var user = await _userRepository.GetById(requestDto.UserId);
        if (user is null)
        {
            throw NotFoundException.With<UserEntity>(requestDto.UserId);
        }

        if (!_currentUserService.User.IsAdmin)
        {
            var isVerify = _hashService.VerifyPassword(requestDto.CurrentPassword, user.Hash, user.Salt);
            if (!isVerify)
            {
                throw new ValidationException("Текущий пароль введён не верно");
            }
        }
        
        var (hash, salt) = _hashService.GenerateHash(requestDto.NewPassword);

        user.Hash = hash;
        user.Salt = salt;

        await _userRepository.Update(user);
    }

    public async Task<CurrentUserResponseDto> GetCurrentUserInfo()
    {
        var user = await _userRepository.GetById(_currentUserService.User.Id);

        if (user is null)
        {
            throw new UnauthorizedAccessException();
        }

        return new CurrentUserResponseDto
        {
            Id = user.Id,
            Login = user.Login,
            Email = user.Email,
            RoleId = user.RoleId,
            FullName = user.FullName,
            IsAdmin = user.RoleId.ToString() == IdentifierConstants.AdminRoleId
        };
    }

    private static string GetAuthToken(UserEntity userEntity, IEnumerable<ClaimsRolesEntity> roleClaims)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetUserClaims(userEntity, roleClaims);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }

    private static SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(AppConfiguration.JwtConfiguration.SecurityKey);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private static JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
        IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: AppConfiguration.JwtConfiguration.Issuer,
            audience: AppConfiguration.JwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(Convert.ToDouble(AppConfiguration.JwtConfiguration.LifeTime)),
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials);

        return tokenOptions;
    }

    private static IEnumerable<Claim> GetUserClaims(UserEntity userEntity, IEnumerable<ClaimsRolesEntity> roleClaims)
    {
        var claims = new List<Claim>()
        {
            new(AuthClaimTypes.UserId, userEntity.Id.ToString()),
            new(ClaimTypes.Name, userEntity.Login),
            new(ClaimTypes.Role, userEntity.RoleId.ToString())
        };

        foreach (var roleClaim in roleClaims)
        {
            claims.Add(new Claim(AuthClaimTypes.RoleClaims, roleClaim.Claim));
        }

        return claims.ToArray();
    }
}