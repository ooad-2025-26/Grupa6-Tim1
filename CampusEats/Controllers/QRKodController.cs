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

        public QRKodController(CampusEats.Interfaces.IQRKodService service)
        {
            _service = service;
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
        public IActionResult Create()
        {
            ViewData["RezervacijaId"] = new SelectList(HttpContext.RequestServices.GetRequiredService<CampusEats.Data.DataContext>().Rezervacije, "Id", "Id");
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
            ViewData["RezervacijaId"] = new SelectList(HttpContext.RequestServices.GetRequiredService<CampusEats.Data.DataContext>().Rezervacije, "Id", "Id", qRKod.RezervacijaId);
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
            ViewData["RezervacijaId"] = new SelectList(HttpContext.RequestServices.GetRequiredService<CampusEats.Data.DataContext>().Rezervacije, "Id", "Id", qRKod.RezervacijaId);
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
            ViewData["RezervacijaId"] = new SelectList(HttpContext.RequestServices.GetRequiredService<CampusEats.Data.DataContext>().Rezervacije, "Id", "Id", qRKod.RezervacijaId);
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
