namespace AdsPortal.WebApi.Application.Interfaces.JobScheduler
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IJob
    {
        ValueTask Handle(Guid jobId, object? args, CancellationToken cancellationToken);
    }
}
