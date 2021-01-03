﻿namespace AdsPortal.Application.Configurations
{
    using System;

    public sealed class JobSchedulerConfiguration
    {
        public bool IsEnabled { get; init; }
        public TimeSpan StartupDelay { get; init; }
        public int Tick { get; init; }

        public int MaxConcurent { get; init; }
        public int ConcurentBatchSize { get; init; }
    }
}