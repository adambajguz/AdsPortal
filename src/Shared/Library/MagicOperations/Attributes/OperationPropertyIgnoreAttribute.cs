namespace MagicOperations.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class OperationPropertyIgnoreAttribute : Attribute
    {

    }
}
