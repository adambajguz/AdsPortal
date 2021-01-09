namespace AdsPortal.WebApi.Application.Interfaces.JobScheduler
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IJob
    {
        ValueTask Handle(object? args, CancellationToken cancellationToken);
    }
}
