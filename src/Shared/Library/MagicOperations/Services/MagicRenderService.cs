namespace MagicOperations.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MagicOperations.Components;
    using MagicOperations.Extensions;
    using MagicOperations.Interfaces;
    using MagicOperations.Internal.Extensions;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;

    public class MagicRenderService : IMagicRenderService
    {
        private NavigationManager _navigationManager { get; init; } = default!;
        private MagicOperationsConfiguration _configuration { get; init; } = default!;
        private IOperationModelFactory _modelFactory { get; init; } = default!;
        private ILogger _logger { get; init; } = default!;

        public MagicRenderService(NavigationManager navigationManager,
                                      MagicOperationsConfiguration configuration,
                                      IOperationModelFactory operationModelFactory,
                                      ILogger<MagicRenderService> logger)
        {
            _navigationManager = navigationManager;
            _configuration = configuration;
            _modelFactory = operationModelFactory;
            _logger = logger;
        }

        public RenderFragment RenderOperationRouter(string? basePath, string? argsFallback)
        {
            string path = _navigationManager.GetCurrentPageUriWithQuery()
                                            .TrimStart(basePath ?? string.Empty, StringComparison.InvariantCulture)
                                            .TrimStart('/');

            if (string.IsNullOrWhiteSpace(path))
            {
                path = argsFallback ?? string.Empty;
            }

            return RenderOperation(basePath, path);
        }

        public RenderFragment RenderOperation(string path)
        {
            string basePath = _navigationManager.GetCurrentPageUri().TrimStart('/');

            return RenderOperation(basePath, path);
        }

        public RenderFragment RenderOperation(string? basePath, string path)
        {
            path = path.TrimStart(basePath ?? string.Empty, StringComparison.InvariantCulture)
                       .TrimStart('/');

            bool isBasePath = _navigationManager.GetCurrentPageUri().TrimStart('/') == basePath;

            OperationSchema? schema = ResolveSchema(path);

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

                    Type operationRendererType = schema.OperationRenderer ?? _configuration.DefaultOperationRenderers[schema.BaseOperationRenderer];

                    if (operationRendererType.IsGenericTypeDefinition)
                    {
                        Type typeParam0 = schema.OperationModelType;
                        Type typeParam1 = schema.ResponseType ?? typeof(object);
                        operationRendererType = operationRendererType.MakeGenericType(typeParam0, typeParam1);
                    }

                    return (builder) =>
                    {
                        builder.OpenComponent(0, operationRendererType);
                        builder.AddAttribute(1, nameof(OperationRenderer<object, object>.IsBasePath), isBasePath);
                        builder.AddAttribute(2, nameof(OperationRenderer<object, object>.BasePath), basePath ?? string.Empty);
                        builder.AddAttribute(3, nameof(OperationRenderer<object, object>.OperationModel), model);
                        builder.AddAttribute(4, nameof(OperationRenderer<object, object>.OperationSchema), schema);
                        builder.CloseComponent();
                    };
                }

                _logger.LogDebug("Unknown route {Route}.", path);
                return RenderError($"Unknown route {path}.");
            }
            catch (ArgumentBinderException ex)
            {
                _logger.LogDebug(ex, "Argument binder exception occured.");
                return RenderError(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unknown error during route resolving.");
                return RenderError("Unknown error during MagicOperations route resolving.");
            }
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

        public OperationSchema? ResolveSchema(string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return null;
            }

            //Fast group based match
            OperationGroupSchema? group = _configuration.OperationGroups.Values.Where(x => x.Path is not null && route.StartsWith(x.Path))
                                                                               .FirstOrDefault();

            if (group is OperationGroupSchema g && g.Operations.FirstOrDefault(x => x.MatchesPath(route)) is OperationSchema os0)
            {
                return os0;
            }

            //Slow all routes match when group has no operation group route specified
            if (_configuration.OperationSchemas.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Group.Path) && x.MatchesPath(route)) is OperationSchema os1)
            {
                return os1;
            }

            return null;
        }
    }
}
