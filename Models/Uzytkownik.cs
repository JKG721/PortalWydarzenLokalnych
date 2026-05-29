//model użytkownika - dziedziczy po IdentityUser (logowanie, haslo)
using Microsoft.AspNetCore.Identity;
namespace PortalWydarzenLokalnych.Models
{
    public class Uzytkownik : IdentityUser
    {
        //imie użytkownika np "Jan"
        public string Imie { get; set; } = "";
        //nazwisko użytkownika np "Kowalski"
        public string Nazwisko { get; set; } = "";
        //data kiedy użytkownik zarejestrował się w serwisie
        public DateTime DataRejestracji { get; set; } = DateTime.Now;
        //lista wydarzeń, na które użytkownik się zapisał
        public List<Zapis> Zapisy { get; set; } = new List<Zapis>();
    }
}