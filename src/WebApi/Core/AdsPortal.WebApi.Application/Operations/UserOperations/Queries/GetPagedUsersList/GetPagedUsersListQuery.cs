namespace AdsPortal.Application.Operations.UserOperations.Queries.GetPagedJournalsList
{
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.Operations.UserOperations.Queries.GetUsersList;
    using AdsPortal.WebApi.Domain.Entities;

    public sealed record GetPagedUsersListQuery : IGetPagedListQuery<GetUsersListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private class Handler : GetPagedListQueryHandler<GetPagedUsersListQuery, User, GetUsersListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}

