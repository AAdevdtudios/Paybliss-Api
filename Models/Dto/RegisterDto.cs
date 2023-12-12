using System.ComponentModel.DataAnnotations;

namespace Paybliss.Models.Dto
{
    public class RegisterDto
    {
        [Required, MinLength(3)]
        public string firstname { get; set; } = string.Empty;
        [Required, MinLength(3)]
        public string lastname { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string email { get; set; } = string.Empty;
        [Required]
        public string bvn { get; set; } = string.Empty;
        [Required, MinLength(10)]
        public string phoneNumber { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public string password { get; set; } = string.Empty;
    }
}
