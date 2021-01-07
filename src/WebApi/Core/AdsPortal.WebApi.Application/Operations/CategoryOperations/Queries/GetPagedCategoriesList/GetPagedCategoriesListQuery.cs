namespace AdsPortal.Application.Operations.CategoryOperations.Queries.GetPagedCategoriesList
{
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.CategoryOperations.Queries.GetCategoriesList;
    using AdsPortal.WebApi.Domain.Entities;

    public sealed record GetPagedCategoriesListQuery : IGetPagedListQuery<GetCategoriesListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private class Handler : GetPagedListQueryHandler<GetPagedCategoriesListQuery, Category, GetCategoriesListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}
