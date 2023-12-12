namespace Paybliss.Models
{
    public enum TransactionType
    {
        Card,
        Airtime,
        Transfer,
        Electricity,
        Television
    }
    public class Transactions
    {
        public int Id { get; set; }
        public string Details { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int amount { get; set; }
        public string reference { get; set; } = string.Empty;
        public string blocId { get; set; } = string.Empty;
        public TransactionType transactionType { get; set; } = TransactionType.Airtime;
        public string AccountId { get; set; } = string.Empty;
    }
}
