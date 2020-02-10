using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenBasedAuth_NetCore.Providers
{
   public interface ICacheProvider
    {
        T GetFromCache<T>(string key)  where T : class;
        void SetCache<T>(string key, T value) where T : class;

        void SetCache<T>(string key, T value, DateTimeOffset duration) where T : class;

        void SetCache<T>(string key, T value, MemoryCacheEntryOptions memoryCacheEntryOptions) where T : class;

        void ClearCache(string key);
    }
}
