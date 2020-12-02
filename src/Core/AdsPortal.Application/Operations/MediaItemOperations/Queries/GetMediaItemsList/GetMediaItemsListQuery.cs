namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetMediaItemsListQuery : IGetListQuery<GetMediaItemsListResponse>
    {
        public GetMediaItemsListQuery()
        {

        }

        private class Handler : GetListQueryHandler<GetMediaItemsListQuery, MediaItem, GetMediaItemsListResponse>
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

