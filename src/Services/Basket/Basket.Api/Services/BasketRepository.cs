using Basket.Api.Model;

using Microsoft.Extensions.Caching.Distributed;

using Newtonsoft.Json;

namespace Basket.Api.Services
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;


        public BasketRepository(IDistributedCache cache)
        {
            _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }


        public async Task<ShoppingBasket?>? GetBasket(string userName)
        {
            //Get username key from the Redis database
            string? usernameKey = await _redisCache.GetStringAsync(userName);

            //Check if username is present
            if (string.IsNullOrEmpty(usernameKey)) return default;

            //Get user shopping basket
            ShoppingBasket? shoppingBasket = JsonConvert.DeserializeObject<ShoppingBasket>(usernameKey);

            //Check if user's basket exists
            if (shoppingBasket is null) return default;

            //return user's shopping basket
            return shoppingBasket;
        }

        public async Task<ShoppingBasket?>? UpdateBasket(ShoppingBasket shoppingBasket)
        {
            //Check if username is present
            if (string.IsNullOrEmpty(shoppingBasket.UserName)) return default;

            //Update basket items by username
            await _redisCache.SetStringAsync(shoppingBasket.UserName, JsonConvert.SerializeObject(shoppingBasket));

            //Get shopping basket
            return await GetBasket(shoppingBasket.UserName);
        }
    }
}