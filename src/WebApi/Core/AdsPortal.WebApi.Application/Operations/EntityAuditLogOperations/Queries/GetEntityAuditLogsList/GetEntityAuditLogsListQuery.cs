namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogsList
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using MediatR.GenericOperations.Queries;

    public sealed record GetEntityAuditLogsListQuery : IGetListQuery<GetEntityAuditLogsListResponse>
    {
        private sealed class Handler : GetListQueryHandler<GetEntityAuditLogsListQuery, EntityAuditLog, GetEntityAuditLogsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override ValueTask<GetEntityAuditLogsListQuery> OnInit(GetEntityAuditLogsListQuery command, CancellationToken cancellationToken)
            {
                OrderBy = (x) => x.OrderByDescending(x => x.CreatedOn);

                return base.OnInit(command, cancellationToken);
            }
        }
    }
}
