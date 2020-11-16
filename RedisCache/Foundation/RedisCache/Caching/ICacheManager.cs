using System;
using System.Collections.Generic;

namespace Foundation.RedisCache.Caching
{
    public interface ICacheManager
    {
        object Get(string key);

        object Get(string key, string site);

        void Set(string key, object value);

        void Set(string key, object value, string site);

        void Remove(string key);

        IList<string> GetAllKeys();

        void ClearCache(object sender, EventArgs args);

        TObj GetCachedObject<TObj>(string cacheKey, Func<TObj> creator) where TObj : class;

        TObj GetCachedObject<TObj>(string cacheKey, Func<TObj> creator, string site) where TObj : class;
    }
}
