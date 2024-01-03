using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StaffNook.Domain.Claims;
using StaffNook.Domain.Dtos;
using StaffNook.Domain.Interfaces.Services.Identity;
using StaffNook.Infrastructure.Configuration;
using StaffNook.Infrastructure.Persistence;

namespace StaffNook.Infrastructure.Handlers
{
    public class AuthHandler : AuthenticationHandler<AppAuthenticationSchemeOptions>
    {
        private readonly ICurrentUserService _currentUserService;
        
        public AuthHandler(
            IOptionsMonitor<AppAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ICurrentUserService currentUserService) 
            : base(options, logger, encoder, clock)
        {
            _currentUserService = currentUserService;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var token))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            
            if (!ValidateJwtToken(token, out var validatedToken))
            {
                return Task.FromResult(AuthenticateResult.Fail("Token not valid"));
            }
                
            SetCurrentUserFromToken(validatedToken);

            var identity = new GenericIdentity(AppConfiguration.JwtConfiguration.Issuer, "JWT");
            identity.AddClaims(validatedToken.Claims);

            var result = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name));
            return Task.FromResult(result);
        }

        private bool ValidateJwtToken(string token, out JwtSecurityToken securityToken)
        {
            try
            {
                token = token.Split(' ')[1];
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(AppConfiguration.JwtConfiguration.SecurityKey));
            
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = key,
                    ValidIssuer = AppConfiguration.JwtConfiguration.Issuer,
                    ValidAudience = AppConfiguration.JwtConfiguration.Audience,
                    // set clockskew to zero so tokens expire exactly at token expiration time.
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                securityToken = (JwtSecurityToken) validatedToken;

                return true;
            }
            catch
            {
                // ignored
            }

            securityToken = null;
            return false;
        }

        private void SetCurrentUserFromToken(JwtSecurityToken token)
        {
            var roleId = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            var userId = token.Claims.FirstOrDefault(x => x.Type == AuthClaimTypes.UserId)?.Value;
            
            var user = new CurrentUserModel
            {
                Id = !string.IsNullOrEmpty(userId) ? Guid.Parse(userId) : Guid.Empty,
                Login = token.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.Name)?.Value,
                RoleId = !string.IsNullOrEmpty(roleId) ? Guid.Parse(roleId) : Guid.Empty,
                IsAdmin = roleId == IdentifierConstants.AdminRoleId
            };

            var claims = token.Claims.Where(x => x.Type == AuthClaimTypes.RoleClaims)
                .Select(x => x.Value)
                .ToArray();
            
            _currentUserService.Set(user, claims);
        }
    }
}