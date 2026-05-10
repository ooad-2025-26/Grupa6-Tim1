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
    public class ObrokController : Controller
    {
        private readonly DataContext _context;

        public ObrokController(DataContext context)
        {
            _context = context;
        }

        // GET: Obrok
        public async Task<IActionResult> Index()
        {
            return View(await _context.Obroci.ToListAsync());
        }

        // GET: Obrok/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obrok = await _context.Obroci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obrok == null)
            {
                return NotFound();
            }

            return View(obrok);
        }

        // GET: Obrok/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Obrok/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Naziv,Cijena,Opis,Sastojci,Dostupan")] Obrok obrok)
        {
            if (ModelState.IsValid)
            {
                _context.Add(obrok);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obrok);
        }

        // GET: Obrok/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obrok = await _context.Obroci.FindAsync(id);
            if (obrok == null)
            {
                return NotFound();
            }
            return View(obrok);
        }

        // POST: Obrok/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Cijena,Opis,Sastojci,Dostupan")] Obrok obrok)
        {
            if (id != obrok.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(obrok);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObrokExists(obrok.Id))
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
            return View(obrok);
        }

        // GET: Obrok/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obrok = await _context.Obroci
                .FirstOrDefaultAsync(m => m.Id == id);
            if (obrok == null)
            {
                return NotFound();
            }

            return View(obrok);
        }

        // POST: Obrok/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var obrok = await _context.Obroci.FindAsync(id);
            if (obrok != null)
            {
                _context.Obroci.Remove(obrok);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ObrokExists(int id)
        {
            return _context.Obroci.Any(e => e.Id == id);
        }
    }
}
