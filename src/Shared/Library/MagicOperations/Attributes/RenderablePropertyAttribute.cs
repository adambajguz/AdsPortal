namespace MagicOperations.Attributes
{
    using System;

    /// <summary>
    /// Renderable property attribute that can be used to customize proprty rendering (by default all properties in renderable class are rendered using default settings).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RenderablePropertyAttribute : Attribute
    {
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
    }
}
