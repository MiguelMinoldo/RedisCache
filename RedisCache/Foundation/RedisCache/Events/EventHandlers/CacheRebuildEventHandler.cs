using System;

using Foundation.RedisCache.Events.CustomEventArgs;
using Foundation.RedisCache.Services;

using Sitecore.Diagnostics;

namespace Foundation.RedisCache.Events.EventHandlers
{
    public class CacheRebuildEventHandler
    {
        public void OnCustomCacheRebuild(object sender, EventArgs args)
        {
            Assert.IsNotNull(args, "Args");

            try
            {
                var cacheRebuildArgs = args as CacheRebuildEventArgs;
                var rebuildService = new CacheRebuildService();

                Log.Info(
                    $"CacheRebuildEventHandler: Event Raised: key:{cacheRebuildArgs?.EventInfo?.CacheKey} database:{cacheRebuildArgs?.EventInfo?.Database} field:{cacheRebuildArgs?.EventInfo?.Field}",
                    this);

                rebuildService.Rebuild(sender, cacheRebuildArgs);
            }

            catch (Exception exc)
            {
                Log.Warn($"CacheRebuildEventHandler: Exception while trying to rebuild cache {exc.Message}", this);
            }
        }
    }
}
