namespace AdsPortal.WebPortal.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [CreateOperation(ResponseType = typeof(IdResult), DisplayName = "Create advertisement")]
    public class CreateAdvertisement
    {
        public string? Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public bool IsPublished { get; set; }
        public DateTime VisibleTo { get; set; }

        public Guid? CoverImageId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
