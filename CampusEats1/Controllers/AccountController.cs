using CampusEats.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<Korisnik> _signInManager;
    private readonly UserManager<Korisnik> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public AccountController(
        SignInManager<Korisnik> signInManager,
        UserManager<Korisnik> userManager,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError(string.Empty, "Neispravan email ili lozinka.");
        return View(model);
    }

    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new Korisnik
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true,
            Ime = model.Ime,
            Prezime = model.Prezime,
            Uloga = model.Uloga,
            BrojIndeksa = model.BrojIndeksa,
            Telefon = model.Telefon,
            PhoneNumber = model.Telefon,
            Adresa = model.Adresa
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await EnsureRoleExistsAsync(model.Uloga);
            await _userManager.AddToRoleAsync(user, model.Uloga.ToString());
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    private async Task EnsureRoleExistsAsync(UlogaKorisnika uloga)
    {
        var roleName = uloga.ToString();
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }
    }
}
