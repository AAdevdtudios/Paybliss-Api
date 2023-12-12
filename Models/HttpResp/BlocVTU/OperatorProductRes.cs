namespace Paybliss.Models.HttpResp.BlocVTU
{
    public class OperatorProductRes
    {
        public bool success { get; set; }
        public List<OperatorData> data { get; set; }
        public string message { get; set; }
    }
    public class OperatorData
    {
        public string category { get; set; }
        public object desc { get; set; }
        public string fee_type { get; set; }
        public string id { get; set; }
        public Meta meta { get; set; }
        public string name { get; set; }
        public string @operator { get; set; }
    }

    public class Meta
    {
        public string currency { get; set; }
        public string maximum_fee { get; set; }
        public string minimum_fee { get; set; }
        public string data_expiry { get; set; }
        public string data_value { get; set; }
        public string fee { get; set; }
    }
}
