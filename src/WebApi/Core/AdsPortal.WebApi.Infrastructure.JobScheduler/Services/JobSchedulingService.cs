namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Enums;
    using AdsPortal.WebApi.Domain.Interfaces.UoW;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Configurations;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Interfaces;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class JobSchedulingService : IJobSchedulingService
    {
        private readonly IAppRelationalUnitOfWork _uow;
        private readonly IArgumentsSerializer _serializer;
        private readonly JobSchedulerConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly Lazy<Guid[]> _knownInstancesIds;

        private static readonly JobStatuses[] JobNotFinishedStatuses = new[] { JobStatuses.Queued, JobStatuses.Taken, JobStatuses.Running, JobStatuses.Cancelled, JobStatuses.Error };

        public JobSchedulingService(IAppRelationalUnitOfWork uow,
                                    IArgumentsSerializer serializer,
                                    IOptions<JobSchedulerConfiguration> configuration,
                                    ILogger<JobSchedulingService> logger,
                                    IEnumerable<IHostedService> hostedServices)
        {
            _uow = uow;
            _serializer = serializer;
            _configuration = configuration.Value;
            _logger = logger;

            _knownInstancesIds = new Lazy<Guid[]>(() =>
            {
                return hostedServices.Select(x => x as IJobsProcessingHostedService)
                                                     .Where(x => x is not null)
                                                     .Select(x => x!.InstanceId)
                                                     .ToArray();
            });
        }

        public async Task ScheduleAsync(Type operationType,
                                        int priority = 0,
                                        DateTime? postponeTo = null,
                                        TimeSpan? timeoutAfter = null,
                                        Guid? runAfter = null,
                                        object? operationArguments = null,
                                        CancellationToken cancellationToken = default)
        {
            Job entity = new Job
            {
                Operation = operationType.AssemblyQualifiedName ?? throw new ArgumentException("Invalid type."),
                Priority = priority,
                PostponeTo = postponeTo,
                TimeoutAfter = timeoutAfter,
                RunAfterId = runAfter,
                Arguments = _serializer.Serialize(operationArguments)
            };

            _uow.Jobs.Add(entity);
            await _uow.SaveChangesAsync(cancellationToken);

            _logger.LogDebug("Scheduled job {Id} {No} {Type}", entity.Id, entity.JobNo, entity.Operation);
        }

        public async Task ScheduleAsync<T>(int priority = 0,
                                           DateTime? postponeTo = null,
                                           TimeSpan? timeoutAfter = null,
                                           Guid? runAfter = null,
                                           object? operationArguments = null,
                                           CancellationToken cancellationToken = default)
            where T : class, IJob
        {
            await ScheduleAsync(typeof(T), priority, postponeTo, timeoutAfter, runAfter, operationArguments, cancellationToken);
        }

        public async Task<bool> ScheduleSingleAsync(Type operationType,
                                                    int priority = 0,
                                                    DateTime? postponeTo = null,
                                                    TimeSpan? timeoutAfter = null,
                                                    Guid? runAfter = null,
                                                    object? operationArguments = null,
                                                    CancellationToken cancellationToken = default)
        {
            string operation = operationType.AssemblyQualifiedName ?? throw new ArgumentException("Invalid type.");

            int maxTries = _configuration.MaxTries <= 0 ? 10 : _configuration.MaxTries;
            Guid[] knownInstancesIds = _knownInstancesIds.Value;

            bool exists = await _uow.Jobs.ExistsAsync(filter: x => x.Operation == operation && x.Id != runAfter && JobNotFinishedStatuses.Contains(x.Status),
                                                      cancellationToken: cancellationToken);

            if (exists)
            {
                return false;
            }

            await ScheduleAsync(operationType, priority, postponeTo, timeoutAfter, runAfter, operationArguments, cancellationToken);

            return true;
        }

        public async Task<bool> ScheduleSingleAsync<T>(int priority = 0,
                                                       DateTime? postponeTo = null,
                                                       TimeSpan? timeoutAfter = null,
                                                       Guid? runAfter = null,
                                                       object? operationArguments = null,
                                                       CancellationToken cancellationToken = default)
            where T : class, IJob
        {
            return await ScheduleSingleAsync(typeof(T), priority, postponeTo, timeoutAfter, runAfter, operationArguments, cancellationToken);
        }
    }
}
