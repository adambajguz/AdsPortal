namespace AdsPortal.WebPortal
{
    using AdsPortal.Shared.Extensions.Extensions;
    using AdsPortal.WebPortal.Configurations;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Services;
    using AdsPortal.WebPortal.Services.Auth;
    using AdsPortal.WebPortal.Shared.Components.OperationRenderers;
    using Blazored.SessionStorage;
    using MagicOperations;
    using MagicOperations.Components.OperationRenderers;
    using MagicOperations.Interfaces;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.Server;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddWebPortal(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<ManagementUIConfiguration>(configuration)
                    .AddConfiguration<HeaderConfiguration>(configuration)
                    .AddConfiguration<FooterConfiguration>(configuration)
                    .AddConfiguration<ApplicationConfiguration>(configuration, out ApplicationConfiguration applicationConfiguration);

            services.AddScoped<IMarkdownService, MarkdownService>();
            services.AddScoped<IMediaService, MediaService>();

            services.AddHttpContextAccessor();
            //services.AddHttpClient();

            services.AddMagicOperations((builder) =>
            {
                builder.ModelsBuilder.AddRenderableClassesFromThisAssembly();

                builder.UseBaseUri(applicationConfiguration.ApiUrl ?? string.Empty);
                builder.AddOperationsFromThisAssembly();

                builder.UseDefaultOperationRenderer(typeof(GetPagedOperationRenderer<,>), typeof(GetPagedRenderer<,>));
                builder.UseDefaultOperationRenderer(typeof(LoginOperationRenderer<,>), typeof(LoginRenderer<,>));

                builder.AddGroupConfiguration(OperationGroups.Advertisement, (g) =>
                {
                    g.Path = "advertisement";
                    g.DisplayName = "Advertisement";
                });

                builder.AddGroupConfiguration(OperationGroups.Category, (g) =>
                {
                    g.Path = "category";
                    g.DisplayName = "Category";
                });

                builder.AddGroupConfiguration(OperationGroups.User, (g) =>
                {
                    g.Path = "user";
                    g.DisplayName = "User";
                });
            });

            services.AddBlazoredSessionStorage();

            //services.AddAuthorizationCore();
            services.AddAuthentication();

            //services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>();
            services.AddScoped<IHostEnvironmentAuthenticationStateProvider>(sp =>
            {
                // this is safe because the `RevalidatingIdentityAuthenticationStateProvider` extends the `ServerAuthenticationStateProvider`
                var provider = (ServerAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>();
                return provider;
            });

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenManagerService, TokenManagerService>();

            return services;
        }
    }
}
