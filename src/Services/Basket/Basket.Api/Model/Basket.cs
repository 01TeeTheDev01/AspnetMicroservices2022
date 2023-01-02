using System.Diagnostics;

namespace Basket.Api.Model
{
    public class ShoppingBasket
    {
        public string? UserName { get; set; }
        public List<BasketItem>? BasketItems { get; set; } = new();
        public decimal? BasketTotalPrice
        {
            get
            {
                try
                {
                    var total = BasketItems?.Sum(prod => prod.ProductPrice * prod.ProductQuantity);

                    if (total is not null)
                        return total;

                    return default;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    return default;
                }
            }
        }

        public ShoppingBasket() { }

        public ShoppingBasket(string? userName) { UserName = userName; }
    }
}
