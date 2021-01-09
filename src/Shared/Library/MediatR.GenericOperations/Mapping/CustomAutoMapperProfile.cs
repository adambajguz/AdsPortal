namespace MediatR.GenericOperations.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using AutoMapper.Configuration;
    using Microsoft.Extensions.Logging;

    public class CustomAutoMapperProfile : Profile
    {
        private readonly ILogger _logger;

        public CustomAutoMapperProfile(Assembly assembly, ILogger logger)
        {
            _logger = logger;
            LoadCustomMappings(assembly);
        }

        private void LoadCustomMappings(Assembly rootAssembly)
        {
            IReadOnlyList<Type> types = GetCustomMappings(rootAssembly).ToList();

            if (types.Count == 0)
            {
                _logger.LogDebug("Skipping registering mappings in {Assembly}", rootAssembly);
                return;
            }

            int count = 0;
            bool anyEmpty = false;
            IEnumerable<ITypeMapConfiguration> typeMapConfigs = (this as IProfileConfiguration).TypeMapConfigs;

            _logger.LogDebug("Registering mappings for {Count} types implementing {Interface} in {Assembly} {Types}", types.Count, nameof(ICustomMapping), rootAssembly, types);

            foreach (Type type in types)
            {
                ICustomMapping? map = Activator.CreateInstance(type) as ICustomMapping;
                map?.CreateMappings(this);

                int v = typeMapConfigs.Count();
                if (v == count)
                {
                    anyEmpty = true;
                    _logger.LogWarning("{Type} does not contain any mapping definitions", type);
                }
                count = v;
            }

            _logger.LogInformation("Registered {Count} maps from {ICustomMappingTypesCount} classes implementing {Interface} for {Assembly}", typeMapConfigs.Count(), types.Count, nameof(ICustomMapping), rootAssembly);
            if (anyEmpty)
            {
                _logger.LogWarning("At least one class implementing {Interface} does not contain any mapping definitions in {Assembly}", nameof(ICustomMapping), rootAssembly);
            }
        }

        private static IEnumerable<Type> GetCustomMappings(Assembly rootAssembly)
        {
            Type[] types = rootAssembly.GetExportedTypes();

            IEnumerable<Type>? withCustomMappings = from type in types
                                                    from instance in type.GetInterfaces()
                                                    where typeof(ICustomMapping).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface
                                                    select type;

            return withCustomMappings.Distinct();
        }
    }
}
