//kontroler panelu admina do zarządzania kategoriami
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Data;
using PortalWydarzenLokalnych.Models;

namespace PortalWydarzenLokalnych.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminKategorieController : Controller
    {
        private readonly AppDbContext _db;
        public AdminKategorieController(AppDbContext db)
        {
            _db = db;
        }
        //lista kategorii (wszystkie)
        public async Task<IActionResult> Index()
        {
            var kategorie = await _db.Kategorie.ToListAsync();
            return View(kategorie);
        }
        //formularz dodawania kategorii
        public IActionResult Dodaj()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Dodaj(Kategoria kategoria)
        {
            _db.Kategorie.Add(kategoria);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //formularz edycji kategorii
        public async Task<IActionResult> Edytuj(int id)
        {
            var kategoria = await _db.Kategorie.FindAsync(id);
            if (kategoria == null) return NotFound();
            return View(kategoria);
        }
        [HttpPost]
        public async Task<IActionResult> Edytuj(Kategoria kategoria)
        {
            _db.Kategorie.Update(kategoria);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        //usuwanie kategorii
        public async Task<IActionResult> Usun(int id)
        {
            var kategoria = await _db.Kategorie.FindAsync(id);
            if (kategoria == null) return NotFound();
            _db.Kategorie.Remove(kategoria);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}   
