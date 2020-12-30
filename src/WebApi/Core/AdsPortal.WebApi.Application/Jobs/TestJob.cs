namespace AdsPortal.Application.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.JobScheduler;

    public class TestJob : IJob
    {
        public TestJob()
        {

        }

        public async ValueTask Handle(string? args, CancellationToken cancellationToken)
        {
            int millisecondsDelay = 5 + new Random().Next(0, 2000);
            //Console.WriteLine($"TestJob {millisecondsDelay}");
            await Task.Delay(millisecondsDelay, cancellationToken);
        }
    }
}
