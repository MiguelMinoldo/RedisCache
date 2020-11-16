using Foundation.RedisCache.Events.CustomEventArgs;
using Foundation.RedisCache.Services;

namespace Foundation.RedisCache.Events.EventRaisers
{
    public class CacheRebuildEventRaiser
    {
        public void RaiseEvent()
        {
            var @event = new CacheRebuildEvent();

            RaiseEvent(@event);
        }

        public void RaiseEvent(CacheRebuildEvent @event)
        {
            var rebuildService = new CacheRebuildService();

            rebuildService.Rebuild(this, new CacheRebuildEventArgs(@event));
        }
    }
}
