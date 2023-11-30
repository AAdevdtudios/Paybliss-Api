using System.Text.Json.Serialization;

namespace Paybliss.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string custormerId { get; set; }
        public AccountDetails? Account { get; set; }
        public int Pin { get; set; }
        public string? ReferralsCode { get; set; }
        public byte[] passwordHash { get; set; } = new byte[32];
        public byte[] passwordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? PasswordReset { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public bool IsVerified { get; set; }
    }
}
