namespace Basket.Api.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Data.Interfaces;
    using Entities;
    using Interfaces;
    using Newtonsoft.Json;

    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public Task<bool> DeleteBasket(string userName) 
            => _context.Redis.KeyDeleteAsync(userName);

        public async Task<BasketCart> GetBasket(string userName)
        {
            var basket = await _context
                         .Redis
                         .StringGetAsync(userName);

            if(basket.IsNullOrEmpty) return null;

            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basket)
        {
            var updated = await _context
                          .Redis
                          .StringSetAsync(basket.UserName, 
                          JsonConvert.SerializeObject(basket));

            return updated ? await GetBasket(basket.UserName) : null;
        }
    }
}