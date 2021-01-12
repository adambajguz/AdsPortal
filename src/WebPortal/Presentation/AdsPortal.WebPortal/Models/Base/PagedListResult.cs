namespace AdsPortal.WebPortal.Models.Base
{
    public interface IPagedListResult : IListResult
    {
        int CurrentPageNumber { get; }
        int EntiresPerPage { get; }
        int LastPage { get; }

        int Seen { get; }
        int Left { get; }
        int TotalCount { get; }
    }

    public class PagedListResult<TResultEntry> : ListResult<TResultEntry>, IPagedListResult
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
