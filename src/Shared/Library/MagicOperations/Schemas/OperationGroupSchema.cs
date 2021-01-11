namespace MagicOperations.Schemas
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MagicOperations.Extensions;

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

        public string GetRouteToOperation(Type operationType, IReadOnlyDictionary<string, string> arguments)
        {
            if (!KnownTypesHelpers.IsOperationRenderer(operationType))
            {
                throw new MagicOperationsException($"{operationType.FullName} is not a valid base operation renderer type.");
            }

            var operationSchema = Operations.FirstOrDefault(x => x.BaseOperationRenderer.IsEquivalentTo(operationType)) ?? throw new MagicOperationsException($"Base operation of type {operationType.FullName} not found in group with key {Key}.");

            return operationSchema.GetFullPathFromDictionary(arguments);
        }
    }
}
