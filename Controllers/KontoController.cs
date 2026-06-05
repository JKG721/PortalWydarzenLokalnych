// Kontroler obsługujący rejestrację i logowanie użytkowników
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Data;
using PortalWydarzenLokalnych.Models;

namespace PortalWydarzenLokalnych.Controllers
{
    public class KontoController : Controller
    {
        private readonly UserManager<Uzytkownik> _userManager;
        private readonly SignInManager<Uzytkownik> _signInManager;
        private readonly AppDbContext _db;

        public KontoController(UserManager<Uzytkownik> userManager, SignInManager<Uzytkownik> signInManager, AppDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        // Strona rejestracji
        public IActionResult Rejestracja()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Rejestracja(string imie, string nazwisko, string email, string haslo)
        {
            // Tworzymy nowego użytkownika
            var uzytkownik = new Uzytkownik
            {
                Imie = imie,
                Nazwisko = nazwisko,
                Email = email,
                UserName = email,
                DataRejestracji = DateTime.Now
            };

            var wynik = await _userManager.CreateAsync(uzytkownik, haslo);

            if (wynik.Succeeded)
            {
                // Automatyczne logowanie po rejestracji
                await _signInManager.SignInAsync(uzytkownik, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Jeśli błąd to pokazujemy go użytkownikowi
            foreach (var blad in wynik.Errors)
            {
                ModelState.AddModelError("", blad.Description);
            }

            return View();
        }

        // Strona logowania
        public IActionResult Logowanie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logowanie(string email, string haslo, bool zapamietaj)
        {
            var wynik = await _signInManager.PasswordSignInAsync(email, haslo, zapamietaj, false);

            if (wynik.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Nieprawidłowy email lub hasło");
            return View();
        }

        // Wylogowanie
        public async Task<IActionResult> Wylogowanie()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Strona braku dostępu
        public IActionResult BrakDostepu()
        {
            return View();
        }

        // Moje zapisy na wydarzenia
        [Authorize]
        public async Task<IActionResult> MojeZapisy()
        {
            // Pobieramy id zalogowanego użytkownika
            var uzytkownikId = _db.Users
                .Where(u => u.UserName == User.Identity!.Name)
                .Select(u => u.Id)
                .FirstOrDefault();

            // Pobieramy wszystkie zapisy użytkownika
            var zapisy = await _db.Zapisy
                .Include(z => z.Wydarzenie)
                .ThenInclude(w => w!.Kategoria)
                .Where(z => z.UzytkownikId == uzytkownikId)
                .OrderBy(z => z.Wydarzenie!.DataRozpoczecia)
                .ToListAsync();

            return View(zapisy);
        }

        // Strona edycji profilu
        [Authorize]
        public async Task<IActionResult> Profil()
        {
            var uzytkownik = await _userManager.FindByNameAsync(User.Identity!.Name!);
            if (uzytkownik == null) return NotFound();
            return View(uzytkownik);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profil(string imie, string nazwisko, string email)
        {
            var uzytkownik = await _userManager.FindByNameAsync(User.Identity!.Name!);
            if (uzytkownik == null) return NotFound();

            // Aktualizujemy dane użytkownika
            uzytkownik.Imie = imie;
            uzytkownik.Nazwisko = nazwisko;
            uzytkownik.Email = email;
            uzytkownik.UserName = email;

            var wynik = await _userManager.UpdateAsync(uzytkownik);

            if (wynik.Succeeded)
            {
                // Odświeżamy sesję po zmianie emaila
                await _signInManager.RefreshSignInAsync(uzytkownik);
                ViewBag.Sukces = "Profil zaktualizowany!";
            }
            else
            {
                foreach (var blad in wynik.Errors)
                {
                    ModelState.AddModelError("", blad.Description);
                }
            }

            return View(uzytkownik);
        }
    }
}