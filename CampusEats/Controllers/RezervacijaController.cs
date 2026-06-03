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
        private readonly DataContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<CampusEats.Models.ApplicationUser> _userManager;

        public RezervacijaController(DataContext context, Microsoft.AspNetCore.Identity.UserManager<CampusEats.Models.ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Rezervacija
        public async Task<IActionResult> Index()
        {
            var query = _context.Rezervacije.Include(r => r.Korisnik).Include(r => r.Obrok).AsQueryable();
            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin"))
            {
                // regular users only see their own
                query = query.Where(r => r.KorisnikId == userId);
            }
            return View(await query.ToListAsync());
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Obrok)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        public IActionResult Create()
        {
            var model = new Rezervacija
            {
                Datum = DateTime.Now,
                Status = StatusRezervacije.Kreirana
            };
            ViewData["ObrokId"] = new SelectList(_context.Obroci, "Id", "Naziv");
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
                // assign current user as owner
                rezervacija.KorisnikId = _userManager.GetUserId(User);
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ObrokId"] = new SelectList(_context.Obroci, "Id", "Naziv", rezervacija.ObrokId);
            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var rezervacija = await _context.Rezervacije.Include(r => r.Korisnik).FirstOrDefaultAsync(r => r.Id == id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && rezervacija.KorisnikId != userId)
            {
                return Forbid();
            }
            ViewData["ObrokId"] = new SelectList(_context.Obroci, "Id", "Naziv", rezervacija.ObrokId);
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
                try
                {
                    // preserve owner
                    var existing = await _context.Rezervacije.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
                    if (existing == null) return NotFound();
                    var userId = _userManager.GetUserId(User);
                    if (!User.IsInRole("Admin") && existing.KorisnikId != userId)
                    {
                        return Forbid();
                    }
                    rezervacija.KorisnikId = existing.KorisnikId;
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ObrokId"] = new SelectList(_context.Obroci, "Id", "Naziv", rezervacija.ObrokId);
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Obrok)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null) return NotFound();
            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && rezervacija.KorisnikId != userId)
            {
                return Forbid();
            }
            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacije.Any(e => e.Id == id);
        }
    }
}
