namespace AdsPortal.Application.Operations.MediaItemOperations.Queries.GetMediaItemsList
{
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetMediaItemsListQuery : IGetListQuery<GetMediaItemsListResponse>
    {
        private class Handler : GetListQueryHandler<GetMediaItemsListQuery, MediaItem, GetMediaItemsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}

