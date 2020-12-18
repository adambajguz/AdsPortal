namespace AdsPortal.WebAPI.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AdsPortal.Common;
    using AdsPortal.WebAPI.Configuration.SwaggerFilters;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerUI;

    public static class SwaggerConfiguration
    {
        //TODO: https://swagger.io/blog/api-documentation/building-a-documentation-portal-for-multiple-apis/
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = false;
                c.RouteTemplate = "api/{documentName}/swagger.json";
            });

            app.UseReDoc(c =>
            {
                c.RoutePrefix = GlobalAppConfig.AppInfo.ReDocRoute;
                c.SpecUrl(GlobalAppConfig.AppInfo.SwaggerStartupUrl);
            });

            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                //c.EnableValidator();
                c.ShowExtensions();
                c.RoutePrefix = GlobalAppConfig.AppInfo.SwaggerRoute;
                c.SwaggerEndpoint(GlobalAppConfig.AppInfo.SwaggerStartupUrl, GlobalAppConfig.AppInfo.AppNameWithVersion);
                c.OverrideIndexStream();
            });

            return app;
        }

        private static void OverrideIndexStream(this SwaggerUIOptions c, string resourceFileName = "index")
        {
            Assembly? assembly = MethodBase.GetCurrentMethod()?.DeclaringType?.Assembly;
            if (assembly is null)
            {
                Log.ForContext(typeof(SwaggerConfiguration)).Error("Custom SwaggerUI index.html cannot be retrived. Using default.");
            }
            else
            {
                string? ns = assembly.GetName().Name;
                string name = $"{ns}.{resourceFileName}.html";

                if (assembly.GetManifestResourceNames().Contains(name))
                {
                    Log.ForContext(typeof(SwaggerConfiguration)).Information("Custom SwaggerUI index.html stream will be used: {Name}.", name);
                    c.IndexStream = () => assembly.GetManifestResourceStream(name);
                }
                else
                    Log.ForContext(typeof(SwaggerConfiguration)).Error("Custom SwaggerUI index.html stream is null for name: {Name}. Using default.", name);
            }
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.OperationFilter<AuthResponsesOperationFilter>(); //TODO fix
                //c.OperationFilter<FileUploadOperationFilter>(); //TODO fix

                c.AddFluentValidationRules();
                c.SwaggerDoc(GlobalAppConfig.AppInfo.SwaggerDocumentName, new OpenApiInfo
                {
                    Version = GlobalAppConfig.AppInfo.AppVersionText,
                    Title = GlobalAppConfig.AppInfo.AppName,
                    Description = GlobalAppConfig.AppInfo.AppDescriptionHTML
                });

                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, //Name the security scheme
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. Use /api/login endpoint to retrive a token.",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme, //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            if (FeaturesSettings.UseNewtonsoftJson)
                services.AddSwaggerGenNewtonsoftSupport();
            else
                services.AddSwaggerGen();
        }
    }
}
