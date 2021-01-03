﻿namespace MagicOperations.Components.OperationRenderers.Base
{
    using System;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class SingleItemOperationRenderer<TOperation, TResponse> : OperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {
        protected RenderFragment RenderItem()
        {
            return (builder) =>
            {
                int i = 0;
                foreach (RenderablePropertySchema propertySchema in OperationSchema.PropertySchemas)
                {
                    Type propertyType = propertySchema.Property.PropertyType;
                    Type operationPropertyRendererType = propertySchema.Renderer ?? Configuration.DefaultOperationPropertyRenderers[propertyType];

                    builder.OpenComponent(i++, operationPropertyRendererType);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.Model), OperationModel);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.PropertySchema), propertySchema);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.OperationSchema), OperationSchema);
                    builder.CloseComponent();
                }
            };
        }
    }
}
