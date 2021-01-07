namespace AdsPortal.Application.Operations.UserOperations.Queries.GetUsersList
{
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetUsersListQuery : IGetListQuery<GetUsersListResponse>
    {
        private class Handler : GetListQueryHandler<GetUsersListQuery, User, GetUsersListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}

