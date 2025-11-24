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

## 📘 Opis klas w projekcie System Rezerwacji Biletów
### 🟥 Klasa Wydarzenie

Reprezentuje **pojedyncze wydarzenie**, jego parametry oraz operacje na rezerwacjach.

**Pola / Właściwości**

1. ` string Nazwa – nazwa wydarzenia (np. „Koncert Rockowy”). `

2. ` DateTime Data ` – data wydarzenia.

3. ` int LiczbaMiejsc `– całkowita liczba dostępnych miejsc.

4. ` int Zarezerwowane ` – liczba miejsc aktualnie zarezerwowanych.

5. Właściwości są tylko do odczytu — dane mogą być ustawione wyłącznie w konstruktorze.

### Konstruktor
` public Wydarzenie(string nazwa, DateTime data, int liczbaMiejsc, int zarezerwowane = 0)  ` 


**Odpowiada za:**

- ustawianie wartości pól,

- upewnienie się, że liczba miejsc nie jest ujemna,

- ograniczenie rezerwacji tak, aby nie przekraczała liczby miejsc.

**Metody**

` bool Rezerwuj(int ile) `
Próbuje zarezerwować ile miejsc.
Zwraca true, jeśli operacja się udała.

` bool Anuluj(int ile) `
Anuluje określoną liczbę miejsc — tylko jeśli użytkownik nie próbuje anulować więcej niż jest zarezerwowane.

` int WolneMiejsca() `
Oblicza liczbę dostępnych jeszcze miejsc.

` override string ToString() `
Zwraca czytelny opis wydarzenia w konsoli.

` string ToFileLine() `
Zwraca wydarzenie w formacie tekstowym nadającym się do zapisania w pliku.

` static bool TryParseFromFile(string line, out Wydarzenie wydarzenie) `
Próbuje odczytać wydarzenie z linii tekstu zapisanej w formacie plikowym.

### 🟦 Klasa SystemRezerwacji

Zawiera listę wszystkich wydarzeń oraz operacje na tej liście.

**Pola**

` List<Wydarzenie> wydarzenia – kolekcja zarządzanych wydarzeń. `

**Metody**

` void DodajWydarzenie(Wydarzenie w) `
Dodaje nowe wydarzenie do systemu.

` IEnumerable<Wydarzenie> ListaWydarzen() `
Zwraca wydarzenia posortowane po dacie i nazwie.

` Wydarzenie WyszukajPoNazwie(string nazwa) `
Wyszukuje wydarzenie na podstawie nazwy (ignorując wielkość liter).

` bool ZapiszDoPliku(string sciezka) `
Zapisuje wszystkie wydarzenia do pliku tekstowego.

` bool WczytajZPliku(string sciezka) `
Wczytuje wydarzenia z pliku w zadanym formacie.

***To klasa, która spaja całą logikę i zarządzanie danymi.***

### 🟩 Klasa Program

Odpowiada za **interakcję użytkownika z systemem**: obsługę menu, wczytywanie danych z klawiatury, uruchamianie odpowiednich funkcji.

#### Główne elementy
` Main() `

- tworzy obiekt SystemRezerwacji,

- automatycznie wczytuje dane z pliku events.txt,

- uruchamia pętlę menu.

#### Funkcje pomocnicze:

` PokazMenu() ` – drukuje menu na ekran.

` DodajWydarzenie(SystemRezerwacji) ` – obsługuje dodawanie nowego wydarzenia z walidacją.

` Rezerwuj(SystemRezerwacji) ` – rezerwacja miejsc na wskazane wydarzenie.

` Anuluj(SystemRezerwacji) ` – anulowanie miejsc.

` WyswietlListe(SystemRezerwacji) ` – wypisuje posortowaną listę wydarzeń.

` Zapisz/System(SystemRezerwacji) ` – zapis do pliku.

` Wczytaj/System(SystemRezerwacji) ` – odczyt z pliku.

***To klasa czysto interfejsu użytkownika — nie trzyma danych biznesowych, a jedynie steruje pracą programu.***

