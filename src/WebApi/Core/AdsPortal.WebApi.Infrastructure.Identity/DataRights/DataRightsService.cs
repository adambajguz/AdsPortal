namespace AdsPortal.WebApi.Infrastructure.Identity.DataRights
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Jwt;
    using Microsoft.AspNetCore.Http;

    public class DataRightsService : IDataRightsService
    {
        public Guid? UserId => _currentUser.UserId;
        public bool IsAuthenticated => _currentUser.IsAuthenticated;
        public bool IsAdmin => _currentUser.IsAdmin;

        private readonly IHttpContextAccessor _context;
        private readonly ICurrentUserService _currentUser;
        private readonly IAppRelationalUnitOfWork _uow;

        public DataRightsService(IHttpContextAccessor context, ICurrentUserService currentUserService, IAppRelationalUnitOfWork uow)
        {
            _context = context;
            _currentUser = currentUserService;
            _uow = uow;
        }

        public bool HasRole(Roles role)
        {
            return _currentUser.HasRole(role);
        }

        public string[] GetRoles()
        {
            return _currentUser.GetRoles();
        }

        public async Task IsOwnerOrCreatorOrAdminElseThrow<T>(T? model, Expression<Func<T, Guid?>> userIdFieldExpression)
            where T : class, IEntityCreation
        {
            if (model is null)
            {
                throw new NotFoundException(nameof(model));
            }

            if (model.CreatedBy != null)
            {
                await IsOwnerOrAdminElseThrow((Guid)model.CreatedBy);
            }

            Func<T, Guid?> func = userIdFieldExpression.Compile();
            Guid? userId = func(model);

            if (userId != null)
            {
                await IsOwnerOrAdminElseThrow((Guid)userId);
            }
        }

        public async Task IsOwnerOrAdminElseThrow(Guid userIdToValidate)
        {
            Guid userId = _currentUser.UserId ?? throw new ForbiddenException();

            if (!_currentUser.IsAdmin && userIdToValidate != userId)
            {
                throw new ForbiddenException();
            }

            User? user = await _uow.Users.SingleByIdOrDefaultAsync(userIdToValidate, noTracking: true);
            if (user is null)
            {
                throw new NotFoundException(nameof(User), userIdToValidate);
            }
        }

        public void IsAdminElseThrow()
        {
            HasRoleElseThrow(Roles.Admin);
        }

        public void HasRoleElseThrow(Roles role)
        {
            if (!role.IsDefined())
            {
                throw new ForbiddenException();
            }

            string roleName = role.ToString();

            if (role == Roles.None)
            {
                return;
            }

            if (_context.HttpContext is null)
            {
                throw new ForbiddenException();
            }

            ClaimsIdentity? identity = _context.HttpContext.User.Identity as ClaimsIdentity;
            Claim? result = identity?.FindAll(ClaimTypes.Role)
                                     .Where(x => x.Value == roleName)
                                     .FirstOrDefault();

            if (result == null)
            {
                throw new ForbiddenException();
            }
        }
    }
}
