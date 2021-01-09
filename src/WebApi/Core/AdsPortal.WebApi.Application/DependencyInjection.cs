namespace AdsPortal.Application
{
    using System.Reflection;
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Application.Jobs;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Configurations;
    using AutoMapper;
    using MediatR;
    using MediatR.GenericOperations.Mapping;
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
                    cfg.AddProfile(new CustomAutoMapperProfile(typeof(DependencyInjection).Assembly, loggerFactory.CreateLogger<CustomAutoMapperProfile>()));
                    cfg.AddProfile(new CustomAutoMapperProfile(typeof(WebApi.Domain.DependencyInjection).Assembly, loggerFactory.CreateLogger<CustomAutoMapperProfile>()));
                });

            services.AddMediatR(new Assembly[]
                {
                    typeof(DependencyInjection).Assembly,
                    typeof(WebApi.Domain.DependencyInjection).Assembly
                });

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient<TestJob>();
            services.AddTransient<IJob, TestJob>();

            return services;
        }
    }
}
