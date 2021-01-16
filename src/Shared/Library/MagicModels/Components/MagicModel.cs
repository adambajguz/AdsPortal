namespace MagicModels.Components
{
    using MagicModels.Interfaces;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Rendering;

    public class MagicModel : ComponentBase
    {
        [Inject] private IModelRenderService RenderService { get; init; } = default!;

        [Parameter] public object? Model { get; init; }

        [Parameter] public object? Context { get; init; }

        [Parameter] public bool IsWrite { get; init; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            RenderFragment fragment = RenderService.RenderModel(Model, Context, IsWrite);
            builder.AddContent(0, fragment);
        }
    }
}
