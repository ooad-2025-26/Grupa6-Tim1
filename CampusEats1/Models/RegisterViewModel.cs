using System.ComponentModel.DataAnnotations;

namespace CampusEats.Models;

public class RegisterViewModel
{
    [Required]
    [StringLength(50)]
    public string Ime { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Prezime { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [StringLength(20)]
    public string? BrojIndeksa { get; set; }

    [StringLength(30)]
    public string? Telefon { get; set; }

    [StringLength(150)]
    public string? Adresa { get; set; }
}
