namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicOperations.Attributes;
    using MagicOperations.Extensions;

    public static class RenderableClassSchemaResolver
    {
        public static IReadOnlyDictionary<Type, RenderableClassSchema> Resolve(IEnumerable<Type> renderableTypes)
        {
            Dictionary<Type, RenderableClassSchema> renderableTypeToSchemaMappings = new();

            foreach (Type operationModelType in renderableTypes)
            {
                RenderableClassSchema operationSchema = ResolveOperation(operationModelType);
                renderableTypeToSchemaMappings.Add(operationModelType, operationSchema);
            }

            return renderableTypeToSchemaMappings;
        }

        private static RenderableClassSchema ResolveOperation(Type type)
        {
            OperationAttribute? operationAttr = type.GetCustomAttribute<OperationAttribute>(true);
            RenderableClassAttribute? renderableClassAttr = type.GetCustomAttribute<RenderableClassAttribute>(true);

            if (operationAttr is null && renderableClassAttr is null)
                throw new MagicOperationsException($"Operation {type.FullName} does not have {typeof(RenderableClassAttribute).FullName} or {typeof(OperationAttribute).FullName}.");

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
