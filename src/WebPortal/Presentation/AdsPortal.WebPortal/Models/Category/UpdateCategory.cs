namespace AdsPortal.WebPortal.Models.Category
{
    using System;
    using AdsPortal.WebPortal.Models;
    using MagicOperations.Attributes;
    using Newtonsoft.Json;

    [OperationGroup(OperationGroups.Category)]
    [UpdateOperation(DisplayName = "Update category")]
    public class UpdateCategory
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
