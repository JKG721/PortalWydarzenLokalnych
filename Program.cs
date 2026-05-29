// Konfiguracja i uruchomienie aplikacji
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Data;
using PortalWydarzenLokalnych.Models;

var builder = WebApplication.CreateBuilder(args);

// Podłączenie bazy danych
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfiguracja systemu logowania (Identity)
builder.Services.AddIdentity<Uzytkownik, IdentityRole>(options =>
{
    // Uproszczone wymagania hasła na potrzeby projektu
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Przekierowanie na stronę logowania gdy użytkownik nie jest zalogowany
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Konto/Logowanie";
    options.AccessDeniedPath = "/Konto/BrakDostepu";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Obsługa błędów na produkcji
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Kolejność jest ważna - najpierw autentykacja potem autoryzacja
app.UseAuthentication();
app.UseAuthorization();

// Domyślna trasa
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();