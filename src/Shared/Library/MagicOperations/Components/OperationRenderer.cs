namespace MagicOperations.Components
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Interfaces;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class OperationRenderer<TOperation, TResponse> : ComponentBase, IOperationRenderer
    {
        [Parameter]
        public string BasePath { get; init; } = string.Empty;

        [Parameter]
        public TOperation OperationModel { get; init; } = default!;
        object IOperationRenderer.OperationModel => OperationModel!;

        public object? ErrorModel { get; protected set; }
        public TResponse? ResponseModel { get; protected set; }
        object? IOperationRenderer.ResponseModel => ResponseModel;

        [Parameter]
        public OperationSchema OperationSchema { get; init; } = default!;

        [Inject] protected IMagicApiService Api { get; init; } = default!;
        [Inject] protected MagicOperationsConfiguration Configuration { get; init; } = default!;

        public string GetRouteToThisOperation(IReadOnlyDictionary<string, string>? arguments = null)
        {
            string v = OperationSchema.Group.GetRouteToOperation(OperationSchema.BaseOperationRenderer, arguments);

            return string.IsNullOrWhiteSpace(BasePath) ? v : $"{BasePath}/{v}";
        }

        public string GetRouteToRealtedOperation(Type operationType, IReadOnlyDictionary<string, string>? arguments = null)
        {
            string v = OperationSchema.Group.GetRouteToOperation(operationType, arguments);

            return string.IsNullOrWhiteSpace(BasePath) ? v : $"{BasePath}/{v}";
        }
    }
}
