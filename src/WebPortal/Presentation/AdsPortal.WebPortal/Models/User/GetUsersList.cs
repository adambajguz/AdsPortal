namespace AdsPortal.WebPortal.Models.User
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using MagicModels.Attributes;
    using MagicModels.Components.TablePropertyRenderers;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.User)]
    [GetAllOperation(ResponseType = typeof(ListResult<UsersListItem>), DisplayName = "All users")]
    public class GetUsersList
    {

    }

    [OperationGroup(OperationGroups.User)]
    [GetPagedOperation(Action = "get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}",
                       ResponseType = typeof(PagedListResult<UsersListItem>),
                       DefaultParameters = new[] { "0", "10" },
                       DisplayName = "All/paged users")]
    public class Get : GetPagedListQuery
    {

    }

    [RenderableClass]
    public class UsersListItem
    {
        [RenderableProperty(DisplayName = "Actions", Order = int.MaxValue, Renderer = typeof(TableManagementControlsRenderer))]
        public Guid Id { get; set; }

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Name { get; set; } = default!;

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Surname { get; set; } = default!;
    }
}
