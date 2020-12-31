namespace AdsPortal.WebApi.SpecialPages
{
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.SpecialPages.Core;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class RegisteredServicesPage : SpecialPage
    {
        public override string Route { get; } = "/services";

        public RegisteredServicesPage()
        {

        }

        public override async Task Render(HttpContext httpContext, IWebHostEnvironment environment, IServiceCollection services)
        {
            StringBuilder sb = new StringBuilder();
            sb.BeginHTML();
            sb.Append("<h1>Registered Services</h1>");

            ServiceDescriptor[] servicesCopy = services.ToArray();

            AddServicesTable(servicesCopy, sb, ServiceLifetime.Singleton);
            AddServicesTable(servicesCopy, sb, ServiceLifetime.Scoped);
            AddServicesTable(servicesCopy, sb, ServiceLifetime.Transient);

            sb.EndHTML();

            await httpContext.Response.WriteAsync(sb.ToString());
        }

        private static void AddServicesTable(ServiceDescriptor[] services, StringBuilder sb, ServiceLifetime lifetime)
        {
            IOrderedEnumerable<ServiceDescriptor> tmp = services.Where(x => x.Lifetime == lifetime).OrderBy(x => x.ServiceType.FullName);

            sb.Append($"<h2>{lifetime} ({tmp.Count()})</h2>");

            sb.Append("<table border=\"1\"><thead>");

            sb.Append("<th>Service type</th>");
            sb.Append("<th>Lifetime</th>");
            sb.Append("<th>Implementation type</th>");

            sb.Append("</thead><tbody>");

            foreach (ServiceDescriptor x in tmp)
            {
                sb.Append("<tr>");
                sb.Append($"<td>{x.ServiceType.FullName}</td>");
                sb.Append($"<td>{x.Lifetime}</td>");
                sb.Append($"<td>{x.ImplementationType?.FullName}</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody></table>");
        }
    }
}
