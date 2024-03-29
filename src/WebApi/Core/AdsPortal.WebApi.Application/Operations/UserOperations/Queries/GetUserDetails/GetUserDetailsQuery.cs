namespace AdsPortal.WebApi.Application.Operations.UserOperations.Queries.GetUserDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AutoMapper;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Queries;

    public sealed record GetUserDetailsQuery : IGetDetailsQuery<GetUserDetailsResponse>, IIdentifiableOperation<GetUserDetailsResponse>
    {
        public Guid Id { get; init; }

        private sealed class Handler : GetDetailsByIdQueryHandler<GetUserDetailsQuery, User, GetUserDetailsResponse>
        {
            private readonly IDataRightsService _drs;

            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper, IDataRightsService drs) : base(uow, mapper)
            {
                _drs = drs;
            }

            protected override async ValueTask OnValidate(CancellationToken cancellationToken)
            {
                await _drs.IsOwnerOrAdminElseThrowAsync(Query.Id);
            }
        }
    }
}
