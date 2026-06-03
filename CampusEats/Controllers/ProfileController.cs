using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CampusEats.Models;
using System.Threading.Tasks;
using System.Linq;

namespace CampusEats.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();
            var roles = await _userManager.GetRolesAsync(user);
            var model = new ProfileViewModel
            {
                Ime = user.Ime,
                Prezime = user.Prezime,
                Email = user.Email,
                BrojIndeksa = user.BrojIndeksa,
                Adresa = user.Adresa,
                Roles = roles.ToList()
            };
            return View(model);
        }
    }
}
