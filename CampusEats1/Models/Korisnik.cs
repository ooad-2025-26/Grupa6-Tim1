using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CampusEats.Models;

public class Korisnik : IdentityUser<int>
{
    [Required]
    [StringLength(50)]
    public string Ime { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Prezime { get; set; } = string.Empty;

    [StringLength(20)]
    public string? BrojIndeksa { get; set; }

    public UlogaKorisnika Uloga { get; set; }

    [StringLength(30)]
    public string? Telefon { get; set; }

    [StringLength(150)]
    public string? Adresa { get; set; }

    [StringLength(300)]
    public string? Alergije { get; set; }

    [StringLength(300)]
    public string? OmiljenaHrana { get; set; }

    public bool Vegetarijanac { get; set; }

    public ICollection<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();
    public ICollection<Rezervacija> Dostave { get; set; } = new List<Rezervacija>();
    public ICollection<Obavijest> Obavijesti { get; set; } = new List<Obavijest>();
}
