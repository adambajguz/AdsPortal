namespace AdsPortal.WebApi.Infrastructure.JobScheduler
{
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Configurations;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Services;
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
