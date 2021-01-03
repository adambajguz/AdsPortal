namespace MagicOperations.Components
{
    using MagicOperations.Interfaces;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class OperationRenderer : ComponentBase
    {
        [Parameter]
        public string BasePath { get; init; } = string.Empty;

        [Parameter]
        public object OperationModel { get; init; } = default!;

        public object? ErrorModel { get; protected set; }
        public object? ResponseModel { get; protected set; }

        [Parameter]
        public OperationSchema OperationSchema { get; init; } = default!;

        [Inject] protected IMagicApiService Api { get; init; } = default!;
        [Inject] protected MagicOperationsConfiguration Configuration { get; init; } = default!;
    }
}
