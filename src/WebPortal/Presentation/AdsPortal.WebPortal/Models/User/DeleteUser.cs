namespace AdsPortal.WebPortal.Models.User
{
    using System;
    using AdsPortal.WebPortal.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [DeleteOperation]
    public class DeleteUser
    {
        public Guid Id { get; set; }
    }
}
