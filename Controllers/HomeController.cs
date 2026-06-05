// Kontroler strony głównej
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Data;

namespace PortalWydarzenLokalnych.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        // Strona główna - pokazuje najbliższe wydarzenia
        public async Task<IActionResult> Index()
        {
            var wydarzenia = await _db.Wydarzenia
                .Include(w => w.Kategoria)
                .Where(w => w.DataRozpoczecia >= DateTime.Now)
                .OrderBy(w => w.DataRozpoczecia)
                .Take(6)
                .ToListAsync();

            return View(wydarzenia);
        }
    }
}