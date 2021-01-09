namespace AdsPortal.WebApi
{
    using System;
    using System.IO;
    using System.Net.Mime;
    using System.Reflection;
    using System.Threading.Tasks;
    using AdsPortal.Infrastructure;
    using AdsPortal.Persistence;
    using AdsPortal.Shared.Extensions.Logging;
    using AdsPortal.WebApi.Application;
    using AdsPortal.WebApi.Application.Interfaces.JobScheduler;
    using AdsPortal.WebApi.Application.Jobs;
    using AdsPortal.WebApi.Exceptions.Handler;
    using AdsPortal.WebApi.Grpc;
    using AdsPortal.WebApi.Infrastructure;
    using AdsPortal.WebApi.Infrastructure.Identity;
    using AdsPortal.WebApi.Infrastructure.JobScheduler;
    using AdsPortal.WebApi.Infrastructure.Mailing;
    using AdsPortal.WebApi.Rest;
    using AdsPortal.WebApi.SpecialPages.Core;
    using FluentValidation;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
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

            ValidatorOptions.Global.LanguageManager.Enabled = false;
            //ValidatorOptions.LanguageManager.Culture = new CultureInfo("en");

            logger.LogInformation("FluentValidation's support for localization disabled. Default English messages are forced to be used, regardless of the thread's CurrentUICulture.");
        }

        // This method gets called by the runtime. Use it to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string emailTemplatesRootFolder = Path.GetFullPath("EmailTemplates");

            services.AddInfrastructureLayer(Configuration)
                    .AddInfrastructureIdentityLayer(Configuration)
                    .AddInfrastructureMediaLayer(Configuration)
                    .AddInfrastructureJobSchedulerLayer(Configuration)
                    .AddMailing(Configuration, emailTemplatesRootFolder);

            services.AddPersistenceLayer(Configuration)
                    .AddApplicationLayer(LoggerFactory, Configuration)
                    .AddWebApi()
                    .AddRestApi()
                    .AddGrpcApi();

            services.AddMvc()
                    .AddMvcSerializer()
                    .AddValidation();

            services.AddHealthChecks()
                    .AddPersistenceHealthChecks();

            //Mvc
            services.AddControllers()
                    .SetCompatibilityVersion(CompatibilityVersion.Latest)
                    .AddApplicationPart(typeof(Rest.DependencyInjection).Assembly).AddControllersAsServices();

            _services = services;
        }

        // This method gets called by the runtime. Use it to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = async (ctx) => await CustomExceptionHandler.HandleExceptionAsync(ctx)
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

            //app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRouting()
               .UseResponseCompression()
               .UseCors("AllowAll");

            app.UseStatusCodePages(StatusCodePageRespone);

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

            if (false)
            {
                using (IServiceScope scope = app.ApplicationServices.CreateScope())
                {
                    IJobSchedulingService x = scope.ServiceProvider.GetRequiredService<IJobSchedulingService>();

                    for (int i = 0; i < 500; ++i)
                    {
                        if (i % 100 == 0)
                            Console.WriteLine($"Added {i}");

                        x.Schedule<TestJob>().Wait();
                    }

                    for (int i = 0; i < 500; ++i)
                    {
                        if (i % 100 == 0)
                            Console.WriteLine($"Added {i}");

                        x.Schedule<TestJob>(1).Wait();
                    }
                }
            }
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
