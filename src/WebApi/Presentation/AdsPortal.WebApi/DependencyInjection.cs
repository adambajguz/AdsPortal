namespace AdsPortal.WebApi
{
    using System;
    using System.IO.Compression;
    using System.Net.Mime;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using AdsPortal.WebApi.Configurations;
    using AdsPortal.WebApi.Converters;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.ResponseCompression;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Converters;

    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

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

        public static IMvcBuilder AddMvcSerializer(this IMvcBuilder mvcBuilder)
        {
            if (FeaturesSettings.UseNewtonsoftJson)
            {
                mvcBuilder.AddNewtonsoftJson(cfg =>
                {
                    cfg.SerializerSettings.Converters.Add(new StringEnumConverter());
                });
            }
            else
            {
                mvcBuilder.AddJsonOptions(cfg =>
                {
                    cfg.JsonSerializerOptions.Converters.Add(new JsonTimeSpanConverter());
                    cfg.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            }

            return mvcBuilder;
        }

        public static IMvcBuilder AddValidation(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(c => c.RegisterValidatorsFromAssemblies(new Assembly[]
                {
                    typeof(AdsPortal.Application.DependencyInjection).GetTypeInfo().Assembly,
                    typeof(Domain.DependencyInjection).GetTypeInfo().Assembly
                }));

            return mvcBuilder;
        }
    }
}
