namespace Paybliss.Models.HttpResp
{
    public class AirtimeResponseData
    {
        public class ApiResponse
        {
            public bool Status { get; set; }
            public Message Message { get; set; }
            public int StatusCode { get; set; }
        }

        public class Message
        {
            public string Description { get; set; }
            public Details Details { get; set; }
        }

        public class Details
        {
            public List<ProcessedItem> Processed { get; set; }
            public string TransactionStatus { get; set; }
            public decimal Amount { get; set; }
            public decimal TotalCharge { get; set; }
            public decimal Discount { get; set; }
            public string TransactionId { get; set; }
            public string Datetime { get; set; }
        }

        public class ProcessedItem
        {
            public string Number { get; set; }
            public decimal Amount { get; set; }
            public string Id { get; set; }
        }

    }
}
