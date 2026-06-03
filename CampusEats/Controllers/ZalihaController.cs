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
    public class ZalihaController : Controller
    {
        private readonly CampusEats.Interfaces.IZalihaService _service;

        public ZalihaController(CampusEats.Interfaces.IZalihaService service)
        {
            _service = service;
        }

        // GET: Zaliha
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: Zaliha/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zaliha = await _service.GetByIdAsync(id.Value);
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
                await _service.CreateAsync(zaliha);
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

            var zaliha = await _service.GetByIdAsync(id.Value);
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
                    var updated = await _service.UpdateAsync(zaliha);
                    if (!updated) return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _service.GetByIdAsync(zaliha.Id) == null)
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

            var zaliha = await _service.GetByIdAsync(id.Value);
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
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                // not found
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ZalihaExists(int id)
        {
            return await _service.ExistsAsync(id);
        }
    }
}
