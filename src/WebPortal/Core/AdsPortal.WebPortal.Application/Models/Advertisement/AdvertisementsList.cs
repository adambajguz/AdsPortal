namespace AdsPortal.WebPortal.Application.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [GetAllOperation]
    public class AdvertisementsList : ListResult<AdvertisementsListItem>
    {

    }

    [OperationGroup(OperationGroups.Advertisement)]
    [GetPagedOperation]
    public class PagedAdvertisementsList : ListResult<AdvertisementsListItem>
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
