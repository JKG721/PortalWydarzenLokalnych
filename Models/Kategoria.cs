// model kategori wydarzeń (muzyka, sport, kultura)
namespace PortalWydarzenLokalnych.Models
{
    public class Kategoria
    {
        public int Id { get; set; }
        //nazwa kategorii np "muzyka"
        public string Nazwa { get; set; } = "";
        //opis kategorii np "wydarzenia muzyczne"
        public string Opis { get; set; } = "";
        //lista wydarzeń przypisanych do tej kategorii
        public List<Wydarzenie> Wydarzenia { get; set; } = new List<Wydarzenie>();
    }
}