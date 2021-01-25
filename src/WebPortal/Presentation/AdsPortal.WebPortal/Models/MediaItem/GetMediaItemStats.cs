namespace AdsPortal.WebPortal.Models.MediaItem
{
    using AdsPortal.WebPortal.Models;
    using MagicModels.Attributes;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Media)]
    [DetailsOperation(ResponseType = typeof(MediaItemStats), DisplayName = "Media item stats", Action = "statistics")]
    public class GetMediaItemStats
    {

    }

    [RenderableClass]
    public sealed class MediaItemStats
    {
        public int Count { get; init; }

        [RenderableProperty(DisplayName = "Total byte size")]
        public long TotalByteSize { get; init; }

        [RenderableProperty(DisplayName = "Min byte size")]
        public long MinByteSize { get; init; }

        [RenderableProperty(DisplayName = "Average byte size")]
        public double AverageByteSize { get; init; }

        [RenderableProperty(DisplayName = "Max byte size")]
        public long MaxByteSize { get; init; }

    }
}
