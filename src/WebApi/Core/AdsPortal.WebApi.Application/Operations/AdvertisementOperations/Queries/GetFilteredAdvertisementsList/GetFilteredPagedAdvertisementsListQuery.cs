namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Queries.GetFilteredAdvertisementsList
{
    using System;
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
        public string? Description { get; init; }
        public bool Visible { get; init; }

        private sealed class Handler : GetPagedListQueryHandler<GetFilteredPagedAdvertisementsListQuery, Advertisement, GetAdvertisementsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override ValueTask<GetFilteredPagedAdvertisementsListQuery> OnInit(GetFilteredPagedAdvertisementsListQuery query, CancellationToken cancellationToken)
            {
                DateTime now = DateTime.UtcNow;

                if (query.Title is string && query.Description is string)
                {
                    Filter = x => x.IsPublished && x.VisibleTo >= now && x.Title.Contains(query.Title) && x.Description.Contains(query.Description);
                }
                else if (query.Title is string)
                {
                    Filter = x => x.IsPublished && x.VisibleTo >= now && x.Title.Contains(query.Title);

                }
                else if (query.Description is string)
                {
                    Filter = x => x.IsPublished && x.VisibleTo >= now && x.Description.Contains(query.Description);
                }

                return base.OnInit(query, cancellationToken);
            }
        }
    }
}
