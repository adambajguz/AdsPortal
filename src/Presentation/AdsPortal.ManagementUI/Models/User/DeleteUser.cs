namespace AdsPortal.ManagementUI.Models.User
{
    using System;
    using AdsPortal.ManagementUI.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [DeleteOperation]
    public class DeleteUser
    {
        public Guid Id { get; init; }
    }
}
