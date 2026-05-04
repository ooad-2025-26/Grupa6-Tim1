namespace CampusEats.Models
{
    public class Obrok
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public double Cijena { get; set; }
        public string Opis { get; set; }
        public string Sastojci { get; set; }
        public bool Dostupan { get; set; }
    }
}
