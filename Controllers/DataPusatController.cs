using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ProjekPklInventaris.Models;

namespace ProjekPklInventaris.Controllers
{
    [Authorize]

    public class DataPusatController : Controller
    {
        private readonly DataContext _context;

        public DataPusatController(DataContext context)
        {
            _context = context;
        }

        // GET: DataPusat
        public async Task<IActionResult> Index()
        {
            var dataPusatList = await _context.DataPusat.ToListAsync();
            var sortedList = dataPusatList.OrderByDescending(k => k.Id).ToList();
            return View("~/Views/Backend/DataPusat/Index.cshtml", sortedList);
        }

        // GET: DataPusat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataPusat = await _context.DataPusat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataPusat == null)
            {
                return NotFound();
            }

            return View("~/Views/Backend/DataPusat/Details.cshtml", dataPusat);
        }

        // GET: DataPusat/Create
        public IActionResult Create()
        {
            return View("~/Views/Backend/DataPusat/Create.cshtml");
        }

        // POST: DataPusat/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nama,Stok,Brand")] DataPusat dataPusat)
        {
            if (ModelState.IsValid)
            {
                DateTime utcNow = DateTime.UtcNow;

                TimeZoneInfo indonesiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime indonesiaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, indonesiaTimeZone);

                dataPusat.CreatedAt = indonesiaNow;
                dataPusat.UpdatedAt = indonesiaNow;

                _context.Add(dataPusat);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Data berhasil ditambah.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Views/Backend/DataPusat/Create.cshtml", dataPusat);
        }

        // GET: DataPusat/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataPusat = await _context.DataPusat.FindAsync(id);
            if (dataPusat == null)
            {
                return NotFound();
            }
            return View("~/Views/Backend/DataPusat/Edit.cshtml", dataPusat);
        }

        // POST: DataPusat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nama,Stok,Brand")] DataPusat dataPusat)
        {
            if (id != dataPusat.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingDataPusat = await _context.DataPusat.FindAsync(id);

                if (existingDataPusat == null)
                {
                    return NotFound();
                }


                try
                {
                    DateTime utcNow = DateTime.UtcNow;

                    TimeZoneInfo indonesiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime indonesiaNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, indonesiaTimeZone);

                    dataPusat.CreatedAt = existingDataPusat.CreatedAt;
                    dataPusat.UpdatedAt = indonesiaNow;

                    _context.Entry(existingDataPusat).CurrentValues.SetValues(dataPusat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DataPusatExists(dataPusat.Id))
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
            return View("~/Views/Backend/DataPusat/Create.cshtml", dataPusat);
        }

        // GET: DataPusat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dataPusat = await _context.DataPusat
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dataPusat == null)
            {
                return NotFound();
            }

            return View("~/Views/Backend/DataPusat/Delete.cshtml", dataPusat);
        }

        // POST: DataPusat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dataPusat = await _context.DataPusat.FindAsync(id);
            if (dataPusat != null)
            {
                _context.DataPusat.Remove(dataPusat);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Data berhasil dihapus.";
            return RedirectToAction(nameof(Index));
        }

        private bool DataPusatExists(int id)
        {
            return _context.DataPusat.Any(e => e.Id == id);
        }
    }
}
