namespace AdsPortal.WebPortal.Application.Models.Category
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [GetAllOperation(ResponseType = typeof(ListResult<CategoriesListItem>))]
    public class CategoriesList
    {

    }

    [OperationGroup(OperationGroups.Category)]
    [GetPagedOperation(ResponseType = typeof(ListResult<CategoriesListItem>))]
    public class PagedCategoriesList
    {

    }

    public class CategoriesListItem
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }
}
