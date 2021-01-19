namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.CleanupEntityAuditLog
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using FluentValidation;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public sealed record CleanupEntityAuditLogByTableNameCommand : IOperation
    {
        public string? TableName { get; init; }

        private sealed class Handler : IRequestHandler<CleanupEntityAuditLogByTableNameCommand>
        {
            private readonly IAppRelationalUnitOfWork _uow;

            public Handler(IAppRelationalUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(CleanupEntityAuditLogByTableNameCommand request, CancellationToken cancellationToken)
            {
                await new CleanupEntityAuditLogByTableNameValidator().ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

                List<EntityAuditLog> logs = await _uow.EntityAuditLogs.AllAsync(x => x.TableName == request.TableName, cancellationToken: cancellationToken);

                _uow.EntityAuditLogs.RemoveMultiple(logs);
                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
