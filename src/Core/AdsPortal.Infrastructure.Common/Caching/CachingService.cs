namespace AdsPortal.Infrastructure.Cache
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Common.Cache;
    using AdsPortal.Common.Interfaces;
    using AdsPortal.Infrastructure.Common.Caching;
    using AdsPortal.Infrastructure.Common.Extensions;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;

    public class CachingService : ICachingService
    {
        private readonly TimeSpan DEFUALT_TIMEOUT = TimeSpan.FromMinutes(3);

        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        public IDistributedCache Provider { get; }
        public ILogger Logger { get; }

        public CachingService(IDistributedCache distributedCache, ILogger<CachingService> logger)
        {
            Provider = distributedCache;
            Logger = logger;
        }

        //TODO add action in methods
        public ICacheEntryConfig CreateConfig()
        {
            return new CacheEntryConfig();
        }

        public ICustomCacheEntry CreateEntry()
        {
            return new CustomCacheEntry();
        }

        #region Unsynchronized / Thread unsafe
        public async Task DeleteAsync(string key, object? extendedKey = null, CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            string fullKey = CacheEntryConfig.GetFullKey(key, extendedKey, extendedKeyMode);

            await Provider.RemoveAsync(fullKey);
        }

        public async Task<ICustomCacheEntry?> GetAsync(string key, object? extendedKey = null, CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode)
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            string fullKey = CacheEntryConfig.GetFullKey(key, extendedKey, extendedKeyMode);

            return await Provider.GetAsync<CustomCacheEntry>(fullKey);
        }

        public async Task<ICustomCacheEntry?> GetWithFullKeyAsync(string fullKey)
        {
            if (fullKey is null)
                throw new ArgumentNullException(nameof(fullKey));

            return await Provider.GetAsync<CustomCacheEntry>(fullKey);
        }

        public async Task SetAsync(ICacheEntryConfig entryConfig, object? value)
        {
            await SetAsync(new CustomCacheEntry(entryConfig)
            {
                Value = value
            });
        }

        public async Task SetAsync(ICustomCacheEntry entry)
        {
            DateTimeOffset now = DateTimeOffset.Now;

            CustomCacheEntry customCacheEntry = (entry as CustomCacheEntry)!;
            customCacheEntry.LastSetOn = now;

            string fullKey = entry.GetFullKey();
            await Provider.SetAsync<CustomCacheEntry>(fullKey,
                                                      customCacheEntry,
                                                      new DistributedCacheEntryOptions
                                                      {
                                                          AbsoluteExpiration = entry.AbsoluteExpiration,
                                                          AbsoluteExpirationRelativeToNow = entry.AbsoluteExpirationRelativeToNow,
                                                          SlidingExpiration = entry.SlidingExpiration
                                                      });

            Logger.LogInformation($"{nameof(CachingService) }.Set with key: {fullKey} and SyncId {entry?.SynchronizationId} (null? {entry?.Value is null})", this);
        }

        public async Task<T?> GetOrSet<T>(ICacheEntryConfig entryConfig, Func<T> createValue)
            where T : class
        {
            string fullKey = entryConfig.GetFullKey();

            CustomCacheEntry? cacheEntry = await Provider.GetAsync<CustomCacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
            {
                // Key not in cache, so get data.
                value = createValue();
                await SetAsync(entryConfig, value);
            }

            return value;
        }

        public async Task<T?> GetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<Task<T>> createValue)
            where T : class
        {
            string fullKey = entryConfig.GetFullKey();

            CustomCacheEntry? cacheEntry = await Provider.GetAsync<CustomCacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
            {
                // Key not in cache, so get data.
                value = await createValue();
                await SetAsync(entryConfig, value);
            }

            return value;
        }
        #endregion

        //
        // Summary:
        //     Use Synchronized... when:
        //         When the creation time of an item has some sort of cost, and you want to minimize creations as much as possible.
        //         When the creation time of an item is very long.
        //         When the creation of an item has to be ensured to be done once per key.
        //     Don’t use  Synchronized... when:
        //         There’s no danger of multiple threads accessing the same cache item.
        //         You don’t mind creating the item more than once. For example, if one extra trip to the database won’t change much.
        #region Synchronized / Thread safe
        public async Task<T?> SynchronizedGetAsync<T>(string key,
                                                     object? extendedKey = null,
                                                     CacheExtendedKeyModes extendedKeyMode = CacheExtendedKeyModes.UseGetHashCode,
                                                     TimeSpan? timeout = null)
            where T : class
        {
            if (key is null)
                throw new ArgumentNullException(nameof(key));

            string fullKey = CacheEntryConfig.GetFullKey(key, extendedKey, extendedKeyMode);

            CustomCacheEntry? cacheEntry = await Provider.GetAsync<CustomCacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
                if (_locks.TryGetValue(fullKey, out SemaphoreSlim? myLock))
                {
                    await myLock.WaitAsync(timeout ?? DEFUALT_TIMEOUT);

                    cacheEntry = await Provider.GetAsync<CustomCacheEntry>(fullKey);
                    value = cacheEntry.GetValue<T>();
                }

            return value;
        }

        private async Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig,
                                                            Func<Task<T>>? createValueAsync,
                                                            Func<T>? createValue,
                                                            TimeSpan? timeout)
            where T : class
        {
            if (entryConfig is null)
                throw new ArgumentNullException(nameof(entryConfig));

            if (createValueAsync is null && createValue is null)
                throw new ArgumentNullException(nameof(createValueAsync) + " && " + nameof(createValue));

            string fullKey = entryConfig.GetFullKey();

            CustomCacheEntry? cacheEntry = await Provider.GetAsync<CustomCacheEntry>(fullKey);
            if (!cacheEntry.TryGetValue(out T? value))
            {
                SemaphoreSlim myLock = _locks.GetOrAdd(fullKey, k => new SemaphoreSlim(1, 1));

                await myLock.WaitAsync(timeout ?? DEFUALT_TIMEOUT);
                try
                {
                    cacheEntry = await Provider.GetAsync<CustomCacheEntry>(fullKey);
                    if (!cacheEntry.TryGetValue(out value))
                    {
                        // Key not in cache, so get data.
                        if (createValueAsync == null)
                            value = createValue!();
                        else
                            value = await createValueAsync();

                        await SetAsync(entryConfig, value);
                    }
                }
                finally
                {
                    myLock.Release();
                }
            }

            return value;
        }

        public async Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<Task<T>> createValueAsync, TimeSpan? timeout = null)
            where T : class
        {
            return await SynchronizedGetOrSetAsync(entryConfig, createValueAsync, null, timeout);
        }

        public async Task<T?> SynchronizedGetOrSetAsync<T>(ICacheEntryConfig entryConfig, Func<T> createValue, TimeSpan? timeout = null)
            where T : class
        {
            return await SynchronizedGetOrSetAsync(entryConfig, null, createValue, timeout);
        }
        #endregion

        //private IQueryable<string> GetAllKeysQueryable()
        //{
        //    return Provider.GetCacheKeys().AsQueryable();
        //}

        //public string[] GetAllNotExtendedKeys()
        //{
        //    return GetAllKeysQueryable().Where(x => !x.Contains(CacheEntryConfig.ExtendedKeySeperator)).ToArray();
        //}

        //public string[] GetAllExtendedKeys()
        //{
        //    return GetAllKeysQueryable().Where(x => x.Contains(CacheEntryConfig.ExtendedKeySeperator)).ToArray();
        //}
    }
}