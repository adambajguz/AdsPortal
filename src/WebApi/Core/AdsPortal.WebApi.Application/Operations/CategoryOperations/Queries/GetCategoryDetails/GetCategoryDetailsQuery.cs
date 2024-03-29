namespace AdsPortal.WebApi.Application.Operations.CategoryOperations.Queries.GetCategoryDetails
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

    public sealed record GetCategoryDetailsQuery : IGetDetailsQuery<GetCategoryDetailsResponse>, IIdentifiableOperation<GetCategoryDetailsResponse>
    {
        public Guid Id { get; init; }

        private sealed class Handler : GetDetailsByIdQueryHandler<GetCategoryDetailsQuery, Category, GetCategoryDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override async ValueTask<GetCategoryDetailsResponse> OnFetch(CancellationToken cancellationToken)
            {
                return await Repository.ProjectedSingleByIdWithRelatedAsync<GetCategoryDetailsResponse>(Query.Id, noTracking: true, cancellationToken, x => x.Advertisements);
            }
        }
    }
}
