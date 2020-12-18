namespace AdsPortal.Infrastructure
{
    using AdsPortal.Common.Extensions;
    using AdsPortal.Infrastructure.Configurations;
    using AdsPortal.Infrastructure.Email;
    using Application.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //email
            services.AddConfiguration<EmailConfiguration>(configuration);
            services.AddSingleton<IEmailService, EmailService>();

            return services;
        }
    }
}
