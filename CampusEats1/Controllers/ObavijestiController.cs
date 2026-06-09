using CampusEats.Models;
using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

[Authorize]
public class ObavijestiController : Controller
{
    private readonly IObavijestService _obavijestService;
    private readonly UserManager<Korisnik> _userManager;

    public ObavijestiController(IObavijestService obavijestService, UserManager<Korisnik> userManager)
    {
        _obavijestService = obavijestService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("Administrator"))
        {
            return View(await _obavijestService.GetLatestAsync(50));
        }

        var korisnik = await _userManager.GetUserAsync(User);
        return korisnik is null ? Challenge() : View(await _obavijestService.GetForUserAsync(korisnik.Id));
    }
}
