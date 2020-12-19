namespace RestCRUD.Settings.Plain
{
    using Microsoft.Extensions.DependencyInjection;
    using RestCRUD.Repository.Plain;

    public static class ServiceCollectionExtensions
    {
        public static void AddRestCRUD(this IServiceCollection services, VxComponentsRepository repository = null, VxFormOptions options = null)
        {
            FormGeneratorServiceServiceCollectionExtension.AddVxFormGenerator(services, repository ?? new VxComponentsRepository(), options ?? new VxFormOptions());
        }
    }
}
