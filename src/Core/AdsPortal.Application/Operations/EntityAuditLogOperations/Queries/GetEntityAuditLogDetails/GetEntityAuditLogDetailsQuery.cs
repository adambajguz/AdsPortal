namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Queries.GetRouteLogDetails
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using AdsPortal.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;

    public class GetEntityAuditLogDetailsQuery : IGetDetailsByIdQuery<GetEntityAuditLogDetailsResponse>
    {
        public Guid Id { get; }

        public GetEntityAuditLogDetailsQuery(Guid id)
        {
            Id = id;
        }

        private class Handler : GetDetailsByIdQueryHandler<GetEntityAuditLogDetailsQuery, EntityAuditLog, GetEntityAuditLogDetailsResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow, IMapper mapper) : base(uow, mapper)
            {

            }

            protected override Task OnInit(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
