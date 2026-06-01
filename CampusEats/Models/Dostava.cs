namespace CampusEats.Models
{
    public class Dostava
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Adresa { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string Status { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime VrijemeDostave { get; set; }

        public int RezervacijaId { get; set; }
        public Rezervacija Rezervacija { get; set; }
    }
}
