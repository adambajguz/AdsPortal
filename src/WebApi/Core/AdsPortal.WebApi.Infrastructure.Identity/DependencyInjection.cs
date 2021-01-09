namespace AdsPortal.WebApi.Infrastructure.Identity
{
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Infrastructure.Identity.Configurations;
    using AdsPortal.WebApi.Infrastructure.Identity.CurrentUser;
    using AdsPortal.WebApi.Infrastructure.Identity.DataRights;
    using AdsPortal.WebApi.Infrastructure.Identity.Jwt;
    using AdsPortal.WebApi.Infrastructure.Identity.UserManager;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureIdentityLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtService, JwtService>();
            services.AddSingleton<IUserManagerService, UserManagerService>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IDataRightsService, DataRightsService>();

            //password hasher configruation
            services.AddConfiguration<PasswordHasherConfiguration>(configuration);

            //jwt authentication configuration
            {
                services.AddConfiguration(configuration, out JwtConfiguration jwtSettings);
                services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;
                    x.TokenValidationParameters = JwtService.GetValidationParameters(jwtSettings);
                });
            }

            return services;
        }
    }
}
