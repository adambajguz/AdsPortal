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
        /// Operation group route relative to base URI. If null, operation has no extra route.
        /// </summary>
        public string? Route { get; }

        private readonly List<OperationSchema> _operations = new();

        /// <summary>
        /// Operations.
        /// </summary>
        public IReadOnlyList<OperationSchema> Operations => _operations;

        public OperationGroupSchema(string key, string? route)
        {
            Key = key;
            Route = route;
        }

        internal void AddOperation(OperationSchema operationSchema)
        {
            _operations.Add(operationSchema);
        }
    }
}
