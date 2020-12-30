namespace AdsPortal.Shared.Extensions.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfiguration<TOptions>(this IServiceCollection services, IConfiguration configuration, string? overrideSectionName = null)
            where TOptions : class
        {
            string sectionName = overrideSectionName ?? GetFallbackName<TOptions>();

            IConfigurationSection section = configuration.GetSection(sectionName);
            services.Configure<TOptions>(section);

            return services;
        }

        public static IServiceCollection AddConfiguration<TOptions>(this IServiceCollection services, IConfiguration configuration, out TOptions options, string? overrideSectionName = null)
            where TOptions : class
        {
            string sectionName = overrideSectionName ?? GetFallbackName<TOptions>();

            IConfigurationSection section = configuration.GetSection(sectionName);
            services.Configure<TOptions>(section);

            options = section.Get<TOptions>();

            return services;
        }

        public static TOptions GetValue<TOptions>(this IConfiguration configuration, string? overrideSectionName = null)
            where TOptions : class
        {
            string sectionName = overrideSectionName ?? GetFallbackName<TOptions>();

            IConfigurationSection section = configuration.GetSection(sectionName);
            TOptions options = section.Get<TOptions>();

            return options;
        }

        private static string GetFallbackName<TOptions>() where TOptions : class
        {
            return typeof(TOptions).Name.Replace("Configuration", string.Empty);
        }
    }
}
