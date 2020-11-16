using StackExchange.Redis;

namespace Foundation.RedisCache.Redis
{
    public interface IRedisCacheProvider
    {
        IDatabase GetRedisCache();

        IServer GetServer();
    }
}
