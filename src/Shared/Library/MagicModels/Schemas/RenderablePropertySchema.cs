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

        /// <summary>
        /// Property mode override (write when true, read when false, and default from model when null).
        /// </summary>
        public bool? IsWrite { get; init; }

        public RenderablePropertySchema(PropertyInfo property,
                                        Type? renderer,
                                        string displayName,
                                        int order,
                                        bool? isWrite)
        {
            Property = property;
            Renderer = renderer;
            DisplayName = displayName;
            Order = order;
            IsWrite = isWrite;
        }
    }
}
