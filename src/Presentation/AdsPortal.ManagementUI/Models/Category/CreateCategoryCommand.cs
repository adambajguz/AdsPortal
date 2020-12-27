namespace AdsPortal.ManagementUI.Models.Category
{
    using MagicOperations.Attributes;

    [OperationGroup("category")]
    [CreateOperation]
    public class CreateCategoryCommand
    {
        public string? Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
    }
}
