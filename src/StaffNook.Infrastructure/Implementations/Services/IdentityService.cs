using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StaffNook.Domain.Dtos.Identity;
using StaffNook.Domain.Dtos.User;
using StaffNook.Domain.Entities.Identity;
using StaffNook.Domain.Interfaces.Services;
using StaffNook.Infrastructure.Configuration;
using StaffNook.Infrastructure.Exceptions;

namespace StaffNook.Infrastructure.Implementations.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly RoleManager<RoleEntity> _roleManager;
    private readonly IMapper _mapper;

    public IdentityService(
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager, 
        IMapper mapper, RoleManager<RoleEntity> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _roleManager = roleManager;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto requestDto)
    {
        var userEntity = await _userManager.FindByNameAsync(requestDto.UserName);

        if (userEntity is null)
        {
            throw new AuthorizationException("Ошибка авторизации. Некорректные данные.");
        }

        var signInResult = await _signInManager.PasswordSignInAsync(requestDto.UserName, requestDto.Password,
            false, lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            throw new AuthorizationException("Ошибка авторизации. Некорректные данные.");
        }
        
        //ToDo:: нужен костыль для администратора чтоб при авторизации прописывались все клеймы

        //ToDo:: Если будет добавлена блокировка пользователей, то добавить проверку не заблокирован ли пользователь

        var token = GetAuthToken(userEntity);

        return new LoginResponseDto
        {
            UserId = userEntity.Id,
            Token = token
        };
    }
    
    public async Task Create(CreateUserDto dto)
    {
        var existUser = await _userManager.FindByNameAsync(dto.UserName);

        if (existUser is not null)
        {
            throw new DataException($"Пользователь с логином {dto.UserName} уже существует");
        }

        var existRole = await _roleManager.FindByNameAsync(dto.Role);
        if (existRole is null)
        {
            throw new BusinessException($"Невозможно создать пльзователя с ролью {dto.Role}");
        }

        var userEntity = _mapper.Map<UserEntity>(dto);

        var result = await _userManager.CreateAsync(userEntity, dto.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(x => x.Description).ToArray();
            throw new BusinessException(string.Join("\n", errors));
        }
        
        var addToRoleResult = await _userManager.AddToRoleAsync(userEntity, dto.Role);
        if (!addToRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(userEntity);
            throw new BusinessException($"Не удалось создать пользователя с ролью {dto.Role}");
        }

        //ToDo:: Добавление клеймов
    }

    public async Task<string[]> GetUserClaims(Guid id)
    {
        var userEntity = await _userManager.FindByIdAsync(id.ToString());
        
        if (userEntity is null)
        {
            throw NotFoundException.With<UserEntity>(id);
        }

        var claims = await _userManager.GetClaimsAsync(userEntity);
        
        // ToDo:: лучше сделать модель для удобства отображения
        return claims.Select(x=>x.Value).ToArray();
    }

    private string GetAuthToken(UserEntity user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims(user);
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

    private static JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, Claim[] claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: AppConfiguration.JwtConfiguration.Issuer,
            audience: AppConfiguration.JwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(AppConfiguration.JwtConfiguration.LifeTime)),
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials);

        return tokenOptions;
    }

    private Claim[] GetClaims(UserEntity user)
    {
        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(ClaimTypes.GivenName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = _userManager.GetRolesAsync(user).Result;
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims.ToArray();
    }
}