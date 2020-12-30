namespace RestCRUD.Components
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRestCRUD(this IServiceCollection services,
                                                     CustomComponentsRepository? repository = null,
                                                     CustomFormOptions? options = null)
        {
            services.AddRestCrudGenerator(repository ?? new CustomComponentsRepository(), options ?? new CustomFormOptions());

            return services;
        }
    }
}
