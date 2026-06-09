using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusEats.Controllers;

[Authorize(Roles = "Administrator,RadnikMenze,Kurir")]
public class QRCodeController : Controller
{
    private readonly IQRCodeService _qrCodeService;

    public QRCodeController(IQRCodeService qrCodeService)
    {
        _qrCodeService = qrCodeService;
    }

    public IActionResult Scan()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Scan(string kod)
    {
        var result = await _qrCodeService.EvidentirajAsync(kod);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }

        ViewBag.Poruka = result.Message;
        ViewBag.Rezervacija = result.Rezervacija;
        return View();
    }
}
