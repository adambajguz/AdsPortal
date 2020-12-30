namespace AdsPortal.WebPortal.Application.Models.Base
{
    using MagicOperations.Attributes;

    [OperationGroup("category")]
    public class GetPagedListQuery
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }
    }
}
