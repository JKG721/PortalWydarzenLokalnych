// Kontroler obsługujący rejestrację i logowanie użytkowników
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PortalWydarzenLokalnych.Models;

namespace PortalWydarzenLokalnych.Controllers
{
    public class KontoController : Controller
    {
        private readonly UserManager<Uzytkownik> _userManager;
        private readonly SignInManager<Uzytkownik> _signInManager;

        public KontoController(UserManager<Uzytkownik> userManager, SignInManager<Uzytkownik> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //strona rejestracji
        public IActionResult Rejestracja()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Rejestracja(string imie, string nazwisko, string email, string haslo)
        {
            //tworzymy nowego użytkownika
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
                // automatyczne logowanie po rejestracji
                await _signInManager.SignInAsync(uzytkownik, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // jeśli błąd to pokazujemy go użytkownikowi
            foreach (var blad in wynik.Errors)
            {
                ModelState.AddModelError("", blad.Description);
            }

            return View();
        }

        //strona logowania
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

        //strona wylogowania
        public async Task<IActionResult> Wylogowanie()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //strona braku dostępu
        public IActionResult BrakDostepu()
        {
            return View();
        }
    }
}