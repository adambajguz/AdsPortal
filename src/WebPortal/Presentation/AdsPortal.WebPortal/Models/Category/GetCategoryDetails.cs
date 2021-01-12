namespace AdsPortal.WebPortal.Models.Category
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [DetailsOperation(ResponseType = typeof(CategoryDetails), DisplayName = "Category details")]
    public class GetCategoryDetails
    {
        public Guid Id { get; set; }
    }

    [RenderableClass]
    public class CategoryDetails
    {
        [RenderableProperty(DisplayName = "Actions", Renderer = typeof(DetailsControlsRenderer))]
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public IList<CategoryAdvertisementsDetails> Advertisements { get; set; } = default!;
    }

    [RenderableClass]
    public class CategoryAdvertisementsDetails
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
    }
}
