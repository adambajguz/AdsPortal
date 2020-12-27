namespace AdsPortal.ManagementUI.Models.Category
{
    using System;
    using System.Collections.Generic;
    using MagicOperations.Attributes;

    [OperationGroup("category")]
    [GetDetailsOperation]
    public class GetCategoryDetailsResponse
    {
        public Guid Id { get; init; }

        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime LastSavedOn { get; init; }
        public Guid? LastSavedBy { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        public IList<GetCategoryAdvertisementsDetailsResponse> Advertisements { get; set; } = default!;
    }

    public class GetCategoryAdvertisementsDetailsResponse
    {
        public Guid Id { get; init; }

        public string Title { get; init; } = string.Empty;
    }
}
