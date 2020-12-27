namespace MagicOperations.Schemas
{
    using System;

    public sealed class OperationPropertySchema
    {
        /// <summary>
        /// Renderer component type. When null, default renderer will be used.
        /// </summary>
        public Type? Renderer { get; init; }

        /// <summary>
        /// Property display name.
        /// </summary>
        public string DisplayName { get; init; }

        public OperationPropertySchema(Type? renderer,
                                       string displayName)
        {
            Renderer = renderer;
            DisplayName = displayName;
        }
    }
}
