namespace Paybliss.Models
{
    public class ResponseData<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Successful { get; set; } = false;
        public T? Data { get; set; }
    }
}
