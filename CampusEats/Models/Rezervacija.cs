using System;
using System.ComponentModel.DataAnnotations;

namespace CampusEats.Models
{
    public class Rezervacija
    {
        public int Id { get; set; }
        public int ObrokId { get; set; }
        public Obrok Obrok { get; set; }
        public string KorisnikId { get; set; }
        public ApplicationUser Korisnik { get; set; }
        [Display(Name = "Date")]
        public DateTime Datum { get; set; }
        public StatusRezervacije Status { get; set; }
    }
}
