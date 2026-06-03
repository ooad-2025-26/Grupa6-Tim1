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
    public class QRKodController : Controller
    {
        private readonly CampusEats.Interfaces.IQRKodService _service;
        private readonly CampusEats.Interfaces.IRezervacijaRepository _rezRepo;

        public QRKodController(CampusEats.Interfaces.IQRKodService service, CampusEats.Interfaces.IRezervacijaRepository rezRepo)
        {
            _service = service;
            _rezRepo = rezRepo;
        }

        // GET: QRKod
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: QRKod/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qRKod = await _service.GetByIdAsync(id.Value);
            if (qRKod == null)
            {
                return NotFound();
            }

            return View(qRKod);
        }

        // GET: QRKod/Create
        public async Task<IActionResult> Create()
        {
            var rez = await _rezRepo.GetAllAsync();
            ViewData["RezervacijaId"] = new SelectList(rez, "Id", "Id");
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
                await _service.CreateAsync(qRKod);
                return RedirectToAction(nameof(Index));
            }
            var rez2 = await _rezRepo.GetAllAsync();
            ViewData["RezervacijaId"] = new SelectList(rez2, "Id", "Id", qRKod.RezervacijaId);
            return View(qRKod);
        }

        // GET: QRKod/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qRKod = await _service.GetByIdAsync(id.Value);
            if (qRKod == null)
            {
                return NotFound();
            }
            var rez3 = await _rezRepo.GetAllAsync();
            ViewData["RezervacijaId"] = new SelectList(rez3, "Id", "Id", qRKod.RezervacijaId);
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
                    var updated = await _service.UpdateAsync(qRKod);
                    if (!updated) return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _service.GetByIdAsync(qRKod.Id) == null)
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
            ViewData["RezervacijaId"] = new SelectList(rez4, "Id", "Id", qRKod.RezervacijaId);
            return View(qRKod);
        }

        // GET: QRKod/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qRKod = await _service.GetByIdAsync(id.Value);
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
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                // not found
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> QRKodExists(int id)
        {
            return await _service.GetByIdAsync(id) != null;
        }
    }
}
