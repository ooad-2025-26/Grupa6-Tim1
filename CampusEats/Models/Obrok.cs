namespace CampusEats.Models
{
    public class Obrok
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Naziv { get; set; }

        [System.ComponentModel.DataAnnotations.Range(0.0, double.MaxValue)]
        public double Cijena { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Opis { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Sastojci { get; set; }

        public bool Dostupan { get; set; }
    }
}
