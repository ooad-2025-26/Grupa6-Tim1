using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusEats.Models;

public class Obrok
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string Naziv { get; set; } = string.Empty;

    [Column(TypeName = "decimal(8,2)")]
    [Range(0.01, 1000)]
    public decimal Cijena { get; set; }

    [StringLength(300)]
    public string Opis { get; set; } = string.Empty;

    [StringLength(300)]
    public string Sastojci { get; set; } = string.Empty;

    public bool Dostupan { get; set; } = true;

    [Range(0, 10000)]
    public int Kolicina { get; set; }

    public ICollection<Meni> Meniji { get; set; } = new List<Meni>();
    public ICollection<Rezervacija> Rezervacije { get; set; } = new List<Rezervacija>();
}
