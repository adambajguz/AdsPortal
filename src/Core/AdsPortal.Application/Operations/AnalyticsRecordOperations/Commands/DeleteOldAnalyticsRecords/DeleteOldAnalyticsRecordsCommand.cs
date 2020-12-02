namespace AdsPortal.Application.Operations.AnalyticsRecordOperations.Commands.DeleteOldAnalyticsRecords
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Application.OperationsAbstractions;
    using MediatR;

    public class DeleteOldAnalyticsRecordsCommand : IOperation
    {
        public DateTime? OlderThanOrEqualToDate { get; init; }

        private class Handler : IRequestHandler<DeleteOldAnalyticsRecordsCommand>
        {
            private readonly IMongoUnitOfWork _uow;

            public Handler(IMongoUnitOfWork uow)
            {
                _uow = uow;
            }

            public async Task<Unit> Handle(DeleteOldAnalyticsRecordsCommand command, CancellationToken cancellationToken)
            {
                DateTime date = command.OlderThanOrEqualToDate?.Date ?? DateTime.UtcNow.Date;
                await _uow.AnalyticsRecords.RemoveManyAsync(x => x.Timestamp <= date, cancellationToken);

                return await Unit.Task;
            }
        }
    }
}
