using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CampusEats.Data;
using CampusEats.Models;

namespace CampusEats.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class RezervacijaController : Controller
    {
        private readonly CampusEats.Interfaces.IRezervacijaService _service;
        private readonly CampusEats.Interfaces.IObrokRepository _obrokRepo;
        private readonly Microsoft.AspNetCore.Identity.UserManager<CampusEats.Models.ApplicationUser> _userManager;

        public RezervacijaController(CampusEats.Interfaces.IRezervacijaService service, CampusEats.Interfaces.IObrokRepository obrokRepo, Microsoft.AspNetCore.Identity.UserManager<CampusEats.Models.ApplicationUser> userManager)
        {
            _service = service;
            _obrokRepo = obrokRepo;
            _userManager = userManager;
        }

        // GET: Rezervacija
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var canManageAll = User.IsInRole("Admin") || User.IsInRole("Radnik");
            var list = await _service.GetAllAsync(userId, canManageAll);
            return View(list);
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _service.GetByIdAsync(id.Value);
            if (rezervacija == null)
            {
                return NotFound();
            }
            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        public async Task<IActionResult> Create(int? obrokId)
        {
            var model = new Rezervacija
            {
                Datum = DateTime.Now,
                Status = StatusRezervacije.Kreirana
            };
            var obroci = await _obrokRepo.GetAllAsync();
            ViewData["ObrokId"] = new SelectList(obroci, "Id", "Naziv", obrokId);
            return View(model);
        }

        // POST: Rezervacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Datum,Status,ObrokId")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                await _service.CreateReservationAsync(userId, rezervacija.ObrokId);
                return RedirectToAction(nameof(Index));
            }
            var obroci = await _obrokRepo.GetAllAsync();
            ViewData["ObrokId"] = new SelectList(obroci, "Id", "Naziv", rezervacija.ObrokId);
            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rezervacija = await _service.GetByIdAsync(id.Value);
            if (rezervacija == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            var canManageAll = User.IsInRole("Admin") || User.IsInRole("Radnik");
            if (!canManageAll && rezervacija.KorisnikId != userId)
            {
                return Forbid();
            }
            ViewData["ObrokId"] = new SelectList(HttpContext.RequestServices.GetRequiredService<CampusEats.Data.DataContext>().Obroci, "Id", "Naziv", rezervacija.ObrokId);
            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Datum,Status,ObrokId")] Rezervacija rezervacija)
        {
            if (id != rezervacija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var canManageAll = User.IsInRole("Admin") || User.IsInRole("Radnik");
                var updated = await _service.UpdateAsync(rezervacija, userId, canManageAll);
                if (!updated)
                {
                    if (await _service.GetByIdAsync(rezervacija.Id) == null)
                    {
                        return NotFound();
                    }
                    return Forbid();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ObrokId"] = new SelectList(HttpContext.RequestServices.GetRequiredService<CampusEats.Data.DataContext>().Obroci, "Id", "Naziv", rezervacija.ObrokId);
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _service.GetByIdAsync(id.Value);
            if (rezervacija == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && rezervacija.KorisnikId != userId)
            {
                return Forbid();
            }

            return View(rezervacija);
        }

        // POST: Rezervacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var canManageAll = User.IsInRole("Admin") || User.IsInRole("Radnik");
            var deleted = await _service.DeleteAsync(id, userId, canManageAll);
            if (!deleted) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RezervacijaExists(int id)
        {
            return await _service.GetByIdAsync(id) != null;
        }
    }
}
