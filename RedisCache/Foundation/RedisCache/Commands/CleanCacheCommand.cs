using System;

using Foundation.RedisCache.Events;
using Foundation.RedisCache.Events.EventRaisers;

using Sitecore.Web.UI.Sheer;

namespace Foundation.RedisCache.Commands
{
    [Serializable]
    public class CleanCacheCommand : Sitecore.Shell.Framework.Commands.Command
    {
        public override void Execute(Sitecore.Shell.Framework.Commands.CommandContext context)
        {
            var raiser = new CacheRebuildEventRaiser();
            var ev = new CacheRebuildEvent { CacheKey = Constants.ClearAll };

            raiser.RaiseEvent(ev);

            SheerResponse.Alert("Redis Cache flushed on all environments...");
        }
    }
}
