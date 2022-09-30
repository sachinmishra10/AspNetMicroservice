using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.API.Data
{
    public class BasketRepository:IBasketRepository
    {
        private readonly IDistributedCache redisCache;

        public BasketRepository(IDistributedCache _rediscache)
        {
            this.redisCache = _rediscache;
        }
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var userBasket = await redisCache.GetStringAsync(userName);
            if (userBasket == null) return null;
            return JsonConvert.DeserializeObject<ShoppingCart>(userBasket);

        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await redisCache.SetStringAsync(basket.UserName,JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            await redisCache.RemoveAsync(userName);
        }
    }
}
