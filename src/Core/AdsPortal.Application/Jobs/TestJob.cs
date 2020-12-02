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

        public async ValueTask<object?> Handle(string? args, CancellationToken cancellationToken)
        {
            int millisecondsDelay = 50 + new Random().Next(0, 1000);
            Console.WriteLine($"TestJob {millisecondsDelay}");
            //await Task.Delay(millisecondsDelay, cancellationToken);

            return default;
        }
    }
}
