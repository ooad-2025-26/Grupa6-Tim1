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
    public class ZalihaController : Controller
    {
        private readonly DataContext _context;

        public ZalihaController(DataContext context)
        {
            _context = context;
        }

        // GET: Zaliha
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zalihe.ToListAsync());
        }

        // GET: Zaliha/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaliha = await _context.Zalihe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zaliha == null)
            {
                return NotFound();
            }

            return View(zaliha);
        }

        // GET: Zaliha/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zaliha/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NazivArtikla,Kolicina,MinimalnaKolicina")] Zaliha zaliha)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zaliha);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zaliha);
        }

        // GET: Zaliha/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaliha = await _context.Zalihe.FindAsync(id);
            if (zaliha == null)
            {
                return NotFound();
            }
            return View(zaliha);
        }

        // POST: Zaliha/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NazivArtikla,Kolicina,MinimalnaKolicina")] Zaliha zaliha)
        {
            if (id != zaliha.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zaliha);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZalihaExists(zaliha.Id))
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
            return View(zaliha);
        }

        // GET: Zaliha/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaliha = await _context.Zalihe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zaliha == null)
            {
                return NotFound();
            }

            return View(zaliha);
        }

        // POST: Zaliha/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zaliha = await _context.Zalihe.FindAsync(id);
            if (zaliha != null)
            {
                _context.Zalihe.Remove(zaliha);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZalihaExists(int id)
        {
            return _context.Zalihe.Any(e => e.Id == id);
        }
    }
}
