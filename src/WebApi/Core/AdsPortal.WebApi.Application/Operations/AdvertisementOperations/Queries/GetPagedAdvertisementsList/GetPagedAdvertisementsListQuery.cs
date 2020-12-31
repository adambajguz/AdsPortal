namespace AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetPagedAdvertisementsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList;
    using AdsPortal.WebApi.Domain.Entities;

    public class GetPagedAdvertisementsListQuery : IGetPagedListQuery<GetAdvertisementsListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private class Handler : GetPagedListQueryHandler<GetPagedAdvertisementsListQuery, Advertisement, GetAdvertisementsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
