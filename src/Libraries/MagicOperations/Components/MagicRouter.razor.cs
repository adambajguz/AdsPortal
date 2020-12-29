namespace MagicOperations.Components
{
    using System;
    using MagicOperations.Schemas;
    using MagicOperations.Services;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;

    public partial class MagicRouter : ComponentBase
    {
        protected OperationSchema? Schema { get; set; }
        protected RenderFragment? RenderFragment { get; private set; }

        [Inject] private MagicOperationsConfiguration Configuration { get; init; } = default!;
        [Inject] private IMagicRouteResolver RouteResolver { get; init; } = default!;
        [Inject] private ILogger<MagicRouter> Logger { get; init; } = default!;

        [Parameter]
        public string? Args { get; init; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            try
            {
                if (string.IsNullOrWhiteSpace(Args))
                {
                    if (Configuration.OperationListingRenderer is Type operationListingRendererType)
                    {
                        RenderFragment = (builder) =>
                        {
                            builder.OpenComponent(0, operationListingRendererType);
                            builder.CloseComponent();
                        };
                    }

                    return;
                }

                (OperationSchema Schema, UriTemplate.UriTemplateMatch Arguments)? operation = RouteResolver.Resolve(Args);

                if (operation is not null)
                {
                    Schema = operation.Value.Schema;
                    Type type = Schema.ModelType;
                    object model = Activator.CreateInstance(type)!;

                    Type operationRendererType = Schema.Renderer ?? Configuration.DefaultOperationRenderers[Schema.OperationType];

                    RenderFragment = (builder) =>
                    {
                        builder.OpenComponent(0, operationRendererType);
                        builder.AddAttribute(1, nameof(OperationRenderer.Model), model);
                        builder.AddAttribute(2, nameof(OperationRenderer.OperationSchema), Schema);
                        builder.CloseComponent();
                    };

                    return;
                }

                Logger.LogDebug("Unknown route {Route}.", Args);
                RenderError();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unknown error during route resolving.");
            }
        }

        private void RenderError()
        {
            Type errorRendererType = Configuration.ErrorRenderer;

            RenderFragment = (builder) =>
            {
                builder.OpenComponent(0, errorRendererType);
                builder.AddAttribute(1, nameof(OperationErrorRenderer.Route), Args);
                builder.CloseComponent();
            };
        }
    }
}
