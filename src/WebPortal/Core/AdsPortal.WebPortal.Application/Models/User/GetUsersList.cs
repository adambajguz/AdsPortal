namespace AdsPortal.WebPortal.Application.Models.User
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [GetAllOperation(ResponseType = typeof(ListResult<UsersListItem>))]
    public class GetUsersList
    {

    }

    [OperationGroup(OperationGroups.User)]
    [GetPagedOperation(Action = "get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}",
                       ResponseType = typeof(PagedListResult<UsersListItem>),
                       DefaultParameters = new[] { "0", "10" })]
    public class Get : GetPagedListQuery
    {

    }

    [RenderableClass]
    public class UsersListItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;
    }
}
