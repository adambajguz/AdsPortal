namespace AdsPortal.CLI.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;

    public interface IWebHostRunnerService
    {
        Task RunAsync(CancellationToken cancellationToken = default);
        Task<IWebHost> StartAsync(CancellationToken cancellationToken = default);
    }
}
