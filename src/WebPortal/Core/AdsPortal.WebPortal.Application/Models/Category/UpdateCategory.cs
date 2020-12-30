namespace AdsPortal.WebPortal.Application.Models.Category
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [UpdateOperation]
    public class UpdateCategory
    {
        public Guid Id { get; init; }

        public string? Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
    }
}
