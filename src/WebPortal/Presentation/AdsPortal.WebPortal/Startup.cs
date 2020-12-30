namespace AdsPortal.WebPortal
{
    using AdsPortal.Shared.Extensions.Logging;
    using AdsPortal.WebPortal.Application;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;

    public class Startup
    {
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebPortal()
                    .AddApplicationLayer(Configuration);

            services.AddRazorPages();
            services.AddServerSideBlazor(c =>
            {
                c.DetailedErrors = Environment.IsDevelopment();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            if (env.IsDevelopment())
            {
                app.UseSerilogRequestLogging();
                app.UseStaticFiles();
            }
            else
            {
                app.UseStaticFiles();
                app.UseSerilogRequestLogging(); //Logging after static files to prevent showing static files in logs
            }

            app.UseRouting();

            app.UseResponseCompression()
               .UseCors("AllowAll");

            app.UseAuthentication()
               .UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
