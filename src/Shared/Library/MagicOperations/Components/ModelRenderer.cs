namespace MagicOperations.Components
{
    using System;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class ModelRenderer<TModel> : ComponentBase
        where TModel : notnull
    {
        [Parameter]
        public TModel Model { get; init; } = default!;

        [Parameter]
        public RenderableClassSchema Schema { get; init; } = default!;

        [Parameter]
        public bool IsWrite { get; init; } = default!;

        [Inject] protected MagicOperationsConfiguration Configuration { get; init; } = default!;

        protected RenderFragment RenderProperties()
        {
            return (builder) =>
            {
                int i = 0;
                foreach (RenderablePropertySchema propertySchema in Schema.PropertySchemas)
                {
                    Type propertyType = propertySchema.Property.PropertyType;
                    Type operationPropertyRendererType = propertySchema.Renderer ?? Configuration.DefaultOperationPropertyRenderers[propertyType];

                    builder.OpenComponent(i++, operationPropertyRendererType);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.Model), Model);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.PropertySchema), propertySchema);
                    builder.AddAttribute(i++, nameof(OperationPropertyRenderer<object>.IsWrite), IsWrite);
                    builder.CloseComponent();
                }
            };
        }
    }
}
