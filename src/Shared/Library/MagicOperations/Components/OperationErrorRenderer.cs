namespace MagicOperations.Components
{
    using Microsoft.AspNetCore.Components;

    public abstract class OperationErrorRenderer : ComponentBase
    {
        [Parameter]
        public string? Route { get; set; }
    }
}
