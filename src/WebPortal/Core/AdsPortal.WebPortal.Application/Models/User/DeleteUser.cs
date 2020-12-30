namespace AdsPortal.WebPortal.Application.Models.User
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [DeleteOperation]
    public class DeleteUser
    {
        public Guid Id { get; init; }
    }
}
