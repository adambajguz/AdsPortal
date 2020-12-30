namespace AdsPortal.WebPortal.Application.Models.Category
{
    using AdsPortal.WebPortal.Application.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [CreateOperation]
    public class CreateCategory
    {
        public string? Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
    }
}
