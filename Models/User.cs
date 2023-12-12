using System.Text.Json.Serialization;

namespace Paybliss.Models
{
    public enum Tier
    {
        Tier0,
        Tier1,
        Tier2,
        Tier3
    }
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string custormerId { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public AccountDetails? Account { get; set; }
        public string bvn { get; set; }
        public Tier tier { get; set; } = Tier.Tier0;
        public int Pin { get; set; }
        public string? ReferralsCode { get; set; }
        [JsonIgnore]
        public byte[] passwordHash { get; set; } = new byte[32];
        [JsonIgnore]
        public byte[] passwordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public string? PasswordReset { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public bool IsVerified { get; set; }
        public virtual ICollection<Transactions> transactions { get; set; }
    }
}
