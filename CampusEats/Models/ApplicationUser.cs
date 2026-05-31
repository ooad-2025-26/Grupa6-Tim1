using Microsoft.AspNetCore.Identity;

namespace CampusEats.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Ime { get; set; }

        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public string BrojIndeksa { get; set; }
        public string Telefon { get; set; }
        public string Adresa { get; set; }
    }
}
