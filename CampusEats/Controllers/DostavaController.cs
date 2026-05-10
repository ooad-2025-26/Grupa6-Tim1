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
    public class DostavaController : Controller
    {
        private readonly DataContext _context;

        public DostavaController(DataContext context)
        {
            _context = context;
        }

        // GET: Dostava
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Dostave.Include(d => d.Rezervacija);
            return View(await dataContext.ToListAsync());
        }

        // GET: Dostava/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostava = await _context.Dostave
                .Include(d => d.Rezervacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dostava == null)
            {
                return NotFound();
            }

            return View(dostava);
        }

        // GET: Dostava/Create
        public IActionResult Create()
        {
            ViewData["RezervacijaId"] = new SelectList(_context.Rezervacije, "Id", "Id");
            return View();
        }

        // POST: Dostava/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Adresa,Status,VrijemeDostave,RezervacijaId")] Dostava dostava)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dostava);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RezervacijaId"] = new SelectList(_context.Rezervacije, "Id", "Id", dostava.RezervacijaId);
            return View(dostava);
        }

        // GET: Dostava/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostava = await _context.Dostave.FindAsync(id);
            if (dostava == null)
            {
                return NotFound();
            }
            ViewData["RezervacijaId"] = new SelectList(_context.Rezervacije, "Id", "Id", dostava.RezervacijaId);
            return View(dostava);
        }

        // POST: Dostava/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Adresa,Status,VrijemeDostave,RezervacijaId")] Dostava dostava)
        {
            if (id != dostava.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dostava);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DostavaExists(dostava.Id))
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
            ViewData["RezervacijaId"] = new SelectList(_context.Rezervacije, "Id", "Id", dostava.RezervacijaId);
            return View(dostava);
        }

        // GET: Dostava/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostava = await _context.Dostave
                .Include(d => d.Rezervacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dostava == null)
            {
                return NotFound();
            }

            return View(dostava);
        }

        // POST: Dostava/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dostava = await _context.Dostave.FindAsync(id);
            if (dostava != null)
            {
                _context.Dostave.Remove(dostava);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DostavaExists(int id)
        {
            return _context.Dostave.Any(e => e.Id == id);
        }
    }
}
