namespace MagicOperations.Components
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public partial class MagicRouter : ComponentBase
    {
        protected RenderFragment? RenderFragment { get; private set; }

        [Inject] private MagicOperationsConfiguration Configuration { get; init; } = default!;

        [Parameter]
        public object Model { get; init; } = default!;

        protected OperationSchema? Schema { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Type type = Model.GetType();
            Schema = Configuration.ModelToSchemaMappings.GetValueOrDefault(type);

            if (Schema is not null)
            {
                Type operationRendererType = Schema.Renderer ?? Configuration.DefaultOperationRenderers[Schema.OperationType];

                RenderFragment = (builder) =>
                {
                    builder.OpenComponent(0, operationRendererType);
                    builder.AddAttribute(1, nameof(OperationRenderer.Model), Model);
                    builder.AddAttribute(2, nameof(OperationRenderer.Schema), Schema);
                    builder.CloseComponent();
                };
            }
        }
    }
}
