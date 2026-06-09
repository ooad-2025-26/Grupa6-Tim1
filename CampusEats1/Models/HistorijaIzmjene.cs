using System.ComponentModel.DataAnnotations;

namespace CampusEats.Models;

public class HistorijaIzmjene
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string Entitet { get; set; } = string.Empty;

    public int EntitetId { get; set; }

    [Required]
    [StringLength(80)]
    public string TipIzmjene { get; set; } = string.Empty;

    [StringLength(120)]
    public string? KorisnikEmail { get; set; }

    [StringLength(700)]
    public string Opis { get; set; } = string.Empty;

    public DateTime Vrijeme { get; set; } = DateTime.Now;
}
