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
    public class KategoriController : Controller
    {
        private readonly DataContext _context;

        public KategoriController(DataContext context)
        {
            _context = context;
        }

        // GET: Kategori
        public async Task<IActionResult> Index()
        {
            var kategoriList = await _context.Kategori.ToListAsync();
            var sortedList = kategoriList.OrderByDescending(k => k.Id).ToList();
            return View("~/Views/Backend/Kategori/Index.cshtml", sortedList);
        }

        // GET: Kategori/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategori
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategori == null)
            {
                return NotFound();
            }

            return View("~/Views/Backend/Kategori/Details.cshtml",kategori);
        }

        // GET: Kategori/Create
        public IActionResult Create()
        {
            return View("~/Views/Backend/Kategori/Create.cshtml");
        }

        // POST: Kategori/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nama")] Kategori kategori)
        {
            if (ModelState.IsValid)
            {
                var existingKategori = await _context.Kategori
                    .FirstOrDefaultAsync(k => k.Nama == kategori.Nama);

                if (existingKategori != null)
                {
                    TempData["ErrorMessage"] = "Nama kategori sudah ada. Silakan gunakan nama lain.";
                    return View(kategori);
                }

                _context.Add(kategori);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Data berhasil ditambah.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Backend/Kategori/Create.cshtml",kategori);
        }

        // GET: Kategori/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategori.FindAsync(id);
            if (kategori == null)
            {
                return NotFound();
            }
            return View("~/Views/Backend/Kategori/Edit.cshtml",kategori);
        }

        // POST: Kategori/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nama")] Kategori kategori)
        {
            if (id != kategori.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingKategori = await _context.Kategori
                    .FirstOrDefaultAsync(k => k.Nama == kategori.Nama);

                if (existingKategori != null)
                {
                    TempData["ErrorMessage"] = "Nama kategori sudah ada. Silakan gunakan nama lain.";
                    return View(kategori);
                }

                try
                {
                    _context.Update(kategori);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategoriExists(kategori.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Data berhasil diedit.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Backend/Kategori/Edit.cshtml",kategori);
        }

        // GET: Kategori/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategori = await _context.Kategori
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategori == null)
            {
                return NotFound();
            }

            return View("~/Views/Backend/Kategori/Delete.cshtml",kategori);
        }

        // POST: Kategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategori = await _context.Kategori.FindAsync(id);
            if (kategori != null)
            {
                _context.Kategori.Remove(kategori);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Data berhasil dihapus.";
            return RedirectToAction(nameof(Index));
        }

        private bool KategoriExists(int id)
        {
            return _context.Kategori.Any(e => e.Id == id);
        }
    }
}
