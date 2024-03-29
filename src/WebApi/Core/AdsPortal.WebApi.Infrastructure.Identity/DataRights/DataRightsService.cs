﻿namespace AdsPortal.WebApi.Infrastructure.Identity.DataRights
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Exceptions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Domain.Jwt;
    using Microsoft.AspNetCore.Http;

    public class DataRightsService : IDataRightsService
    {
        public ICurrentUserService CurrentUser { get; }

        private readonly IHttpContextAccessor _context;
        private readonly IAppRelationalUnitOfWork _uow;

        public DataRightsService(IHttpContextAccessor context, ICurrentUserService currentUserService, IAppRelationalUnitOfWork uow)
        {
            _context = context;
            CurrentUser = currentUserService;
            _uow = uow;
        }

        public bool HasRole(Roles role)
        {
            return CurrentUser.HasRole(role);
        }

        public string[] GetRoles()
        {
            return CurrentUser.GetRoles();
        }

        public async Task IsOwnerOrCreatorOrAdminElseThrowAsync<T>(T? model, Expression<Func<T, Guid?>> userIdFieldExpression)
            where T : class, IEntityCreation
        {
            if (model is null)
            {
                throw new NotFoundException(nameof(model));
            }

            if (model.CreatedBy != null)
            {
                await IsOwnerOrAdminElseThrowAsync((Guid)model.CreatedBy);
            }

            Func<T, Guid?> func = userIdFieldExpression.Compile();
            Guid? userId = func(model);

            if (userId != null)
            {
                await IsOwnerOrAdminElseThrowAsync((Guid)userId);
            }
        }

        public async Task IsOwnerOrAdminElseThrowAsync(Guid userIdToValidate)
        {
            Guid userId = CurrentUser.Id ?? throw new ForbiddenException();

            if (!CurrentUser.IsAdmin && userIdToValidate != userId)
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
