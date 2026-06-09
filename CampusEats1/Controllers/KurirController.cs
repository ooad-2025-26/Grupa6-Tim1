using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CampusEats.Models;

namespace CampusEats.Controllers;

[Authorize(Roles = "Kurir,Administrator")]
public class KurirController : Controller
{
    private readonly IRezervacijaService _rezervacijaService;
    private readonly UserManager<Korisnik> _userManager;

    public KurirController(IRezervacijaService rezervacijaService, UserManager<Korisnik> userManager)
    {
        _rezervacijaService = rezervacijaService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _rezervacijaService.GetDeliveriesAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Preuzmi(int id)
    {
        if (!int.TryParse(_userManager.GetUserId(User), out var kurirId))
        {
            return Challenge();
        }

        await _rezervacijaService.DodijeliKuriruAsync(id, kurirId);
        return RedirectToAction(nameof(Index));
    }
}
