namespace AdsPortal.Infrastructure
{
    using Application.Interfaces;
    using AdsPortal.Common.Extensions;
    using AdsPortal.Infrastructure.Email;
    using Microsoft.Extensions.DependencyInjection;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //email
            services.AddConfiguration<EmailSettings>(configuration);
            services.AddSingleton<IEmailService, EmailService>();

            return services;
        }
    }
}
