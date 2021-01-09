namespace AdsPortal.WebApi.Application.Operations.EntityAuditLogOperations.Commands.CleanupEntityAuditLog
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using FluentValidation;
    using MediatR;
    using MediatR.GenericOperations.Abstractions;

    public sealed record CleanupEntityAuditLogCommand : IOperation
    {
        public DateTime CreatedOn { get; init; }
        public string? TableName { get; init; }
        public Guid? Key { get; init; }

        private sealed class Handler : IRequestHandler<CleanupEntityAuditLogCommand>
        {
            private readonly IAppRelationalUnitOfWork _uow;

            public Handler(IAppRelationalUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(CleanupEntityAuditLogCommand request, CancellationToken cancellationToken)
            {
                //EntityAuditLog routeLog = await _uow.EntityAuditLogsRepository.GetByIdAsync(data.Id);

                await new CleanupEntityAuditLogValidator().ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

                //_uow.EntityAuditLogsRepository.Remove(routeLog);
                //await _uow.SaveChangesAsync(cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
