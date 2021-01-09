namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList
{
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetPagedAdvertisementsListQuery : IGetPagedListQuery<GetAdvertisementsListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private sealed class Handler : GetPagedListQueryHandler<GetPagedAdvertisementsListQuery, Advertisement, GetAdvertisementsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}
