namespace AdsPortal.WebPortal
{
    using System;
    using System.IO.Compression;
    using System.Net.Mime;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddWebPortal(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

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
            IMvcBuilder mvcBuilder = services.AddControllers()
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

            return services;
        }
    }
}
