namespace AdsPortal.Infrastructure.JobScheduler
{
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Infrastructure.JobScheduler.Services;
    using Microsoft.Extensions.DependencyInjection;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureJobSchedulerLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IJobSchedulerRunnerService, JobSchedulerRunnerService>();
            services.AddScoped<IJobSchedulingService, JobSchedulingService>();

            services.AddHostedService<JobSchedulerRunnerService>();

            return services;
        }
    }
}
