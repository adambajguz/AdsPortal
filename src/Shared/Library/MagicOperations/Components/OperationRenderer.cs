namespace MagicOperations.Components
{
    using MagicOperations.Schemas;
    using MagicOperations.Services;
    using Microsoft.AspNetCore.Components;

    public abstract class OperationRenderer : ComponentBase
    {
        [Parameter]
        public object Model { get; init; } = default!;

        [Parameter]
        public OperationSchema OperationSchema { get; set; } = default!;

        [Inject] protected MagicApiService MagicApi { get; init; } = default!;
        [Inject] protected MagicOperationsConfiguration Configuration { get; init; } = default!;
    }
}
