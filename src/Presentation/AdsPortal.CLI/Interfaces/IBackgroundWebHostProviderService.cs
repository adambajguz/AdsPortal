namespace AdsPortal.CLI.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;

    public enum WebHostStatuses
    {
        Stopped,
        Running
    }

    public interface IBackgroundWebHostProviderService : IDisposable
    {
        IWebHost WebHost { get; }

        DateTime StartupTime { get; }
        TimeSpan Runtime { get; }
        WebHostStatuses Status { get; }

        Task StartAsync(CancellationToken cancellationToken = default);
        Task RestartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
