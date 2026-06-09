using CampusEats.Models;
using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

[Authorize(Roles = "Student,Administrator")]
public class PreporukeController : Controller
{
    private readonly IPreporukaService _preporukaService;
    private readonly UserManager<Korisnik> _userManager;

    public PreporukeController(IPreporukaService preporukaService, UserManager<Korisnik> userManager)
    {
        _preporukaService = preporukaService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var korisnik = await _userManager.GetUserAsync(User);
        return korisnik is null ? Challenge() : View(await _preporukaService.GetPreporukeAsync(korisnik.Id));
    }
}
