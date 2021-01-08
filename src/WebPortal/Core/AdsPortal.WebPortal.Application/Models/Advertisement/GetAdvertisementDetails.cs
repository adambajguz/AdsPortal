﻿namespace AdsPortal.WebPortal.Application.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [DetailsOperation(ResponseType = typeof(AdvertisementDetails))]
    public class GetAdvertisementDetails
    {

    }

    [RenderableClass]
    public class AdvertisementDetails
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsPublished { get; set; }
        public DateTime VisibleTo { get; set; }

        public Guid? CoverImageId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
