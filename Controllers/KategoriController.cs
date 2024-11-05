using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProjekPklInventaris.Models;

namespace ProjekPklInventaris.Controllers
{
    [Authorize]
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

            var wibTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            foreach (var kategori in kategoriList)
            {
                kategori.CreatedAt = TimeZoneInfo.ConvertTimeFromUtc(kategori.CreatedAt, wibTimeZone);
                kategori.UpdatedAt = TimeZoneInfo.ConvertTimeFromUtc(kategori.UpdatedAt, wibTimeZone);
            }

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

            return View("~/Views/Backend/Kategori/Details.cshtml", kategori);
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
                    return View("~/Views/Backend/Kategori/Create.cshtml", kategori);
                }
                DateTime utcNow = DateTime.UtcNow;

                TimeZoneInfo indonesiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime indonesiaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, indonesiaTimeZone);

                kategori.CreatedAt = indonesiaNow;
                kategori.UpdatedAt = indonesiaNow;

                _context.Add(kategori);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Data berhasil ditambah.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Nama kategori harus diisi.";
            return View("~/Views/Backend/Kategori/Create.cshtml", kategori);
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
            return View("~/Views/Backend/Kategori/Edit.cshtml", kategori);
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
              
                var existingKategori = await _context.Kategori.FindAsync(id);

                if (existingKategori == null)
                {
                    return NotFound();
                }

                var duplicateKategori = await _context.Kategori
                    .FirstOrDefaultAsync(k => k.Nama == kategori.Nama && k.Id != id);

                if (duplicateKategori != null)
                {
                    TempData["ErrorMessage"] = "Nama kategori sudah ada. Silakan gunakan nama lain.";
                    return View("~/Views/Backend/Kategori/Edit.cshtml", kategori);
                }

                try
                {
                    DateTime utcNow = DateTime.UtcNow;

                    TimeZoneInfo indonesiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime indonesiaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, indonesiaTimeZone);

                    kategori.CreatedAt = existingKategori.CreatedAt;
                    kategori.UpdatedAt = indonesiaNow;

                    _context.Entry(existingKategori).CurrentValues.SetValues(kategori); 
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
            TempData["ErrorMessage"] = "Nama kategori harus diisi.";
            return View("~/Views/Backend/Kategori/Edit.cshtml", kategori);
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

            return View("~/Views/Backend/Kategori/Delete.cshtml", kategori);
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
