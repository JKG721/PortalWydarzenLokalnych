// Kontroler statystyk panelu admina
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Data;

namespace PortalWydarzenLokalnych.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminStatystykiController : Controller
    {
        private readonly AppDbContext _db;

        public AdminStatystykiController(AppDbContext db)
        {
            _db = db;
        }

        // Strona statystyk
        public async Task<IActionResult> Index()
        {
            // Zliczamy podstawowe dane
            ViewBag.LiczbaWydarzen = await _db.Wydarzenia.CountAsync();
            ViewBag.LiczbaKategorii = await _db.Kategorie.CountAsync();
            ViewBag.LiczbaUzytkownikow = await _db.Users.CountAsync();
            ViewBag.LiczbaZapisow = await _db.Zapisy.CountAsync();

            // Najbliższe wydarzenia
            ViewBag.NadchodzaceWydarzenia = await _db.Wydarzenia
                .Include(w => w.Kategoria)
                .Include(w => w.Zapisy)
                .Where(w => w.DataRozpoczecia >= DateTime.Now)
                .OrderBy(w => w.DataRozpoczecia)
                .Take(5)
                .ToListAsync();

            // Najpopularniejsze wydarzenia
            ViewBag.PopularneWydarzenia = await _db.Wydarzenia
                .Include(w => w.Kategoria)
                .Include(w => w.Zapisy)
                .OrderByDescending(w => w.Zapisy.Count)
                .Take(5)
                .ToListAsync();

            return View();
        }
    }
}