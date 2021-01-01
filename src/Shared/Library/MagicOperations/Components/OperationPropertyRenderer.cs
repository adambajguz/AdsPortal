namespace MagicOperations.Components
{
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract partial class OperationPropertyRenderer<T> : ComponentBase
    {
        public T? Value
        {
            get => (T)PropertySchema.Property.GetValue(Model);
            set => PropertySchema.Property.SetValue(Model, value);
        }

        [Parameter]
        public object Model { get; init; } = default!;

        [Parameter]
        public OperationPropertySchema PropertySchema { get; init; } = default!;

        [Parameter]
        public OperationSchema OperationSchema { get; init; } = default!;
    }
}
