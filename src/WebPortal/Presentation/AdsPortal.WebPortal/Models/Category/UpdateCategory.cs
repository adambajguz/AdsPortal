namespace AdsPortal.WebPortal.Models.Category
{
    using System;
    using AdsPortal.WebPortal.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [UpdateOperation]
    public class UpdateCategory
    {
        public Guid Id { get; set; }

        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
