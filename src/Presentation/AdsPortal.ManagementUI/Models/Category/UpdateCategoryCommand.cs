namespace AdsPortal.ManagementUI.Models.Category
{
    using System;
    using MagicOperations.Attributes;

    [OperationGroup("category")]
    [UpdateOperation]
    public class UpdateCategoryCommand
    {
        public Guid Id { get; init; }

        public string? Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
    }
}
