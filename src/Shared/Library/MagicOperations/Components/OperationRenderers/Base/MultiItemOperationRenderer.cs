namespace MagicOperations.Components.OperationRenderers.Base
{
    using System;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class MultiItemOperationRenderer<TOperation, TResponse> : OperationRenderer<TOperation, TResponse>
        where TOperation : notnull
    {
        protected RenderFragment RenderItem(object model)
        {
            return (builder) =>
            {
                int i = 0;
                foreach (RenderablePropertySchema propertySchema in OperationSchema.PropertySchemas)
                {
                    Type propertyType = propertySchema.Property.PropertyType;
                    Type operationPropertyRendererType = propertySchema.Renderer ?? Configuration.DefaultPropertyRenderers[propertyType];

                    builder.OpenComponent(i++, operationPropertyRendererType);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.Model), model);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.PropertySchema), propertySchema);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.IsWrite), OperationSchema.IsCreateOrUpdate);
                    builder.CloseComponent();
                }
            };
        }
    }
}
