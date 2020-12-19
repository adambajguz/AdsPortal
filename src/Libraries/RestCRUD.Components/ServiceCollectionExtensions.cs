namespace RestCRUD.Components
{
    using RestCRUD.Settings;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void AddRestCRUD(this IServiceCollection services, CustomComponentsRepository repository = null, CustomFormOptions options = null)
        {
            FormGeneratorServiceServiceCollectionExtension.AddRestCrudGenerator(services, repository ?? new CustomComponentsRepository(), options ?? new CustomFormOptions());
        }
    }
}
