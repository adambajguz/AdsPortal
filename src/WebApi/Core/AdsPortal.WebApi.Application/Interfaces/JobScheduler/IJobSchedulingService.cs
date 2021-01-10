namespace AdsPortal.WebApi.Application.Interfaces.JobScheduler
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IJobSchedulingService
    {
        /// <summary>
        /// Schedules a job.
        /// </summary>
        Task ScheduleAsync(Type operationType,
                           int priority = 0,
                           DateTime? postponeTo = null,
                           TimeSpan? timeoutAfter = null,
                           object? operationArguments = null,
                           CancellationToken cancellationToken = default);

        /// <summary>
        /// Schedules a job.
        /// </summary>
        Task ScheduleAsync<T>(int priority = 0,
                              DateTime? postponeTo = null,
                              TimeSpan? timeoutAfter = null,
                              object? operationArguments = null,
                              CancellationToken cancellationToken = default)
            where T : class, IJob;

        /// <summary>
        /// Schedules a job if job with same type was not already scheduled.
        /// </summary>
        Task<bool> ScheduleSingleAsync(Type operationType,
                                       int priority = 0,
                                       DateTime? postponeTo = null,
                                       TimeSpan? timeoutAfter = null,
                                       object? operationArguments = null,
                                       CancellationToken cancellationToken = default);

        /// <summary>
        /// Schedules a job if job with same type was not already scheduled.
        /// </summary>
        Task<bool> ScheduleSingleAsync<T>(int priority = 0,
                                          DateTime? postponeTo = null,
                                          TimeSpan? timeoutAfter = null,
                                          object? operationArguments = null,
                                          CancellationToken cancellationToken = default)
            where T : class, IJob;
    }
}