namespace MagicOperations.Components.OperationRenderers.Base
{
    public abstract class MultiItemOperationRenderer : OperationRenderer
    {
        //protected RenderFragment RenderItem(object model)
        //{
        //    return (builder) =>
        //    {
        //        int i = 0;
        //        foreach (OperationPropertySchema propertySchema in OperationSchema.PropertySchemas)
        //        {
        //            Type propertyType = propertySchema.Property.PropertyType;
        //            Type operationPropertyRendererType = propertySchema.Renderer ?? Configuration.DefaultOperationPropertyRenderers[propertyType];

        //            builder.OpenComponent(i++, operationPropertyRendererType);
        //            builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.Model), model);
        //            builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.PropertySchema), propertySchema);
        //            builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.OperationSchema), OperationSchema);
        //            builder.CloseComponent();
        //        }
        //    };
        //}
    }
}
