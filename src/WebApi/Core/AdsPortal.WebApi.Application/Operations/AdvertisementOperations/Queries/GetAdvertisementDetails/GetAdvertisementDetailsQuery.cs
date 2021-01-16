namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public sealed record GetAdvertisementDetailsQuery : IGetDetailsQuery<GetAdvertisementDetailsResponse>, IIdentifiableOperation<GetAdvertisementDetailsResponse>
    {
        public Guid Id { get; init; }

        private sealed class Handler : GetDetailsByIdQueryHandler<GetAdvertisementDetailsQuery, Advertisement, GetAdvertisementDetailsResponse>
        {
            private readonly IDataRightsService _drs;

            public Handler(IDataRightsService drs, IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override async ValueTask<GetAdvertisementDetailsResponse> OnFetch(CancellationToken cancellationToken)
            {
                return await Repository.ProjectedSingleByIdWithRelatedAsync<GetAdvertisementDetailsResponse>(Query.Id,
                                                                                                             noTracking: true,
                                                                                                             cancellationToken,
                                                                                                             x => x.Category,
                                                                                                             x => x.Author);
            }

            protected override async ValueTask<GetAdvertisementDetailsResponse> OnFetched(GetAdvertisementDetailsResponse response, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrowAsync(response.AuthorId);

                return response;
            }
        }
    }
}
