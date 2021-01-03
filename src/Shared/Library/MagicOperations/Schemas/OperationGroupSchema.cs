namespace MagicOperations.Schemas
{
    using System.Collections.Generic;

    public sealed class OperationGroupSchema
    {
        /// <summary>
        /// Operation group key.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Operation group path relative to base URI. If null, operation has no extra route.
        /// </summary>
        public string? Path { get; }

        /// <summary>
        /// Property display name.
        /// </summary>
        public string DisplayName { get; }

        private readonly List<OperationSchema> _operations = new();

        /// <summary>
        /// Operations.
        /// </summary>
        public IReadOnlyList<OperationSchema> Operations => _operations;

        public OperationGroupSchema(string key, string? path, string? displayName)
        {
            Key = key;
            Path = path;
            DisplayName = displayName ?? path ?? key;
        }

        internal void AddOperation(OperationSchema operationSchema)
        {
            _operations.Add(operationSchema);
        }
    }
}
