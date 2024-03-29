﻿namespace MagicModels.Components
{
    using System;
    using System.Collections.Generic;
    using MagicModels.Internal.Extensions;
    using MagicModels.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class ModelRenderer<TModel> : ComponentBase
        where TModel : notnull
    {
        [Parameter] public object? Context { get; init; }

        [Parameter] public TModel Model { get; init; } = default!;

        [Parameter] public RenderableClassSchema Schema { get; init; } = default!;

        [Parameter] public bool IsWrite { get; init; } = default!;

        [Inject] protected MagicModelsConfiguration Configuration { get; init; } = default!;

        protected RenderFragment RenderProperties()
        {
            return (builder) =>
            {
                int i = 0;
                foreach (RenderablePropertySchema propertySchema in Schema.PropertySchemas)
                {
                    Type propertyType = propertySchema.Property.PropertyType;

                    Type? rendererType = propertySchema.Renderer ?? Configuration.DefaultPropertyRenderers.GetValueOrDefault(propertyType);

                    //Fallback to base types other than object
                    if (rendererType is null)
                    {
                        foreach (Type baseType in propertyType.GetBaseTypesOtherThanObject())
                        {
                            rendererType ??= Configuration.DefaultPropertyRenderers.GetValueOrDefault(baseType);
                        }
                    }

                    //Fallback to interfaces
                    if (rendererType is null)
                    {
                        foreach (Type interafaceType in propertyType.GetInterfaces())
                        {
                            rendererType ??= Configuration.DefaultPropertyRenderers.GetValueOrDefault(interafaceType);
                        }
                    }

                    //Fallback to object and any type
                    rendererType ??= Configuration.DefaultPropertyRenderers.GetValueOrDefault(typeof(object)); //TODO: neccessary?
                    rendererType ??= Configuration.AnyPropertyRenderer.MakeGenericType(propertyType);

                    //Build render tree
                    builder.OpenComponent(i++, rendererType);
                    builder.AddAttribute(i++, nameof(PropertyRenderer<object>.Model), Model);
                    builder.AddAttribute(i++, nameof(PropertyRenderer<object>.Context), Context);
                    builder.AddAttribute(i++, nameof(PropertyRenderer<object>.PropertySchema), propertySchema);
                    builder.AddAttribute(i++, nameof(PropertyRenderer<object>.IsWrite), propertySchema.IsWrite ?? IsWrite);
                    builder.CloseComponent();
                }
            };
        }
    }
}
