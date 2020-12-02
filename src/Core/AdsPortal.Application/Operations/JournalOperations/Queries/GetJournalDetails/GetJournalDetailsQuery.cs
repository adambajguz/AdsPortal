namespace AdsPortal.Application.Operations.JournalOperations.Queries.GetJournalDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AutoMapper;

    public class GetJournalDetailsQuery : IGetDetailsByIdQuery<GetJournalDetailsResponse>
    {
        public Guid Id { get; }

        public GetJournalDetailsQuery(Guid id)
        {
            Id = id;
        }

        private class Handler : GetDetailsByIdQueryHandler<GetJournalDetailsQuery, Journal, GetJournalDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task<Journal> OnFetch(CancellationToken cancellationToken)
            {
                return await Repository.SingleByIdWithRelatedAsync(Query.Id, relatedSelector0: x => x.Publications, noTracking: true, cancellationToken);
            }
        }
    }
}
