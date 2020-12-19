namespace MagicCRUD
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection MagicCRUD(this IServiceCollection services, Action<MagicCRUDBuilder> builder)
        {
            return services;
        }
    }
}
