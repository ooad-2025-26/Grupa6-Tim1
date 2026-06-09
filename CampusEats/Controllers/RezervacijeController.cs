using CampusEats.Models;
using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Controllers;

[Authorize]
public class RezervacijeController : Controller
{
    private readonly IKorisnikService _korisnikService;
    private readonly IObrokService _obrokService;
    private readonly IRezervacijaService _rezervacijaService;

    public RezervacijeController(
        IRezervacijaService rezervacijaService,
        IObrokService obrokService,
        IKorisnikService korisnikService)
    {
        _rezervacijaService = rezervacijaService;
        _obrokService = obrokService;
        _korisnikService = korisnikService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _rezervacijaService.GetAllAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        var rezervacija = await _rezervacijaService.GetByIdWithDetailsAsync(id);
        return rezervacija is null ? NotFound() : View(rezervacija);
    }

    public async Task<IActionResult> Create()
    {
        await PopuniListeAsync();
        return View(new Rezervacija());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("KorisnikId,ObrokId,TerminPreuzimanja,NacinPreuzimanja")] Rezervacija rezervacija)
    {
        if (!ModelState.IsValid)
        {
            await PopuniListeAsync();
            return View(rezervacija);
        }

        var result = await _rezervacijaService.CreateAsync(rezervacija);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            await PopuniListeAsync();
            return View(rezervacija);
        }

        return RedirectToAction(nameof(Details), new { id = rezervacija.Id });
    }

    [Authorize(Roles = "Administrator,RadnikMenze")]
    public async Task<IActionResult> Edit(int? id)
    {
        var rezervacija = await _rezervacijaService.GetByIdWithDetailsAsync(id);
        if (rezervacija is null)
        {
            return NotFound();
        }

        await PopuniListeAsync(rezervacija.KorisnikId, rezervacija.ObrokId);
        return View(rezervacija);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator,RadnikMenze")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,KorisnikId,ObrokId,Datum,TerminPreuzimanja,NacinPreuzimanja,Status")] Rezervacija rezervacija)
    {
        if (id != rezervacija.Id)
        {
            return NotFound();
        }

        if (User.IsInRole("RadnikMenze") && !User.IsInRole("Administrator"))
        {
            var statusUpdated = await _rezervacijaService.UpdateStatusAsync(id, rezervacija.Status);
            return statusUpdated ? RedirectToAction(nameof(Index)) : NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopuniListeAsync(rezervacija.KorisnikId, rezervacija.ObrokId);
            return View(rezervacija);
        }

        try
        {
            await _rezervacijaService.UpdateAsync(id, rezervacija);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _rezervacijaService.ExistsAsync(id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int? id)
    {
        var rezervacija = await _rezervacijaService.GetByIdWithDetailsAsync(id);
        return rezervacija is null ? NotFound() : View(rezervacija);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrator")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _rezervacijaService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopuniListeAsync(int? korisnikId = null, int? obrokId = null)
    {
        ViewBag.KorisnikId = new SelectList(await _korisnikService.GetStudentsAsync(), "Id", "Email", korisnikId);
        ViewBag.ObrokId = new SelectList(await _obrokService.GetAvailableAsync(), "Id", "Naziv", obrokId);
    }
}
