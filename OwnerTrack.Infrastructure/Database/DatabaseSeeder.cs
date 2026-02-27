using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using System;

namespace OwnerTrack.Infrastructure.Database
{
    public class DatabaseSeeder
    {
        private readonly OwnerTrackDbContext _db;

        public DatabaseSeeder(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public void SeedTestData()
        {
            if (_db.Klijenti.Any())
            {
                Console.WriteLine("Baza već ima podatke. Skip seed.");
                return;
            }

            Console.WriteLine("Dodajem test podatke u bazu...");

            var sifre = new[]
            {
                (AppKonstante.DefaultSifraDjelatnosti, "Računovodstvene, knjigovodstvene i revizorske djelatnosti"),
                ("47.11", "Maloprodaja u nespecijaliziranim prodavnicama pretežno hrane"),
                ("41.20", "Gradnja stambenih i nestambenih zgrada"),
                ("56.10", "Restorani i ostali objekti za pripremu hrane"),
                ("62.01", "Računalno programiranje")
            };

            foreach (var (sifra, naziv) in sifre)
                if (!_db.Djelatnosti.Any(d => d.Sifra == sifra))
                    _db.Djelatnosti.Add(new Djelatnost { Sifra = sifra, Naziv = naziv });

            _db.SaveChanges();

            var klijent1 = new Klijent
            {
                Naziv = "TEST RAČUNOVODSTVENA KUĆA DOO",
                IdBroj = "4512010570004",
                Adresa = "Travnik, Šumeće bb",
                SifraDjelatnosti = AppKonstante.DefaultSifraDjelatnosti,
                VrstaKlijenta = VrstaKlijenta.PravnoLice,
                DatumOsnivanja = new DateTime(2015, 3, 15),
                DatumUspostave = new DateTime(2015, 4, 1),
                Velicina = VelicinaFirme.MIKRO.ToString(),
                PepRizik = DaNeKonstante.Ne,
                UboRizik = DaNeKonstante.Ne,
                GotovinaRizik = DaNeKonstante.Ne,
                GeografskiRizik = DaNeKonstante.Ne,
                UkupnaProcjena = "NIZAK",
                DatumProcjene = new DateTime(2024, 1, 15),
                OvjeraCr = DaNeKonstante.Da,
                Status = StatusEntiteta.AKTIVAN
            };

            var klijent2 = new Klijent
            {
                Naziv = "TRGOVINA BOSNA DOO",
                IdBroj = "4512020570005",
                Adresa = "Sarajevo, Titova 25",
                SifraDjelatnosti = "47.11",
                VrstaKlijenta = VrstaKlijenta.PravnoLice,
                DatumOsnivanja = new DateTime(2018, 6, 20),
                DatumUspostave = new DateTime(2018, 7, 1),
                Velicina = VelicinaFirme.MALO.ToString(),
                PepRizik = DaNeKonstante.Ne,
                UboRizik = DaNeKonstante.Ne,
                GotovinaRizik = DaNeKonstante.Da,
                GeografskiRizik = DaNeKonstante.Ne,
                UkupnaProcjena = "SREDNJI",
                DatumProcjene = new DateTime(2024, 2, 10),
                OvjeraCr = DaNeKonstante.Da,
                Status = StatusEntiteta.AKTIVAN
            };

            var klijent3 = new Klijent
            {
                Naziv = "GRAĐEVINSKA FIRMA PROFESSIONAL DOO",
                IdBroj = "4512030570006",
                Adresa = "Zenica, Industrijska zona 10",
                SifraDjelatnosti = "41.20",
                VrstaKlijenta = VrstaKlijenta.PravnoLice,
                DatumOsnivanja = new DateTime(2010, 1, 10),
                DatumUspostave = new DateTime(2010, 2, 1),
                Velicina = VelicinaFirme.SREDNJE.ToString(),
                PepRizik = DaNeKonstante.Da,
                UboRizik = DaNeKonstante.Da,
                GotovinaRizik = DaNeKonstante.Da,
                GeografskiRizik = DaNeKonstante.Ne,
                UkupnaProcjena = "VISOK",
                DatumProcjene = new DateTime(2024, 1, 20),
                OvjeraCr = DaNeKonstante.Da,
                Status = StatusEntiteta.AKTIVAN
            };

            var klijent4 = new Klijent
            {
                Naziv = "RESTORAN STARI GRAD",
                IdBroj = "4512040570007",
                Adresa = "Mostar, Kujundžiluk 5",
                SifraDjelatnosti = "56.10",
                VrstaKlijenta = VrstaKlijenta.PravnoLice,
                DatumOsnivanja = new DateTime(2020, 5, 1),
                DatumUspostave = new DateTime(2020, 6, 1),
                Velicina = VelicinaFirme.MIKRO.ToString(),
                PepRizik = DaNeKonstante.Ne,
                UboRizik = DaNeKonstante.Ne,
                GotovinaRizik = DaNeKonstante.Da,
                GeografskiRizik = DaNeKonstante.Ne,
                UkupnaProcjena = "SREDNJI",
                DatumProcjene = new DateTime(2024, 3, 5),
                OvjeraCr = DaNeKonstante.Da,
                Status = StatusEntiteta.AKTIVAN
            };

            var klijent5 = new Klijent
            {
                Naziv = "IT SOLUTIONS DOO",
                IdBroj = "4512050570008",
                Adresa = "Banja Luka, Tech Park 12",
                SifraDjelatnosti = "62.01",
                VrstaKlijenta = VrstaKlijenta.PravnoLice,
                DatumOsnivanja = new DateTime(2019, 9, 15),
                DatumUspostave = new DateTime(2019, 10, 1),
                Velicina = VelicinaFirme.MALO.ToString(),
                PepRizik = DaNeKonstante.Ne,
                UboRizik = DaNeKonstante.Ne,
                GotovinaRizik = DaNeKonstante.Ne,
                GeografskiRizik = DaNeKonstante.Ne,
                UkupnaProcjena = "NIZAK",
                DatumProcjene = new DateTime(2024, 2, 20),
                OvjeraCr = DaNeKonstante.Da,
                Status = StatusEntiteta.AKTIVAN
            };

            _db.Klijenti.AddRange(klijent1, klijent2, klijent3, klijent4, klijent5);
            _db.SaveChanges();

            _db.Vlasnici.AddRange(
                new Vlasnik { KlijentId = klijent1.Id, ImePrezime = "Marko Marković", ProcenatVlasnistva = 60, DatumUtvrdjivanja = new DateTime(2015, 3, 15), IzvorPodatka = "SUDSKI REGISTAR", Status = StatusEntiteta.AKTIVAN },
                new Vlasnik { KlijentId = klijent1.Id, ImePrezime = "Ana Anić", ProcenatVlasnistva = 40, DatumUtvrdjivanja = new DateTime(2015, 3, 15), IzvorPodatka = "SUDSKI REGISTAR", Status = StatusEntiteta.AKTIVAN },
                new Vlasnik { KlijentId = klijent2.Id, ImePrezime = "Petar Petrović", ProcenatVlasnistva = 100, DatumUtvrdjivanja = new DateTime(2018, 6, 20), IzvorPodatka = "SUDSKI REGISTAR", Status = StatusEntiteta.AKTIVAN },
                new Vlasnik { KlijentId = klijent3.Id, ImePrezime = "Ivan Ivanović", ProcenatVlasnistva = 51, DatumUtvrdjivanja = new DateTime(2010, 1, 10), IzvorPodatka = "SUDSKI REGISTAR", Status = StatusEntiteta.AKTIVAN },
                new Vlasnik { KlijentId = klijent3.Id, ImePrezime = "Jovana Jovanović", ProcenatVlasnistva = 49, DatumUtvrdjivanja = new DateTime(2010, 1, 10), IzvorPodatka = "SUDSKI REGISTAR", Status = StatusEntiteta.AKTIVAN },
                new Vlasnik { KlijentId = klijent4.Id, ImePrezime = "Ahmed Ahmetović", ProcenatVlasnistva = 100, DatumUtvrdjivanja = new DateTime(2020, 5, 1), IzvorPodatka = "SUDSKI REGISTAR", Status = StatusEntiteta.AKTIVAN },
                new Vlasnik { KlijentId = klijent5.Id, ImePrezime = "Nikola Nikolić", ProcenatVlasnistva = 70, DatumUtvrdjivanja = new DateTime(2019, 9, 15), IzvorPodatka = "SUDSKI REGISTAR", Status = StatusEntiteta.AKTIVAN },
                new Vlasnik { KlijentId = klijent5.Id, ImePrezime = "Maja Majić", ProcenatVlasnistva = 30, DatumUtvrdjivanja = new DateTime(2019, 9, 15), IzvorPodatka = "SUDSKI REGISTAR", Status = StatusEntiteta.AKTIVAN }
            );
            _db.SaveChanges();

            _db.Direktori.AddRange(
                new Direktor { KlijentId = klijent1.Id, ImePrezime = "Marko Marković", DatumValjanosti = new DateTime(2025, 12, 31), TipValjanosti = TipValjanostiKonstante.Vremenski, Status = StatusEntiteta.AKTIVAN },
                new Direktor { KlijentId = klijent2.Id, ImePrezime = "Petar Petrović", DatumValjanosti = null, TipValjanosti = TipValjanostiKonstante.Trajno, Status = StatusEntiteta.AKTIVAN },
                new Direktor { KlijentId = klijent3.Id, ImePrezime = "Ivan Ivanović", DatumValjanosti = null, TipValjanosti = TipValjanostiKonstante.Trajno, Status = StatusEntiteta.AKTIVAN },
                new Direktor { KlijentId = klijent4.Id, ImePrezime = "Ahmed Ahmetović", DatumValjanosti = new DateTime(2026, 6, 30), TipValjanosti = TipValjanostiKonstante.Vremenski, Status = StatusEntiteta.AKTIVAN },
                new Direktor { KlijentId = klijent5.Id, ImePrezime = "Nikola Nikolić", DatumValjanosti = null, TipValjanosti = TipValjanostiKonstante.Trajno, Status = StatusEntiteta.AKTIVAN }
            );
            _db.SaveChanges();

            _db.Ugovori.AddRange(
                new Ugovor { KlijentId = klijent1.Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2015, 4, 1) },
                new Ugovor { KlijentId = klijent2.Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2018, 7, 1) },
                new Ugovor { KlijentId = klijent3.Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2010, 2, 1) },
                new Ugovor { KlijentId = klijent4.Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2020, 6, 1) },
                new Ugovor { KlijentId = klijent5.Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2019, 10, 1) }
            );
            _db.SaveChanges();

            Console.WriteLine("Test podaci uspješno dodati!");
            Console.WriteLine($"Dodato klijenata: {_db.Klijenti.Count()}");
            Console.WriteLine($"Dodato vlasnika: {_db.Vlasnici.Count()}");
            Console.WriteLine($"Dodato direktora: {_db.Direktori.Count()}");
            Console.WriteLine($"Dodato ugovora: {_db.Ugovori.Count()}");
        }
    }
}