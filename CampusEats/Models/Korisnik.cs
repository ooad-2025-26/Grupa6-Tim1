namespace CampusEats.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public string BrojIndeksa { get; set; }
        public string Telefon { get; set; }
        public string Adresa { get; set; }
        public Uloga Uloga { get; set; }
    }
}
