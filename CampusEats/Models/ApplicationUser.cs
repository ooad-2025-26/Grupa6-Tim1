using Microsoft.AspNetCore.Identity;

namespace CampusEats.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Ime { get; set; }

        public string Prezime { get; set; }
    }
}
