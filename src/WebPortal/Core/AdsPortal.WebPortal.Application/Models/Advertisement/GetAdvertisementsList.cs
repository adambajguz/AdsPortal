namespace AdsPortal.WebPortal.Application.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicModels.Attributes;
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
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsPublished { get; set; }
        public DateTime VisibleTo { get; set; }

        public Guid? CoverImageId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
