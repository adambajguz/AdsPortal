namespace AdsPortal.WebPortal.Application.Models.Category
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.WebPortal.Application.Models;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [DetailsOperation(ResponseType = typeof(CategoryDetails))]
    public class GetCategoryDetails
    {

    }

    [RenderableClass]
    public class CategoryDetails
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public IList<CategoryAdvertisementsDetails> Advertisements { get; set; } = default!;
    }

    [RenderableClass]
    public class CategoryAdvertisementsDetails
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;
    }
}
