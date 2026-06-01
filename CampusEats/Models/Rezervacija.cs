namespace CampusEats.Models
{
    public class Rezervacija
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public DateTime Datum { get; set; }

        public StatusRezervacije Status { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        public string KorisnikId { get; set; }
        public ApplicationUser Korisnik { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        public int ObrokId { get; set; }
        public Obrok Obrok { get; set; }
    }
}
