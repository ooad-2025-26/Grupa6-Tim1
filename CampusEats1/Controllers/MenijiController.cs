using CampusEats.Models;
using CampusEats.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CampusEats.Controllers;

public class MenijiController : Controller
{
    private readonly IMeniService _meniService;
    private readonly IObrokService _obrokService;

    public MenijiController(IMeniService meniService, IObrokService obrokService)
    {
        _meniService = meniService;
        _obrokService = obrokService;
    }

    public async Task<IActionResult> Index()
    {
        var meniji = User.IsInRole("Administrator") || User.IsInRole("RadnikMenze")
            ? await _meniService.GetAllAsync()
            : await _meniService.GetVisibleAsync();

        return View(meniji);
    }

    public async Task<IActionResult> Details(int? id)
    {
        var meni = await _meniService.GetByIdWithObrokAsync(id);
        return meni is null ? NotFound() : View(meni);
    }

    [Authorize(Roles = "Administrator,RadnikMenze")]
    public async Task<IActionResult> Create()
    {
        await PopuniObrokeAsync();
        return View(new Meni());
    }

    [HttpPost]
    [Authorize(Roles = "Administrator,RadnikMenze")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Datum,ObrokId")] Meni meni)
    {
        if (!ModelState.IsValid)
        {
            await PopuniObrokeAsync(meni.ObrokId);
            return View(meni);
        }

        await _meniService.CreateAsync(meni);
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator,RadnikMenze")]
    public async Task<IActionResult> Edit(int? id)
    {
        var meni = await _meniService.GetByIdAsync(id);
        if (meni is null)
        {
            return NotFound();
        }

        await PopuniObrokeAsync(meni.ObrokId);
        return View(meni);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator,RadnikMenze")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Datum,ObrokId")] Meni meni)
    {
        if (id != meni.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopuniObrokeAsync(meni.ObrokId);
            return View(meni);
        }

        try
        {
            await _meniService.UpdateAsync(id, meni);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _meniService.ExistsAsync(id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrator,RadnikMenze")]
    public async Task<IActionResult> Delete(int? id)
    {
        var meni = await _meniService.GetByIdWithObrokAsync(id);
        return meni is null ? NotFound() : View(meni);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Administrator,RadnikMenze")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _meniService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task PopuniObrokeAsync(int? selectedId = null)
    {
        ViewBag.ObrokId = new SelectList(await _obrokService.GetAvailableAsync(), "Id", "Naziv", selectedId);
    }
}
