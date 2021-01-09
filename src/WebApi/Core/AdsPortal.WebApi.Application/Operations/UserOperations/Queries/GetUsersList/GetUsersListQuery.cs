namespace AdsPortal.WebApi.Application.Operations.UserOperations.Queries.GetUsersList
{
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetUsersListQuery : IGetListQuery<GetUsersListResponse>
    {
        private sealed class Handler : GetListQueryHandler<GetUsersListQuery, User, GetUsersListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }
        }
    }
}

