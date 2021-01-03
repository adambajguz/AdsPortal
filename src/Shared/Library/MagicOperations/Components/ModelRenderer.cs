namespace MagicOperations.Components
{
    using MagicOperations.Schemas;
    using Microsoft.AspNetCore.Components;

    public abstract class ModelRenderer<TModel> : ComponentBase
        where TModel : notnull
    {
        [Parameter]
        public TModel Model { get; init; } = default!;

        [Parameter]
        public OperationSchema ModelSchema { get; init; } = default!;

        [Inject] protected MagicOperationsConfiguration Configuration { get; init; } = default!;
    }
}
