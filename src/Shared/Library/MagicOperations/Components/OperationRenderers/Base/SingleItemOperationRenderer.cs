namespace MagicOperations.Components.OperationRenderers.Base
{
    using System;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class Single45ItemOperationRenderer<TOperation, TResponse> : OperationRenderer<TOperation, TResponse>
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
                    Type operationPropertyRendererType = propertySchema.Renderer ?? Configuration.DefaultPropertyRenderers[propertyType];

                    builder.OpenComponent(i++, operationPropertyRendererType);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.Model), OperationModel);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.PropertySchema), propertySchema);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.IsWrite), OperationSchema.IsCreateOrUpdate); //TODO: how to render table when we only have write/read bool prop?
                    builder.CloseComponent();
                }
            };
        }
    }
}
