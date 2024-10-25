using Microsoft.AspNetCore.Mvc;

namespace ProjekInventaris.Controllers
{
    public class BackendController : Controller
    {
        // GET: /dashboard
        public IActionResult Index()
        {
            return View("Dashboard");
        }
    }
}
