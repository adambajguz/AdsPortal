namespace MagicOperations.Schemas
{
    using System;
    using System.Reflection;

    public sealed class OperationPropertySchema
    {
        /// <summary>
        /// Property.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// Renderer component type. When null, default renderer will be used.
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

        public OperationPropertySchema(PropertyInfo property,
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
