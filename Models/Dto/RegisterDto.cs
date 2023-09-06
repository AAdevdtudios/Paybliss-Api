using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Paybliss.Models.Dto
{
    public class RegisterDto
    {
        [Required, MinLength(3)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MinLength(3)]
        public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MinLength(10)]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
        [Required]
        public int Pin { get; set; }
        public string? ReferralsCode { get; set; }
        public string? VerificationToken { get; set; }
        [JsonIgnore]
        public DateTime? VerifiedAt { get; set; }
        [JsonIgnore]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
