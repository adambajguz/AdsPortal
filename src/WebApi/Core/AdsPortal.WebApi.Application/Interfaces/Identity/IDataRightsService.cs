﻿namespace AdsPortal.WebApi.Application.Interfaces.Identity
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Abstractions.Base;
    using AdsPortal.WebApi.Domain.Jwt;

    public interface IDataRightsService
    {
        ICurrentUserService CurrentUser { get; }

        bool HasRole(Roles role);
        string[] GetRoles();

        Task IsOwnerOrCreatorOrAdminElseThrowAsync<T>(T? model, Expression<Func<T, Guid?>> userIdFieldExpression)
            where T : class, IEntityCreation;

        Task IsOwnerOrAdminElseThrowAsync(Guid userIdToValidate);

        void IsAdminElseThrow();
        void HasRoleElseThrow(Roles role);
    }
}
