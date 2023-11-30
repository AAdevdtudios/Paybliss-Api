namespace Paybliss.Models
{
    public class AccountDetails
    {
        public int Id { get; set; }
        public string accountNumber { get; set; }
        public string amount { get; set; }
        public string accountName { get; set; }
        public string currency { get; set; }
        public string reference { get; set; }
        public string bank_name { get; set; }
        public string bank { get; set; }
        public int userId { get; set; }
        public User owner { get; set; }
    }
}
