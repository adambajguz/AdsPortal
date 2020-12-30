namespace RestCRUD
{
    using Microsoft.Extensions.DependencyInjection;
    using RestCRUD.Core.Repository;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestCrudGenerator(this IServiceCollection services,
                                                              IFormGeneratorComponentsRepository repository,
                                                              IFormGeneratorOptions options)
        {
            services.AddSingleton(typeof(IFormGeneratorComponentsRepository), repository);
            services.AddSingleton(typeof(IFormGeneratorOptions), options);

            return services;
        }
    }
}
