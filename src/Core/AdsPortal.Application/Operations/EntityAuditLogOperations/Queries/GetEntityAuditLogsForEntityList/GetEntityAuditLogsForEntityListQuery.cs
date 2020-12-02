namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogsList
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetEntityAuditLogsForEntityListQuery : IGetListQuery<GetEntityAuditLogsForEntityListResponse>
    {
        public Guid Id { get; init; }

        private class Handler : GetListQueryHandler<GetEntityAuditLogsForEntityListQuery, EntityAuditLog, GetEntityAuditLogsForEntityListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                Filter = x => x.Key == Query.Id;
                OrderBy = (x) => x.OrderByDescending(x => x.CreatedOn);

                return Task.CompletedTask;
            }
        }
    }
}


