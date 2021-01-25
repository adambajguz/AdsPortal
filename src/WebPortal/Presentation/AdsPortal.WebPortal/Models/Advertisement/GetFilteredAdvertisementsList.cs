namespace AdsPortal.WebPortal.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using AdsPortal.WebPortal.Shared.Components.OperationRenderers;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers.Image;
    using MagicModels.Attributes;
    using MagicModels.Components.TablePropertyRenderers;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [GetAllOperation(ResponseType = typeof(ListResult<FilteredAdvertisementsListItem>), Action = "get-all/filtered", DisplayName = "Filtered advertisements")]
    public class GetFilteredAdvertisementsList
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Visible { get; set; } = true;
    }

    [OperationGroup(OperationGroups.Advertisement)]
    [GetPagedOperation(Action = "get-paged/filtered?Page={Page}&EntiresPerPage={EntiresPerPage}",
                       OperationRenderer = typeof(GetFilteredPagedAdvertisementsRenderer),
                       ResponseType = typeof(PagedListResult<FilteredAdvertisementsListItem>),
                       DefaultParameters = new[] { "0", "10" },
                       DisplayName = "Filtered and paged advertisements")]
    public class GetFilteredPagedAdvertisementsList : GetPagedListQuery
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Visible { get; set; } = true;
    }

    [RenderableClass]
    public class FilteredAdvertisementsListItem
    {
        [RenderableProperty(DisplayName = "Actions", Order = int.MaxValue, Renderer = typeof(TableManagementDetailsControlRenderer))]
        public Guid Id { get; set; }

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Title { get; set; } = string.Empty;

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Description { get; set; } = string.Empty;

        [RenderablePropertyIgnore]
        public bool IsPublished { get; set; }

        [RenderablePropertyIgnore]
        public DateTime VisibleTo { get; set; }

        [RenderableProperty(DisplayName = "Cover", Renderer = typeof(TableImageFromFileResponseRenderer))]
        public FileResponse? CoverImage { get; init; }

        [RenderableProperty(DisplayName = "Category", Renderer = typeof(TableAnyRenderer<Guid>))]
        public Guid CategoryId { get; set; }

        [RenderableProperty(DisplayName = "Author", Renderer = typeof(TableAnyRenderer<Guid>))]
        public Guid AuthorId { get; set; }
    }
}
