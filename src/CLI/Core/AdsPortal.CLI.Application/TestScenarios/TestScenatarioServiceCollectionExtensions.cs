namespace AdsPortal.CLI.Application.TestScenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public static class TestScenatarioServiceCollectionExtensions
    {
        public static IServiceCollection AddAllTestScenariosAsTransient(this IServiceCollection services)
        {
            IEnumerable<Type>? scenarios = GetScenarios(typeof(TestScenatarioServiceCollectionExtensions).Assembly);

            foreach (var s in scenarios)
            {
                services.AddTransient(typeof(ITestScenario), s);
            }

            return services;
        }

        private static IEnumerable<Type> GetScenarios(Assembly rootAssembly)
        {
            Type[] types = rootAssembly.GetExportedTypes();

            IEnumerable<Type>? withCustomMappings = from type in types
                                                    from instance in type.GetInterfaces()
                                                    where typeof(ITestScenario).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface
                                                    select type;

            return withCustomMappings.Distinct();
        }
    }
}
