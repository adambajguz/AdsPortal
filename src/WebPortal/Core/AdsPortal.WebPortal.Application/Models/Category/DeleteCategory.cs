namespace AdsPortal.WebPortal.Application.Models.Category
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [DeleteOperation(Action = "delete/{Id}")]
    public class DeleteCategory
    {
        public Guid Id { get; init; }
    }
}
