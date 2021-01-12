namespace AdsPortal.WebPortal.Models.User
{
    using System;
    using AdsPortal.WebPortal.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [UpdateOperation(DisplayName = "Update user")]
    public class UpdateUser
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public Roles Role { get; set; }
    }
}
