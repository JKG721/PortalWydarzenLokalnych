//kontroler panelu admina do zarzązania użytkownikami
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Models;

namespace PortalWydarzenLokalnych.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminUzytkownicyController : Controller
    {
        private readonly UserManager<Uzytkownik> _userManager;
        public AdminUzytkownicyController(UserManager<Uzytkownik> userManager)
        {
            _userManager = userManager;
        }
        //lista użytkowników
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
        //formularz edycji użytkownika
        public async Task<IActionResult> Edytuj(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edytuj(Uzytkownik user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null) return NotFound();

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                ModelState.AddModelError("", "Nie można zaktualizować użytkownika");

            return View(existingUser);
        }
        //usuwanie użytkownika
        public async Task<IActionResult> Usun(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                ModelState.AddModelError("", "Nie można usunąć użytkownika");

            return View("Index", await _userManager.Users.ToListAsync());
        }
    }
}