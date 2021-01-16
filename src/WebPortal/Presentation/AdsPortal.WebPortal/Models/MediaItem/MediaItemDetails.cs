namespace AdsPortal.WebPortal.Models.MediaItem
{
    using System;
    using AdsPortal.WebPortal.Models.User;

    public sealed class MediaItemDetails
    {
        public Guid Id { get; init; }

        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime LastSavedOn { get; init; }
        public Guid? LastSavedBy { get; init; }

        public string FileName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Alt { get; init; } = string.Empty;

        public string VirtualDirectory { get; init; } = string.Empty;

        public string Hash { get; init; } = string.Empty;
        public long ByteSize { get; init; }

        public Guid? OwnerId { get; init; }
        public Roles Role { get; init; }
    }
}
