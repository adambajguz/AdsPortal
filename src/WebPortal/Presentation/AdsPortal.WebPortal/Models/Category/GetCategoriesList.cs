namespace AdsPortal.WebPortal.Models.Category
{
    using System;
    using AdsPortal.WebPortal.Models;
    using AdsPortal.WebPortal.Models.Base;
    using AdsPortal.WebPortal.Shared.Components.PropertyRenderers;
    using MagicModels.Attributes;
    using MagicModels.Components.TablePropertyRenderers;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [GetAllOperation(ResponseType = typeof(ListResult<CategoriesListItem>))]
    public class GetCategoriesList
    {

    }

    [OperationGroup(OperationGroups.Category)]
    [GetPagedOperation(Action = "get-paged?Page={Page}&EntiresPerPage={EntiresPerPage}",
                       ResponseType = typeof(PagedListResult<CategoriesListItem>),
                       DefaultParameters = new[] { "0", "10" })]
    public class GetPagedCategoriesList : GetPagedListQuery
    {

    }

    [RenderableClass]
    public class CategoriesListItem
    {
        [RenderableProperty(DisplayName = "Actions", Order = int.MaxValue, Renderer = typeof(ManagementControlsRenderer))]
        public Guid Id { get; set; }

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Name { get; set; } = string.Empty;

        [RenderableProperty(Renderer = typeof(TableStringRenderer))]
        public string Description { get; set; } = string.Empty;
    }
}
