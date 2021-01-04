namespace AdsPortal.WebPortal.Application.Models.Category
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.WebPortal.Application.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [DetailsOperation(ResponseType = typeof(CategoryDetails))]
    public class GetCategoryDetails
    {

    }

    [RenderableClass]
    public class CategoryDetails
    {
        public Guid Id { get; init; }

        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime LastSavedOn { get; init; }
        public Guid? LastSavedBy { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        [RenderablePropertyIgnore]
        public IList<CategoryAdvertisementsDetails> Advertisements { get; set; } = default!;
    }

    [RenderableClass]
    public class CategoryAdvertisementsDetails
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;
    }
}
