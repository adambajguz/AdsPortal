namespace MagicOperations.Components
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Extensions;
    using MagicOperations.Interfaces;
    using MagicOperations.Internal.Extensions;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;

    public sealed partial class MagicRouter : ComponentBase
    {
        private string? Path { get; set; }
        private OperationSchema? Schema { get; set; }

        [Inject] private NavigationManager NavigationManager { get; init; } = default!;
        [Inject] private MagicOperationsConfiguration Configuration { get; init; } = default!;
        [Inject] private IOperationModelFactory ModelFactory { get; init; } = default!;
        [Inject] private IMagicRouteResolver RouteResolver { get; init; } = default!;
        [Inject] private ILogger<MagicRouter> Logger { get; init; } = default!;

        [Parameter]
        public string? BasePath { get; init; }

        [Parameter]
        public string? ArgsFallback { get; init; }

        [Parameter]
        public bool Debug { get; init; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            string path = NavigationManager.GetCurrentPageUriWithQuery()
                                           .TrimStart(BasePath ?? string.Empty, StringComparison.InvariantCulture)
                                           .TrimStart('/');

            if (string.IsNullOrWhiteSpace(path))
                path = ArgsFallback ?? string.Empty;

            Path = path;
            Schema = RouteResolver.ResolveSchema(Path);
        }

        private RenderFragment Render()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Path))
                {
                    return RenderOperationsList();
                }

                IEnumerable<OperationUriArgument>? arguments = Schema?.ExtractArguments(Path);

                if (arguments is not null)
                {
                    object model = ModelFactory.CreateInstanceAndBindData(Schema!.ModelType, arguments);

                    Type operationRendererType = Schema.Renderer ?? Configuration.DefaultOperationRenderers[Schema.OperationType];

                    return (builder) =>
                    {
                        builder.OpenComponent(0, operationRendererType);
                        builder.AddAttribute(1, nameof(OperationRenderer.BasePath), BasePath ?? string.Empty);
                        builder.AddAttribute(2, nameof(OperationRenderer.OperationModel), model);
                        builder.AddAttribute(3, nameof(OperationRenderer.OperationSchema), Schema);
                        builder.CloseComponent();
                    };
                }

                Logger.LogDebug("Unknown route {Route}.", Path);
                RenderError($"Unknown route {Path}.");
            }
            catch (ArgumentBinderException abex)
            {
                Logger.LogWarning(abex, "Argument binder exception occured.");
                return RenderError(abex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unknown error during route resolving.");
                return RenderError("Unknown error during MagicOperations route resolving.");
            }

            return (builder) => { };
        }

        private RenderFragment RenderOperationsList()
        {
            if (Configuration.OperationListingRenderer is Type operationListingRendererType)
            {
                return (builder) =>
                {
                    builder.OpenComponent(0, operationListingRendererType);
                    builder.CloseComponent();
                };
            }

            return (builder) => { };
        }

        private RenderFragment RenderError(string? message = null)
        {
            Type errorRendererType = Configuration.ErrorRenderer;

            return (builder) =>
            {
                builder.OpenComponent(0, errorRendererType);
                builder.AddAttribute(1, nameof(OperationErrorRenderer.Message), message);
                builder.CloseComponent();
            };
        }
    }
}
