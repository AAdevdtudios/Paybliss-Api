using System.ComponentModel.DataAnnotations;

namespace Paybliss.Models.Dto
{
    public class VerifyDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string token { get; set; } = string.Empty;
    }
}
