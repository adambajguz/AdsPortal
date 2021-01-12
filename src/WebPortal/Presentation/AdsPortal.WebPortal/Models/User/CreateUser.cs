﻿namespace AdsPortal.WebPortal.Models.User
{
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [CreateOperation(ResponseType = typeof(IdResult), DisplayName = "Create user")]
    public class CreateUser
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        public Roles Role { get; } = Roles.User;
    }
}
