using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjekPklInventaris.Models;
using System.Threading.Tasks;

namespace ProjekPklInventaris.Controllers
{
    [Authorize]
    public class BackendController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BackendController(DataContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var barangCount = await _context.Barang.CountAsync();
            var pemasokCount = await _context.Pemasok.CountAsync();
            var kategoriCount = await _context.Kategori.CountAsync();
            var userCount = await _userManager.Users.CountAsync();

            ViewBag.BarangCount = barangCount;
            ViewBag.PemasokCount = pemasokCount;
            ViewBag.KategoriCount = kategoriCount;
            ViewBag.UserCount = userCount;

            return View("Dashboard");
        }
    }
}
