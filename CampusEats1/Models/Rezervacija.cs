using System.ComponentModel.DataAnnotations;

namespace CampusEats.Models;

public class Rezervacija
{
    public int Id { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime Datum { get; set; } = DateTime.Now;

    [DataType(DataType.Time)]
    public TimeSpan TerminPreuzimanja { get; set; } = new(12, 0, 0);

    public StatusRezervacije Status { get; set; } = StatusRezervacije.Kreirana;

    public NacinPreuzimanja NacinPreuzimanja { get; set; }

    public int KorisnikId { get; set; }
    public Korisnik? Korisnik { get; set; }

    public int? KurirId { get; set; }
    public Korisnik? Kurir { get; set; }

    public int ObrokId { get; set; }
    public Obrok? Obrok { get; set; }

    public QRKod? QRKod { get; set; }
}
