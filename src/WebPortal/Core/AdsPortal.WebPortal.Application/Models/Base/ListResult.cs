namespace AdsPortal.WebPortal.Application.Models.Base
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    [JsonObject]
    public class ListResult<TResultEntry> : IEnumerable<TResultEntry>
        where TResultEntry : class
    {
        public int Count { get; init; }
        public List<TResultEntry>? Entries { get; init; }

        public IEnumerator<TResultEntry> GetEnumerator()
        {
            return Entries?.GetEnumerator() ?? Enumerable.Empty<TResultEntry>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entries?.GetEnumerator() ?? Enumerable.Empty<TResultEntry>().GetEnumerator();
        }
    }
}
