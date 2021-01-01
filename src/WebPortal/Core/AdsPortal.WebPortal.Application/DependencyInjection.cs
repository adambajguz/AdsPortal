namespace AdsPortal.WebPortal.Application
{
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebPortal.Application.Configurations;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Services;
    using MagicOperations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<ManagementUIConfiguration>(configuration)
                    .AddConfiguration<HeaderConfiguration>(configuration)
                    .AddConfiguration<FooterConfiguration>(configuration)
                    .AddConfiguration<ApplicationConfiguration>(configuration, out ApplicationConfiguration applicationConfiguration);

            services.AddScoped<IMarkdownService, MarkdownService>();

            //services.AddHttpClient();

            services.AddMagicOperations((builder) =>
            {
                builder.UseBaseUri(applicationConfiguration.ApiUrl ?? string.Empty);
                builder.AddOperationsFromThisAssembly();

                builder.AddGroupConfiguration(OperationGroups.Advertisement, (g) =>
                {
                    g.Route = "advertisement";
                    g.DisplayName = "Advertisement";
                });

                builder.AddGroupConfiguration(OperationGroups.Category, (g) =>
                {
                    g.Route = "category";
                    g.DisplayName = "Category";
                });

                builder.AddGroupConfiguration(OperationGroups.User, (g) =>
                {
                    g.Route = "user";
                    g.DisplayName = "User";
                });
            });

            return services;
        }
    }
}
