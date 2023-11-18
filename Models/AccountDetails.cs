namespace Paybliss.Models
{
    public class AccountDetails
    {
        public int Id { get; set; }
        public string accountNumber { get; set; }
        public string amount { get; set; }
        public int userId { get; set; }
        public User owner { get; set; }
    }
}
