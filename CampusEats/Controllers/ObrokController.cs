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
    public class ObrokController : Controller
    {
        private readonly CampusEats.Interfaces.IObrokService _service;

        public ObrokController(CampusEats.Interfaces.IObrokService service)
        {
            _service = service;
        }

        // GET: Obrok
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: Obrok/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var obrok = await _service.GetByIdAsync(id.Value);
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
                await _service.CreateAsync(obrok);
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

            var obrok = await _service.GetByIdAsync(id.Value);
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
                    var updated = await _service.UpdateAsync(obrok);
                    if (!updated) return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _service.GetByIdAsync(obrok.Id) == null)
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

            var obrok = await _service.GetByIdAsync(id.Value);
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
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                // if not deleted, treat as not found
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ObrokExists(int id)
        {
            return await _service.GetByIdAsync(id) != null;
        }
    }
}
