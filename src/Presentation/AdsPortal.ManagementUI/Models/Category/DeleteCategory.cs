namespace AdsPortal.ManagementUI.Models.Category
{
    using System;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [DeleteOperation]
    public class DeleteCategory
    {
        public Guid Id { get; init; }
    }
}
