namespace AdsPortal.WebPortal.Application.Models.User
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
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
