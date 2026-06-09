using CampusEats.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

[Authorize(Roles = "Student,Administrator")]
public class PreferencijeController : Controller
{
    private readonly UserManager<Korisnik> _userManager;

    public PreferencijeController(UserManager<Korisnik> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var korisnik = await _userManager.GetUserAsync(User);
        return korisnik is null ? Challenge() : View(korisnik);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([Bind("Alergije,OmiljenaHrana,Vegetarijanac")] Korisnik model)
    {
        var korisnik = await _userManager.GetUserAsync(User);
        if (korisnik is null)
        {
            return Challenge();
        }

        korisnik.Alergije = model.Alergije;
        korisnik.OmiljenaHrana = model.OmiljenaHrana;
        korisnik.Vegetarijanac = model.Vegetarijanac;
        await _userManager.UpdateAsync(korisnik);

        TempData["Poruka"] = "Preferencije su sacuvane.";
        return RedirectToAction(nameof(Index));
    }
}
