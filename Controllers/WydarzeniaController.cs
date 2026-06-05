// Kontroler publicznych widoków wydarzeń
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Data;
using PortalWydarzenLokalnych.Models;

namespace PortalWydarzenLokalnych.Controllers
{
    public class WydarzeniaController : Controller
    {
        private readonly AppDbContext _db;

        public WydarzeniaController(AppDbContext db)
        {
            _db = db;
        }

        // Lista wszystkich wydarzeń z filtrowaniem
        public async Task<IActionResult> Index(int? kategoriaId, string? szukaj)
        {
            // Pobieramy wszystkie przyszłe wydarzenia
            var query = _db.Wydarzenia
                .Include(w => w.Kategoria)
                .Where(w => w.DataRozpoczecia >= DateTime.Now)
                .AsQueryable();

            // Filtrowanie po kategorii
            if (kategoriaId != null)
            {
                query = query.Where(w => w.KategoriaId == kategoriaId);
            }

            // Wyszukiwanie po nazwie lub lokalizacji
            if (!string.IsNullOrEmpty(szukaj))
            {
                query = query.Where(w => w.Nazwa.Contains(szukaj) || w.Lokalizacja.Contains(szukaj));
            }

            var wydarzenia = await query.OrderBy(w => w.DataRozpoczecia).ToListAsync();

            // Przekazujemy kategorie do filtrów
            ViewBag.Kategorie = await _db.Kategorie.ToListAsync();
            ViewBag.SzukajFraza = szukaj;
            ViewBag.KategoriaId = kategoriaId;

            return View(wydarzenia);
        }

        // Szczegóły wydarzenia
        public async Task<IActionResult> Szczegoly(int id)
        {
            var wydarzenie = await _db.Wydarzenia
                .Include(w => w.Kategoria)
                .Include(w => w.Zapisy)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (wydarzenie == null) return NotFound();

            return View(wydarzenie);
        }

        // Zapis na wydarzenie
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Zapisz(int id)
        {
            // Pobieramy id zalogowanego użytkownika
            var uzytkownikId = _db.Users
                .Where(u => u.UserName == User.Identity!.Name)
                .Select(u => u.Id)
                .FirstOrDefault();

            // Sprawdzamy czy użytkownik nie jest już zapisany
            var czyZapisany = await _db.Zapisy
                .AnyAsync(z => z.WydarzenieId == id && z.UzytkownikId == uzytkownikId);

            if (!czyZapisany)
            {
                var zapis = new Zapis
                {
                    WydarzenieId = id,
                    UzytkownikId = uzytkownikId,
                    DataZapisu = DateTime.Now
                };

                _db.Zapisy.Add(zapis);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Szczegoly", new { id });
        }
    }
}