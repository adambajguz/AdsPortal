namespace AdsPortal.Infrastructure.JobScheduler
{
    using AdsPortal.Application.Configurations;
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Common.Extensions;
    using AdsPortal.Infrastructure.JobScheduler.Services;
    using Microsoft.Extensions.DependencyInjection;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureJobSchedulerLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<JobSchedulerConfiguration>(configuration);
            services.AddScoped<IJobSchedulingService, JobSchedulingService>();

            services.AddHostedService<JobsProcessingService>();

            return services;
        }
    }
}
