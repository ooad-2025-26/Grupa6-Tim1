using Microsoft.AspNetCore.Identity;

namespace CampusEats.Models
{
    public class ApplicationUser : IdentityUser
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Ime { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required]
        public string Prezime { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required]
        public string BrojIndeksa { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Required]
        public string Adresa { get; set; } = string.Empty;
    }
}
