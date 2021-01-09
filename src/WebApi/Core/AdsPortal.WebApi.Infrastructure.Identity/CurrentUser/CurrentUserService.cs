namespace AdsPortal.WebApi.Infrastructure.Identity.CurrentUser
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Jwt;
    using Microsoft.AspNetCore.Http;

    public class CurrentUserService : ICurrentUserService
    {
        /*
         * IHttpContextAccessor.HttpContext.User.Identity shows all null properties in CurrentUserService   rvice
         * https://stackoverflow.com/questions/59793111/ihttpcontextaccessor-httpcontext-user-identity-ows-all-null-properties-in-curr
         *
         *      public string UserId { get; }
         *      public CurrentUserService(IHttpContextAccessor httpContextAccessor)
         *      {
         *           _context = httpContextAccessor.HttpContext;
         *           UserId = GetUserIdFromContext(_context);
         *      }
         *
         * Under the ASP.NET MVC framework, the HttpContext (and therefore HttpContext.Session) is not set then the controller class is contructed as you might expect, but it set ("injected") later by the ControllerBuilder class.
         * The CurrentUserService class that comes with the template I'm using tried to read the user claims in the constructor, so it did not work.
         */

        public Guid? UserId => IsAuthenticated ? GetUserIdFromContext(_context) : null;
        public bool IsAuthenticated => _context?.User?.Identity?.IsAuthenticated ?? false;
        public bool IsAdmin => IsAuthenticated && HasRole(Roles.Admin);

        private readonly HttpContext? _context;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _context = httpContextAccessor.HttpContext;
        }

        public bool HasRole(Roles role)
        {
            if (!role.IsDefined())
            {
                return false;
            }

            string roleName = role.ToString();

            if (_context is null)
            {
                throw new ForbiddenException();
            }

            ClaimsIdentity? identity = _context?.User?.Identity as ClaimsIdentity;
            Claim? result = identity?.FindAll(ClaimTypes.Role)
                                     .Where(x => x.Value == roleName)
                                     .FirstOrDefault();

            return result != null;
        }

        //TODO change to return Roles enum
        public string[] GetRoles()
        {
            ClaimsIdentity? identity = _context?.User?.Identity as ClaimsIdentity;
            string[]? roles = identity?.FindAll(ClaimTypes.Role)
                                       .Select(x => x.Value)
                                       .ToArray();

            return roles ?? Array.Empty<string>();
        }

        public static Guid? GetUserIdFromContext(IHttpContextAccessor context)
        {
            return GetUserIdFromContext(context.HttpContext);
        }

        public static Guid? GetUserIdFromContext(HttpContext? context)
        {
            ClaimsIdentity? identity = context?.User?.Identity as ClaimsIdentity;
            Claim? claim = identity?.FindFirst(ClaimTypes.NameIdentifier);

            return claim == null ? null : (Guid?)Guid.Parse(claim.Value);
        }
    }
}
