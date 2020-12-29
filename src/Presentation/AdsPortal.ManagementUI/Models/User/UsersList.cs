namespace AdsPortal.ManagementUI.Models.User
{
    using System;
    using AdsPortal.ManagementUI.Models;
    using AdsPortal.ManagementUI.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [GetAllOperation]
    public class UsersList : ListResult<UsersListItem>
    {

    }

    [OperationGroup(OperationGroups.User)]
    [GetPagedOperation]
    public class PagedUsersList : ListResult<UsersListItem>
    {

    }

    public class UsersListItem
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = default!;
        public string Surname { get; init; } = default!;
    }
}
