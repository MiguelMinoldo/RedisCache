using System;
using System.Configuration;
using System.Linq;

using Foundation.DI;

using StackExchange.Redis;

namespace Foundation.RedisCache.Redis
{
    [Service(typeof(IRedisCacheProvider), Lifetime = Lifetime.Singleton)]
    public class RedisCacheProvider : IRedisCacheProvider
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            var connectionString = ConfigurationManager.ConnectionStrings["redis.sessions"].ConnectionString;
            var options = ConfigurationOptions.Parse(connectionString);

            options.AllowAdmin = true;
            options.SyncTimeout = 60000;
            options.ConnectRetry = 5;

            return ConnectionMultiplexer.Connect(options);
        });

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        private readonly IDatabase _redisCache;

        public RedisCacheProvider()
        {
            _redisCache = Connection.GetDatabase();
        }

        public IDatabase GetRedisCache()
        {
            return _redisCache;
        }

        public IServer GetServer()
        {
            return Connection.GetServer(Connection.GetEndPoints().FirstOrDefault());
        }
    }
}
