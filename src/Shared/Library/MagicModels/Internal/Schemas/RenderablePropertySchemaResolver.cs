namespace MagicModels.Internal.Schemas
{
    using System.Reflection;
    using MagicModels.Attributes;
    using MagicModels.Builder;
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

            return new RenderablePropertySchema(renderablePropertyInfo,
                                                attribute?.Renderer ?? options?.Renderer,
                                                attribute?.DisplayName ?? options?.DisplayName ?? renderablePropertyInfo.Name,
                                                attribute?.Order ?? options?.Order ?? 0);
        }
    }
}
