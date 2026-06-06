# Dokumentacja techniczna - Portal wydarzeń lokalnych

## Architektura aplikacji

Aplikacja zbudowana w architekturze MVC (Model-View-Controller) przy użyciu ASP.NET Core.

- **Model** - klasy reprezentujące dane (Wydarzenie, Kategoria, Uzytkownik, Zapis)
- **View** - widoki Razor (.cshtml) odpowiedzialne za prezentację
- **Controller** - kontrolery obsługujące logikę i żądania HTTP

## Technologie

| Technologia | Wersja | Zastosowanie |
|---|---|---|
| ASP.NET Core MVC | .NET 10 | Framework backendowy |
| Entity Framework Core | 9.0 | ORM - dostęp do bazy danych |
| MS SQL Server | 2022 | Baza danych |
| Bootstrap | 5.3 | Framework CSS |
| Leaflet.js | 1.9.4 | Mapy interaktywne |
| ASP.NET Identity | 9.0 | Autentykacja i autoryzacja |

## Struktura bazy danych

### Tabela: Wydarzenia
| Kolumna | Typ | Opis |
|---|---|---|
| Id | int | Klucz główny |
| Nazwa | nvarchar | Nazwa wydarzenia |
| Opis | nvarchar | Opis wydarzenia |
| DataRozpoczecia | datetime | Data i godzina |
| Lokalizacja | nvarchar | Adres tekstowy |
| Szerokosc | float | Szerokość geograficzna |
| Dlugosc | float | Długość geograficzna |
| ZdjecieSciezka | nvarchar | Ścieżka do zdjęcia |
| MaksUczestnikow | int | Limit uczestników |
| DataDodania | datetime | Data dodania |
| KategoriaId | int | Klucz obcy do Kategorie |

### Tabela: Kategorie
| Kolumna | Typ | Opis |
|---|---|---|
| Id | int | Klucz główny |
| Nazwa | nvarchar | Nazwa kategorii |
| Opis | nvarchar | Opis kategorii |

### Tabela: Zapisy
| Kolumna | Typ | Opis |
|---|---|---|
| Id | int | Klucz główny |
| DataZapisu | datetime | Data zapisu |
| UzytkownikId | nvarchar | Klucz obcy do użytkownika |
| WydarzenieId | int | Klucz obcy do wydarzenia |

### Tabela: AspNetUsers (Identity)
| Kolumna | Typ | Opis |
|---|---|---|
| Id | nvarchar | Klucz główny |
| Imie | nvarchar | Imię użytkownika |
| Nazwisko | nvarchar | Nazwisko użytkownika |
| Email | nvarchar | Email użytkownika |
| DataRejestracji | datetime | Data rejestracji |

## Główne funkcjonalności

### Strona publiczna
- Przeglądanie listy wydarzeń z filtrami (kategoria, data, szukaj)
- Sortowanie wydarzeń (data, nazwa)
- Paginacja listy wydarzeń
- Mapa wszystkich wydarzeń (Leaflet.js)
- Szczegóły wydarzenia ze zdjęciem i mapą
- Zapis i wypisanie z wydarzenia
- Rejestracja i logowanie użytkowników
- Widok "Moje zapisy"
- Edycja profilu użytkownika

### Panel administratora
- Zarządzanie wydarzeniami (CRUD + upload zdjęć)
- Zarządzanie kategoriami (CRUD)
- Zarządzanie użytkownikami
- Statystyki portalu

## Bezpieczeństwo
- Autentykacja oparta na ASP.NET Identity
- Role użytkowników (Admin, zwykły użytkownik)
- Panel admina zabezpieczony atrybutem [Authorize(Roles = "Admin")]
- Hasła hashowane przez Identity