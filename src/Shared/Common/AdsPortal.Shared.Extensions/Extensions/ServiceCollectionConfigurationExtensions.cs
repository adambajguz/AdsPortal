namespace AdsPortal.Shared.Extensions.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionConfigurationExtensions
    {
        public static IServiceCollection AddConfiguration<TOptions>(this IServiceCollection services, IConfiguration configuration, string? overrideSectionName = null)
            where TOptions : class
        {
            string sectionName = overrideSectionName ?? GetFallbackName<TOptions>();

            IConfigurationSection section = configuration.GetSection(sectionName);
            services.Configure<TOptions>(section);

            return services;
        }

        public static IServiceCollection AddConfiguration<TOptions>(this IServiceCollection services, IConfiguration configuration, [NotNull] out TOptions options, string? overrideSectionName = null)
            where TOptions : class
        {
            string sectionName = overrideSectionName ?? GetFallbackName<TOptions>();

            IConfigurationSection section = configuration.GetSection(sectionName);
            services.Configure<TOptions>(section);

            options = section.Get<TOptions>() ?? throw new NullReferenceException($"Invalid configuration with section name '{sectionName}'.");

            return services;
        }

        public static TOptions GetConfiguration<TOptions>(this IConfiguration configuration, string? overrideSectionName = null)
            where TOptions : notnull
        {
            string sectionName = overrideSectionName ?? GetFallbackName<TOptions>();

            IConfigurationSection section = configuration.GetSection(sectionName);
            TOptions options = section.Get<TOptions>() ?? throw new NullReferenceException($"Invalid configuration with section name '{sectionName}'.");

            return options;
        }

        private static string GetFallbackName<TOptions>()
            where TOptions : notnull
        {
            return typeof(TOptions).Name.Replace("Configuration", string.Empty);
        }
    }
}
