namespace AdsPortal.Domain.Entities
{
    using System;
    using AdsPortal.Domain.Abstractions.Base;

    public class AnalyticsRecord : IBaseMongoEntity
    {
        public Guid Id { get; set; }

        public long Hash { get; set; }

        public DateTime Timestamp { get; set; }
        public string Uri { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public bool IsValid { get; set; }
        public ulong Visits { get; set; }
    }
}
