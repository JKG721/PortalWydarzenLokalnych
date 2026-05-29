//klasa łącząca się z bazą danych
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortalWydarzenLokalnych.Models;

namespace PortalWydarzenLokalnych.Data
{
    public class AppDbContext : IdentityDbContext<Uzytkownik>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Wydarzenie> Wydarzenia { get; set; }
        public DbSet<Kategoria> Kategorie { get; set; }
        public DbSet<Zapis> Zapisy { get; set; }

    }
}