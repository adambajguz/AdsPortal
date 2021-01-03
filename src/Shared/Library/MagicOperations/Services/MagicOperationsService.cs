namespace MagicOperations.Services
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Components;
    using MagicOperations.Extensions;
    using MagicOperations.Interfaces;
    using MagicOperations.Internal.Extensions;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;

    public class MagicOperationsService : IMagicOperationsService
    {
        private NavigationManager _navigationManager { get; init; } = default!;
        private MagicOperationsConfiguration _configuration { get; init; } = default!;
        private IOperationModelFactory _modelFactory { get; init; } = default!;
        private IMagicRouteResolver _routeResolver { get; init; } = default!;
        private ILogger _logger { get; init; } = default!;

        public MagicOperationsService(NavigationManager navigationManager,
                                      MagicOperationsConfiguration configuration,
                                      IOperationModelFactory operationModelFactory,
                                      IMagicRouteResolver routeResolver,
                                      ILogger<MagicOperationsService> logger)
        {
            _navigationManager = navigationManager;
            _configuration = configuration;
            _modelFactory = operationModelFactory;
            _routeResolver = routeResolver;
            _logger = logger;
        }

        public RenderFragment RenderModel(object? model)
        {
            if (model is null)
                return (builder) => { };

            return (builder) => { };
        }

        public RenderFragment RenderOperationRouter(string? basePath, string? argsFallback)
        {
            string path = _navigationManager.GetCurrentPageUriWithQuery()
                                            .TrimStart(basePath ?? string.Empty, StringComparison.InvariantCulture)
                                            .TrimStart('/');

            if (string.IsNullOrWhiteSpace(path))
                path = argsFallback ?? string.Empty;

            OperationSchema? schema = _routeResolver.ResolveSchema(path);

            try
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return RenderOperationsList();
                }

                IEnumerable<OperationUriArgument>? arguments = schema?.ExtractArguments(path);

                if (arguments is not null)
                {
                    object model = _modelFactory.CreateInstanceAndBindData(schema!.OperationModelType, arguments);

                    Type operationRendererType = schema.Renderer ?? _configuration.DefaultOperationRenderers[schema.OperationType];
                    if (operationRendererType.IsGenericType)
                    {
                        operationRendererType = operationRendererType.MakeGenericType(schema.OperationModelType, schema.ResponseType ?? typeof(object));
                    }

                    return (builder) =>
                    {
                        builder.OpenComponent(0, operationRendererType);
                        builder.AddAttribute(1, nameof(OperationRenderer<object, object>.BasePath), basePath ?? string.Empty);
                        builder.AddAttribute(2, nameof(OperationRenderer<object, object>.OperationModel), model);
                        builder.AddAttribute(3, nameof(OperationRenderer<object, object>.OperationSchema), schema);
                        builder.CloseComponent();
                    };
                }

                _logger.LogDebug("Unknown route {Route}.", path);
                RenderError($"Unknown route {path}.");
            }
            catch (ArgumentBinderException abex)
            {
                _logger.LogDebug(abex, "Argument binder exception occured.");
                return RenderError(abex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error during route resolving.");
                return RenderError("Unknown error during MagicOperations route resolving.");
            }

            return (builder) => { };
        }

        private RenderFragment RenderOperationsList()
        {
            if (_configuration.OperationListingRenderer is Type operationListingRendererType)
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
            Type errorRendererType = _configuration.ErrorRenderer;

            return (builder) =>
            {
                builder.OpenComponent(0, errorRendererType);
                builder.AddAttribute(1, nameof(OperationErrorRenderer.Message), message);
                builder.CloseComponent();
            };
        }
    }
}
