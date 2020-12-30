namespace AdsPortal.Infrastructure.Common.Caching
{
    using System;
    using AdsPortal.Common.Cache;

    public class CacheEntryConfig : ICacheEntryConfig
    {
        //
        // Summary:
        //     Gets or sets the key of the cache entry.
        public string? Key { get; set; }

        //
        // Summary:
        //     Gets or sets extra data that is and appended to the key of the cache entry. If null ExtendedKey is not appended.
        public object? ExtendedKey { get; set; }

        //
        // Summary:
        //     Gets or sets which mode/method is used to get string representation of ExtendedKey.
        public CacheExtendedKeyModes ExtendedKeyMode { get; set; } = CacheExtendedKeyModes.UseGetHashCode;

        //
        // Summary:
        //     Gets creation date for the cache entry.
        public DateTimeOffset CreatedOn { get; } = DateTimeOffset.Now;

        //
        // Summary:
        //     Gets or sets an absolute expiration date for the cache entry.
        public DateTimeOffset? AbsoluteExpiration { get; set; }

        //
        // Summary:
        //     Gets or sets an absolute expiration time, relative to now.
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

        //
        // Summary:
        //     Gets or sets how long a cache entry can be inactive (e.g. not accessed) before
        //     it will be removed. This will not extend the entry lifetime beyond the absolute
        //     expiration (if set).
        public TimeSpan? SlidingExpiration { get; set; }

        public string GetFullKey()
        {
            return GetFullKey(Key, ExtendedKey, ExtendedKeyMode);
        }

        public static string ExtendedKeySeperator { get; } = "$$EXT$$";

        public static string GetFullKey(string? key, object? extendedKey, CacheExtendedKeyModes keyExtraMode)
        {
            string fullKey = key ?? throw new ArgumentNullException(nameof(key));
            if (extendedKey != null)
            {
                fullKey += ExtendedKeySeperator;
                switch (keyExtraMode)
                {
                    case CacheExtendedKeyModes.UseToString:
                        fullKey += extendedKey.ToString();
                        break;
                    case CacheExtendedKeyModes.UseGetHashCode:
                        fullKey += extendedKey.GetHashCode().ToString();
                        break;
                    case CacheExtendedKeyModes.Serialize:
                        fullKey += System.Text.Json.JsonSerializer.Serialize(extendedKey);
                        break;
                    default:
                        break;
                }
            }

            return fullKey;
        }
    }
}