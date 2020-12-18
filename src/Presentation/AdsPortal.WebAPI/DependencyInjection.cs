namespace AdsPortal.WebAPI
{
    using System.Reflection;
    using System.Text.Json.Serialization;
    using AdsPortal.WebAPI.Configurations;
    using AdsPortal.WebAPI.Converters;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Converters;

    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services.AddSwagger();

            services.AddDistributedMemoryCache();

            return services;
        }

        public static IApplicationBuilder ConfigureWebApi(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureSwagger();

            return app;
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
                                                    typeof(Application.DependencyInjection).GetTypeInfo().Assembly,
                                                    typeof(Domain.DependencyInjection).GetTypeInfo().Assembly
                                                }));

            return mvcBuilder;
        }
    }
}
