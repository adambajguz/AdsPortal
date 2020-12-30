namespace AdsPortal.SpecialPages.Core
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public static class SpecialPagesExtensions
    {
        public static IApplicationBuilder AddSpecialPage<T>(this IApplicationBuilder app, IWebHostEnvironment environment, IServiceCollection services)
            where T : SpecialPage, new()
        {
            T page = new T();
            app.Map(page.Route, builder => builder.Run(async context => await page.Render(context, environment, services)));

            return app;
        }
    }
}
