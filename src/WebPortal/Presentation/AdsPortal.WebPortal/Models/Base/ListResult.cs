namespace AdsPortal.WebPortal.Models.Base
{
    using System.Collections.Generic;
    using MagicModels.Attributes;
    using Newtonsoft.Json;

    public interface IListResult
    {
        int Count { get; }
    }

    [JsonObject]
    [RenderableClass]
    public class ListResult<TResultEntry> : IListResult
        where TResultEntry : class
    {
        public int Count { get; init; }
        public List<TResultEntry>? Entries { get; init; }
    }
}
