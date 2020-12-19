namespace AdsPortal.Application.Operations.CategoryOperations.Queries.GetCategoriesList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public class GetCategoriesListQuery : IGetListQuery<GetCategoriesListResponse>
    {
        private class Handler : GetListQueryHandler<GetCategoriesListQuery, Category, GetCategoriesListResponse>
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
