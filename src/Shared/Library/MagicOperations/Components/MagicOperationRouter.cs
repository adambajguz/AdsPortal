namespace MagicOperations.Components
{
    using System;
    using System.Threading.Tasks;
    using MagicOperations.Extensions;
    using MagicOperations.Interfaces;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Rendering;
    using Microsoft.AspNetCore.Components.Routing;

    public class MagicOperationRouter : ComponentBase, IAsyncDisposable
    {
        [Inject] private IMagicRenderService RenderService { get; init; } = default!;
        [Inject] private NavigationManager NavManager { get; init; } = default!;

        public string? Path { get; private set; }

        protected override void OnParametersSet()
        {
            NavManager.LocationChanged += LocationChanged;
            Path = NavManager.GetCurrentPageUriWithQuery();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            RenderFragment fragment = RenderService.RenderOperation(Path);
            builder.AddContent(0, fragment);
        }

        private async void LocationChanged(object? sender, LocationChangedEventArgs args)
        {
            Path = NavManager.GetCurrentPageUriWithQuery();
            await InvokeAsync(StateHasChanged);
        }

        public ValueTask DisposeAsync()
        {
            NavManager.LocationChanged -= LocationChanged;

            return default;
        }
    }
}
