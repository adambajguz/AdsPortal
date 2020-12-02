namespace AdsPortal.Application.Operations.EntityAuditLogOperations.Commands.CleanupEntityAuditLog
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using FluentValidation;
    using MediatR;

    public class CleanupEntityAuditLogCommand : IOperation
    {
        public CleanupEntityAuditLogRequest Data { get; }

        public CleanupEntityAuditLogCommand(CleanupEntityAuditLogRequest data)
        {
            Data = data;
        }

        private class Handler : IRequestHandler<CleanupEntityAuditLogCommand>
        {
            private readonly IAppRelationalUnitOfWork _uow;

            public Handler(IAppRelationalUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(CleanupEntityAuditLogCommand request, CancellationToken cancellationToken)
            {
                CleanupEntityAuditLogRequest data = request.Data;

                //EntityAuditLog routeLog = await _uow.EntityAuditLogsRepository.GetByIdAsync(data.Id);

                await new CleanupEntityAuditLogValidator().ValidateAndThrowAsync(data, cancellationToken: cancellationToken);

                //_uow.EntityAuditLogsRepository.Remove(routeLog);
                //await _uow.SaveChangesAsync();

                return await Unit.Task;
            }
        }
    }
}
