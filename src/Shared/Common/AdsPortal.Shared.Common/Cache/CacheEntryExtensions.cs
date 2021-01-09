namespace AdsPortal.Shared.Common.Cache
{
    public static class CacheEntryExtensions
    {
        public static T? GetValue<T>(this ICustomCacheEntry? entry) where T : class
        {
            return entry?.Value as T;
        }

        public static bool TryGetValue<T>(this ICustomCacheEntry? entry, out T? value) where T : class
        {
            value = entry?.Value as T;

            return value != null;
        }

        public static string? GetStringValue(this ICustomCacheEntry? entry)
        {
            string? value = entry?.Value as string;
            if (!string.IsNullOrWhiteSpace(value))
                return null;

            return entry?.Value?.ToString();
        }

        public static bool TryGetStringValue(this ICustomCacheEntry? entry, out string? value)
        {
            value = entry?.Value as string;
            if (!string.IsNullOrWhiteSpace(value))
                return true;

            value = entry?.Value?.ToString();
            return value != null;
        }

        public static bool HasValue(this ICustomCacheEntry? entry)
        {
            return entry?.Value != null;
        }

        public static bool HasValue<T>(this ICustomCacheEntry? entry) where T : class
        {
            return entry?.Value is T;
        }
    }
}