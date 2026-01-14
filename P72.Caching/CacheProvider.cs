using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Collections.Generic;
using System;
using P72.Caching;

namespace P72.Caching
{
    public class CacheProvider : ICacheProvider
    {
        private const string EntriesCollection = "EntriesCollection";
        private const string Key = "Key";
        private readonly IMemoryCache memoryCache;

        public CacheProvider(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public T GetOrCreateCache<T>(string key, Func<T> createCache, int cacheExpiryDuration)
        {
            if (memoryCache.TryGetValue(key, out T cacheEntry)) return cacheEntry;
            cacheEntry = createCache();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(cacheExpiryDuration)
            };
            memoryCache.Set(key, cacheEntry, cacheEntryOptions);
            return cacheEntry;
        }

        public T GetCache<T>(string cacheKey)
        {
            if (memoryCache.TryGetValue(cacheKey, out T result)) return result;
            return result;
        }

        public void SetCache<T>(string cacheKey, T data, int cacheExpiryDuration)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(cacheExpiryDuration)
            };
            memoryCache.Set(cacheKey, data, cacheEntryOptions);
        }

        public bool ClearCache(string cacheKey)
        {
            var entriesCollection = typeof(MemoryCache).GetProperty(EntriesCollection, BindingFlags.NonPublic | BindingFlags.Instance);
            var cachedEntries = entriesCollection?.GetValue(memoryCache) as dynamic;

            var cachedKeys = GetCachedKeys(cachedEntries, cacheKey);
            return RemoveCache(cachedKeys);
        }

        private static IEnumerable<string> GetCachedKeys(dynamic cachedEntries, string cacheKey)
        {
            var cachedKeys = new List<string>();
            foreach (var cacheItem in cachedEntries)
            {
                var cachedItemKey = cacheItem.GetType().GetProperty(Key).GetValue(cacheItem, null);
                if (cachedItemKey.ToString().StartsWith(cacheKey))
                    cachedKeys.Add(cachedItemKey);
            }

            return cachedKeys;
        }

        private bool RemoveCache(IEnumerable<string> cachedKeys)
        {
            foreach (var cachedKey in cachedKeys)
            {
                if (!string.IsNullOrEmpty(cachedKey))
                    memoryCache.Remove(cachedKey);
            }
            return true;
        }
    }
}
