namespace AdsPortal.WebApi
{
    using System;
    using System.IO;
    using System.Net.Mime;
    using System.Reflection;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.Shared.Extensions.Logging;
    using AdsPortal.WebApi.Application;
    using AdsPortal.WebApi.Exceptions.Handler;
    using AdsPortal.WebApi.Grpc;
    using AdsPortal.WebApi.Infrastructure;
    using AdsPortal.WebApi.Infrastructure.Identity;
    using AdsPortal.WebApi.Infrastructure.JobScheduler;
    using AdsPortal.WebApi.Infrastructure.Mailing;
    using AdsPortal.WebApi.Infrastructure.Media;
    using AdsPortal.WebApi.Persistence;
    using AdsPortal.WebApi.Rest;
    using AdsPortal.WebApi.Services;
    using AdsPortal.WebApi.SpecialPages.Core;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public class Startup
    {
        private IServiceCollection? _services;

        public Microsoft.Extensions.Logging.ILogger Logger { get; }
        public ILoggerFactory LoggerFactory { get; }
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(ILogger<Startup> logger, ILoggerFactory logerFactory, IWebHostEnvironment environment, IConfiguration configuration)
        {
            Logger = logger;
            LoggerFactory = logerFactory;
            SharedLogger.LoggerFactory = logerFactory;

            Environment = environment;
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use it to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string emailTemplatesRootFolder = Path.GetFullPath("EmailTemplates");

            services.AddPersistenceLayer(Configuration); //Persistence layer must be always registered first because hosted services (db migration) will be executed at startup in the same order they are added to the DI container

            services.AddInfrastructureLayer(Configuration)
                    .AddInfrastructureIdentityLayer(Configuration)
                    .AddInfrastructureMediaLayer(Configuration)
                    .AddInfrastructureJobSchedulerLayer(Configuration)
                    .AddMailing(Configuration, emailTemplatesRootFolder);

            services.AddApplicationLayer(LoggerFactory, Configuration)
                    .AddWebApi()
                    .AddRestApi()
                    .AddGrpcApi();

            services.AddMvc()
                    .AddControllersAsServices()
                    .AddMvcSerializer()
                    .AddValidation();

            services.AddHealthChecks()
                    .AddPersistenceHealthChecks();

            //Mvc
            services.AddControllers()
                    .SetCompatibilityVersion(CompatibilityVersion.Latest)
                    .AddApplicationPart(typeof(Rest.DependencyInjection).Assembly).AddControllersAsServices();

            if (Environment.IsDevelopment())
            {
                services.AddMultipleInstanceHostedService<DeveloperSandbox>();
            }

            _services = services;
        }

        // This method gets called by the runtime. Use it to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            //app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseStatusCodePages(StatusCodePageRespone);

            //app.UseExceptionHandler(new ExceptionHandlerOptions
            //{
            //    ExceptionHandlingPath = PathString.Empty,
            //    AllowStatusCode404Response = true,
            //    ExceptionHandler = CustomExceptionHandler.HandleExceptionAsync
            //});
            app.UseCustomExceptionHandlerMiddleware(); //Custom middleware because UseExceptionHandler is broken - logs exception that was handled

            app.UseRouting()
               .UseResponseCompression()
               .UseCors("AllowAll");

            app.UseAuthentication()
               .UseAuthorization();
            //app.UseSession();

            app.ConfigureSpecialPages(Environment, _services)
               .UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });

            app.ConfigureRestApi(env)
               .ConfigureGrpcApi(env);
        }

        private async Task StatusCodePageRespone(StatusCodeContext statusCodeContext)
        {
            HttpResponse httpRespone = statusCodeContext.HttpContext.Response;
            httpRespone.ContentType = MediaTypeNames.Text.Plain;

            string reasonPhrase = ReasonPhrases.GetReasonPhrase(httpRespone.StatusCode);

            string response = $"{Configuration.GetValue<string>("Application:Name")} Error Page\n" +
                              $"{Assembly.GetEntryAssembly()?.GetName()?.Version ?? new Version(0, 0, 0, 0)}\n\n" +
                              $"Status code: {httpRespone.StatusCode} - {reasonPhrase}";

            await httpRespone.WriteAsync(response);
        }
    }
}
