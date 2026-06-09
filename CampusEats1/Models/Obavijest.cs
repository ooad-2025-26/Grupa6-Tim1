using System.ComponentModel.DataAnnotations;

namespace CampusEats.Models;

public class Obavijest
{
    public int Id { get; set; }

    public int? KorisnikId { get; set; }
    public Korisnik? Korisnik { get; set; }

    public int? RezervacijaId { get; set; }
    public Rezervacija? Rezervacija { get; set; }

    [Required]
    [StringLength(120)]
    public string Naslov { get; set; } = string.Empty;

    [Required]
    [StringLength(600)]
    public string Poruka { get; set; } = string.Empty;

    public DateTime DatumSlanja { get; set; } = DateTime.Now;

    public bool Procitana { get; set; }
}
