using System.ComponentModel.DataAnnotations;

namespace Paybliss.Models.Dto
{
    public class UpdatePasswordDto
    {
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [MinLength(6)]
        public string newPassword { get; set; }
    }
}
