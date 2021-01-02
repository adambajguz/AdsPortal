namespace AdsPortal.WebPortal.Application.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [GetAllOperation(ResponseType = typeof(ListResult<AdvertisementsListItem>))]
    public class AdvertisementsList
    {

    }

    [OperationGroup(OperationGroups.Advertisement)]
    [GetPagedOperation(Action = "get-paged?page={Page}&per-page={EntiresPerPage}", ResponseType = typeof(ListResult<AdvertisementsListItem>))]
    public class PagedAdvertisementsList : GetPagedListQuery
    {

    }

    public class AdvertisementsListItem
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        public bool IsPublished { get; init; }
        public DateTime VisibleTo { get; init; }

        public Guid? CoverImageId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid AuthorId { get; init; }
    }
}
