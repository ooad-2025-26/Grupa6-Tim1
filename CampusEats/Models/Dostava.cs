namespace CampusEats.Models
{
    public class Dostava
    {
        public int Id { get; set; }
        public string Adresa { get; set; }
        public string Status { get; set; }
        public DateTime VrijemeDostave { get; set; }

        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
    }
}
