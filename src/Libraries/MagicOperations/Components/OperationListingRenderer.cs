namespace MagicOperations.Components
{
    using Microsoft.AspNetCore.Components;

    public abstract class OperationListingRenderer : ComponentBase
    {
        [Inject] protected MagicOperationsConfiguration Configuration { get; init; } = default!;
    }
}
