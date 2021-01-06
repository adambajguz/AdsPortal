namespace MagicModels.Attributes
{
    using System;

    /// <summary>
    /// Attribute that can be used to mark a class as renderable. By default all classes marked with operation attribute are renderable (thus, this attribute can be used to change operation renderer).
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class RenderableClassAttribute : Attribute
    {
        /// <summary>
        /// Renderer component type. When null, default renderer will be used.
        /// </summary>
        public Type? Renderer { get; init; }
    }
}
