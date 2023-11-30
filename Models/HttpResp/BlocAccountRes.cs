namespace Paybliss.Models.HttpResp
{
    public class BlocAccountRes
    {
        public bool success { get; set; }
        public AcountData data { get; set; }
        public string message { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CollectionRules
    {
        public int frequency { get; set; }
        public int amount { get; set; }
    }

    public class Customer
    {
        public string id { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public string kyc_tier { get; set; }
        public string phone_number { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string customer_type { get; set; }
        public string bvn { get; set; }
    }

    public class AcountData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string preferred_bank { get; set; }
        public string bvn { get; set; }
        public string kyc_tier { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string status { get; set; }
        public string environment { get; set; }
        public string organization_id { get; set; }
        public int balance { get; set; }
        public string currency { get; set; }
        public object meta_data { get; set; }
        public string customer_id { get; set; }
        public Customer customer { get; set; }
        public string account_number { get; set; }
        public string bank_name { get; set; }
        public string type { get; set; }
        public bool collection_account { get; set; }
        public bool hide_account { get; set; }
        public bool SkipNumber { get; set; }
        public object managers { get; set; }
        public ExternalAccount external_account { get; set; }
        public string alias { get; set; }
        public CollectionRules collection_rules { get; set; }
    }

    public class ExternalAccount
    {
    }

    public class Root
    {
        public bool success { get; set; }
        public Data data { get; set; }
        public string message { get; set; }
    }


}
