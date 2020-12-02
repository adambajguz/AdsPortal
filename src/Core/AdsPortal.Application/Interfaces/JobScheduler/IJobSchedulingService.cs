namespace AdsPortal.Application.Interfaces.JobScheduler
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IJobSchedulingService
    {
        Task Schedule(Type operationType,
                      ushort priority = 0,
                      DateTime? postponeTo = null,
                      object? operationArguments = null,
                      CancellationToken cancellationToken = default);

        Task Schedule<T>(ushort priority = 0,
                         DateTime? postponeTo = null,
                         object? operationArguments = null,
                         CancellationToken cancellationToken = default)
            where T : class, IJob;

        Task<bool> ScheduleOne(Type operationType,
                               ushort priority = 0,
                               DateTime? postponeTo = null,
                               object? operationArguments = null,
                               CancellationToken cancellationToken = default);

        Task<bool> ScheduleOne<T>(ushort priority = 0,
                                  DateTime? postponeTo = null,
                                  object? operationArguments = null,
                                  CancellationToken cancellationToken = default)
            where T : class, IJob;
    }
}