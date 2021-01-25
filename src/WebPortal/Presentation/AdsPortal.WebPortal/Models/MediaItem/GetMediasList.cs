namespace AdsPortal.WebPortal.Models.MediaItem
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using AdsPortal.WebPortal.Models.User;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using MagicModels.Attributes;
    using MagicModels.Components.TablePropertyRenderers;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Media)]
    [GetAllOperation(ResponseType = typeof(ListResult<MediaItemsListItem>), DisplayName = "All media items")]
    public class GetMediasList
    {

    }

    [OperationGroup(OperationGroups.Media)]
    [GetPagedOperation(Action = "get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}",
                       ResponseType = typeof(PagedListResult<MediaItemsListItem>),
                       DefaultParameters = new[] { "0", "10" },
                       DisplayName = "All/Paged media items")]
    public class GetPagedMediasList : GetPagedListQuery
    {

    }

    [RenderableClass]
    public class MediaItemsListItem
    {
        [RenderableProperty(DisplayName = "Actions", Order = int.MaxValue, Renderer = typeof(TableManagementControlsRenderer))]
        public Guid Id { get; init; }

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string FileName { get; set; } = string.Empty;

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Description { get; set; } = string.Empty;

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Alt { get; set; } = string.Empty;

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string VirtualDirectory { get; set; } = string.Empty;

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string ContentType { get; set; } = string.Empty;

        [RenderableProperty(Renderer = typeof(TableAnyRenderer<long>))]
        public long ByteSize { get; set; }

        [RenderableProperty(Renderer = typeof(TableAnyRenderer<Guid?>))]
        public Guid? OwnerId { get; set; }

        [RenderableProperty(Renderer = typeof(TableAnyRenderer<Roles>))]
        public Roles Role { get; set; }
    }
}
