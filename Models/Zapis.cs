//Model zapisu użytkownika na wydarzenie
namespace PortalWydarzenLokalnych.Models
{
    public class Zapis
    {
        // Klucz główny
        public int Id { get; set; }
       //data kiedy użytkownik zapisał się na wydarzenie
        public DateTime DataZapisu { get; set; } = DateTime.Now;
        //klucz obcy do użytkownika
        public string UzytkownikId  { get; set; } = "";
        public Uzytkownik? Uzytkownik { get; set; }
        //klucz obcy do wydarzenia
        public int WydarzenieId { get; set; }
        public Wydarzenie? Wydarzenie { get; set; }
    }
}