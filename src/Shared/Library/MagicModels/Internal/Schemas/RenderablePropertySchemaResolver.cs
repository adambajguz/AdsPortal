namespace MagicModels.Internal.Schemas
{
    using System;
    using System.Reflection;
    using MagicModels.Attributes;
    using MagicModels.Builder;
    using MagicModels.Extensions;
    using MagicModels.Schemas;

    internal static class RenderablePropertySchemaResolver
    {
        public static RenderablePropertySchema? TryResolve(PropertyInfo renderablePropertyInfo, RenderablePropertyConfiguration? options)
        {
            RenderablePropertyIgnoreAttribute? ignoreAttribute = renderablePropertyInfo.GetCustomAttribute<RenderablePropertyIgnoreAttribute>(true);

            if (ignoreAttribute is not null || (options?.Ignore ?? false))
            {
                return null;
            }

            RenderablePropertyAttribute? attribute = renderablePropertyInfo.GetCustomAttribute<RenderablePropertyAttribute>(true);

            bool? mode = attribute?.Mode switch
            {
                PropertyMode.Read => false,
                PropertyMode.Write => true,
                _ => null
            };

            mode ??= options?.Mode switch
            {
                PropertyMode.Read => false,
                PropertyMode.Write => true,
                _ => null
            };

            Type? renderer = attribute?.Renderer ?? options?.Renderer;

            if (renderer?.IsGenericTypeDefinition ?? false)
            {
                throw new MagicModelsException($"Renderer cannot be generic type definition ('{renderablePropertyInfo.Name}' in '{renderablePropertyInfo.DeclaringType?.AssemblyQualifiedName}').");
            }

            return new RenderablePropertySchema(renderablePropertyInfo,
                                                renderer,
                                                attribute?.DisplayName ?? options?.DisplayName ?? renderablePropertyInfo.Name,
                                                attribute?.Order ?? options?.Order ?? 0,
                                                mode);
        }
    }
}
