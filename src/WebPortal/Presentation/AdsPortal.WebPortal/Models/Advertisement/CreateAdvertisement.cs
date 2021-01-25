namespace AdsPortal.WebPortal.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers.Selects;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [CreateOperation(ResponseType = typeof(IdResult), DisplayName = "Create advertisement")]
    public class CreateAdvertisement
    {
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;

        [RenderableProperty(DisplayName = "Is published")]
        public bool IsPublished { get; set; }

        [RenderableProperty(DisplayName = "Visible to")]
        public DateTime VisibleTo { get; set; }

        [RenderableProperty(DisplayName = "Cover", Renderer = typeof(MediaItemSelectRenderer))]
        public Guid? CoverImageId { get; set; }

        [RenderableProperty(DisplayName = "Category", Renderer = typeof(CategorySelectRenderer))]
        public Guid CategoryId { get; set; }

        [RenderableProperty(DisplayName = "Author", Renderer = typeof(UserIdRenderer))]
        public Guid AuthorId { get; set; }
    }
}
