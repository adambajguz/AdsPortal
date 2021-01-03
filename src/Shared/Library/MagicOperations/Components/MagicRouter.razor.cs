namespace MagicOperations.Components
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using MagicOperations.Extensions;
    using MagicOperations.Internal;
    using MagicOperations.Internal.Extensions;
    using MagicOperations.Schemas;
    using MagicOperations.Services;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;

    public partial class MagicRouter : ComponentBase
    {
        protected OperationSchema? Schema { get; set; }
        protected RenderFragment? RenderFragment { get; private set; }

        [Inject] private NavigationManager NavigationManager { get; init; } = default!;
        [Inject] private MagicOperationsConfiguration Configuration { get; init; } = default!;
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

            try
            {
                if (string.IsNullOrWhiteSpace(path))
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

                (OperationSchema Schema, IEnumerable<OperationArgument> Arguments)? operation = RouteResolver.Resolve(path);

                if (operation is not null)
                {
                    Schema = operation.Value.Schema;
                    Type type = Schema.ModelType;
                    object model = Activator.CreateInstance(type)!;
                    TryBindData(model, operation.Value.Arguments);

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

                Logger.LogDebug("Unknown route {Route}.", path);
                RenderError($"Unknown route {path}.");
            }
            catch (ArgumentBinderException abex)
            {
                Logger.LogWarning(abex, "Argument binder exception occured.");
                RenderError(abex.Message);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unknown error during route resolving.");
                RenderError("Unknown error during MagicOperations route resolving.");
            }
        }

        private void TryBindData(object model, IEnumerable<OperationArgument> arguments)
        {
            foreach (OperationArgument arg in arguments)
            {
                PropertyInfo propertyInfo = arg.Schema.Property;

                object? value = arg.Convert();

                propertyInfo.SetValue(model, value);
            }
        }

        private void RenderError(string? message = null)
        {
            Type errorRendererType = Configuration.ErrorRenderer;

            RenderFragment = (builder) =>
            {
                builder.OpenComponent(0, errorRendererType);
                builder.AddAttribute(1, nameof(OperationErrorRenderer.Message), message);
                builder.CloseComponent();
            };
        }
    }
}
