namespace CampusEats.Models
{
    public class Obrok
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Naziv { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Range(0.0, double.MaxValue)]
        public double Cijena { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Opis { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required]
        public string Sastojci { get; set; } = string.Empty;

        public bool Dostupan { get; set; }
        public string Category { get; set; } = "Meals";
        public string ImageUrl { get; set; } = string.Empty;
    }
}
