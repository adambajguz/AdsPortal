namespace AdsPortal.WebApi.Rest.Configurations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AdsPortal.WebApi.Rest.Configurations.SwaggerFilters;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Serilog;
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

            app.UseSwaggerUI(c =>
            {
                c.DisplayRequestDuration();
                //c.EnableValidator();
                c.ShowExtensions();
                c.RoutePrefix = "api";
                c.SwaggerEndpoint("/api/v1/swagger.json", "AdsPortal");
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
                {
                    Log.ForContext(typeof(SwaggerConfiguration)).Error("Custom SwaggerUI index.html stream is null for name: {Name}. Using default.", name);
                }
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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "1.0.0",
                    Title = "AdsPortal",
                    Description = $"Backend Api for AdsPortal<br>" +
                                  $"© AdsPortal — 2020<br>" +
                                  "<hr>" +
                                  "<p>" +
                                  "<b>Links:</b><br>" +
                                  @$"OpenAPI specification can be found at <a href=""/api/v1/swagger.json"">/api/v1/swagger.json</a><br>" +
                                  @$"Swagger can be accessed through <a href=""/api/index.html"">/api/index.html</a><br>" +
                                  @$"<br>" +
                                  @$"<a href=""https://localhost:5001"">Go back to Portal</a><br>" +
                                  @$"<br>" +
                                  @$"REST API base url is <a href=""/api"">/api</a><br>" +
                                  @$"<br>" +
                                  @$"App health can be checked under <a href=""/health"">/health</a><br>" +
                                  "</p>"
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

            //if (FeaturesSettings.UseNewtonsoftJson)
            services.AddSwaggerGenNewtonsoftSupport();
            //else
            //    services.AddSwaggerGen();
        }
    }
}
