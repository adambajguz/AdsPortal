namespace AdsPortal.ManagementUI.Models.Advertisement
{
    using System;
    using AdsPortal.ManagementUI.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [UpdateOperation]
    public class UpdateAdvertisement
    {
        public Guid Id { get; init; }

        public string? Title { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public bool IsPublished { get; init; }
        public DateTime VisibleTo { get; init; }

        public Guid? CoverImageId { get; init; }
        public Guid CategoryId { get; init; }
        public Guid AuthorId { get; init; }
    }
}
