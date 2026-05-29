// kontroler panelu admina do zarządzania wydarzeniami
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Data;
using PortalWydarzenLokalnych.Models;

namespace PortalWydarzenLokalnych.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminWydarzeniaController : Controller
    {
        private readonly AppDbContext _db;
        public AdminWydarzeniaController(AppDbContext db)
        {
            _db = db;
        }
        //lista wydarzeń
        public async Task<IActionResult> Index()
        {
            var wydarzenia = await _db.Wydarzenia.Include(w => w.Kategoria).ToListAsync();
            return View(wydarzenia);
        }
        //formularz dodawania wydarzenia
        public async Task<IActionResult> Dodaj()
        //pobieranie kategorii do listy wyboru
        {
            ViewBag.Kategorie = await _db.Kategorie.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Dodaj(Wydarzenie wydarzenie, IFormFile? zdjecie)
        {
            //obsuługa uploadu zdjęcia
            if (zdjecie != null && zdjecie.Length > 0)
            {
                var nazwaPliku = Guid.NewGuid().ToString() + Path.GetExtension(zdjecie.FileName);
                var sciezka = Path.Combine("wwwroot/uploads", nazwaPliku);
                using (var stream = new FileStream(sciezka, FileMode.Create))
                {
                    await zdjecie.CopyToAsync(stream);
                }
                wydarzenie.ZdjecieSciezka = "/uploads/" + nazwaPliku;
            }

            wydarzenie.DataDodania = DateTime.Now;
            _db.Wydarzenia.Add(wydarzenie);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //formularz edycji wydarzenia
        public async Task<IActionResult> Edytuj(int id)
        {
            var wydarzenie = await _db.Wydarzenia.FindAsync(id);
            if (wydarzenie == null) return NotFound();
            ViewBag.Kategorie = await _db.Kategorie.ToListAsync();
            return View(wydarzenie);
        }
        [HttpPost]
         public async Task<IActionResult> Edytuj(Wydarzenie wydarzenie, IFormFile? zdjecie)
         {
            //obsuługa uploadu zdjęcia nowego
            if (zdjecie != null && zdjecie.Length > 0)
            {
                var nazwaPliku = Guid.NewGuid().ToString() + Path.GetExtension(zdjecie.FileName);
                var sciezka = Path.Combine("wwwroot/uploads", nazwaPliku);

                using (var stream = new FileStream(sciezka, FileMode.Create))
                {
                    await zdjecie.CopyToAsync(stream);
                }

                wydarzenie.ZdjecieSciezka = "/uploads/" + nazwaPliku;
            }

            _db.Wydarzenia.Update(wydarzenie);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
         }
            //usuwanie wydarzenia
        public async Task<IActionResult> Usun(int id)
        {
            var wydarzenie = await _db.Wydarzenia.FindAsync(id);
            if (wydarzenie == null) return NotFound();

            _db.Wydarzenia.Remove(wydarzenie);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}