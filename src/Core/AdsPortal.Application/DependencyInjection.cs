﻿namespace AdsPortal.Application
{
    using System.Reflection;
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Application.Jobs;
    using AdsPortal.Application.Mapping;
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, ILoggerFactory loggerFactory)
        {
            services.AddAutoMapper(cfg =>
                {
                    cfg.AddProfile(new CustomAutoMapperProfile(typeof(DependencyInjection).Assembly, loggerFactory.CreateLogger<CustomAutoMapperProfile>()));
                    cfg.AddProfile(new CustomAutoMapperProfile(typeof(Domain.DependencyInjection).Assembly, loggerFactory.CreateLogger<CustomAutoMapperProfile>()));
                });

            services.AddMediatR(new Assembly[]
                {
                    typeof(DependencyInjection).Assembly,
                    typeof(Domain.DependencyInjection).Assembly
                });

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient<TestJob>();
            services.AddTransient<IJob, TestJob>();

            return services;
        }
    }
}
