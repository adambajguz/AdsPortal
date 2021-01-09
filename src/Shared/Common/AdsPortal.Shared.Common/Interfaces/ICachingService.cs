namespace AdsPortal.Shared.Common.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using AdsPortal.Shared.Common.Cache;

    public interface ICachingService
    {
        public ICacheEntryConfig CreateConfig();
        public ICustomCacheEntry CreateEntry();

        #region Unsynchronized / Thread unsafe
        Task DeleteAsync(string key, object? extendedKey = null, CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode);
        Task<ICustomCacheEntry?> GetAsync(string key, object? extendedKey = null, CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode);
        Task<ICustomCacheEntry?> GetWithFullKeyAsync(string fullKey);
        Task SetAsync(ICacheEntryConfig entryConfig, object value);
        Task SetAsync(ICustomCacheEntry entry);

        Task<T?> GetOrSet<T>(ICacheEntryConfig entryConfig, Func<T> createValue)
            where T : class;

        Task<T?> GetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<Task<T>> createValue)
            where T : class;
        #endregion

        //
        // Summary:
        //     Use Synchronized... when:
        //         When the creation time of an item has some sort of cost, and you want to minimize creations as much as possible.
        //         When the creation time of an item is very long.
        //         When the creation of an item has to be ensured to be done once per key.
        //     Don’t use Synchronized... when:
        //         There’s no danger of multiple threads accessing the same cache item.
        //         You don’t mind creating the item more than once. For example, if one extra trip to the database won’t change much.
        #region Synchronized / Thread safe
        Task<T?> SynchronizedGetAsync<T>(string key,
                                         object? extendedKey = null,
                                         CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode,
                                         TimeSpan? timeout = null)
            where T : class;
        Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<Task<T>> createValueAsync, TimeSpan? timeout = null)
            where T : class;
        Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<T> createValue, TimeSpan? timeout = null)
            where T : class;
        #endregion
    }
}