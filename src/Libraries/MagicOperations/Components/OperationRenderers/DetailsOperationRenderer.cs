namespace MagicOperations.Components
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class DetailsOperationRenderer : OperationRenderer
    {
        protected IReadOnlyList<RenderFragment> RenderFragments { get; private set; } = new List<RenderFragment>();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Type type = Model.GetType();
            OperationSchema? schema = Configuration.ModelToSchemaMappings.GetValueOrDefault(type);

            List<RenderFragment> renderFragments = new();

            if (schema is not null)
            {
                foreach (OperationPropertySchema propertySchema in schema.PropertySchemas)
                {
                    Type propertyType = propertySchema.Property.PropertyType;
                    Type operationPropertyRendererType = propertySchema.Renderer ?? Configuration.DefaultOperationPropertyRenderers[propertyType];

                    object? value = propertySchema.Property.GetValue(Model);

                    renderFragments.Add((builder) =>
                    {
                        builder.OpenComponent(0, operationPropertyRendererType);
                        builder.AddAttribute(1, nameof(OperationPropertyRenderer<object>.Value), value);
                        builder.AddAttribute(2, nameof(OperationPropertyRenderer<object>.Schema), propertySchema);
                        builder.CloseComponent();
                    });
                }
            }

            RenderFragments = renderFragments;
        }
    }
}
