namespace AdsPortal.Application.Operations.JournalOperations.Queries.GetPagedJournalsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.JournalOperations.Queries.GetJournalsList;
    using AdsPortal.Domain.Entities;

    public class GetPagedJournalsListQuery : IGetPagedListQuery<GetJournalsListResponse>
    {
        public int Page { get; set; }
        public int EntiresPerPage { get; set; }

        private class Handler : GetPagedListQueryHandler<GetPagedJournalsListQuery, Journal, GetJournalsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                //OrderBy = (q) => q.OrderBy(x => x.Points);

                return Task.CompletedTask;
            }
        }
    }
}
