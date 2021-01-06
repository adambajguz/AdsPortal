namespace AdsPortal.WebPortal.Application.Models.Base
{
    using System.Collections.Generic;
    using MagicModels.Attributes;
    using Newtonsoft.Json;

    [JsonObject]
    [RenderableClass]
    public class ListResult<TResultEntry>
        where TResultEntry : class
    {
        public int Count { get; init; }
        public List<TResultEntry>? Entries { get; init; }
    }
}
