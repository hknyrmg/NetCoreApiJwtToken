using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace TokenBasedAuth_NetCore.Providers
{
    public class CacheProvider: ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        public CacheProvider(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public T GetFromCache<T>(string key) where T : class
        {
            _memoryCache.TryGetValue<T>(key, out T value);
            return value;
        }

        public void SetCache<T>(string key, T value) where T : class
        {
            _memoryCache.Set<T>(key, value, DateTimeOffset.Now.AddSeconds(10));
        }

        public void SetCache<T>(string key, T value, DateTimeOffset duration) where T : class
        {
            _memoryCache.Set<T>(key, value, duration);
        }

        public void SetCache<T>(string key, T value, MemoryCacheEntryOptions memoryCacheEntryOptions) where T : class
        {
            _memoryCache.Set<T>(key, value, memoryCacheEntryOptions);
        }
        public void ClearCache(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
