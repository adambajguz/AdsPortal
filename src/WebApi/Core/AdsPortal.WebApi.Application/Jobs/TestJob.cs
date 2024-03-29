﻿namespace AdsPortal.WebApi.Application.Jobs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;

    public class TestJob : IJob
    {
        public TestJob()
        {

        }

        public async ValueTask Handle(Guid jobId, object? args, CancellationToken cancellationToken)
        {
            int millisecondsDelay = 5 + new Random().Next(0, 2000);
            //Console.WriteLine($"TestJob {millisecondsDelay}");
            await Task.Delay(millisecondsDelay, cancellationToken);
            //throw new NullReferenceException();
        }
    }
}
