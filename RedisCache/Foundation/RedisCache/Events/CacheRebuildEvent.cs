namespace Foundation.RedisCache.Events
{
    public class CacheRebuildEvent
    {
        public string CacheKey { get; set; }

        public string Database { get; set; }

        public string Field { get; set; }
    }
}
