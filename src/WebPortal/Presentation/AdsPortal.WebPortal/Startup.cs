namespace AdsPortal.WebPortal
{
    using System;
    using System.IO.Compression;
    using System.Net.Mime;
    using AdsPortal.Shared.Extensions.Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.ResponseCompression;
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
            //WebHostBuilder b = new();
            //var x = b.GetSetting(WebHostDefaults.ContentRootKey);

            services.AddWebPortal(Configuration);

            services.AddRazorPages();
            services.AddServerSideBlazor(c =>
            {
                c.DetailedErrors = Environment.IsDevelopment();
            });

            //ReverseProxy configuration
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                //options.KnownProxies.Add(IPAddress.Parse("52.143.144.236"));
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
                //options.ExcludedHosts.Add("example.com");
                //options.ExcludedHosts.Add("www.example.com");
            });

            //if (GlobalAppConfig.DEV_MODE)//(_env.IsDevelopment())
            //{
            //    services.AddHttpsRedirection(options =>
            //    {
            //        options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //        options.HttpsPort = 5001;
            //    });
            //}
            //else
            //{
            //    services.AddHttpsRedirection(options =>
            //    {
            //        options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            //        options.HttpsPort = 443;
            //    });
            //}

            //Mvc
            IMvcBuilder mvcBuilder = services.AddMvc()
                                             .AddControllersAsServices()
                                             .AddTagHelpersAsServices()
                                             .AddViewComponentsAsServices()
                                             .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;

                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();

                options.MimeTypes = new[]
                {
                    // General
                    MediaTypeNames.Text.Plain,

                    // Static files
                    "text/css",
                    "application/javascript",
                    "font/woff2",

                    // MVC
                    MediaTypeNames.Text.Html,
                    MediaTypeNames.Application.Xml,
                    MediaTypeNames.Text.Xml,
                    MediaTypeNames.Application.Json,
                    "text/json",

                    // WebAssembly
                    "application/wasm",

                    // Images
                    "image/*"

                    // Other
                    //MediaTypeNames.Application.Pdf,
                    //MediaTypeNames.Application.Rtf
                };
                options.ExcludedMimeTypes = new[]
                {
                    "audio/*",
                    "video/*"
                };
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            //Cors
            services.AddCors(options => //TODO: Change cors only to our server
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        //  .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                        // .AllowCredentials();
                    });
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
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
