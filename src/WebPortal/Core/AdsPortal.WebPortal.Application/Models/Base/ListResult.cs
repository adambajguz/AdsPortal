namespace AdsPortal.WebPortal.Application.Models.Base
{
    using System.Collections.Generic;
    using MagicOperations.Attributes;
    using Newtonsoft.Json;

    [JsonObject]
    [RenderableClass]
    public class ListResult<TResultEntry>
        where TResultEntry : class
    {
        public int Count { get; init; }
        public List<TResultEntry>? Entries { get; init; }

        //public IEnumerator<TResultEntry> GetEnumerator()
        //{
        //    return Entries?.GetEnumerator() ?? Enumerable.Empty<TResultEntry>().GetEnumerator();
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return Entries?.GetEnumerator() ?? Enumerable.Empty<TResultEntry>().GetEnumerator();
        //}
    }
}
