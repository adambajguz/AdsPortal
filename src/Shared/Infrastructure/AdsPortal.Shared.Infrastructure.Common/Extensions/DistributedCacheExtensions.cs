namespace AdsPortal.Infrastructure.Common.Extensions
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Distributed;

    public static class DistributedCaching
    {
        public static async Task SetAsync<T>(this IDistributedCache distributedCache,
                                             string key,
                                             T? value,
                                             DistributedCacheEntryOptions options,
                                             CancellationToken token = default)
            where T : class
        {
            byte[] byteArray = value.ToByteArrayCeras();

            await distributedCache.SetAsync(key, byteArray, options, token);
        }

        public static async Task<T?> GetAsync<T>(this IDistributedCache distributedCache,
                                                 string key,
                                                 CancellationToken token = default)
            where T : class
        {
            byte[] result = await distributedCache.GetAsync(key, token);

            return result.FromByteArrayCeras<T>();
        }
    }
}
