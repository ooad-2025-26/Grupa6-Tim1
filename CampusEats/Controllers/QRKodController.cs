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
    public class QRKodController : Controller
    {
        private readonly DataContext _context;

        public QRKodController(DataContext context)
        {
            _context = context;
        }

        // GET: QRKod
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.QRKodovi.Include(q => q.Rezervacija);
            return View(await dataContext.ToListAsync());
        }

        // GET: QRKod/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qRKod = await _context.QRKodovi
                .Include(q => q.Rezervacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qRKod == null)
            {
                return NotFound();
            }

            return View(qRKod);
        }

        // GET: QRKod/Create
        public IActionResult Create()
        {
            ViewData["RezervacijaId"] = new SelectList(_context.Rezervacije, "Id", "Id");
            return View();
        }

        // POST: QRKod/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Validan,VrijemeGenerisanja,Kod,RezervacijaId")] QRKod qRKod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qRKod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RezervacijaId"] = new SelectList(_context.Rezervacije, "Id", "Id", qRKod.RezervacijaId);
            return View(qRKod);
        }

        // GET: QRKod/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qRKod = await _context.QRKodovi.FindAsync(id);
            if (qRKod == null)
            {
                return NotFound();
            }
            ViewData["RezervacijaId"] = new SelectList(_context.Rezervacije, "Id", "Id", qRKod.RezervacijaId);
            return View(qRKod);
        }

        // POST: QRKod/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Validan,VrijemeGenerisanja,Kod,RezervacijaId")] QRKod qRKod)
        {
            if (id != qRKod.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qRKod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QRKodExists(qRKod.Id))
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
            ViewData["RezervacijaId"] = new SelectList(_context.Rezervacije, "Id", "Id", qRKod.RezervacijaId);
            return View(qRKod);
        }

        // GET: QRKod/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qRKod = await _context.QRKodovi
                .Include(q => q.Rezervacija)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qRKod == null)
            {
                return NotFound();
            }

            return View(qRKod);
        }

        // POST: QRKod/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qRKod = await _context.QRKodovi.FindAsync(id);
            if (qRKod != null)
            {
                _context.QRKodovi.Remove(qRKod);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QRKodExists(int id)
        {
            return _context.QRKodovi.Any(e => e.Id == id);
        }
    }
}
