// Kontroler strony z mapą wszystkich wydarzeń
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Data;

namespace PortalWydarzenLokalnych.Controllers
{
    public class MapaController : Controller
    {
        private readonly AppDbContext _db;

        public MapaController(AppDbContext db)
        {
            _db = db;
        }

        // Strona z mapą wszystkich wydarzeń
        public async Task<IActionResult> Index()
        {
            // Pobieramy wszystkie przyszłe wydarzenia ze współrzędnymi
            var wydarzenia = await _db.Wydarzenia
                .Include(w => w.Kategoria)
                .Where(w => w.DataRozpoczecia >= DateTime.Now && w.Szerokosc != 0 && w.Dlugosc != 0)
                .OrderBy(w => w.DataRozpoczecia)
                .ToListAsync();

            return View(wydarzenia);
        }
    }
}