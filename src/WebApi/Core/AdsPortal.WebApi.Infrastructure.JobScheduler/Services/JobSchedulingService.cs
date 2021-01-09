namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Interfaces;

    public class JobSchedulingService : IJobSchedulingService
    {
        private readonly IAppRelationalUnitOfWork _uow;
        private readonly IArgumentsSerializer _serializer;

        public JobSchedulingService(IAppRelationalUnitOfWork uow, IArgumentsSerializer serializer)
        {
            _uow = uow;
            _serializer = serializer;
        }

        public async Task ScheduleAsync(Type operationType,
                                        ushort priority = 0,
                                        DateTime? postponeTo = null,
                                        TimeSpan? timeoutAfter = null,
                                        object? operationArguments = null,
                                        CancellationToken cancellationToken = default)
        {
            Job entity = new Job
            {
                Operation = operationType.AssemblyQualifiedName ?? throw new ArgumentException("Invalid type."),
                Priority = priority,
                PostponeTo = postponeTo,
                TimeoutAfter = timeoutAfter,
                Arguments = _serializer.Serialize(operationArguments)
            };

            _uow.Jobs.Add(entity);
            await _uow.SaveChangesAsync(cancellationToken);
        }

        public async Task ScheduleAsync<T>(ushort priority = 0,
                                           DateTime? postponeTo = null,
                                           TimeSpan? timeoutAfter = null,
                                           object? operationArguments = null,
                                           CancellationToken cancellationToken = default)
            where T : class, IJob
        {
            await ScheduleAsync(typeof(T), priority, postponeTo, timeoutAfter, operationArguments, cancellationToken);
        }

        public async Task<bool> ScheduleSingleAsync(Type operationType,
                                                    ushort priority = 0,
                                                    DateTime? postponeTo = null,
                                                    TimeSpan? timeoutAfter = null,
                                                    object? operationArguments = null,
                                                    CancellationToken cancellationToken = default)
        {
            string operation = operationType.AssemblyQualifiedName ?? throw new ArgumentException("Invalid type.");

            if (await _uow.Jobs.ExistsAsync(x => x.Operation == operation, cancellationToken))
            {
                return false;
            }

            await ScheduleAsync(operationType, priority, postponeTo, timeoutAfter, operationArguments, cancellationToken);

            return true;
        }

        public async Task<bool> ScheduleSingleAsync<T>(ushort priority = 0,
                                                       DateTime? postponeTo = null,
                                                       TimeSpan? timeoutAfter = null,
                                                       object? operationArguments = null,
                                                       CancellationToken cancellationToken = default)
            where T : class, IJob
        {
            return await ScheduleSingleAsync(typeof(T), priority, postponeTo, timeoutAfter, operationArguments, cancellationToken);
        }
    }
}
