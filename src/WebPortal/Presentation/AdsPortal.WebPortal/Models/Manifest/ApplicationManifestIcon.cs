namespace AdsPortal.WebPortal.Models.Manifest
{
    using System.Text.Json.Serialization;

    public class ApplicationManifestIcon
    {
        [JsonPropertyName("src")]
        public string? Src { get; init; }

        [JsonPropertyName("type")]
        public string? Type { get; init; }

        [JsonPropertyName("sizes")]
        public string? Sizes { get; init; }
    }
}
