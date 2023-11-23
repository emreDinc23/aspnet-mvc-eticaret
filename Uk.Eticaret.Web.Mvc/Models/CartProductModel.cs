namespace Uk.Eticaret.Web.Mvc.Models
{
    public class CartProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string? ProductImage { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;
    }
}