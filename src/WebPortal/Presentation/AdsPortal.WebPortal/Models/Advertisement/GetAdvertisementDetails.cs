namespace AdsPortal.WebPortal.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers.Image;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [DetailsOperation(ResponseType = typeof(AdvertisementDetails), DisplayName = "Advertisement details")]
    public class GetAdvertisementDetails
    {
        public Guid Id { get; set; }
    }

    [RenderableClass]
    public class AdvertisementDetails
    {
        [RenderableProperty(DisplayName = "Actions", Renderer = typeof(DetailsControlsRenderer))]
        public Guid Id { get; set; }

        [RenderableProperty(DisplayName = "Created on")]
        public DateTime CreatedOn { get; set; }

        [RenderableProperty(DisplayName = "Created by")]
        public Guid? CreatedBy { get; set; }

        [RenderableProperty(DisplayName = "Last saved on")]
        public DateTime LastSavedOn { get; set; }

        [RenderableProperty(DisplayName = "Last saved by")]
        public Guid? LastSavedBy { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [RenderableProperty(DisplayName = "Is published")]
        public bool IsPublished { get; set; }

        [RenderableProperty(DisplayName = "Visible to")]
        public DateTime VisibleTo { get; set; }

        [RenderableProperty(DisplayName = "Cover", Renderer = typeof(ImageFromFileResponseRenderer))]
        public FileResponse? CoverImage { get; init; }

        [RenderableProperty(DisplayName = "Category")]
        public Guid CategoryId { get; set; }

        [RenderableProperty(DisplayName = "Author")]
        public Guid AuthorId { get; set; }
    }
}
