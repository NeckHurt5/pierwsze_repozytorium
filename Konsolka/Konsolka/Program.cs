// System rezerwacji biletów - C# (Program.cs)
// Autor: Bartłomiej Kozieł
// Data wykonania: 2025-11-03
//
// Instrukcja: skopiuj ten plik jako Program.cs do projektu .NET Console (dotnet new console)
// kompilacja i uruchomienie:
//    dotnet new console -o SystemRezerwacji
//    skopiuj Program.cs do katalogu SystemRezerwacji (nadpisz wygenerowany)
//    cd SystemRezerwacji
//    dotnet run

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SystemRezerwacji
{
    class Wydarzenie
    {
        public string Nazwa { get; private set; }
        public DateTime Data { get; private set; }
        public int LiczbaMiejsc { get; private set; }
        public int Zarezerwowane { get; private set; }

        public Wydarzenie(string nazwa, DateTime data, int liczbaMiejsc, int zarezerwowane = 0)
        {
            Nazwa = nazwa;
            Data = data;
            LiczbaMiejsc = Math.Max(0, liczbaMiejsc);
            Zarezerwowane = Math.Clamp(zarezerwowane, 0, LiczbaMiejsc);
        }

        public bool Rezerwuj(int ile)
        {
            if (ile <= 0) return false;
            if (ile <= WolneMiejsca())
            {
                Zarezerwowane += ile;
                return true;
            }
            return false;
        }

        public bool Anuluj(int ile)
        {
            if (ile <= 0) return false;
            if (ile <= Zarezerwowane)
            {
                Zarezerwowane -= ile;
                return true;
            }
            return false;
        }

        public int WolneMiejsca()
        {
            return LiczbaMiejsc - Zarezerwowane;
        }

        public override string ToString()
        {
            return $"{Nazwa} | {Data:yyyy-MM-dd} | Miejsca: {LiczbaMiejsc} | Zarezerwowane: {Zarezerwowane} | Wolne: {WolneMiejsca()}";
        }

        // Format zapisu: nazwa|yyyy-MM-dd|liczbaMiejsc|zarezerwowane
        public string ToFileLine()
        {
            var cleanName = Nazwa.Replace("|", "/");
            return string.Join("|", cleanName, Data.ToString("yyyy-MM-dd"), LiczbaMiejsc, Zarezerwowane);
        }

        public static bool TryParseFromFile(string line, out Wydarzenie wydarzenie)
        {
            wydarzenie = null;
            if (string.IsNullOrWhiteSpace(line)) return false;
            var parts = line.Split('|');
            if (parts.Length != 4) return false;
            var nazwa = parts[0];
            if (!DateTime.TryParseExact(parts[1], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
                return false;
            if (!int.TryParse(parts[2], out var liczbaMiejsc)) return false;
            if (!int.TryParse(parts[3], out var zarezerwowane)) return false;

            wydarzenie = new Wydarzenie(nazwa, data, liczbaMiejsc, zarezerwowane);
            return true;
        }
    }

    class SystemRezerwacji
    {
        private readonly List<Wydarzenie> wydarzenia = new List<Wydarzenie>();

        public void DodajWydarzenie(Wydarzenie w)
        {
            if (w == null) return;
            wydarzenia.Add(w);
        }

        public IEnumerable<Wydarzenie> ListaWydarzen()
        {
            return wydarzenia.OrderBy(w => w.Data).ThenBy(w => w.Nazwa);
        }

        public Wydarzenie WyszukajPoNazwie(string nazwa)
        {
            if (string.IsNullOrWhiteSpace(nazwa)) return null;
            return wydarzenia.FirstOrDefault(w => string.Equals(w.Nazwa, nazwa, StringComparison.OrdinalIgnoreCase));
        }

        public bool ZapiszDoPliku(string sciezka)
        {
            try
            {
                using (var sw = new StreamWriter(sciezka, false))
                {
                    foreach (var w in wydarzenia)
                    {
                        sw.WriteLine(w.ToFileLine());
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool WczytajZPliku(string sciezka)
        {
            if (!File.Exists(sciezka)) return false;
            try
            {  var lines = File.ReadAllLines(sciezka);
                wydarzenia.Clear();
                foreach (var line in lines)
                {
                    if (Wydarzenie.TryParseFromFile(line, out var w))
                        wydarzenia.Add(w);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var system = new SystemRezerwacji();
            const string domyslnyPlik = "events.txt";

            if (File.Exists(domyslnyPlik))
            {
                if (system.WczytajZPliku(domyslnyPlik))
                    Console.WriteLine($"Wczytano wydarzenia z pliku '{domyslnyPlik}'.");
                else
                    Console.WriteLine("Nie udało się wczytać wydarzeń z pliku.");
            }

            while (true)
            {
                PokazMenu();
                Console.Write("Wybierz opcję: ");
                var key = Console.ReadLine();
                Console.WriteLine();
                switch (key)
                {
                    case "1": DodajWydarzenie(system); break;
                    case "2": Rezerwuj(system); break;
                    case "3": Anuluj(system); break;
                    case "4": WyswietlListe(system); break;
                    case "5": Zapisz(system, domyslnyPlik); break;
                    case "6": Wczytaj(system, domyslnyPlik); break;
                    case "0":
                        Console.WriteLine("Zapisuję i kończę...");
                        system.ZapiszDoPliku(domyslnyPlik);
                        return;
                    default:
                        Console.WriteLine("Niepoprawna opcja. Spróbuj ponownie.");
                        break;
                }
                Console.WriteLine();
            }
        }

        static void PokazMenu()
        {
            Console.WriteLine("=== System rezerwacji biletów ===");
            Console.WriteLine("1. Dodaj wydarzenie");
            Console.WriteLine("2. Zarezerwuj miejsca");
            Console.WriteLine("3. Anuluj rezerwację");
            Console.WriteLine("4. Wyświetl listę wydarzeń");
            Console.WriteLine("5. Zapisz do pliku");
            Console.WriteLine("6. Wczytaj z pliku");
            Console.WriteLine("0. Zakończ (zapisz)");
        }

        static void DodajWydarzenie(SystemRezerwacji system)
        {
            Console.Write("Nazwa wydarzenia: ");
            var nazwa = Console.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(nazwa))
            {
                Console.WriteLine("Nazwa nie może być pusta.");
                return;
            }

            Console.Write("Data (YYYY-MM-DD): ");
            var dataStr = Console.ReadLine();
            if (!DateTime.TryParseExact(dataStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
            {
                Console.WriteLine("Błędny format daty. Użyj YYYY-MM-DD.");
                return;
            }

            Console.Write("Liczba miejsc: ");
            if (!int.TryParse(Console.ReadLine(), out var liczba) || liczba <= 0)
            {
                Console.WriteLine("Niepoprawna liczba miejsc. Musi być liczba naturalna > 0.");
                return;
            }

            var w = new Wydarzenie(nazwa, data, liczba);
            system.DodajWydarzenie(w);
            Console.WriteLine("Wydarzenie dodane pomyślnie.");
        }

        static void Rezerwuj(SystemRezerwacji system)
        {
            Console.Write("Podaj nazwę wydarzenia: ");
            var nazwa = Console.ReadLine();
            var w = system.WyszukajPoNazwie(nazwa);
            if (w == null)
            {
                Console.WriteLine("Nie znaleziono wydarzenia o takiej nazwie.");
                return;
            }
            Console.WriteLine(w);
            Console.Write("Ile miejsc chcesz zarezerwować? ");
            if (!int.TryParse(Console.ReadLine(), out var ile) || ile <= 0)
            {
                Console.WriteLine("Podaj poprawną liczbę miejsc (>0).");
                return;
            }
            if (w.Rezerwuj(ile))
                Console.WriteLine("Rezerwacja przebiegła pomyślnie.");
            else
                Console.WriteLine("Brak wystarczającej liczby miejsc.");
        }

        static void Anuluj(SystemRezerwacji system)
        {
            Console.Write("Podaj nazwę wydarzenia: ");
            var nazwa = Console.ReadLine();
            var w = system.WyszukajPoNazwie(nazwa);
            if (w == null)
            {
                Console.WriteLine("Nie znaleziono wydarzenia o takiej nazwie.");
                return;
            }
            Console.WriteLine(w);
            Console.Write("Ile miejsc chcesz anulować? ");
            if (!int.TryParse(Console.ReadLine(), out var ile) || ile <= 0)
            {
                Console.WriteLine("Podaj poprawną liczbę miejsc (>0).");
                return;
            }
            if (w.Anuluj(ile))
                Console.WriteLine("Anulowano rezerwację.");
            else
                Console.WriteLine("Nie można anulować więcej miejsc niż zarezerwowano.");
        }

        static void WyswietlListe(SystemRezerwacji system)
        {
            var list = system.ListaWydarzen().ToList();
            if (!list.Any())
            {
                Console.WriteLine("Brak wydarzeń do wyświetlenia.");
                return;
            }
            Console.WriteLine("Lista wydarzeń:");
            foreach (var w in list)
                Console.WriteLine(w);
        }

        static void Zapisz(SystemRezerwacji system, string sciezka)
        {
            if (system.ZapiszDoPliku(sciezka))
                Console.WriteLine($"Zapisano do pliku '{sciezka}'.");
            else
                Console.WriteLine("Nie udało się zapisać do pliku.");
        }

        static void Wczytaj(SystemRezerwacji system, string sciezka)
        {
            if (!File.Exists(sciezka))
            {
                Console.WriteLine($"Plik '{sciezka}' nie istnieje.");
                return;
            }
            if (system.WczytajZPliku(sciezka))
                Console.WriteLine("Pomyślnie wczytano wydarzenia z pliku.");
            else
                Console.WriteLine("Nie udało się wczytać wydarzeń z pliku.");
        }
    }
}
