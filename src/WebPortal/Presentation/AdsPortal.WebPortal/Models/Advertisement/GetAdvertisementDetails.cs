namespace AdsPortal.WebPortal.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
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

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsPublished { get; set; }
        public DateTime VisibleTo { get; set; }

        public Guid? CoverImageId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
