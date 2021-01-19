namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Queries.GetEntityAuditLogsList
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using FluentValidation;
    using MediatR.GenericOperations.Queries;

    public sealed record GetEntityAuditLogsByEntityKeyListQuery : IGetListQuery<GetEntityAuditLogsListResponse>
    {
        public Guid Key { get; init; }

        private sealed class Handler : GetListQueryHandler<GetEntityAuditLogsByEntityKeyListQuery, EntityAuditLog, GetEntityAuditLogsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override ValueTask<GetEntityAuditLogsByEntityKeyListQuery> OnInit(GetEntityAuditLogsByEntityKeyListQuery query, CancellationToken cancellationToken)
            {
                Filter = x => x.Key == Query.Key;
                OrderBy = (x) => x.OrderByDescending(x => x.CreatedOn);

                return base.OnInit(query, cancellationToken);
            }

            protected override async ValueTask OnValidate(CancellationToken cancellationToken)
            {
                await new GetEntityAuditLogsByEntityKeyListQueryValidator().ValidateAndThrowAsync(Query, cancellationToken: cancellationToken);
            }
        }
    }
}


