namespace MagicOperations.Components
{
    using MagicOperations.Services;
    using Microsoft.AspNetCore.Components;

    public abstract class OperationRenderer : ComponentBase
    {
        [Parameter]
        public object? Model { get; init; }

        [Inject] public MagicApiService MagicApi { get; init; } = default!;
    }
}
