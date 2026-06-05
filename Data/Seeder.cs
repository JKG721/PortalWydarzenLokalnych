// Seeder tworze domyślne dane przy pierwszym uruchomieniu
using Microsoft.AspNetCore.Identity;
using PortalWydarzenLokalnych.Models;


namespace PortalWydarzenLokalnych.Data
{
    public class Seeder
    {
        // Tworze rolę Admin i domyślne konto admina
        public static async Task SeedAdmin(UserManager<Uzytkownik> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Tworzymy rolę Admin jeśli nie istnieje
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Tworzyme konto admina jeśli nie istnieje
            var admin = await userManager.FindByEmailAsync("admin@portal.pl");
            if (admin == null)
            {
                var nowyAdmin = new Uzytkownik
                {
                    UserName = "admin@portal.pl",
                    Email = "admin@portal.pl",
                    Imie = "Admin",
                    Nazwisko = "Portal",
                    DataRejestracji = DateTime.Now
                };

                // Tworzyme konto z hasłem
                var wynik = await userManager.CreateAsync(nowyAdmin, "Admin123!");
                if (wynik.Succeeded)
                {
                    await userManager.AddToRoleAsync(nowyAdmin, "Admin");
                }
            }
        }
    }
}