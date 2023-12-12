namespace Paybliss.Models.Dto
{
    public record struct UserDto(
        string firstname,
        string lastname,
        string email,
        string phoneNumber,
        string verificationToken,
        string JWToken,
        string AccountNumber,
        string custormerId,
        string RefreshToken,
        Tier tier = Tier.Tier0
        );
    /*public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int Pin { get; set; }
        public string? ReferralsCode { get; set; }
        public string? VerificationToken { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? PasswordReset { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string JWToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }*/
}
