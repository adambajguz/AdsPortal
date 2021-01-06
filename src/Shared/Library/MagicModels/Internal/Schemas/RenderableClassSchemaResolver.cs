namespace MagicModels.Internal.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MagicModels.Attributes;
    using MagicModels.Builder;
    using MagicModels.Schemas;

    internal static class RenderableClassSchemaResolver
    {
        public static IReadOnlyDictionary<Type, RenderableClassSchema> Resolve(IEnumerable<(Type, Type?, Dictionary<string, RenderablePropertyConfiguration>?)> renderableTypes)
        {
            Dictionary<Type, RenderableClassSchema> renderableTypeToSchemaMappings = new();

            foreach (var cfg in renderableTypes.OrderByDescending(x => (x.Item2, x.Item3)))
            {
                RenderableClassSchema operationSchema = ResolveRenderableClass(cfg.Item1, cfg.Item2, cfg.Item3);
                renderableTypeToSchemaMappings.TryAdd(cfg.Item1, operationSchema);
            }

            return renderableTypeToSchemaMappings;
        }

        private static RenderableClassSchema ResolveRenderableClass(Type type, Type? renderer, Dictionary<string, RenderablePropertyConfiguration>? options)
        {
            RenderableClassAttribute? renderableClassAttr = type.GetCustomAttribute<RenderableClassAttribute>(true);

            RenderablePropertySchema[] propertySchemas = type.GetProperties()
                                                             .Select(x => RenderablePropertySchemaResolver.TryResolve(x, options?.GetValueOrDefault(x.Name)))
                                                             .Where(x => x is not null)
                                                             .OrderBy(x => x!.Order)
                                                             .ToArray()!;

            return new RenderableClassSchema(renderableClassAttr?.Renderer ?? renderer,
                                             type,
                                             propertySchemas);
        }
    }
}
