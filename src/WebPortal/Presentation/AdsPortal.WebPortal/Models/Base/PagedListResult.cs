namespace AdsPortal.WebPortal.Models.Base
{
    using MagicModels.Attributes;

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
        [RenderableProperty(DisplayName = "Current page number")]
        public int CurrentPageNumber { get; init; }

        [RenderableProperty(DisplayName = "Entires per page")]
        public int EntiresPerPage { get; init; }

        [RenderableProperty(DisplayName = "Last page")]
        public int LastPage { get; init; }

        public int Seen { get; init; }
        public int Left { get; init; }

        [RenderableProperty(DisplayName = "Total entries")]
        public int TotalCount { get; init; }
    }
}
