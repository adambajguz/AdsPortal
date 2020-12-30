namespace AdsPortal.WebPortal.Application.Models.Category
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using AdsPortal.WebPortal.Application.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [GetAllOperation]
    public class CategoriesList : ListResult<CategoriesListItem>
    {

    }

    [OperationGroup(OperationGroups.Category)]
    [GetPagedOperation]
    public class PagedCategoriesList : ListResult<CategoriesListItem>
    {

    }

    public class CategoriesListItem
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }
}
