namespace Paybliss.Models.HttpReq
{
    public enum OperationType
    {
        telco,
        data,
        electricity,
        television
    }
    public class VtuServiceDTO
    {
        public OperationType operationType { get; set; }
        public int ammount { get; set; }
        public string phoneNumber { get; set; }
        public string MyProperty { get; set; }
    }
    public class AirtimeDto
    {
        public OperationType operationType { get; set; } = OperationType.telco;
        public string phoneNumber { get; set; }
        public int ammount { get; set;}
        public string Network { get; set; }

    }
}
