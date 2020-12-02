namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogsList
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetEntityAuditLogsListQuery : IGetListQuery<GetEntityAuditLogsListResponse>
    {
        private class Handler : GetListQueryHandler<GetEntityAuditLogsListQuery, EntityAuditLog, GetEntityAuditLogsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                OrderBy = (x) => x.OrderByDescending(x => x.CreatedOn);

                return Task.CompletedTask;
            }
        }
    }
}
