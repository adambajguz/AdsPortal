namespace AdsPortal.WebApi.SpecialPages.Core
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public abstract class SpecialPage
    {
        public abstract string Route { get; }

        public SpecialPage()
        {

        }

        public abstract Task Render(HttpContext httpContext, IWebHostEnvironment environment, IServiceCollection services);
    }
}
