namespace AdsPortal.ManagementUI
{
    using System;
    using System.Net.Http;
    using Blazored.LocalStorage;
    using Blazorise;
    using Blazorise.Bootstrap;
    using Blazorise.Icons.FontAwesome;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddManagementUI(this IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor(c =>
            {
                c.DetailedErrors = true;
            });

            services.AddBlazorise(options =>
            {
                options.ChangeTextOnKeyPress = true; // optional
            });
            services.AddBootstrapProviders()
                    .AddFontAwesomeIcons();

            services.AddBlazorContextMenu();

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

            services.AddBlazoredLocalStorage();

            return services;
        }

        public static void ConfigureManagementUI(this IApplicationBuilder app)
        {
            app.ApplicationServices
               .UseBootstrapProviders()
               .UseFontAwesomeIcons();
        }

        public static void MapManagementUI(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        }
    }
}
