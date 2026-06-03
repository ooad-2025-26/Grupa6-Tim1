using System.Collections.Generic;

namespace CampusEats.Models
{
    public class ProfileViewModel
    {
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BrojIndeksa { get; set; } = string.Empty;
        public string Adresa { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
