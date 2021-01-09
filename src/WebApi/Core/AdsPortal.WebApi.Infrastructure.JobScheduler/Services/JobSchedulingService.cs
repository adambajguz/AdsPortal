namespace AdsPortal.WebApi.Infrastructure.JobScheduler.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.UoW;
    using AdsPortal.WebApi.Domain.Entities;
    using Newtonsoft.Json;

    public class JobSchedulingService : IJobSchedulingService
    {
        private IAppRelationalUnitOfWork Uow { get; }

        public JobSchedulingService(IAppRelationalUnitOfWork uow)
        {
            Uow = uow;
        }

        public async Task Schedule(Type operationType,
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
                Arguments = JsonConvert.SerializeObject(operationArguments)
            };

            Uow.Jobs.Add(entity);
            await Uow.SaveChangesAsync(cancellationToken);
        }

        public async Task Schedule<T>(ushort priority = 0,
                                      DateTime? postponeTo = null,
                                      TimeSpan? timeoutAfter = null,
                                      object? operationArguments = null,
                                      CancellationToken cancellationToken = default)
            where T : class, IJob
        {
            await Schedule(typeof(T), priority, postponeTo, timeoutAfter, operationArguments, cancellationToken);
        }

        public async Task<bool> ScheduleOne(Type operationType,
                                            ushort priority = 0,
                                            DateTime? postponeTo = null,
                                            TimeSpan? timeoutAfter = null,
                                            object? operationArguments = null,
                                            CancellationToken cancellationToken = default)
        {
            string operation = operationType.AssemblyQualifiedName ?? throw new ArgumentException("Invalid type.");

            if (await Uow.Jobs.ExistsAsync(x => x.Operation == operation, cancellationToken))
                return false;

            await Schedule(operationType, priority, postponeTo, timeoutAfter, operationArguments, cancellationToken);

            return true;
        }

        public async Task<bool> ScheduleOne<T>(ushort priority = 0,
                                               DateTime? postponeTo = null,
                                               TimeSpan? timeoutAfter = null,
                                               object? operationArguments = null,
                                               CancellationToken cancellationToken = default)
            where T : class, IJob
        {
            return await ScheduleOne(typeof(T), priority, postponeTo, timeoutAfter, operationArguments, cancellationToken);
        }
    }
}
