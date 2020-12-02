namespace AdsPortal.Application.Interfaces.JobScheduler
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IJob
    {
        ValueTask<object?> Handle(string? args, CancellationToken cancellationToken);
    }
}
