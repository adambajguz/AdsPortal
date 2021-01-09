namespace AdsPortal.WebApi.Application.Operations.UserOperations.Queries.GetUsersList
{
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetPagedUsersListQuery : IGetPagedListQuery<GetUsersListResponse>
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }

        private sealed class Handler : GetPagedListQueryHandler<GetPagedUsersListQuery, User, GetUsersListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}

