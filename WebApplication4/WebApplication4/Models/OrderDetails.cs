namespace WebApplication4.Models
{
    public class OrderDetails
    {
        
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public decimal ListPrice { get; set; }
        public decimal Discount {  get; set; }
    }
}
