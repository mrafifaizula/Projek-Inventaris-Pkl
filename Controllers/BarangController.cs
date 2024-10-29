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
    public class BarangController : Controller
    {
        private readonly DataContext _context;
        private readonly string _uploadFolder;


        public BarangController(DataContext context)
        {
            _context = context;

            _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/barang");

            // Pastikan folder upload ada
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        // GET: Barang
        public async Task<IActionResult> Index()
        {
            var barangList = _context.Barang.Include(b => b.Kategori).Include(b => b.Pemasok);
            var sortedList = barangList.OrderByDescending(k => k.Id).ToList();
            return View("~/Views/Backend/barang/Index.cshtml", sortedList);
        }

        // GET: Barang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barang = await _context.Barang
                .Include(b => b.Kategori)
                .Include(b => b.Pemasok)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barang == null)
            {
                return NotFound();
            }

            return View("~/Views/Backend/barang/Details.cshtml", barang);
        }

        // GET: Barang/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["KategoriId"] = new SelectList(_context.Kategori, "Id", "Nama");
            ViewData["PemasokId"] = new SelectList(_context.Pemasok, "Id", "Nama");
            return View("~/Views/Backend/barang/Create.cshtml");
        }

        // POST: Barang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nama,Harga,Stok,Keterangan,KategoriId,PemasokId")] Barang barang, IFormFile Gambar)
        {
            if (ModelState.IsValid)
            {
                if (Gambar != null && Gambar.Length > 0)
                {
                    var randomName = $"{new Random().Next(2000, 9999)}_{Path.GetFileName(Gambar.FileName)}";
                    var filePath = Path.Combine(_uploadFolder, randomName);

                    // Pastikan folder upload ada
                    if (!Directory.Exists(_uploadFolder))
                    {
                        Directory.CreateDirectory(_uploadFolder);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Gambar.CopyToAsync(stream);
                    }

                    barang.Gambar = randomName;
                }
                else
                {
                    TempData["ErrorMessage"] = "Image harus Diisi.";

                }

                _context.Add(barang);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Data berhasil ditambah.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["KategoriId"] = new SelectList(_context.Kategori, "Id", "Nama", barang.KategoriId);
            ViewData["PemasokId"] = new SelectList(_context.Pemasok, "Id", "Nama", barang.PemasokId);
            return View("~/Views/Backend/barang/Create.cshtml", barang);
        }

        // GET: Barang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barang = await _context.Barang.FindAsync(id);
            if (barang == null)
            {
                return NotFound();
            }
            ViewData["KategoriId"] = new SelectList(_context.Kategori, "Id", "Id", barang.KategoriId);
            ViewData["PemasokId"] = new SelectList(_context.Pemasok, "Id", "Id", barang.PemasokId);
            return View("~/Views/Backend/barang/Edit.cshtml", barang);
        }

        // POST: Barang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nama,Harga,Stok,Keterangan,Gambar,KategoriId,PemasokId")] Barang barang, IFormFile Gambar)
        {
            if (id != barang.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Gambar != null && Gambar.Length > 0)
                {
                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(barang.Gambar))
                    {
                        var oldImagePath = Path.Combine(_uploadFolder, barang.Gambar);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Create a new random name for the uploaded image
                    var randomName = $"{new Random().Next(2000, 9999)}_{Path.GetFileName(Gambar.FileName)}";
                    var filePath = Path.Combine(_uploadFolder, randomName);

                    // Ensure the upload directory exists
                    if (!Directory.Exists(_uploadFolder))
                    {
                        Directory.CreateDirectory(_uploadFolder);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Gambar.CopyToAsync(stream);
                    }

                    // Update the image property in your model
                    barang.Gambar = randomName;
                }
                else
                {
                    TempData["ErrorMessage"] = "Image harus Diisi.";
                }


                try
                {
                    _context.Update(barang);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BarangExists(barang.Id))
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
            ViewData["KategoriId"] = new SelectList(_context.Kategori, "Id", "Id", barang.KategoriId);
            ViewData["PemasokId"] = new SelectList(_context.Pemasok, "Id", "Id", barang.PemasokId);
            return View("~/Views/Backend/barang/Edit.cshtml", barang);
        }

        // GET: Barang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barang = await _context.Barang
                .Include(b => b.Kategori)
                .Include(b => b.Pemasok)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barang == null)
            {
                return NotFound();
            }

            return View("~/Views/Backend/barang/Delete.cshtml", barang);
        }

        // POST: Barang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var barang = await _context.Barang.FindAsync(id);
            if (barang != null)
            {
                _context.Barang.Remove(barang);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Data berhasil dihapus.";
            return RedirectToAction(nameof(Index));
        }

        private bool BarangExists(int id)
        {
            return _context.Barang.Any(e => e.Id == id);
        }
    }
}
