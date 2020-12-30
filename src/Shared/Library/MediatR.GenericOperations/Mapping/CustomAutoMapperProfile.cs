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
            IEnumerable<Type> types = GetCustomMappings(rootAssembly);

            int count = 0;
            bool anyEmpty = false;
            IEnumerable<ITypeMapConfiguration> typeMapConfigs = (this as IProfileConfiguration).TypeMapConfigs;
            foreach (Type type in types)
            {
                _logger.LogDebug("Registering mappings from configuration in {Type}", type);

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

            _logger.LogInformation($"Registered {{Count}} maps from {{ICustomMappingTypesCount}} classes implementing {nameof(ICustomMapping)} for {{Assembly}}", typeMapConfigs.Count(), types.Count(), rootAssembly);
            if (anyEmpty)
            {
                _logger.LogWarning($"At least one class implementing {nameof(ICustomMapping)} does not contain any mapping definitions in {{Assembly}}.", rootAssembly);
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
