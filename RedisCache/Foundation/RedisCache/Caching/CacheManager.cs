using System;
using System.Collections.Generic;
using System.Linq;

using Foundation.DI;
using Foundation.RedisCache.Redis;

using Newtonsoft.Json;

using Sitecore;
using Sitecore.Diagnostics;

using StackExchange.Redis;

namespace Foundation.RedisCache.Caching
{
    [Service(typeof(ICacheManager), Lifetime = Lifetime.Singleton)]
    public class CacheManager : ICacheManager
    {
        private readonly IDatabase _redisCache;
        private readonly IServer _redisServer;

        public CacheManager(IRedisCacheProvider redisCacheProvider)
        {
            _redisCache = redisCacheProvider.GetRedisCache();
            _redisServer = redisCacheProvider.GetServer();
        }

        private static readonly Dictionary<string, object> CacheKeyDictionary = new Dictionary<string, object>();

        public object Get(string key)
        {
            return Get(key, string.Empty);
        }

        public object Get(string key, string site)
        {
            var siteName = string.IsNullOrEmpty(site) ? Context.Site?.Name : site;
            var cacheKey = $"{siteName}{Context.Database?.Name}{Context.Language}{key}";
            var res = _redisCache.StringGet(cacheKey);

            return !string.IsNullOrEmpty(res) ? JsonConvert.DeserializeObject(res) : res;
        }

        public void Set(string key, object value)
        {
            Set(key, value, string.Empty);
        }

        public void Set(string key, object value, string site)
        {
            var siteName = string.IsNullOrEmpty(site) ? Context.Site?.Name : site;
            var cacheKey = $"{siteName}{Context.Database?.Name}{Context.Language}{key}";

            _redisCache.StringSet(cacheKey, JsonConvert.SerializeObject(value));
        }

        public IList<string> GetAllKeys()
        {
            return _redisServer.Keys().Select(k => k.ToString()).ToList();
        }

        public void Remove(string key)
        {
            _redisCache.KeyDelete(key);
        }

        public void ClearCache(object sender, EventArgs args)
        {
            Log.Info($"RedisCache Cache Clearer.", this);

            _redisServer.FlushAllDatabases();

            Log.Info("RedisCache Cache Clearer done.", (object)this);
        }

        public TObj GetCachedObject<TObj>(string cacheKey, Func<TObj> creator) where TObj : class
        {
            return GetCachedObject(cacheKey, creator, string.Empty);
        }

        public TObj GetCachedObject<TObj>(string cacheKey, Func<TObj> creator, string site) where TObj : class
        {
            if (string.IsNullOrEmpty(site))
            {
                site = Context.Site.Name;
            }

            var obj = Get(cacheKey, site) as TObj;

            if (obj == null)
            {
                // get the lock object
                var lockObject = GetCacheLockObject(cacheKey, site);

                try
                {
                    lock (lockObject)
                    {
                        obj = creator.Invoke();

                        Set(cacheKey, obj);
                    }
                }
                finally
                {
                    RemoveCacheLockObject(cacheKey, site);
                }
            }

            return obj;
        }

        private object GetCacheLockObject(string cacheKey, string site)
        {
            cacheKey += site;

            lock (CacheKeyDictionary)
            {
                if (!CacheKeyDictionary.ContainsKey(cacheKey))
                {
                    CacheKeyDictionary.Add(cacheKey, new object());
                }

                return CacheKeyDictionary[cacheKey];
            }
        }

        private void RemoveCacheLockObject(string cacheKey, string site)
        {
            cacheKey += site;

            lock (CacheKeyDictionary)
            {
                if (CacheKeyDictionary.ContainsKey(cacheKey))
                {
                    CacheKeyDictionary.Remove(cacheKey);
                }
            }
        }
    }
}
