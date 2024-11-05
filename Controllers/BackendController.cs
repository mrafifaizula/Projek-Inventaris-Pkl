using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjekPklInventaris.Models;

namespace ProjekPklInventaris.Controllers
{
    [Authorize]
    public class BackendController : Controller
    {
        private readonly DataContext _context;

        public BackendController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var barangCount = _context.Barang.Count();
            var pemasokCount = _context.Pemasok.Count();
            var kategoriCount = _context.Kategori.Count();

            ViewBag.BarangCount = barangCount; 
            ViewBag.PemasokCount = pemasokCount; 
            ViewBag.KategoriCount = kategoriCount; 
            return View("Dashboard");
        }
    }
}
