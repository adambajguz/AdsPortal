﻿namespace MagicOperations.Components
{
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract partial class OperationPropertyRenderer<T> : ComponentBase
    {
        [Parameter]
        public T? Value { get; set; }

        [Parameter]
        public OperationPropertySchema Schema { get; set; } = default!;
    }
}