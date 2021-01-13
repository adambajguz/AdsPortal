namespace MagicModels.Builder
{
    using System;
    using MagicModels.Attributes;

    public sealed class RenderablePropertyConfiguration
    {
        /// <summary>
        /// Whether property is ignored during rendering.
        /// </summary>
        public bool Ignore { get; init; }

        /// <summary>
        /// Renderer component type. When null, default renderer will be used.
        /// </summary>
        public Type? Renderer { get; init; }

        /// <summary>
        /// Property display name. When null, property name will be used.
        /// </summary>
        public string? DisplayName { get; init; }

        /// <summary>
        /// Property order.
        /// </summary>
        public int Order { get; init; }

        /// <summary>
        /// Property mode override (write when true, read when false, and default from model when null).
        /// </summary>
        public PropertyMode Mode { get; init; }
    }
}
