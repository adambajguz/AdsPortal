namespace AdsPortal.WebPortal.Models
{
    using System;

    public sealed class FileResponse
    {
        public Guid Id { get; init; }
        public string Path { get; init; } = string.Empty;
        public string? Alt { get; init; }
        public string? Title { get; set; }
    }
}
