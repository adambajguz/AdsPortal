namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetPagedMediaItemsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemsList;
    using AdsPortal.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public class GetPagedMediaItemsListQuery : IGetPagedListQuery<GetMediaItemsListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private class Handler : GetPagedListQueryHandler<GetPagedMediaItemsListQuery, MediaItem, GetMediaItemsListResponse>
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

