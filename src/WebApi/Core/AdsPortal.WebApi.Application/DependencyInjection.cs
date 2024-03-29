﻿namespace AdsPortal.WebApi.Application
{
    using System.Reflection;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Configurations;
    using AdsPortal.WebApi.Application.Extensions;
    using AdsPortal.WebApi.Application.Jobs;
    using AdsPortal.WebApi.Application.Services;
    using AutoMapper;
    using AutoMapper.Extensions;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            services.AddConfiguration<ApplicationConfiguration>(configuration);

            services.AddAutoMapper(cfg =>
                {
                    cfg.AddCustomMappings(typeof(Application.DependencyInjection).Assembly, loggerFactory);
                    cfg.AddCustomMappings(typeof(Domain.DependencyInjection).Assembly, loggerFactory);
                });

            services.AddMediatR(new Assembly[]
                {
                    typeof(Application.DependencyInjection).Assembly,
                    typeof(Domain.DependencyInjection).Assembly
                });

            services.AddJob<TestJob>();
            services.AddJob<AdvertisementExpirationNotificationSenderJob>();
            services.AddJob<SendEmailJob>();

            services.AddMultipleInstanceHostedService<PeriodicJobStarter>();

            return services;
        }
    }
}
