namespace MagicOperations.Components
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class SingleItemOperationRenderer : OperationRenderer
    {
        protected IReadOnlyList<RenderFragment> RenderFragments { get; private set; } = new List<RenderFragment>();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            List<RenderFragment> renderFragments = new();

            foreach (OperationPropertySchema propertySchema in OperationSchema.PropertySchemas)
            {
                Type propertyType = propertySchema.Property.PropertyType;
                Type operationPropertyRendererType = propertySchema.Renderer ?? Configuration.DefaultOperationPropertyRenderers[propertyType];

                object? value = propertySchema.Property.GetValue(Model);

                renderFragments.Add((builder) =>
                {
                    builder.OpenComponent(0, operationPropertyRendererType);
                    builder.AddAttribute(1, nameof(OperationPropertyRenderer<object>.Value), value);
                    builder.AddAttribute(2, nameof(OperationPropertyRenderer<object>.PropertySchema), propertySchema);
                    builder.AddAttribute(3, nameof(OperationPropertyRenderer<object>.OperationSchema), OperationSchema);
                    builder.CloseComponent();
                });
            }

            RenderFragments = renderFragments;
        }
    }
}
