using System.Text.Json.Serialization;

namespace Paybliss.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User user { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
    }
}
