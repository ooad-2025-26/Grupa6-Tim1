namespace CampusEats.Models
{
    public class Rezervacija
    {
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public StatusRezervacije Status { get; set; }

        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }

        public int ObrokId { get; set; }
        public Obrok Obrok { get; set; }
    }
}
