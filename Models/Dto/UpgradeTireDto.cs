using System.ComponentModel.DataAnnotations;

namespace Paybliss.Models.Dto;

public class UpgradeTireDto
{
    [Required]
    public string place_of_birth { get; set; } = string.Empty;
    [Required]
    public string dob { get; set; } = string.Empty;
    [Required]
    public string gender { get; set; } = string.Empty;
    [Required]
    public string country { get; set; } = string.Empty;
    [Required]
    public string street { get; set; } = string.Empty;
    [Required]
    public string city { get; set; } = string.Empty;
    [Required]
    public string state { get; set; } = string.Empty;
    [Required]
    public string postal_code { get; set; } = string.Empty;
    [Required]
    public string image { get; set; } = string.Empty;
}
