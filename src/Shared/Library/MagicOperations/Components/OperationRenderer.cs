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
        public OperationContext Context { get; init; } = default!;

        [Parameter]
        public TOperation OperationModel { get; init; } = default!;
        object IOperationRenderer.OperationModel => OperationModel!;

        public object? ErrorModel { get; protected set; }
        public TResponse? ResponseModel { get; protected set; }
        object? IOperationRenderer.ResponseModel => ResponseModel;

        [Inject] protected IMagicApiService Api { get; init; } = default!;
        [Inject] protected MagicOperationsConfiguration Configuration { get; init; } = default!;

        public string GetRouteToThisOperation(IReadOnlyDictionary<string, string>? arguments = null)
        {
            string v = Context.Schema.Group.GetRouteToOperation(Context.Schema.BaseOperationRenderer, arguments);

            return string.IsNullOrWhiteSpace(Configuration.PanelPath) ? v : $"{Configuration.PanelPath}/{v}";
        }

        public string GetRouteToRealtedOperation(Type operationType, IReadOnlyDictionary<string, string>? arguments = null)
        {
            string v = Context.Schema.Group.GetRouteToOperation(operationType, arguments);

            return string.IsNullOrWhiteSpace(Configuration.PanelPath) ? v : $"{Configuration.PanelPath}/{v}";
        }
    }
}
