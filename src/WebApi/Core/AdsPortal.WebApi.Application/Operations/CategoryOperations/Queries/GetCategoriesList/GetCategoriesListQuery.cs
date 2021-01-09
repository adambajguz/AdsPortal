﻿namespace AdsPortal.WebApi.Application.Operations.CategoryOperations.Queries.GetCategoriesList
{
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetCategoriesListQuery : IGetListQuery<GetCategoriesListResponse>
    {
        private sealed class Handler : GetListQueryHandler<GetCategoriesListQuery, Category, GetCategoriesListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}
