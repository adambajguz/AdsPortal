namespace AdsPortal.WebPortal.Application.Models.Category
{
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [CreateOperation(ResponseType = typeof(IdResult))]
    public class CreateCategory
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
