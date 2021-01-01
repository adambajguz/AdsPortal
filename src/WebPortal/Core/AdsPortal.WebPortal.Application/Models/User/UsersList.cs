namespace AdsPortal.WebPortal.Application.Models.User
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [GetAllOperation(ResponseType = typeof(ListResult<UsersListItem>))]
    public class UsersList
    {

    }

    [OperationGroup(OperationGroups.User)]
    [GetPagedOperation(ResponseType = typeof(ListResult<UsersListItem>))]
    public class PagedUsersList
    {

    }

    public class UsersListItem
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = default!;
        public string Surname { get; init; } = default!;
    }
}
