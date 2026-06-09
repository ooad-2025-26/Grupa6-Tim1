using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

[Authorize(Roles = "Administrator")]
public class AdministracijaController : Controller
{
    private readonly IAdministracijaService _administracijaService;

    public AdministracijaController(IAdministracijaService administracijaService)
    {
        _administracijaService = administracijaService;
    }

    public async Task<IActionResult> Index()
    {
        var statistika = await _administracijaService.GetStatistikaAsync();
        ViewBag.BrojKorisnika = statistika.BrojKorisnika;
        ViewBag.BrojObroka = statistika.BrojObroka;
        ViewBag.BrojRezervacija = statistika.BrojRezervacija;
        ViewBag.BrojAktivnihObroka = statistika.BrojAktivnihObroka;

        return View();
    }
}
