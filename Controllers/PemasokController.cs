using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjekPklInventaris.Models;

namespace ProjekPklInventaris.Controllers
{
    public class PemasokController : Controller
    {
        private readonly DataContext _context;

        public PemasokController(DataContext context)
        {
            _context = context;
        }

        // GET: Pemasok
        public async Task<IActionResult> Index()
        {
            var pemasokList = await _context.Pemasok.ToListAsync();
            var sortedList = pemasokList.OrderByDescending(k => k.Id).ToList();
            return View("~/Views/Backend/Pemasok/Index.cshtml", sortedList);
        }

        // GET: Pemasok/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pemasok = await _context.Pemasok
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pemasok == null)
            {
                return NotFound();
            }

            return View("~/Views/Backend/Pemasok/Details.cshtml",pemasok);
        }

        // GET: Pemasok/Create
        public IActionResult Create()
        {
            return View("~/Views/Backend/Pemasok/Create.cshtml");
        }

        // POST: Pemasok/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nama,Alamat")] Pemasok pemasok)
        {
            if (ModelState.IsValid)
            {
                var existingPemasok = await _context.Pemasok
                    .FirstOrDefaultAsync(k => k.Nama == pemasok.Nama);

                if (existingPemasok != null)
                {
                    TempData["ErrorMessage"] = "Nama Pemasok sudah ada. Silakan gunakan nama lain.";
                    return View(pemasok);
                }

                _context.Add(pemasok);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Data Berhasil Ditambah.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Backend/Pemasok/Create.cshtml",pemasok);
        }

        // GET: Pemasok/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pemasok = await _context.Pemasok.FindAsync(id);
            if (pemasok == null)
            {
                return NotFound();
            }
            return View("~/Views/Backend/Pemasok/Edit.cshtml",pemasok);
        }

        // POST: Pemasok/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nama,Alamat")] Pemasok pemasok)
        {
            if (id != pemasok.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingPemasok = await _context.Pemasok
                    .FirstOrDefaultAsync(k => k.Nama == pemasok.Nama);

                if (existingPemasok != null)
                {
                    TempData["ErrorMessage"] = "Nama Pemasok sudah ada. Silakan gunakan nama lain.";
                    return View(pemasok);
                }

                try
                {
                    _context.Update(pemasok);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PemasokExists(pemasok.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Data Berhasil Ditambah.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Backend/Pemasok/Edit.cshtml",pemasok);
        }

        // GET: Pemasok/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pemasok = await _context.Pemasok
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pemasok == null)
            {
                return NotFound();
            }

            return View("~/Views/Backend/Pemasok/Delete.cshtml",pemasok);
        }

        // POST: Pemasok/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pemasok = await _context.Pemasok.FindAsync(id);
            if (pemasok != null)
            {
                _context.Pemasok.Remove(pemasok);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Data Berhasil Ditambah.";
            return RedirectToAction(nameof(Index));
        }

        private bool PemasokExists(int id)
        {
            return _context.Pemasok.Any(e => e.Id == id);
        }
    }
}
