namespace Paybliss.Models.HttpResp
{// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Address
    {
        public string street { get; set; }
    }

    public class BlocData
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public string organization_id { get; set; }
        public string environment { get; set; }
        public string email { get; set; }
        public string country { get; set; }
        public string group { get; set; }
        public string status { get; set; }
        public string created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string kyc_tier { get; set; }
        public string bvn { get; set; }
        public DateTime date_of_birth { get; set; }
        public string customer_type { get; set; }
        public string source { get; set; }
        public Address address { get; set; }
    }

    public class BlocResponse
    {
        public bool success { get; set; }
        public BlocData data { get; set; }
        public string message { get; set; }
    }



}
