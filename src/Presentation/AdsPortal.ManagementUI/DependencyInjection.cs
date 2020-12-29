namespace AdsPortal.ManagementUI
{
    using System;
    using System.Net.Http;
    using AdsPortal.Common.Extensions;
    using AdsPortal.ManagementUI.Configurations;
    using AdsPortal.ManagementUI.Models;
    using AdsPortal.ManagementUI.Services;
    using MagicOperations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using RestCRUD.Components;

    public static class DependencyInjection
    {
        public static IServiceCollection AddManagementUI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor(c =>
            {
                c.DetailedErrors = true;
            });

            services.AddRestCRUD();

            services.AddConfiguration<ManagementUIConfiguration>(configuration)
                    .AddConfiguration<HeaderConfiguration>(configuration)
                    .AddConfiguration<FooterConfiguration>(configuration);

            // Setup HttpClient for server side in a client side compatible fashion
            services.AddScoped<HttpClient>(s =>
            {
                // Creating the URI helper needs to wait until the JS Runtime is initialized, so defer it.
                NavigationManager? uriHelper = s.GetRequiredService<NavigationManager>();
                return new HttpClient
                {
                    BaseAddress = new Uri(uriHelper.BaseUri)
                };
            });

            services.AddScoped<IMarkdownService, MarkdownService>();

            services.AddMagicOperations((builder) =>
            {
                builder.UseBaseUri("https://localhost:5001/api/");
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
            });

            return services;
        }

        public static void ConfigureManagementUI(this IApplicationBuilder app)
        {

        }

        public static void MapManagementUI(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        }
    }
}
