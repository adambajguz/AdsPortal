namespace AdsPortal.SpecialPages.Core
{
    using System;
    using AdsPortal.SpecialPages;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public static class SpecialPagesConfiguration
    {
        public static IApplicationBuilder ConfigureSpecialPages(this IApplicationBuilder app, IWebHostEnvironment environment, IServiceCollection? services)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            // Register Development pages
            if (environment.IsDevelopment())
                app.AddSpecialPage<RegisteredServicesPage>(environment, services);

            // Register Development and Production pages

            return app;
        }
    }
}
