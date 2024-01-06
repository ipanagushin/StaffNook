using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StaffNook.Domain.Interfaces.Services.Identity;

namespace StaffNook.Backend.Attributes
{
    public class AuthClaimAttribute : TypeFilterAttribute
    {
        public AuthClaimAttribute(params string[] claims) : base(typeof(AuthorizationClaimAttribute))
        {
            Arguments = new object[] { claims };
        }
    }
    
    internal class AuthorizationClaimAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _methodClaims;
        private readonly ICurrentUserService _currentUserService;

        public AuthorizationClaimAttribute(
            ICurrentUserService currentUserService,
            string[] claims)
        {
            _methodClaims = claims; 
            _currentUserService = currentUserService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // У администратора нет ограничений по клеймам
            if (_currentUserService.User.IsAdmin)
            {
                return;
            }
            
            var hasNoVerifyClaim = _methodClaims.Except(_currentUserService.Claims).Any();
            if (hasNoVerifyClaim)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }
        }
    }
}