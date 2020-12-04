namespace AdsPortal.Infrastructure.JobScheduler.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Application.Interfaces.Persistence.UoW;
    using AdsPortal.Domain.Entities;
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
                                   object? operationArguments = null,
                                   CancellationToken cancellationToken = default)
        {
            Job entity = new Job
            {
                Operation = operationType.AssemblyQualifiedName ?? throw new ArgumentException("Invalid type."),
                Priority = priority,
                PostponeTo = postponeTo,
                Arguments = JsonConvert.SerializeObject(operationArguments)
            };

            Uow.Jobs.Add(entity);
            await Uow.SaveChangesAsync(cancellationToken);
        }

        public async Task Schedule<T>(ushort priority = 0,
                                      DateTime? postponeTo = null,
                                      object? operationArguments = null,
                                      CancellationToken cancellationToken = default)
            where T : class, IJob
        {
            Job entity = new Job
            {
                Operation = typeof(T).AssemblyQualifiedName ?? throw new ArgumentException("Invalid type."),
                Priority = priority,
                PostponeTo = postponeTo,
                Arguments = operationArguments is null ? null : JsonConvert.SerializeObject(operationArguments)
            };

            Uow.Jobs.Add(entity);
            await Uow.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ScheduleOne(Type operationType,
                                            ushort priority = 0,
                                            DateTime? postponeTo = null,
                                            object? operationArguments = null,
                                            CancellationToken cancellationToken = default)
        {
            string operation = operationType.AssemblyQualifiedName ?? throw new ArgumentException("Invalid type.");

            if (await Uow.Jobs.ExistsAsync(x => x.Operation == operation, cancellationToken))
                return false;

            Job entity = new Job
            {
                Operation = operation,
                Priority = priority,
                PostponeTo = postponeTo,
                Arguments = JsonConvert.SerializeObject(operationArguments)
            };

            Uow.Jobs.Add(entity);
            await Uow.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> ScheduleOne<T>(ushort priority = 0,
                                               DateTime? postponeTo = null,
                                               object? operationArguments = null,
                                               CancellationToken cancellationToken = default)
            where T : class, IJob
        {
            string operation = typeof(T).AssemblyQualifiedName ?? throw new ArgumentException("Invalid type.");

            if (await Uow.Jobs.ExistsAsync(x => x.Operation == operation, cancellationToken))
                return false;

            Job entity = new Job
            {
                Operation = typeof(T).AssemblyQualifiedName ?? throw new ArgumentException("Invalid type."),
                Priority = priority,
                PostponeTo = postponeTo,
                Arguments = operationArguments is null ? null : JsonConvert.SerializeObject(operationArguments)
            };

            Uow.Jobs.Add(entity);
            await Uow.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
