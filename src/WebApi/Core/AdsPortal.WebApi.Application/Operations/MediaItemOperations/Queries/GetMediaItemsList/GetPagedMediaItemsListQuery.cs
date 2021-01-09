namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemsList
{
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetPagedMediaItemsListQuery : IGetPagedListQuery<GetMediaItemsListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private sealed class Handler : GetPagedListQueryHandler<GetPagedMediaItemsListQuery, MediaItem, GetMediaItemsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}

