namespace Basket.Api.Model
{
    public class BasketItem
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductColor { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
    }
}
