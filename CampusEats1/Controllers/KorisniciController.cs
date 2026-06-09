using CampusEats.Models;
using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

[Authorize(Roles = "Administrator")]
public class KorisniciController : Controller
{
    private readonly IKorisnikService _korisnikService;
    private readonly UserManager<Korisnik> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public KorisniciController(
        IKorisnikService korisnikService,
        UserManager<Korisnik> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _korisnikService = korisnikService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _korisnikService.GetAllAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PromijeniUlogu(int id, UlogaKorisnika uloga)
    {
        var korisnik = await _userManager.FindByIdAsync(id.ToString());
        if (korisnik is null)
        {
            return NotFound();
        }

        if (korisnik.Email == User.Identity?.Name && uloga != UlogaKorisnika.Administrator)
        {
            TempData["Greska"] = "Ne mozete ukloniti administratorsku ulogu vlastitom nalogu.";
            return RedirectToAction(nameof(Index));
        }

        var roleName = uloga.ToString();
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }

        var currentRoles = await _userManager.GetRolesAsync(korisnik);
        if (currentRoles.Count > 0)
        {
            await _userManager.RemoveFromRolesAsync(korisnik, currentRoles);
        }

        korisnik.Uloga = uloga;
        await _userManager.UpdateAsync(korisnik);
        await _userManager.AddToRoleAsync(korisnik, roleName);

        TempData["Poruka"] = $"Uloga za korisnika {korisnik.Email} je promijenjena u {uloga}.";
        return RedirectToAction(nameof(Index));
    }
}
