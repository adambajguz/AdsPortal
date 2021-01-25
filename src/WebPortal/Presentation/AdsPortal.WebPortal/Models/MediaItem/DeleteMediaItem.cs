namespace AdsPortal.WebPortal.Models.MediaItem
{
    using System;
    using AdsPortal.WebPortal.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Media)]
    [DeleteOperation(DisplayName = "Delete media item")]
    public class DeleteMediaItem
    {
        public Guid Id { get; set; }
    }
}
