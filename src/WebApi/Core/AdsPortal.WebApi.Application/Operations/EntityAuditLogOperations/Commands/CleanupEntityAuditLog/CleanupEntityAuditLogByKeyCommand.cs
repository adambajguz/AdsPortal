namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.CleanupEntityAuditLog
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using FluentValidation;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public sealed record CleanupEntityAuditLogByKeyCommand : IOperation
    {
        public Guid Key { get; init; }

        private sealed class Handler : IRequestHandler<CleanupEntityAuditLogByKeyCommand>
        {
            private readonly IAppRelationalUnitOfWork _uow;

            public Handler(IAppRelationalUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(CleanupEntityAuditLogByKeyCommand request, CancellationToken cancellationToken)
            {
                await new CleanupEntityAuditLogByKeyValidator().ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

                List<EntityAuditLog> logs = await _uow.EntityAuditLogs.AllAsync(x => x.Key == request.Key, cancellationToken: cancellationToken);

                _uow.EntityAuditLogs.RemoveMultiple(logs);
                await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
