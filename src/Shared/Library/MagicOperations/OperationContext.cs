namespace MagicOperations
{
    using System;
    using MagicOperations.Schemas;

    public sealed class OperationContext
    {
        public bool IsPanelPath { get; init; }
        public string Path { get; init; } = string.Empty;

        public OperationSchema Schema { get; init; } = default!;
        public Type OperationRendererType { get; init; } = default!;
    }
}
