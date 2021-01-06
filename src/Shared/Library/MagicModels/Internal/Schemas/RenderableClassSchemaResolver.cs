namespace MagicModels.Internal.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicModels.Attributes;
    using MagicModels.Schemas;

    internal static class RenderableClassSchemaResolver
    {
        public static IReadOnlyDictionary<Type, RenderableClassSchema> Resolve(IEnumerable<Type> renderableTypes)
        {
            Dictionary<Type, RenderableClassSchema> renderableTypeToSchemaMappings = new();

            foreach (Type operationModelType in renderableTypes)
            {
                RenderableClassSchema operationSchema = ResolveRenderableClass(operationModelType);
                renderableTypeToSchemaMappings.Add(operationModelType, operationSchema);
            }

            return renderableTypeToSchemaMappings;
        }

        private static RenderableClassSchema ResolveRenderableClass(Type type)
        {
            //OperationAttribute? operationAttr = type.GetCustomAttribute<OperationAttribute>(true);
            RenderableClassAttribute? renderableClassAttr = type.GetCustomAttribute<RenderableClassAttribute>(true);

            RenderablePropertySchema[] propertySchemas = type.GetProperties()
                                                             .Select(RenderablePropertySchemaResolver.TryResolve)
                                                             .Where(x => x is not null)
                                                             .OrderBy(x => x!.Order)
                                                             .ToArray()!;

            return new RenderableClassSchema(renderableClassAttr?.Renderer,
                                             type,
                                             propertySchemas);
        }
    }
}
