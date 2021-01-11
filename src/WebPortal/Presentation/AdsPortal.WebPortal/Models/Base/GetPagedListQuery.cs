namespace AdsPortal.WebPortal.Models.Base
{
    public abstract class GetPagedListQuery
    {
        public int Page { get; init; }
        public int EntiresPerPage { get; init; }
    }
}
