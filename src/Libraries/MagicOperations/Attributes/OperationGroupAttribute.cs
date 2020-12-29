namespace MagicOperations.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class OperationGroupAttribute : Attribute
    {
        /// <summary>
        /// Operation group key.
        /// </summary>
        public string Key { get; }

        public OperationGroupAttribute(string key)
        {
            Key = key;
        }
    }
}
