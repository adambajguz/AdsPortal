namespace AdsPortal.WebApi.Application.Operations.CategoryOperations.Queries.GetCategoriesList
{
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using MediatR.GenericOperations.Queries;

    public sealed record GetPagedCategoriesListQuery : IGetPagedListQuery<GetCategoriesListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private sealed class Handler : GetPagedListQueryHandler<GetPagedCategoriesListQuery, Category, GetCategoriesListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}
