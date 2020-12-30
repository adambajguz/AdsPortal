namespace AdsPortal.Common.Cache
{
    using System;

    public interface ICustomCacheEntry : ICacheEntryConfig
    {
        DateTimeOffset? LastSetOn { get; }
        Guid SynchronizationId { get; }
        object? Value { get; set; }
    }
}