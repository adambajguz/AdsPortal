namespace AdsPortal.Shared.Infrastructure.Common.Caching
{
    using System;
    using AdsPortal.Shared.Common.Cache;

    public sealed class CustomCacheEntry : CacheEntryConfig, ICustomCacheEntry
    {
        private object? _value;

        //
        // Summary:
        //     Gets or set the value of the cache entry.
        public object? Value
        {
            get => _value;
            set
            {
                _value = value;
                SynchronizationId = Guid.NewGuid();
            }
        }

        //
        // Summary:
        //     Gets the value of synchronization id.
        public Guid SynchronizationId { get; private set; } = Guid.Empty;

        //
        // Summary:
        //     Gets last set in cache date for the cache entry.
        public DateTimeOffset? LastSetOn { get; internal set; }

        public CustomCacheEntry()
        {

        }

        public CustomCacheEntry(ICacheEntryConfig cacheEntryConfig)
        {
            Key = cacheEntryConfig.Key;
            ExtendedKey = cacheEntryConfig.ExtendedKey;
            ExtendedKeyMode = cacheEntryConfig.ExtendedKeyMode;
            AbsoluteExpiration = cacheEntryConfig.AbsoluteExpiration;
            AbsoluteExpirationRelativeToNow = cacheEntryConfig.AbsoluteExpirationRelativeToNow;
            SlidingExpiration = cacheEntryConfig.SlidingExpiration;
        }
    }
}