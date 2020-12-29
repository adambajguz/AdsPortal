namespace AdsPortal.ManagementUI.Models.Category
{
    using System;
    using AdsPortal.ManagementUI.Models.Base;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Category)]
    [GetAllOperation]
    public class GetCategoriesList : ListResult<CategoriesListItem>
    {

    }

    [OperationGroup(OperationGroups.Category)]
    [GetPagedOperation]
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
