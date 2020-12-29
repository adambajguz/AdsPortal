namespace AdsPortal.ManagementUI.Models.Category
{
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [CreateOperation]
    public class CreateCategoryCommand
    {
        public string? Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
    }
}
