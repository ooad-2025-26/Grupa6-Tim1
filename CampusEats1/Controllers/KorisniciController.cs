using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

[Authorize(Roles = "Administrator")]
public class KorisniciController : Controller
{
    private readonly IKorisnikService _korisnikService;

    public KorisniciController(IKorisnikService korisnikService)
    {
        _korisnikService = korisnikService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _korisnikService.GetAllAsync());
    }
}
