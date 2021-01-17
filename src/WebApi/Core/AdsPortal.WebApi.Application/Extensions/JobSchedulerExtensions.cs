namespace AdsPortal.WebApi.Application.Extensions
{
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using Microsoft.Extensions.DependencyInjection;

    public static class JobSchedulerExtensions
    {
        public static IServiceCollection AddJob<TJob>(this IServiceCollection services)
            where TJob : class, IJob
        {
            services.AddTransient<TJob>();
            services.AddTransient<IJob, TJob>();

            return services;
        }
    }
}
