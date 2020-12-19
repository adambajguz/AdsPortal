namespace MagicCRUD.Configurations
{
    using System;

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
    }
}