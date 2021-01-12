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
        private MagicModelsConfiguration _configuration { get; init; } = default!;
        private ILogger _logger { get; init; } = default!;

        public ModelRenderService(MagicModelsConfiguration configuration,
                                  ILogger<ModelRenderService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public RenderFragment RenderModel(object? model, object? context = null, bool isWrite = false)
        {
            if (model is null)
            {
                return (builder) => { };
            }

            Type type = model.GetType();
            RenderableClassSchema? schema = GetSchema(type);

            Type operationRendererType = schema.Renderer ?? _configuration.DefaultModelRenderer;
            if (operationRendererType.IsGenericTypeDefinition)
            {
                operationRendererType = operationRendererType.MakeGenericType(type);
            }

            return (builder) =>
            {
                int i = 0;

                builder.OpenComponent(i++, operationRendererType);
                builder.AddAttribute(i++, nameof(ModelRenderer<object>.Context), context);
                builder.AddAttribute(i++, nameof(ModelRenderer<object>.Model), model);
                builder.AddAttribute(i++, nameof(ModelRenderer<object>.Schema), schema);
                builder.AddAttribute(i++, nameof(ModelRenderer<object>.IsWrite), isWrite);
                builder.CloseComponent();
            };
        }

        public RenderableClassSchema GetSchema(Type type)
        {
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

            return schema;
        }
    }
}
