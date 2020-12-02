namespace AdsPortal.Application.Operations.CategoryOperations.Queries.GetCategoryDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AutoMapper;

    public class GetCategoryDetailsQuery : IGetDetailsByIdQuery<GetCategoryDetailsResponse>
    {
        public Guid Id { get; init; }

        private class Handler : GetDetailsByIdQueryHandler<GetCategoryDetailsQuery, Category, GetCategoryDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task<Category> OnFetch(CancellationToken cancellationToken)
            {
                return await Repository.SingleByIdWithRelatedAsync(Query.Id, relatedSelector0: x => x.Advertisements, noTracking: true, cancellationToken);
            }
        }
    }
}
