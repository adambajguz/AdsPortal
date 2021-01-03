namespace MagicOperations.Attributes
{
    using System;

    /// <summary>
    /// Attribute that can be used to mark property not to render.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class RenderablePropertyIgnoreAttribute : Attribute
    {

    }
}
