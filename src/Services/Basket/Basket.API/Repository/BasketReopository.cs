using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repository
{
    public class BasketReopository:IBasketReopository
    {
        private readonly IDistributedCache _distributedCache;

        public BasketReopository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ??throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task DeleteBasket(string userName)
        {
            await _distributedCache.RemoveAsync(userName);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket =await _distributedCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _distributedCache.SetStringAsync(basket.UserName,JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }
    }
}
