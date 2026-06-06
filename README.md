# Portal wydarzeń lokalnych

Aplikacja webowa umożliwiająca przeglądanie i zapisywanie się na lokalne wydarzenia w okolicy. Projekt zaliczeniowy na przedmiot Techniki Internetowe.

## Opis projektu

Portal pozwala użytkownikom na:
- Przeglądanie wydarzeń lokalnych na liście i mapie
- Filtrowanie i wyszukiwanie wydarzeń
- Rejestrację konta i zapisywanie się na wydarzenia
- Zarządzanie swoimi zapisami i profilem

Administratorzy mogą zarządzać wydarzeniami, kategoriami i użytkownikami przez dedykowany panel CMS.

## Technologie

- **Backend:** ASP.NET Core MVC (.NET 10)
- **Baza danych:** MS SQL Server 2022 + Entity Framework Core 9
- **Frontend:** Bootstrap 5.3, Leaflet.js 1.9.4, Vanilla JS
- **Autentykacja:** ASP.NET Identity
- **Kontrola wersji:** Git + GitHub

## Wymagania

- .NET 10 SDK
- SQL Server 2022 (lub LocalDB)
- Git

## Instrukcja uruchomienia lokalnego

### Krok 1 — Sklonuj repozytorium
```bash
git clone https://github.com/JKG721/PortalWydarzenLokalnych.git
cd PortalWydarzenLokalnych
```

### Krok 2 — Przywróć zależności
```bash
dotnet restore
```

### Krok 3 — Skonfiguruj bazę danych
Otwórz plik `appsettings.json` i sprawdź connection string:
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PortalWydarzenLokalnych;Trusted_Connection=True;"
}
```
Jeśli używasz innego serwera SQL, zmień `Server` na odpowiednią wartość.

### Krok 4 — Utwórz bazę danych
```bash
dotnet ef database update
```

### Krok 5 — Uruchom aplikację
```bash
dotnet run
```

### Krok 6 — Otwórz w przeglądarce
```
http://localhost:5284
```

## Domyślne konto administratora

Po pierwszym uruchomieniu automatycznie tworzone jest konto admina:

| Pole | Wartość |
|---|---|
| Email | admin@portal.pl |
| Hasło | Admin123! |

## Struktura projektu

```
PortalWydarzenLokalnych/
├── Controllers/
│   ├── Admin/                    - kontrolery panelu admina
│   ├── HomeController.cs         - strona główna
│   ├── WydarzeniaController.cs   - lista i szczegóły wydarzeń
│   ├── MapaController.cs         - mapa wydarzeń
│   └── KontoController.cs        - logowanie i rejestracja
├── Models/                       - modele danych
├── Views/                        - widoki Razor
├── Data/                         - DbContext i Seeder
└── wwwroot/                      - pliki statyczne
```

## Autor

Jakub Gorczyca
