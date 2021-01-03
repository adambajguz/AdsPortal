namespace MagicOperations.Components
{
    using MagicOperations.Interfaces;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class OperationRenderer<TOperation, TResponse> : ComponentBase
        where TOperation : notnull
    {
        [Parameter]
        public string BasePath { get; init; } = string.Empty;

        [Parameter]
        public TOperation OperationModel { get; init; } = default!;

        public object? ErrorModel { get; protected set; }
        public TResponse? ResponseModel { get; protected set; }

        [Parameter]
        public OperationSchema OperationSchema { get; init; } = default!;

        [Inject] protected IMagicApiService Api { get; init; } = default!;
        [Inject] protected MagicOperationsConfiguration Configuration { get; init; } = default!;
    }
}
