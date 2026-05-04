namespace CampusEats.Models
{
    public class QRKod
    {
        public int Id { get; set; }
        public bool Validan { get; set; }
        public DateTime VrijemeGenerisanja { get; set; }
        public string Kod { get; set; }

        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
    }
}
