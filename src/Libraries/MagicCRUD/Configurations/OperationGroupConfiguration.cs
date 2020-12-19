namespace MagicCRUD.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class OperationGroupConfiguration
    {
        public Type EntityType { get; init; } = default!;

        public string OperationPath { get; init; } = string.Empty;

        public OperationConfiguration? CreateOperation { get; init; }
        public OperationConfiguration? UpdateOperation { get; init; }
        public OperationConfiguration? PatchOperation { get; init; }
        public OperationConfiguration? DeleteOperation { get; init; }
        public OperationConfiguration? GetDetailsByIdOperation { get; init; }
        public OperationConfiguration? GetListOperation { get; init; }
        public OperationConfiguration? GetPagedListOperation { get; init; }

        public IEnumerable<OperationConfiguration> GetOperations()
        {
            return new[] {
                CreateOperation,
                UpdateOperation,
                PatchOperation,
                DeleteOperation,
                GetDetailsByIdOperation,
                GetListOperation,
                GetPagedListOperation
            }.Where(x => x is not null)!;
        }
    }
}