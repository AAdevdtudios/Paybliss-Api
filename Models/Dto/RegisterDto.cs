using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Paybliss.Models.Dto
{
    public record struct RegisterDto(
        [Required, MinLength(3)] string firstname,
        [Required, MinLength(3)] string lastname,
        [Required, MinLength(3)] string email,
        [Required, MinLength(10)] string phoneNumber,
        [Required, MinLength(6)] string password
        );
}
