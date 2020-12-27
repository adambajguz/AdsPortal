namespace AdsPortal.ManagementUI.Models.Base
{
    using System.Collections.Generic;

    public abstract class PagedListResult<TResultEntry>
        where TResultEntry : class
    {
        public int CurrentPageNumber { get; init; }
        public int EntiresPerPage { get; init; }
        public int LastPage { get; init; }

        public int Seen { get; init; }
        public int Count { get; init; }
        public int Left { get; init; }
        public int TotalCount { get; init; }

        public IReadOnlyList<TResultEntry>? Entries { get; }
    }
}
