using System.ComponentModel.DataAnnotations;

namespace Paybliss.Models.Dto
{
    public record struct ValidateBvnDto([EmailAddress][Required]string email, [Required] string bvn);
}
