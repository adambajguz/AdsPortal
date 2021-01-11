namespace AdsPortal.WebPortal.Models.Base
{
    public class PagedListResult<TResultEntry> : ListResult<TResultEntry>
        where TResultEntry : class
    {
        public int CurrentPageNumber { get; init; }
        public int EntiresPerPage { get; init; }
        public int LastPage { get; init; }

        public int Seen { get; init; }
        public int Left { get; init; }
        public int TotalCount { get; init; }
    }
}
