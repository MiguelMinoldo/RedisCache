using System.Linq;

using Foundation.RedisCache.Caching;
using Foundation.RedisCache.Events.CustomEventArgs;

using Microsoft.Extensions.DependencyInjection;

using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;

namespace Foundation.RedisCache.Services
{
    public class CacheRebuildService
    {
        public void Rebuild(object sender, CacheRebuildEventArgs args)
        {
            var cacheManager = ServiceLocator.ServiceProvider.GetService<ICacheManager>();

            if (string.IsNullOrEmpty(args.EventInfo.CacheKey) || args.EventInfo.CacheKey.Equals(Constants.ClearAll))
            {
                Log.Info("UrlRewriting Cleaning ALL caches", this);

                cacheManager.ClearCache(sender, args);
            }
            else
            {
                Log.Info($"UrlRewriting Cache clean - key:{args.EventInfo?.CacheKey} database:{args.EventInfo?.Database} field:{args.EventInfo?.Field}", this);

                var allKeys = cacheManager.GetAllKeys();
                var candidateKeys = allKeys.Where(s => s.ToString().Contains(args.EventInfo.Database) && s.ToString().Contains(args.EventInfo.Field));
                var refKeys = allKeys.Where(s => s.ToString().Contains(args.EventInfo.CacheKey) && s.ToString().Contains(Constants.Ref));
                var k = args.EventInfo.CacheKey.Trim('/').Split('/').LastOrDefault();

                foreach (var key in candidateKeys)
                {
                    if (key.ToString().Contains(k))
                    {
                        cacheManager.Remove(key);

                        Log.Info($"UrlRewriting - Cleaning caches by key: {key}", this);
                    }
                }

                foreach (var key in refKeys)
                {
                    var val = cacheManager.Get(key)?.ToString();

                    if (!string.IsNullOrEmpty(val))
                    {
                        cacheManager.Remove(val);
                    }

                    Log.Info($"UrlRewriting - Cleaning caches by key: {key}", this);
                }
            }
        }
    }
}
