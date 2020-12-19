namespace RestCRUD.Settings
{
    using Microsoft.Extensions.DependencyInjection;
    using RestCRUD.Core;
    using RestCRUD.Core.Repository;

    public static class FormGeneratorServiceServiceCollectionExtension
    {
        public static void AddRestCrudGenerator(IServiceCollection services, IFormGeneratorComponentsRepository repository = null, IFormGeneratorOptions options = null)
        {
            if (repository == null)
                throw new System.Exception("No repository provided, please refer to the documentation.");

            if (options == null)
                throw new System.Exception("No options provided, please refer to the documentation.");


            services.AddSingleton(typeof(IFormGeneratorComponentsRepository), repository);
            services.AddSingleton(typeof(IFormGeneratorOptions), options);
        }
    }
}
