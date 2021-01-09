namespace MagicModels.Services
{
    using System;
    using System.Collections.Generic;
    using MagicModels;
    using MagicModels.Components;
    using MagicModels.Extensions;
    using MagicModels.Interfaces;
    using MagicModels.Internal.Extensions;
    using MagicModels.Schemas;
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Logging;

    public class ModelRenderService : IModelRenderService
    {
        private NavigationManager _navigationManager { get; init; } = default!;
        private MagicModelsConfiguration _configuration { get; init; } = default!;
        private ILogger _logger { get; init; } = default!;

        public ModelRenderService(NavigationManager navigationManager,
                                  MagicModelsConfiguration configuration,
                                  ILogger<ModelRenderService> logger)
        {
            _navigationManager = navigationManager;
            _configuration = configuration;
            _logger = logger;
        }

        public RenderFragment RenderModel(object? model, bool isWrite = false)
        {
            if (model is null)
            {
                return (builder) => { };
            }

            Type type = model.GetType();
            RenderableClassSchema? schema = _configuration.RenderableTypeToSchemaMap.GetValueOrDefault(type);

            //Fallback to base types other than object
            if (schema is null)
            {
                foreach (Type baseType in type.GetBaseTypesOtherThanObject())
                {
                    schema ??= _configuration.RenderableTypeToSchemaMap.GetValueOrDefault(baseType);
                }
            }

            //Fallback to interfaces
            if (schema is null)
            {
                foreach (Type interafaceType in type.GetInterfaces())
                {
                    schema ??= _configuration.RenderableTypeToSchemaMap.GetValueOrDefault(interafaceType);
                }
            }

            if (schema is null)
            {
                throw new MagicModelsException($"Invalid model. Either model is not renderable or was not registered {type.AssemblyQualifiedName}.");
            }

            Type operationRendererType = schema.Renderer ?? _configuration.DefaultModelRenderer;
            if (operationRendererType.IsGenericType)
            {
                operationRendererType = operationRendererType.MakeGenericType(type);
            }

            return (builder) =>
            {
                builder.OpenComponent(0, operationRendererType);
                builder.AddAttribute(1, nameof(ModelRenderer<object>.Model), model);
                builder.AddAttribute(2, nameof(ModelRenderer<object>.Schema), schema);
                builder.AddAttribute(3, nameof(ModelRenderer<object>.IsWrite), isWrite);
                builder.CloseComponent();
            };
        }
    }
}
