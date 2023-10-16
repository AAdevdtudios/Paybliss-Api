namespace Paybliss.Models.HttpResp
{
    public class InternetResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Details
        {
            public List<Processed> processed { get; set; }
            public string transaction_status { get; set; }
            public int amount { get; set; }
            public int total_charge { get; set; }
            public double discount { get; set; }
            public string transaction_id { get; set; }
            public string datetime { get; set; }
        }

        public class Message
        {
            public string description { get; set; }
            public Details details { get; set; }
        }

        public class Processed
        {
            public string number { get; set; }
            public int amount { get; set; }
            public string id { get; set; }
        }

        public class Root
        {
            public bool status { get; set; }
            public Message message { get; set; }
            public int status_code { get; set; }
        }


    }
}
