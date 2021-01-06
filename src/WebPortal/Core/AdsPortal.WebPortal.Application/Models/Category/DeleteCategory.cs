namespace AdsPortal.WebPortal.Application.Models.Category
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [DeleteOperation(Action = "delete/{Id}", DefaultParameters = new[] { "00000000-0000-0000-0000-000000000011" })]
    public class DeleteCategory
    {
        public Guid Id { get; init; }
    }
}
