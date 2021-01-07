namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetPagedMediaItemsList
{
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemsList;
    using AdsPortal.WebApi.Domain.Entities;

    public sealed record GetPagedMediaItemsListQuery : IGetPagedListQuery<GetMediaItemsListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private class Handler : GetPagedListQueryHandler<GetPagedMediaItemsListQuery, MediaItem, GetMediaItemsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}

