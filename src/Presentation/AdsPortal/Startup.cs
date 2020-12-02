namespace AdsPortal
{
    using System.Net.Mime;
    using System.Threading.Tasks;
    using AdsPortal.Analytics;
    using AdsPortal.Common;
    using AdsPortal.Domain;
    using AdsPortal.Infrastructure.Common;
    using AdsPortal.Infrastructure.Identity;
    using AdsPortal.Infrastructure.JobScheduler;
    using AdsPortal.Infrastructure.Logging;
    using AdsPortal.ManagementUI;
    using AdsPortal.SpecialPages.Core;
    using AdsPortal.WebAPI;
    using AdsPortal.WebAPI.Configuration;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Application;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Persistence;
    using Serilog;

    public class Startup
    {
        private IServiceCollection? _services;

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        public ILoggerFactory Logger { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILoggerFactory logger)
        {
            Configuration = configuration;
            Environment = environment;
            Logger = logger;
        }

        // This method gets called by the runtime. Use it to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDomainLayer()
                    .AddInfrastructureLogging()
                    .AddInfrastructureCrossCuttingLayer()
                    .AddInfrastructureLayer(Configuration)
                    .AddInfrastructureIdentityLayer(Configuration)
                    .AddInfrastructureMediaLayer(Configuration)
                    .AddInfrastructureJobSchedulerLayer(Configuration);

            services.AddPersistenceLayer(Configuration)
                    .AddApplicationLayer()
                    .AddMvc()
                    .AddManagementUI();

            services.AddHealthChecks()
                    .AddPersistenceHealthChecks();

            _services = services;
        }

        // This method gets called by the runtime. Use it to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (FeaturesSettings.AlwaysUseExceptionHandling)
                app.UseExceptionHandler(error => error.UseCustomErrors());

            if (GlobalAppConfig.IsDevMode)
            {
                if (!FeaturesSettings.AlwaysUseExceptionHandling)
                    app.UseDeveloperExceptionPage();
            }
            else
            {
                if (!FeaturesSettings.AlwaysUseExceptionHandling)
                    app.UseExceptionHandler(error => error.UseCustomErrors());

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            if (GlobalAppConfig.IsDevMode)
            {
                app.UseAnalytics();
                app.UseSerilogRequestLogging();

                app.UseStaticFiles();
            }
            else
            {
                app.UseAnalytics();
                app.UseStaticFiles();
                app.UseSerilogRequestLogging(); //Logging after static files to prevent showing static files in logs
            }

            app.ConfigureManagementUI();

            app.UseRouting()
               .UseResponseCompression()
               .UseCors("AllowAll")
               .UseStatusCodePages(StatusCodePageRespone);

            app.UseAuthentication()
               .UseAuthorization();
            //app.UseSession();

            app.ConfigureSpecialPages(Environment, _services)
               .UseHealthChecks(GlobalAppConfig.AppInfo.HealthUrl);

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/health");
                endpoints.MapManagementUI();
                endpoints.MapControllers();
            });

            app.ConfigureWebApi(env);

            //using (IServiceScope scope = app.ApplicationServices.CreateScope())
            //{
            //    IJobSchedulingService x = scope.ServiceProvider.GetRequiredService<IJobSchedulingService>();

            //    for (int i = 0; i < 500; ++i)
            //    {
            //        if(i % 100 == 0)
            //            System.Console.WriteLine($"Added {i}");

            //        x.Schedule<Application.Jobs.TestJob>().Wait();
            //    }
            //}
        }

        private static async Task StatusCodePageRespone(StatusCodeContext statusCodeContext)
        {
            HttpResponse httpRespone = statusCodeContext.HttpContext.Response;
            httpRespone.ContentType = MediaTypeNames.Text.Plain;

            string reasonPhrase = ReasonPhrases.GetReasonPhrase(httpRespone.StatusCode);

            string response = $"{GlobalAppConfig.AppInfo.AppName} Error Page\n" +
                              $"{GlobalAppConfig.AppInfo.AppVersionText}\n\n" +
                              $"Status code: {httpRespone.StatusCode} - {reasonPhrase}";

            await httpRespone.WriteAsync(response);
        }
    }
}
