namespace MagicOperations.Schemas
{
    using System.Collections.Generic;

    public sealed class OperationGroupSchema
    {
        /// <summary>
        /// Operation route relative to base URI. If null, operation has no group.
        /// </summary>
        public string? Route { get; }

        private readonly List<OperationSchema> _operations = new();

        /// <summary>
        /// Operations.
        /// </summary>
        public IReadOnlyList<OperationSchema> Operations => _operations;

        public OperationGroupSchema(string? route)
        {
            Route = route;
        }

        internal void AddOperation(OperationSchema operationSchema)
        {
            _operations.Add(operationSchema);
        }
    }
}
