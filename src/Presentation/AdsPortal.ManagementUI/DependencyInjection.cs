namespace AdsPortal.ManagementUI
{
    using System;
    using System.Net.Http;
    using AdsPortal.Application.Configurations;
    using AdsPortal.Common.Extensions;
    using AdsPortal.ManagementUI.Configurations;
    using AdsPortal.ManagementUI.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using VxFormGenerator.Settings.Plain;

    public static class DependencyInjection
    {
        public static IServiceCollection AddManagementUI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor(c =>
            {
                c.DetailedErrors = true;
            });

            services.AddVxFormGenerator();

            services.AddConfiguration<ApplicationConfiguration>(configuration)
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
