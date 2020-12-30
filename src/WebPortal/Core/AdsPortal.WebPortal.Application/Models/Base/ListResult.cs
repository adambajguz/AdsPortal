namespace AdsPortal.WebPortal.Application.Models.Base
{
    using System.Collections.Generic;

    public class ListResult<TResultEntry>
        where TResultEntry : class
    {
        public int Count { get; init; }
        public IReadOnlyList<TResultEntry>? Entries { get; init; }
    }
}
