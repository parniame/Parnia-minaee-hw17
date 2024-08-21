namespace WebApplication4.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime Date { get; set; }
        public DateTime RequiredDate { get; set; }
        public DateTime ShippingDate { get; set; }
        
        public string StaffFirstName { get; set; }
        public string StaffLastName { get; set; }
    }
}
