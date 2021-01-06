namespace MagicModels.Schemas
{
    using System;
    using System.Reflection;

    public sealed class RenderablePropertySchema
    {
        /// <summary>
        /// Property info.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// Property renderer component type. When null, default renderer will be used.
        /// </summary>
        public Type? Renderer { get; }

        /// <summary>
        /// Property display name.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Property order.
        /// </summary>
        public int Order { get; }

        public RenderablePropertySchema(PropertyInfo property,
                                        Type? renderer,
                                        string displayName,
                                        int order)
        {
            Property = property;
            Renderer = renderer;
            DisplayName = displayName;
            Order = order;
        }
    }
}
