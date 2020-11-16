namespace Foundation.RedisCache
{
    public static class Constants
    {
        public static readonly string CustomCacheRebuildEventName = "customCache:rebuild";

        public static readonly string CustomCacheRebuildEventNameRemote = CustomCacheRebuildEventName + ":remote";

        public static readonly string ClearAll = "ClearAll";

        public static readonly string Ref = "##REF##";
    }
}
