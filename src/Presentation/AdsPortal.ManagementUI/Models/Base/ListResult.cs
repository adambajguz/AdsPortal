namespace AdsPortal.ManagementUI.Models.Base
{
    using System.Collections.Generic;

    public abstract class ListResult<TResultEntry>
        where TResultEntry : class
    {
        public int Count { get; init; }
        public IReadOnlyList<TResultEntry>? Entries { get; init; }
    }
}
