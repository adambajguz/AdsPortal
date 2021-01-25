namespace AdsPortal.WebPortal.Models.MediaItem
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.User;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Media)]
    [DetailsOperation(ResponseType = typeof(MediaItemDetails), DisplayName = "Media item details", Action = "get-by-id/{Id}")]
    public class GetMediaItemDetails
    {
        public Guid Id { get; set; }
    }

    [RenderableClass]
    public sealed class MediaItemDetails
    {
        [RenderableProperty(DisplayName = "Actions", Renderer = typeof(DeleteControlRenderer))]
        public Guid Id { get; init; }

        [RenderableProperty(DisplayName = "Created on")]
        public DateTime CreatedOn { get; set; }

        [RenderableProperty(DisplayName = "Created by")]
        public Guid? CreatedBy { get; set; }

        [RenderableProperty(DisplayName = "Last saved on")]
        public DateTime LastSavedOn { get; set; }

        public Guid? LastSavedBy { get; set; }

        [RenderableProperty(DisplayName = "File name")]
        public string FileName { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Alt { get; init; } = string.Empty;

        [RenderableProperty(DisplayName = "Virtual directory")]
        public string VirtualDirectory { get; init; } = string.Empty;

        public string Hash { get; init; } = string.Empty;

        [RenderableProperty(DisplayName = "Byte size")]
        public long ByteSize { get; init; }

        [RenderableProperty(DisplayName = "Owner id")]
        public Guid? OwnerId { get; init; }
        public Roles Role { get; init; }
    }
}
