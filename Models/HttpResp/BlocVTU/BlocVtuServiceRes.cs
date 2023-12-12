namespace Paybliss.Models.HttpResp.BlocVTU
{
    public class BlocVtuServiceRes
    {
        public bool success { get; set; }
        public List<Datum> data { get; set; }
        public string message { get; set; }
    }
    public class Datum
    {
        public string desc { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string sector { get; set; }
    }


}
