using System.ComponentModel.DataAnnotations;

namespace CampusEats.Models;

public class QRKod
{
    public int Id { get; set; }

    public bool Validan { get; set; } = true;

    public DateTime VrijemeGenerisanja { get; set; } = DateTime.Now;

    [Required]
    [StringLength(80)]
    public string Kod { get; set; } = Guid.NewGuid().ToString("N");

    public int RezervacijaId { get; set; }
    public Rezervacija? Rezervacija { get; set; }
}
