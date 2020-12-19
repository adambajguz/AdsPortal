namespace MagicCRUD.Components
{
    using MagicCRUD.Configurations;
    using Microsoft.AspNetCore.Components;

    public partial class MagicCRUDRouter : ComponentBase
    {
        [Parameter]
        public string? Entity { get; set; }

        [Parameter]
        public string? Operation { get; set; }

        [Parameter]
        public string? Args { get; set; }

        private bool IsEntityListing => string.IsNullOrWhiteSpace(Entity);

        [Inject] private MagicCRUDConfiguration Configuration { get; init; } = default!;

        public MagicCRUDRouter()
        {

        }
    }
}
