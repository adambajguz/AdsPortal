namespace AdsPortal.WebPortal.Application.Models.User
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [UpdateOperation]
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
