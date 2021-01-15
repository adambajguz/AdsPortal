namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList
{
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using MediatR.GenericOperations.Queries;

    public sealed record GetAdvertisementsListQuery : IGetListQuery<GetAdvertisementsListResponse>
    {
        private sealed class Handler : GetListQueryHandler<GetAdvertisementsListQuery, Advertisement, GetAdvertisementsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}

