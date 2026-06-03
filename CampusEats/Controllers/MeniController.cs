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
    public class MeniController : Controller
    {
        private readonly CampusEats.Interfaces.IMeniService _service;

        public MeniController(CampusEats.Interfaces.IMeniService service)
        {
            _service = service;
        }

        // GET: Meni
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetAllAsync());
        }

        // GET: Meni/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meni = await _service.GetByIdAsync(id.Value);
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
                await _service.CreateAsync(meni);
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

            var meni = await _service.GetByIdAsync(id.Value);
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
                    var updated = await _service.UpdateAsync(meni);
                    if (!updated) return NotFound();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _service.GetByIdAsync(meni.Id) == null)
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

            var meni = await _service.GetByIdAsync(id.Value);
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
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
            {
                // not found
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> MeniExists(int id)
        {
            return await _service.GetByIdAsync(id) != null;
        }
    }
}
