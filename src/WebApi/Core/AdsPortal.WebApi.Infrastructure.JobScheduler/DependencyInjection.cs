namespace AdsPortal.WebApi.Infrastructure.JobScheduler
{
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Configurations;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Interfaces;
    using AdsPortal.WebApi.Infrastructure.JobScheduler.Services;
    using Microsoft.Extensions.DependencyInjection;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureJobSchedulerLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<JobSchedulerConfiguration>(configuration, out JobSchedulerConfiguration jobSchedulerConfiguration);
            services.AddScoped<IJobSchedulingService, JobSchedulingService>();
            services.AddSingleton<IArgumentsSerializer, DefaultArgumentsSerializer>();

            if (jobSchedulerConfiguration.IsEnabled)
            {
                services.AddHostedService<JobsProcessingService>();
            }

            return services;
        }
    }
}
