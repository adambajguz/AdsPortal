namespace AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList
{
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
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

