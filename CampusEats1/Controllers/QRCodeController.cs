using CampusEats.Models;
using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

[Authorize(Roles = "Administrator,RadnikMenze,Kurir,Student")]
public class QRCodeController : Controller
{
    private readonly IQRCodeService _qrCodeService;
    private readonly UserManager<Korisnik> _userManager;

    public QRCodeController(IQRCodeService qrCodeService, UserManager<Korisnik> userManager)
    {
        _qrCodeService = qrCodeService;
        _userManager = userManager;
    }

    public IActionResult Scan()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Scan(string kod)
    {
        int? korisnikId = null;
        if (int.TryParse(_userManager.GetUserId(User), out var parsedKorisnikId))
        {
            korisnikId = parsedKorisnikId;
        }

        var result = await _qrCodeService.EvidentirajAsync(
            kod,
            User.IsInRole("Student"),
            User.IsInRole("Kurir"),
            User.IsInRole("RadnikMenze"),
            User.IsInRole("Administrator"),
            korisnikId);

        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            ViewBag.Rezervacija = result.Rezervacija;
            return View();
        }

        ViewBag.Poruka = result.Message;
        ViewBag.Rezervacija = result.Rezervacija;
        return View();
    }
}
