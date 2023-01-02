namespace Basket.Api.Model
{
    public class BasketItem
    {
        //[Required(ErrorMessage = "Please enter a product id"), Range(0, int.MaxValue, ErrorMessage = "Minimum value for ProductId can not be less than zero.")]
        public int ProductId { get; set; }

        //[Required(ErrorMessage = "Please enter a product name")]
        public string? ProductName { get; set; }

        //[Required(ErrorMessage = "Please enter a product color.")]
        public string? ProductColor { get; set; }

        //[Required(ErrorMessage = "Please enter a product price"), Range(0, (double)decimal.MaxValue, ErrorMessage = "Minimum value for product price can not be less than zero.")]
        public decimal ProductPrice { get; set; }

        //[Required(ErrorMessage = "Please enter the quantity required."), Range(0, int.MaxValue, ErrorMessage = "Minimum value for product quantity can not be less than zero.")]
        public int ProductQuantity { get; set; }
    }
}
