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
    public class DostavaController : Controller
    {
        private readonly CampusEats.Interfaces.IDostavaService _service;
        private readonly CampusEats.Interfaces.IRezervacijaRepository _rezRepo;

        public DostavaController(CampusEats.Interfaces.IDostavaService service, CampusEats.Interfaces.IRezervacijaRepository rezRepo)
        {
            _service = service;
            _rezRepo = rezRepo;
        }

        // GET: Dostava
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: Dostava/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostava = await _service.GetByIdAsync(id.Value);
            if (dostava == null)
            {
                return NotFound();
            }

            return View(dostava);
        }

        // GET: Dostava/Create
        public async Task<IActionResult> Create()
        {
            var rez = await _rezRepo.GetAllAsync();
            ViewData["RezervacijaId"] = new SelectList(rez, "Id", "Id");
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
                await _service.CreateAsync(dostava);
                return RedirectToAction(nameof(Index));
            }
            var rez2 = await _rezRepo.GetAllAsync();
            ViewData["RezervacijaId"] = new SelectList(rez2, "Id", "Id", dostava.RezervacijaId);
            return View(dostava);
        }

        // GET: Dostava/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostava = await _service.GetByIdAsync(id.Value);
            if (dostava == null)
            {
                return NotFound();
            }
            var rez3 = await _rezRepo.GetAllAsync();
            ViewData["RezervacijaId"] = new SelectList(rez3, "Id", "Id", dostava.RezervacijaId);
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
                    var updated = await _service.UpdateAsync(dostava);
                    if (!updated) return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _service.GetByIdAsync(dostava.Id) == null)
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
            var rez4 = await _rezRepo.GetAllAsync();
            ViewData["RezervacijaId"] = new SelectList(rez4, "Id", "Id", dostava.RezervacijaId);
            return View(dostava);
        }

        // GET: Dostava/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dostava = await _service.GetByIdAsync(id.Value);
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
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                // not found
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DostavaExists(int id)
        {
            return await _service.ExistsAsync(id);
        }
    }
}
