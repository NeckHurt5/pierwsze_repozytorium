# System Rezerwacji Biletów – C#

Prosty konsolowy system do zarządzania wydarzeniami i rezerwacjami miejsc, napisany w C#.
## Projekt umożliwia:

- dodawanie wydarzeń,

- rezerwowanie miejsc,

- anulowanie rezerwacji,

- wyświetlanie listy wydarzeń,

- zapis i odczyt danych z pliku events.txt.

## 🔧 Wymagania

.NET SDK 6.0 lub nowszy

System operacyjny: Windows / Linux / macOS

## 🚀 Instalacja i uruchomienie

Utwórz projekt konsolowy .NET:

` dotnet new console -o SystemRezerwacji `


**Skopiuj plik Program.cs z repozytorium do katalogu SystemRezerwacji, nadpisując istniejący plik.**

Przejdź do katalogu projektu:

` cd SystemRezerwacji `


Uruchom aplikację:

` dotnet run `

## 📁 Plik danych

Program automatycznie korzysta z pliku:

***events.txt***


Format przechowywania danych o wydarzeniach:

` nazwa|yyyy-MM-dd|liczbaMiejsc|zarezerwowane `


Przykład:

` Koncert Rockowy|2025-12-01|150|45 `

## 📜 Menu programu

Po uruchomieniu aplikacji zobaczysz:
```
=== System rezerwacji biletów ===
1. Dodaj wydarzenie
2. Zarezerwuj miejsca
3. Anuluj rezerwację
4. Wyświetl listę wydarzeń
5. Zapisz do pliku
6. Wczytaj z pliku
0. Zakończ (zapisz)
```

**Każda opcja wykonuje odpowiednie operacje na danych.**

## 🧩 Struktura projektu

**Wydarzenie**
Przechowuje informacje o jednym wydarzeniu (nazwa, data, miejsca, rezerwacje).

**SystemRezerwacji**
Zarządza kolekcją wydarzeń, umożliwia wyszukiwanie, zapis i odczyt z pliku.

**Program**
Zawiera główną logikę interfejsu konsolowego i obsługę menu.

## ✔️ Funkcjonalności

✔ Dodawanie wydarzeń z walidacją danych

✔ Sprawdzanie dostępnych miejsc

✔ Blokowanie rezerwacji powyżej limitu

✔ Anulowanie rezerwacji z kontrolą limitu

✔ Automatyczne ładowanie danych przy starcie

✔ Automatyczny zapis przy wyjściu

✔ Bezpieczny zapis/odczyt do pliku

## 🧪 Przykładowe użycie

Dodaj wydarzenie:

Nazwa: Koncert A

Data: 2025-05-10

Miejsca: 100

Zarezerwuj 20 miejsc → wynik: sukces

Anuluj 5 miejsc → wynik: sukces

Zapisz do events.txt
