// Kontroler publicznych widoków wydarzeń
using Microsoft.AspNetCore.Authorization;
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

        // Lista wszystkich wydarzeń z filtrowaniem, sortowaniem i paginacją
        public async Task<IActionResult> Index(int? kategoriaId, string? szukaj, string? dataOd, string? dataDo, string? sortuj, int strona = 1)
        {
            // Liczba wydarzeń na stronie
            int naStronie = 6;

            var query = _db.Wydarzenia
                .Include(w => w.Kategoria)
                .Include(w => w.Zapisy)
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

            // Filtrowanie po dacie od
            if (!string.IsNullOrEmpty(dataOd))
            {
                var od = DateTime.Parse(dataOd);
                query = query.Where(w => w.DataRozpoczecia >= od);
            }

            // Filtrowanie po dacie do
            if (!string.IsNullOrEmpty(dataDo))
            {
                var doo = DateTime.Parse(dataDo);
                query = query.Where(w => w.DataRozpoczecia <= doo);
            }

            // Sortowanie
            query = sortuj switch
            {
                "nazwa" => query.OrderBy(w => w.Nazwa),
                "nazwa_desc" => query.OrderByDescending(w => w.Nazwa),
                "data_desc" => query.OrderByDescending(w => w.DataRozpoczecia),
                _ => query.OrderBy(w => w.DataRozpoczecia)
            };

            // Paginacja
            int lacznie = await query.CountAsync();
            int liczbaStron = (int)Math.Ceiling(lacznie / (double)naStronie);

            var wydarzenia = await query
                .Skip((strona - 1) * naStronie)
                .Take(naStronie)
                .ToListAsync();

            // Przekazujemy dane do widoku
            ViewBag.Kategorie = await _db.Kategorie.ToListAsync();
            ViewBag.SzukajFraza = szukaj;
            ViewBag.KategoriaId = kategoriaId;
            ViewBag.DataOd = dataOd;
            ViewBag.DataDo = dataDo;
            ViewBag.Sortuj = sortuj;
            ViewBag.Strona = strona;
            ViewBag.LiczbaStron = liczbaStron;
            ViewBag.Lacznie = lacznie;

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

            // Sprawdzamy czy zalogowany użytkownik jest już zapisany
            var uzytkownikId = _db.Users
                .Where(u => u.UserName == User.Identity!.Name)
                .Select(u => u.Id)
                .FirstOrDefault();

            ViewBag.CzyZapisany = await _db.Zapisy
                .AnyAsync(z => z.WydarzenieId == id && z.UzytkownikId == uzytkownikId);

            return View(wydarzenie);
        }

        // Zapis na wydarzenie
        [Authorize]
        public async Task<IActionResult> Zapisz(int id)
        {
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

        // Wypisanie z wydarzenia
        [Authorize]
        public async Task<IActionResult> Wypisz(int id)
        {
            var uzytkownikId = _db.Users
                .Where(u => u.UserName == User.Identity!.Name)
                .Select(u => u.Id)
                .FirstOrDefault();

            var zapis = await _db.Zapisy
                .FirstOrDefaultAsync(z => z.WydarzenieId == id && z.UzytkownikId == uzytkownikId);

            if (zapis != null)
            {
                _db.Zapisy.Remove(zapis);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction("Szczegoly", new { id });
        }
    }
}