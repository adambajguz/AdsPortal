namespace AdsPortal.Application.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using AutoMapper.Configuration;
    using AdsPortal.Domain.Mapping;
    using Serilog;

    public class CustomAutoMapperProfile : Profile
    {
        public CustomAutoMapperProfile()
        {
            Assembly assembly = typeof(CustomAutoMapperProfile).Assembly;

            LoadCustomMappings(assembly);
        }

        public CustomAutoMapperProfile(Assembly assembly)
        {
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
                Log.ForContext<CustomAutoMapperProfile>().Debug("Registering mappings from configuration in {Type}", type);

                ICustomMapping? map = Activator.CreateInstance(type) as ICustomMapping;
                map?.CreateMappings(this);

                int v = typeMapConfigs.Count();
                if (v == count)
                {
                    anyEmpty = true;
                    Log.ForContext<CustomAutoMapperProfile>().Warning("{Type} does not contain any mapping definitions", type);
                }
                count = v;
            }

            Log.ForContext<CustomAutoMapperProfile>().Information($"Registered {{Count}} maps from {{ICustomMappingTypesCount}} classes implementing {nameof(ICustomMapping)} for {{Assembly}}", typeMapConfigs.Count(), types.Count(), rootAssembly);
            if (anyEmpty)
            {
                Log.ForContext<CustomAutoMapperProfile>().Warning($"At least one class implementing {nameof(ICustomMapping)} does not contain any mapping definitions in {{Assembly}}.", rootAssembly);
            }
        }

        private static IEnumerable<Type> GetCustomMappings(Assembly rootAssembly)
        {
            Type[] types = rootAssembly.GetExportedTypes();

            IEnumerable<Type>? withCustomMappings = (from type in types
                                                     from instance in type.GetInterfaces()
                                                     where typeof(ICustomMapping).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface
                                                     select type);

            return withCustomMappings.Distinct();
        }

        //private void ApplyMappingsFromAssembly(Assembly assembly)
        //{
        //    Type[] types = assembly.GetExportedTypes();
        //    Type interfaceType = typeof(IMapFrom<>);
        //    string methodName = nameof(IMapFrom<object>.Mapping);
        //    Type[] argumentTypes = new Type[] { typeof(Profile) };

        //    foreach (Type type in types)
        //    {
        //        List<Type> interfaces = type.GetInterfaces()
        //            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
        //            .ToList();

        //        // Has the type implemented any IMapFrom<T>?
        //        if (interfaces.Count > 0)
        //        {
        //            // Yes, then let's create an instance
        //            object? instance = Activator.CreateInstance(type);

        //            // and invoke each implementation of `.Mapping()`
        //            foreach (Type i in interfaces)
        //            {
        //                MethodInfo? methodInfo = i.GetMethod(methodName, argumentTypes);

        //                methodInfo?.Invoke(instance, new object[] { this });
        //            }
        //        }
        //    }
        //}
    }
}
