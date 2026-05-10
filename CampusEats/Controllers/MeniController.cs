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
    public class MeniController : Controller
    {
        private readonly DataContext _context;

        public MeniController(DataContext context)
        {
            _context = context;
        }

        // GET: Meni
        public async Task<IActionResult> Index()
        {
            return View(await _context.Meniji.ToListAsync());
        }

        // GET: Meni/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meni = await _context.Meniji
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meni == null)
            {
                return NotFound();
            }

            return View(meni);
        }

        // GET: Meni/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Meni/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Datum")] Meni meni)
        {
            if (ModelState.IsValid)
            {
                _context.Add(meni);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(meni);
        }

        // GET: Meni/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meni = await _context.Meniji.FindAsync(id);
            if (meni == null)
            {
                return NotFound();
            }
            return View(meni);
        }

        // POST: Meni/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Datum")] Meni meni)
        {
            if (id != meni.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meni);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeniExists(meni.Id))
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
            return View(meni);
        }

        // GET: Meni/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meni = await _context.Meniji
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meni == null)
            {
                return NotFound();
            }

            return View(meni);
        }

        // POST: Meni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meni = await _context.Meniji.FindAsync(id);
            if (meni != null)
            {
                _context.Meniji.Remove(meni);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeniExists(int id)
        {
            return _context.Meniji.Any(e => e.Id == id);
        }
    }
}
