namespace AdsPortal.ManagementUI.Models.Category
{
    using System;
    using AdsPortal.ManagementUI.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup("category")]
    [GetListOperation]
    public class GetCategoriesList : ListResult<CategoriesListItem>
    {

    }

    [OperationGroup("category")]
    [GetPagedListOperation]
    public class GetPagedCategoriesList : ListResult<CategoriesListItem>
    {

    }

    public class CategoriesListItem
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
    }
}
