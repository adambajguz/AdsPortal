namespace AdsPortal.Application.Operations.JournalOperations.Queries.GetJournalsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetJournalsListQuery : IGetListQuery<GetJournalsListResponse>
    {
        public GetJournalsListQuery()
        {

        }

        private class Handler : GetListQueryHandler<GetJournalsListQuery, Journal, GetJournalsListResponse>
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
