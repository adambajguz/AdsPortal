namespace AdsPortal.Shared.Common.Cache
{
    using System;

    public interface ICacheEntryConfig
    {
        DateTimeOffset? AbsoluteExpiration { get; set; }
        TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        DateTimeOffset CreatedOn { get; }
        object? ExtendedKey { get; set; }
        CacheExtendedKeyModes ExtendedKeyMode { get; set; }
        string? Key { get; set; }
        TimeSpan? SlidingExpiration { get; set; }

        string GetFullKey();
    }
}