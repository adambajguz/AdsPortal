﻿namespace AdsPortal.WebApi.Application.Operations.MediaItemOperations.Queries.GetMediaItemsList
{
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using MediatR.GenericOperations.Queries;

    public sealed record GetMediaItemsListQuery : IGetListQuery<GetMediaItemsListResponse>
    {
        private sealed class Handler : GetListQueryHandler<GetMediaItemsListQuery, MediaItem, GetMediaItemsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}

