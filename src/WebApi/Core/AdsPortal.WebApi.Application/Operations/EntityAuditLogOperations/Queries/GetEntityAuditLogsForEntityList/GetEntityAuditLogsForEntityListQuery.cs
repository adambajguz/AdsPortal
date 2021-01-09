namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Queries.GetEntityAuditLogsForEntityList
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetEntityAuditLogsForEntityListQuery : IGetListQuery<GetEntityAuditLogsForEntityListResponse>
    {
        public Guid Id { get; init; }

        private sealed class Handler : GetListQueryHandler<GetEntityAuditLogsForEntityListQuery, EntityAuditLog, GetEntityAuditLogsForEntityListResponse>
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


