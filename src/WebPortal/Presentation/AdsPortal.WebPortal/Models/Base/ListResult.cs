namespace AdsPortal.WebPortal.Models.Base
{
    using System.Collections.Generic;
    using MagicModels.Attributes;
    using Newtonsoft.Json;

    public interface IListResult
    {
        [RenderableProperty(Mode = PropertyMode.Read)]
        int Count { get; }
    }

    [JsonObject]
    [RenderableClass]
    public class ListResult<TResultEntry> : IListResult
        where TResultEntry : class
    {
        [RenderableProperty(Mode = PropertyMode.Read)]
        public int Count { get; init; }

        [RenderableProperty(Mode = PropertyMode.Read)]
        public List<TResultEntry>? Entries { get; init; }
    }
}
