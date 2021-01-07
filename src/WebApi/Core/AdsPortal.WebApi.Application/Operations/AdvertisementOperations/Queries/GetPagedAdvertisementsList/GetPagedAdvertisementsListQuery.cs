namespace AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetPagedAdvertisementsList
{
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList;
    using AdsPortal.WebApi.Domain.Entities;

    public sealed record GetPagedAdvertisementsListQuery : IGetPagedListQuery<GetAdvertisementsListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private class Handler : GetPagedListQueryHandler<GetPagedAdvertisementsListQuery, Advertisement, GetAdvertisementsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}
