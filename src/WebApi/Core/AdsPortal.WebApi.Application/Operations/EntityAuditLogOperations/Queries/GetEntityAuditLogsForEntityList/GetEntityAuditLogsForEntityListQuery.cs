namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogsList
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetEntityAuditLogsForEntityListQuery : IGetListQuery<GetEntityAuditLogsForEntityListResponse>
    {
        public Guid Id { get; init; }

        private class Handler : GetListQueryHandler<GetEntityAuditLogsForEntityListQuery, EntityAuditLog, GetEntityAuditLogsForEntityListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override ValueTask<GetEntityAuditLogsForEntityListQuery> OnInit(GetEntityAuditLogsForEntityListQuery command, CancellationToken cancellationToken)
            {
                Filter = x => x.Key == Query.Id;
                OrderBy = (x) => x.OrderByDescending(x => x.CreatedOn);

                return base.OnInit(command, cancellationToken);
            }
        }
    }
}


