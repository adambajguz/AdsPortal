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

        public OperationPropertySchema(PropertyInfo property,
                                       Type? renderer,
                                       string displayName)
        {
            Property = property;
            Renderer = renderer;
            DisplayName = displayName;
        }
    }
}
