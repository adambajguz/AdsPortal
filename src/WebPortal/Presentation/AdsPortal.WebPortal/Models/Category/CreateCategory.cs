namespace AdsPortal.WebPortal.Models.Category
{
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [CreateOperation(ResponseType = typeof(IdResult), DisplayName = "Create category")]
    public class CreateCategory
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
