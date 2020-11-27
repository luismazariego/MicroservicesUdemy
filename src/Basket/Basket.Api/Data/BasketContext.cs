namespace Basket.Api.Data
{
    using System;
    using Interfaces;
    using StackExchange.Redis;

    public class BasketContext : IBasketContext
    {
        //private readonly ConnectionMultiplexer _redisConnection;

        public BasketContext(ConnectionMultiplexer redisConnection)
        {
            // _redisConnection = redisConnection 
            //     ?? throw new ArgumentNullException(nameof(redisConnection));

            Redis = redisConnection.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}