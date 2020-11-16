using Sitecore.Events;

using System;

namespace Foundation.RedisCache.Events.CustomEventArgs
{
    [Serializable]
    public class CacheRebuildEventArgs : EventArgs, IPassNativeEventArgs
    {
        public CacheRebuildEvent EventInfo { get; set; }

        public CacheRebuildEventArgs(CacheRebuildEvent @event)
        {
            EventInfo = @event;
        }
    }
}
