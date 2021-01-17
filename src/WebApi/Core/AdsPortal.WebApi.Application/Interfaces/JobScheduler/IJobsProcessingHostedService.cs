namespace AdsPortal.WebApi.Application.Interfaces.JobScheduler
{
    using System;
    using Microsoft.Extensions.Hosting;

    public interface IJobsProcessingHostedService : IHostedService, IDisposable
    {
        Guid InstanceId { get; }
    }
}
