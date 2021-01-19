namespace AdsPortal.WebPortal.Models.Manifest
{
    using System;
    using System.Text.Json.Serialization;

    public class ApplicationManifest
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("short_name")]
        public string? ShortName { get; init; }

        [JsonPropertyName("start_url")]
        public string? StartUrl { get; init; }

        [JsonPropertyName("display")]
        public string? Display { get; init; }

        [JsonPropertyName("background_color")]
        public string? BackgroundColor { get; init; }

        [JsonPropertyName("theme_color")]
        public string? ThemeColor { get; init; }

        public ApplicationManifestIcon[] Icons { get; init; } = Array.Empty<ApplicationManifestIcon>();
    }
}
