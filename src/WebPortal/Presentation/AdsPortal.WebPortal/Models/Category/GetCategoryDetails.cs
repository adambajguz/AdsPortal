namespace AdsPortal.WebPortal.Models.Category
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using MagicModels.Attributes;
    using MagicModels.Components.TablePropertyRenderers;
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

        [RenderableProperty(DisplayName = "Created on")]
        public DateTime CreatedOn { get; set; }

        [RenderableProperty(DisplayName = "Created by")]
        public Guid? CreatedBy { get; set; }

        [RenderableProperty(DisplayName = "Last saved on")]
        public DateTime LastSavedOn { get; set; }

        [RenderableProperty(DisplayName = "Last saved by")]
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public IList<CategoryAdvertisementsDetails> Advertisements { get; set; } = default!;
    }

    [RenderableClass]
    public class CategoryAdvertisementsDetails
    {
        [RenderableProperty(DisplayName = "Actions", Order = int.MaxValue, Renderer = typeof(TableManagementControlsRenderer))]
        public Guid Id { get; set; }

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Title { get; set; } = string.Empty;
    }
}
