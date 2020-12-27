namespace MagicOperations.Components
{
    using Microsoft.AspNetCore.Components;

    public abstract partial class OperationPropertyRenderer<T> : ComponentBase
    {
        protected T? Value { get; set; }
    }
}
