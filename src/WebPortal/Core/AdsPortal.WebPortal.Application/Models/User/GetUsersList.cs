﻿namespace AdsPortal.WebPortal.Application.Models.User
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [GetAllOperation(ResponseType = typeof(ListResult<UsersListItem>))]
    public class GetUsersList
    {

    }

    [OperationGroup(OperationGroups.User)]
    [GetPagedOperation(Action = "get-paged?page={Page}&per-page={EntiresPerPage}", ResponseType = typeof(ListResult<UsersListItem>))]
    public class Get : GetPagedListQuery
    {

    }

    public class UsersListItem
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = default!;
        public string Surname { get; init; } = default!;
    }
}