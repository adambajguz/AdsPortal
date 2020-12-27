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
            OperationSchema? schema = Configuration.ModelToSchemaMappings.GetValueOrDefault(type);
            Schema = schema;

            RenderFragment = (builder) =>
            {
                builder.OpenComponent(0, type);
                builder.CloseComponent();
            };
        }
    }
}
