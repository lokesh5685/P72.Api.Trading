using System;

namespace P72.Caching
{
    public interface ICacheProvider
    {
        T GetOrCreateCache<T>(string key, Func<T> createCache, int cacheExpiryDuration);

        T GetCache<T>(string cacheKey);

        void SetCache<T>(string cacheKey, T data, int cacheExpiryDuration);

        bool ClearCache(string cacheKey);
    }
}
