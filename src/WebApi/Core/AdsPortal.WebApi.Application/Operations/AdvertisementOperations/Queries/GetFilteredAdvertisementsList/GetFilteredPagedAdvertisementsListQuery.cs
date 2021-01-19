namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Queries.GetFilteredAdvertisementsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementsList;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using MediatR.GenericOperations.Queries;

    public sealed record GetFilteredPagedAdvertisementsListQuery : IGetPagedListQuery<GetAdvertisementsListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        public string? Title { get; init; }
        public string? CategoryName { get; init; }

        private sealed class Handler : GetPagedListQueryHandler<GetFilteredPagedAdvertisementsListQuery, Advertisement, GetAdvertisementsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override ValueTask<GetFilteredPagedAdvertisementsListQuery> OnInit(GetFilteredPagedAdvertisementsListQuery query, CancellationToken cancellationToken)
            {
                if (query.Title is string && query.CategoryName is string)
                {
                    Filter = x => x.Title.Contains(query.Title) && x.Category.Name.Contains(query.CategoryName);
                }
                else if (query.Title is string)
                {
                    Filter = x => x.Title.Contains(query.Title);

                }
                else if (query.CategoryName is string)
                {
                    Filter = x => x.Category.Name.Contains(query.CategoryName);
                }

                return base.OnInit(query, cancellationToken);
            }
        }
    }
}
