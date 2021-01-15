namespace AdsPortal.WebApi.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
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
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override async ValueTask<GetAdvertisementDetailsResponse> OnFetch(CancellationToken cancellationToken)
            {
                return await Repository.ProjectedSingleByIdWithRelatedAsync<GetAdvertisementDetailsResponse>(Query.Id,
                                                                                                             noTracking: true,
                                                                                                             cancellationToken,
                                                                                                             x => x.Category,
                                                                                                             x => x.Author);
            }
        }
    }
}
