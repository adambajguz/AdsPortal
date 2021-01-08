namespace AdsPortal.WebPortal.Application.Models.Category
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicModels.Attributes;
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
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
