using Basket.Api.Model;

namespace Basket.Api.Services
{
    public interface IBasketRepository
    {
        Task<ShoppingBasket?>? GetBasket(string userName);
        Task<ShoppingBasket?>? UpdateBasket(ShoppingBasket shoppingBasket);
        Task DeleteBasket(string userName);
    }
}
