namespace AdsPortal.Application.Operations.UserOperations.Queries.GetUserDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public class GetUserDetailsQuery : IGetDetailsQuery<GetUserDetailsResponse>, IIdentifiableOperation<GetUserDetailsResponse>
    {
        public Guid Id { get; init; }

        private class Handler : GetDetailsByIdQueryHandler<GetUserDetailsQuery, User, GetUserDetailsResponse>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }

            protected override async Task OnValidate(User entity, CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrow(Query.Id);
            }
        }
    }
}
