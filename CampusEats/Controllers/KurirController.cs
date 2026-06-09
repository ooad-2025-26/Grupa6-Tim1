using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

[Authorize(Roles = "Kurir,Administrator")]
public class KurirController : Controller
{
    private readonly IRezervacijaService _rezervacijaService;

    public KurirController(IRezervacijaService rezervacijaService)
    {
        _rezervacijaService = rezervacijaService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _rezervacijaService.GetDeliveriesAsync());
    }
}
