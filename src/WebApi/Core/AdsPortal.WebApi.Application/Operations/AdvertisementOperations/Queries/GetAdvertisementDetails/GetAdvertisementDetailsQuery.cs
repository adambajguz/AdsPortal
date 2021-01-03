namespace AdsPortal.Application.Operations.AdvertisementOperations.Queries.GetAdvertisementDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public class GetAdvertisementDetailsQuery : IGetDetailsQuery<GetAdvertisementDetailsResponse>, IIdentifiableOperation<GetAdvertisementDetailsResponse>
    {
        public Guid Id { get; init; }

        private class Handler : GetDetailsByIdQueryHandler<GetAdvertisementDetailsQuery, Advertisement, GetAdvertisementDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task<Advertisement> OnFetch(CancellationToken cancellationToken)
            {
                return await Repository.SingleByIdWithRelatedAsync(Query.Id,
                                                                   noTracking: true,
                                                                   cancellationToken,
                                                                   x => x.Category,
                                                                   x => x.Author);
            }
        }
    }
}