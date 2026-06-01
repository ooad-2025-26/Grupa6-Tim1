namespace CampusEats.Models
{
    public class Zaliha
    {
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string NazivArtikla { get; set; }

        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        public int Kolicina { get; set; }

        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        public int MinimalnaKolicina { get; set; }
    }
}
