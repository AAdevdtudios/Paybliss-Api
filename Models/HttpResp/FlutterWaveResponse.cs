namespace Paybliss.Models.HttpResp
{
    public class FlutterWaveResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }
    public partial class Data
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string FlwRef { get; set; }
        public string OrderRef { get; set; }
        public string AccountNumber { get; set; }
        public string Frequency { get; set; }
        public string BankName { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string ExpiryDate { get; set; }
        public string Note { get; set; }
        public object Amount { get; set; }
    }
}
