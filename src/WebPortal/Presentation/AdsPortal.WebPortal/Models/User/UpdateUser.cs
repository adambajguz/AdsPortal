namespace AdsPortal.WebPortal.Models.User
{
    using System;
    using AdsPortal.WebPortal.Models;
    using MagicOperations.Attributes;
    using Newtonsoft.Json;

    [OperationGroup(OperationGroups.User)]
    [UpdateOperation(DisplayName = "Update user")]
    public class UpdateUser
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }

        public Roles Role { get; set; }
    }
}
