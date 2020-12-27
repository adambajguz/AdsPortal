namespace MagicOperations.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class OperationGroupAttribute : Attribute
    {
        /// <summary>
        /// Operation route relative to base URI.
        /// </summary>
        public string Route { get; }

        public OperationGroupAttribute(string route)
        {
            Route = route;
        }
    }
}
