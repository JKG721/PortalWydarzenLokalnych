//model wydarzenia lokalnego
namespace PortalWydarzenLokalnych.Models
{
    public class Wydarzenie
    {
        public int Id { get; set; }
        //nazwa wydarzenia np "koncert rockowy"
        public string Nazwa { get; set; } = "";
        //opis wydarzenia np "koncert zespołu XYZ w klubie ABC"
        public string Opis { get; set; } = "";
        //data i godzina rozpoczęcia wydarzenia
        public DateTime DataRozpoczecia { get; set; }
        //miejsce wydarzenia np "klub ABC, ul. Kwiatowa 10"
        public string Lokalizacja { get; set; } = "";
        // współrzędne do mapy leaflet
        public double Szerokosc  { get; set; }
        public double Dlugosc { get; set; }
        //scieżka do zdjęcia wydarzenia
        public string ZdjecieSciezka { get; set; } = "";
        //maksymalna liczba uczestników
        public int MaksUczestnikow { get; set; }
        //data i godzina wpisu do bazy danych
        public DateTime DataDodania { get; set; } = DateTime.Now;
        //klucz obcy do kategorii wydarzenia
        public int KategoriaId { get; set; }
        public Kategoria? Kategoria { get; set; }
        //lista zapisów na wydarzenie
        public List<Zapis> Zapisy { get; set; } = new List<Zapis>();
    }
}