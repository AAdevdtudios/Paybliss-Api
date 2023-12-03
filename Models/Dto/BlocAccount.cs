namespace Paybliss.Models.Dto
{
    public class Data
    {
        public string id { get; set; }
        public string name { get; set; }
        public string preferred_bank { get; set; }
        public string bvn { get; set; }
        public int balance { get; set; }
        public string currency { get; set; }
        public string customer_id { get; set; }
        public string account_number { get; set; }
        public string bank_name { get; set; }
        public string type { get; set; }
        public bool collection_account { get; set; }
        public bool hide_account { get; set; }
        public bool SkipNumber { get; set; }
        public object managers { get; set; }
        public string alias { get; set; }
    }

    public class BlocAccount
    {
        public bool success { get; set; }
        public Data data { get; set; }
        public string message { get; set; }
    }


}
