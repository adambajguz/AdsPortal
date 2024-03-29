﻿namespace AdsPortal.WebPortal.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers.Selects;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;
    using Newtonsoft.Json;

    [OperationGroup(OperationGroups.Advertisement)]
    [UpdateOperation(DisplayName = "Update advertisement")]
    public class UpdateAdvertisement
    {
        [JsonIgnore]
        public Guid Id { get; set; }

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

        [RenderableProperty(DisplayName = "Author")]
        public Guid AuthorId { get; set; }
    }
}
