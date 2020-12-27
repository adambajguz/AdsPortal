namespace AdsPortal.ManagementUI.Models.Category
{
    using System;
    using MagicOperations.Attributes;

    [OperationGroup("category")]
    [DeleteOperation]
    public class DeleteCategoryCommand
    {
        public Guid Id { get; init; }
    }
}
