namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Queries.GetEntityAuditLogsList
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.GenericHandlers.Relational.Queries;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using FluentValidation;
    using MediatR.GenericOperations.Queries;

    public sealed record GetEntityAuditLogsByTableListQuery : IGetListQuery<GetEntityAuditLogsListResponse>
    {
        public string? TableName { get; init; }

        private sealed class Handler : GetListQueryHandler<GetEntityAuditLogsByTableListQuery, EntityAuditLog, GetEntityAuditLogsListResponse>
        {
            public Handler(IAppRelationalUnitOfWork uow) : base(uow)
            {

            }

            protected override ValueTask<GetEntityAuditLogsByTableListQuery> OnInit(GetEntityAuditLogsByTableListQuery query, CancellationToken cancellationToken)
            {
                Filter = x => x.TableName == Query.TableName;
                OrderBy = (x) => x.OrderByDescending(x => x.CreatedOn);

                return base.OnInit(query, cancellationToken);
            }

            protected override async ValueTask OnValidate(CancellationToken cancellationToken)
            {
                await new GetEntityAuditLogsByTableListQueryValidator().ValidateAndThrowAsync(Query, cancellationToken: cancellationToken);
            }
        }
    }
}


