namespace AdsPortal
{
    using System.Net.Mime;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.JobScheduler;
    using AdsPortal.Infrastructure.Identity;
    using AdsPortal.Infrastructure.JobScheduler;
    using AdsPortal.Persistence;
    using AdsPortal.Shared.Extensions.Logging;
    using AdsPortal.SpecialPages.Core;
    using AdsPortal.WebAPI;
    using AdsPortal.WebAPI.Configurations;
    using AdsPortal.WebAPI.Exceptions.Handler;
    using Application;
    using FluentValidation;
    using Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.HttpOverrides;
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

            ValidatorOptions.Global.LanguageManager.Enabled = false;
            //ValidatorOptions.LanguageManager.Culture = new CultureInfo("en");

            logger.LogInformation("FluentValidation's support for localization disabled. Default English messages are forced to be used, regardless of the thread's CurrentUICulture.");
        }

        // This method gets called by the runtime. Use it to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructureLayer(Configuration)
                    .AddInfrastructureIdentityLayer(Configuration)
                    .AddInfrastructureMediaLayer(Configuration)
                    .AddInfrastructureJobSchedulerLayer(Configuration);

            services.AddPersistenceLayer(Configuration)
                    .AddApplicationLayer(LoggerFactory)
                    .AddWebApi();

            services.AddMvc()
                    .AddMvcSerializer()
                    .AddValidation();

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

            if (env.IsDevelopment())
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

            app.UseSerilogRequestLogging();

            app.UseRouting()
               .UseResponseCompression()
               .UseCors("AllowAll")
               .UseStatusCodePages(StatusCodePageRespone);

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

            app.ConfigureWebApi(env);

            if (false)
            {
                using (IServiceScope scope = app.ApplicationServices.CreateScope())
                {
                    IJobSchedulingService x = scope.ServiceProvider.GetRequiredService<IJobSchedulingService>();

                    for (int i = 0; i < 500; ++i)
                    {
                        if (i % 100 == 0)
                            System.Console.WriteLine($"Added {i}");

                        x.Schedule<Application.Jobs.TestJob>().Wait();
                    }

                    for (int i = 0; i < 500; ++i)
                    {
                        if (i % 100 == 0)
                            System.Console.WriteLine($"Added {i}");

                        x.Schedule<Application.Jobs.TestJob>(1).Wait();
                    }

                    for (int i = 0; i < 200; ++i)
                    {
                        if (i % 100 == 0)
                            System.Console.WriteLine($"Added {i}");

                        x.Schedule<Application.Jobs.TestJob>(2).Wait();
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
                              $"1.0.0\n\n" +
                              $"Status code: {httpRespone.StatusCode} - {reasonPhrase}";

            await httpRespone.WriteAsync(response);
        }
    }
}
