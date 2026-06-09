using System.ComponentModel.DataAnnotations;

namespace CampusEats.Models;

public class AktivnostLog
{
    public int Id { get; set; }

    [StringLength(120)]
    public string? KorisnikEmail { get; set; }

    [Required]
    [StringLength(80)]
    public string Akcija { get; set; } = string.Empty;

    [StringLength(80)]
    public string? Entitet { get; set; }

    public int? EntitetId { get; set; }

    [StringLength(700)]
    public string Opis { get; set; } = string.Empty;

    public DateTime Vrijeme { get; set; } = DateTime.Now;
}
