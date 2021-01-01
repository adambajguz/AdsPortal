namespace MagicOperations.Components
{
    using MagicOperations.Schemas;
    using MagicOperations.Services;
    using Microsoft.AspNetCore.Components;

    public abstract class OperationRenderer : ComponentBase
    {
        public object? Errors { get; protected set; }

        [Parameter]
        public object Model { get; init; } = default!;

        [Parameter]
        public OperationSchema OperationSchema { get; init; } = default!;

        [Inject] protected MagicApiService Api { get; init; } = default!;
        [Inject] protected MagicOperationsConfiguration Configuration { get; init; } = default!;
    }
}
