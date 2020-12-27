namespace MagicOperations.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class OperationPropertyAttribute : Attribute
    {
        /// <summary>
        /// Renderer component type. When null, default renderer will be used.
        /// </summary>
        public Type? Renderer { get; init; }

        /// <summary>
        /// Property display name. When null, property name will be used.
        /// </summary>
        public string? DisplayName { get; init; }
    }
}
