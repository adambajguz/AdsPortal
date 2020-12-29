namespace AdsPortal.Application.Operations.CategoryOperations.Queries.GetPagedCategoriesList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.CategoryOperations.Queries.GetCategoriesList;
    using AdsPortal.Domain.Entities;

    public class GetPagedCategoriesListQuery : IGetPagedListQuery<GetCategoriesListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private class Handler : GetPagedListQueryHandler<GetPagedCategoriesListQuery, Category, GetCategoriesListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                //OrderBy = (q) => q.OrderBy(x => x.Points);

                return Task.CompletedTask;
            }
        }
    }
}
