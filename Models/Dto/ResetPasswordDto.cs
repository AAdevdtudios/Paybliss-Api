using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Paybliss.Models.Dto
{
    public class ResetPasswordDto
    {
        public string ResetToken { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public byte[] passwordHash { get; init; } = new byte[32];
        [JsonIgnore]
        public byte[] passwordSalt { get; init; } = new byte[32];
    }
}
