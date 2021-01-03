namespace MagicOperations.Schemas
{
    using System.Reflection;
    using MagicOperations.Attributes;

    public static class RenderablePropertySchemaResolver
    {
        public static RenderablePropertySchema? TryResolve(PropertyInfo renderablePropertyInfo)
        {
            RenderablePropertyIgnoreAttribute? ignoreAttribute = renderablePropertyInfo.GetCustomAttribute<RenderablePropertyIgnoreAttribute>(true);

            if (ignoreAttribute is not null)
                return null;

            RenderablePropertyAttribute? attribute = renderablePropertyInfo.GetCustomAttribute<RenderablePropertyAttribute>(true);

            string displayName = attribute?.DisplayName ?? renderablePropertyInfo.Name;

            return new RenderablePropertySchema(renderablePropertyInfo,
                                                attribute?.Renderer,
                                                displayName,
                                                attribute?.Order ?? 0);
        }
    }
}
