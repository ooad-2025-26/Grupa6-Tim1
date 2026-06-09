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
        return View(await _administracijaService.GetPregledAsync());
    }
}
