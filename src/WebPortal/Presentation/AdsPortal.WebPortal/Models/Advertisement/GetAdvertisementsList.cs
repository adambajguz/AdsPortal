﻿namespace AdsPortal.WebPortal.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using MagicModels.Attributes;
    using MagicModels.Components.TablePropertyRenderers;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [GetAllOperation(ResponseType = typeof(ListResult<AdvertisementsListItem>))]
    public class GetAdvertisementsList
    {

    }

    [OperationGroup(OperationGroups.Advertisement)]
    [GetPagedOperation(Action = "get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}",
                       ResponseType = typeof(PagedListResult<AdvertisementsListItem>),
                       DefaultParameters = new[] { "0", "10" })]
    public class GetPagedAdvertisementsList : GetPagedListQuery
    {

    }

    [RenderableClass]
    public class AdvertisementsListItem
    {
        [RenderableProperty(DisplayName = "Actions", Order = int.MaxValue, Renderer = typeof(TableManagementControlsRenderer))]
        public Guid Id { get; set; }

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Title { get; set; } = string.Empty;

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Description { get; set; } = string.Empty;

        [RenderableProperty(Renderer = typeof(TableBoolRenderer))]
        public bool IsPublished { get; set; }

        [RenderableProperty(Renderer = typeof(TableDateTimeRenderer))]
        public DateTime VisibleTo { get; set; }

        [RenderableProperty(Renderer = typeof(TableAnyRenderer<>))]
        public Guid? CoverImageId { get; set; }

        [RenderableProperty(Renderer = typeof(TableAnyRenderer<>))]
        public Guid CategoryId { get; set; }

        [RenderableProperty(Renderer = typeof(TableAnyRenderer<Guid>))]
        public Guid AuthorId { get; set; }
    }
}