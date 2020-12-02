namespace AdsPortal.Application
{
    using System.Reflection;
    using AutoMapper;
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Application.Jobs;
    using AdsPortal.Application.Mapping;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
                {
                    cfg.AddProfile(new CustomAutoMapperProfile(typeof(DependencyInjection).GetTypeInfo().Assembly));
                    cfg.AddProfile(new CustomAutoMapperProfile(typeof(Domain.DependencyInjection).GetTypeInfo().Assembly));
                });

            services.AddMediatR(new Assembly[]
                {
                    typeof(DependencyInjection).GetTypeInfo().Assembly,
                    typeof(Domain.DependencyInjection).GetTypeInfo().Assembly
                });

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient<TestJob>();
            services.AddTransient<IJob, TestJob>();

            return services;
        }
    }
}
