namespace MagicOperations.Components
{
    using System;
    using System.Threading.Tasks;
    using MagicOperations.Extensions;
    using MagicOperations.Interfaces;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Rendering;
    using Microsoft.AspNetCore.Components.Routing;

    public class MagicOperation : ComponentBase, IAsyncDisposable
    {
        [Inject] private IMagicRenderService RenderService { get; init; } = default!;
        [Inject] private NavigationManager NavManager { get; init; } = default!;

        [Parameter]
        public string? Path { get; set; }

        protected override void OnParametersSet()
        {
            NavManager.LocationChanged += LocationChanged;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (string.IsNullOrWhiteSpace(Path))
            {
                throw new MagicOperationsException($"{nameof(Path)} cannot be set to null or whitespace");
            }
            else
            {
                builder.AddContent(0, RenderService.RenderOperation(Path));
            }
        }

        private void LocationChanged(object? sender, LocationChangedEventArgs args)
        {
            Path = NavManager.GetCurrentPageUriWithQuery();
            StateHasChanged();
        }

        public ValueTask DisposeAsync()
        {
            NavManager.LocationChanged -= LocationChanged;

            return default;
        }
    }
}
