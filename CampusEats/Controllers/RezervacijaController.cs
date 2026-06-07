using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusEats.Data;
using CampusEats.Models;
using Microsoft.AspNetCore.Authorization;

namespace CampusEats.Controllers
{
    [Authorize]
    public class RezervacijaController : Controller
    {
        private readonly DataContext _context;

        public RezervacijaController(DataContext context)
        {
            _context = context;
        }

        // GET: Rezervacija
        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (User.IsInRole("Admin") || User.IsInRole("Radnik"))
            {
                var all = _context.Rezervacije.Include(r => r.Obrok).Include(r => r.Korisnik);
                return View(await all.ToListAsync());
            }
            else
            {
                var mine = _context.Rezervacije.Where(r => r.KorisnikId == user.Id).Include(r => r.Obrok).Include(r => r.Korisnik);
                return View(await mine.ToListAsync());
            }
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var rezervacija = await _context.Rezervacije.Include(r => r.Obrok).Include(r => r.Korisnik).FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null) return NotFound();
            // Authorization: students can view their own, workers/admins can view all
            if (!User.IsInRole("Admin") && !User.IsInRole("Radnik") && rezervacija.Korisnik?.UserName != User.Identity.Name)
            {
                return Forbid();
            }
            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        public IActionResult Create(int? obrokId)
        {
            var model = new Rezervacija();
            if (obrokId.HasValue) model.ObrokId = obrokId.Value;
            if (obrokId.HasValue)
            {
                var obrok = _context.Obroci.FirstOrDefault(o => o.Id == obrokId.Value);
                if (obrok != null)
                {
                    ViewData["SelectedMealName"] = obrok.Naziv;
                    ViewData["SelectedMealImage"] = obrok.ImageUrl;
                }
            }
            return View(model);
        }

        // POST: Rezervacija/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ObrokId,Datum,Status")] Rezervacija rezervacija)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                rezervacija.KorisnikId = user.Id;
                rezervacija.Datum = rezervacija.Datum == default ? DateTime.Now : rezervacija.Datum;
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null) return NotFound();
            // Only Admin/Radnik or owner can edit
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (!User.IsInRole("Admin") && !User.IsInRole("Radnik") && rezervacija.KorisnikId != currentUser?.Id)
            {
                return Forbid();
            }
            ViewData["ObrokId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Obroci, "Id", "Naziv", rezervacija.ObrokId);
            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ObrokId,Datum,Status")] Rezervacija rezervacija)
        {
            if (id != rezervacija.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // server-side authorization: ensure current user is allowed to edit
                    var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                    var existing = await _context.Rezervacije.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                    if (existing == null) return NotFound();
                    if (!User.IsInRole("Admin") && !User.IsInRole("Radnik") && existing.KorisnikId != currentUser?.Id)
                    {
                        return Forbid();
                    }

                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Rezervacije.Any(e => e.Id == rezervacija.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ObrokId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Obroci, "Id", "Naziv", rezervacija.ObrokId);
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var rezervacija = await _context.Rezervacije.Include(r => r.Obrok).Include(r => r.Korisnik).FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null) return NotFound();
            // Only owner or Admin can delete
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            if (!User.IsInRole("Admin") && rezervacija.KorisnikId != currentUser.Id)
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
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null) return NotFound();

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            // Only owner or Admin can delete
            if (!User.IsInRole("Admin") && rezervacija.KorisnikId != currentUser?.Id)
            {
                return Forbid();
            }

            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Rezervacija/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, Models.StatusRezervacije status)
        {
            // Only workers and admins can update status
            if (!User.IsInRole("Radnik") && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null) return NotFound();

            rezervacija.Status = status;
            _context.Update(rezervacija);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacije.Any(e => e.Id == id);
        }
    }
}
