using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Paybliss.Models.Dto
{
    public class SetPinDto
    {
        [Required, Range(1000,9999)]
        public int pin { get; set; }
        [EmailAddress, JsonIgnore]
        public string? email { get; set; }
    }
}
