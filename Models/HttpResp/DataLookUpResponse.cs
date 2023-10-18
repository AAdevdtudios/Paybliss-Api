namespace Paybliss.Models.HttpResp
{
    public class DataLookUpResponse
    {
        public class ApiResponse
        {
            public bool Status { get; set; }
            public ResponseMessage Message { get; set; }
            public int StatusCode { get; set; }
        }

        public class ResponseMessage
        {
            public string Description { get; set; }
            public List<NetworkDetail> Details { get; set; }
        }

        public class NetworkDetail
        {
            public string NetworkName { get; set; }
            public string Title { get; set; }
            public string NetworkCode { get; set; }
            public string CheckBalance { get; set; }
            public List<Plan> Plans { get; set; }
        }

        public class Plan
        {
            public string plan_code { get; set; }
            public string Name { get; set; }
            public decimal Amount { get; set; }
        }

    }
}
