namespace WebApplication4.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string ManagerFirstName { get; set; }
        public string ManagerLastName { get; set; }
    }
}
