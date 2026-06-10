using CampusEats.Models;
using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Controllers;

[Authorize(Roles = "Administrator,RadnikMenze")]
public class ObrociController : Controller
{
    private readonly IObrokService _obrokService;

    public ObrociController(IObrokService obrokService)
    {
        _obrokService = obrokService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _obrokService.GetAllAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        var obrok = await _obrokService.GetByIdAsync(id);
        return obrok is null ? NotFound() : View(obrok);
    }

    public IActionResult Create()
    {
        return View(new Obrok());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Naziv,Cijena,Opis,Sastojci,Dostupan,Kolicina")] Obrok obrok)
    {
        if (!ModelState.IsValid)
        {
            return View(obrok);
        }

        await _obrokService.CreateAsync(obrok);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        var obrok = await _obrokService.GetByIdAsync(id);
        return obrok is null ? NotFound() : View(obrok);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Cijena,Opis,Sastojci,Dostupan,Kolicina")] Obrok obrok)
    {
        if (id != obrok.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(obrok);
        }

        try
        {
            await _obrokService.UpdateAsync(id, obrok);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _obrokService.ExistsAsync(id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        var obrok = await _obrokService.DeletePreviewAsync(id);
        return obrok is null ? NotFound() : View(obrok);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _obrokService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
