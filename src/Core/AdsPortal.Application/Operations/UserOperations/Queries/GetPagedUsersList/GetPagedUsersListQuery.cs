namespace AdsPortal.Application.Operations.UserOperations.Queries.GetPagedJournalsList
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.UserOperations.Queries.GetUsersList;
    using AdsPortal.Domain.Entities;

    public class GetPagedUsersListQuery : IGetPagedListQuery<GetUsersListResponse>
    {
        public int Page { get; set; }
        public int EntiresPerPage { get; set; }

        private class Handler : GetPagedListQueryHandler<GetPagedUsersListQuery, Journal, GetUsersListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}

