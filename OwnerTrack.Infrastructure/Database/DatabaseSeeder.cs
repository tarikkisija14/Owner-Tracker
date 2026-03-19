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

            SeedDjelatnosti();
            _db.SaveChanges();

            var klijenti = SeedKlijenti();
            _db.SaveChanges();

            SeedVlasnici(klijenti);
            _db.SaveChanges();

            SeedDirektori(klijenti);
            _db.SaveChanges();

            SeedUgovori(klijenti);
            _db.SaveChanges();

            Console.WriteLine("Test podaci uspješno dodati!");
            Console.WriteLine($"Dodato klijenata: {_db.Klijenti.Count()}");
            Console.WriteLine($"Dodato vlasnika:  {_db.Vlasnici.Count()}");
            Console.WriteLine($"Dodato direktora: {_db.Direktori.Count()}");
            Console.WriteLine($"Dodato ugovora:   {_db.Ugovori.Count()}");
        }

        

        private void SeedDjelatnosti()
        {
            var sifre = new[]
            {
                (AppKonstante.DefaultSifraDjelatnosti, "Računovodstvene, knjigovodstvene i revizorske djelatnosti"),
                ("47.11", "Maloprodaja u nespecijaliziranim prodavnicama pretežno hrane"),
                ("41.20", "Gradnja stambenih i nestambenih zgrada"),
                ("56.10", "Restorani i ostali objekti za pripremu hrane"),
                ("62.01", "Računalno programiranje"),
            };

            foreach (var (sifra, naziv) in sifre)
                if (!_db.Djelatnosti.Any(d => d.Sifra == sifra))
                    _db.Djelatnosti.Add(new Djelatnost { Sifra = sifra, Naziv = naziv });
        }

        private Klijent[] SeedKlijenti()
        {
            var k1 = Klijent("TEST RAČUNOVODSTVENA KUĆA DOO", "4512010570004", "Travnik, Šumeće bb",
                AppKonstante.DefaultSifraDjelatnosti, new DateTime(2015, 3, 15), new DateTime(2015, 4, 1),
                VelicinaFirme.MIKRO, DaNeKonstante.Ne, DaNeKonstante.Ne, DaNeKonstante.Ne, DaNeKonstante.Ne,
                "NIZAK", new DateTime(2024, 1, 15));

            var k2 = Klijent("TRGOVINA BOSNA DOO", "4512020570005", "Sarajevo, Titova 25",
                "47.11", new DateTime(2018, 6, 20), new DateTime(2018, 7, 1),
                VelicinaFirme.MALO, DaNeKonstante.Ne, DaNeKonstante.Ne, DaNeKonstante.Da, DaNeKonstante.Ne,
                "SREDNJI", new DateTime(2024, 2, 10));

            var k3 = Klijent("GRAĐEVINSKA FIRMA PROFESSIONAL DOO", "4512030570006", "Zenica, Industrijska zona 10",
                "41.20", new DateTime(2010, 1, 10), new DateTime(2010, 2, 1),
                VelicinaFirme.SREDNJE, DaNeKonstante.Da, DaNeKonstante.Da, DaNeKonstante.Da, DaNeKonstante.Ne,
                "VISOK", new DateTime(2024, 1, 20));

            var k4 = Klijent("RESTORAN STARI GRAD", "4512040570007", "Mostar, Kujundžiluk 5",
                "56.10", new DateTime(2020, 5, 1), new DateTime(2020, 6, 1),
                VelicinaFirme.MIKRO, DaNeKonstante.Ne, DaNeKonstante.Ne, DaNeKonstante.Da, DaNeKonstante.Ne,
                "SREDNJI", new DateTime(2024, 3, 5));

            var k5 = Klijent("IT SOLUTIONS DOO", "4512050570008", "Banja Luka, Tech Park 12",
                "62.01", new DateTime(2019, 9, 15), new DateTime(2019, 10, 1),
                VelicinaFirme.MALO, DaNeKonstante.Ne, DaNeKonstante.Ne, DaNeKonstante.Ne, DaNeKonstante.Ne,
                "NIZAK", new DateTime(2024, 2, 20));

            _db.Klijenti.AddRange(k1, k2, k3, k4, k5);
            return new[] { k1, k2, k3, k4, k5 };
        }

        private void SeedVlasnici(Klijent[] k)
        {
            _db.Vlasnici.AddRange(
                Vlasnik(k[0], "Marko Marković", 60, new DateTime(2015, 3, 15)),
                Vlasnik(k[0], "Ana Anić", 40, new DateTime(2015, 3, 15)),
                Vlasnik(k[1], "Petar Petrović", 100, new DateTime(2018, 6, 20)),
                Vlasnik(k[2], "Ivan Ivanović", 51, new DateTime(2010, 1, 10)),
                Vlasnik(k[2], "Jovana Jovanović", 49, new DateTime(2010, 1, 10)),
                Vlasnik(k[3], "Ahmed Ahmetović", 100, new DateTime(2020, 5, 1)),
                Vlasnik(k[4], "Nikola Nikolić", 70, new DateTime(2019, 9, 15)),
                Vlasnik(k[4], "Maja Majić", 30, new DateTime(2019, 9, 15))
            );
        }

        private void SeedDirektori(Klijent[] k)
        {
            _db.Direktori.AddRange(
                Direktor(k[0], "Marko Marković", new DateTime(2025, 12, 31), TipValjanostiKonstante.Vremenski),
                Direktor(k[1], "Petar Petrović", null, TipValjanostiKonstante.Trajno),
                Direktor(k[2], "Ivan Ivanović", null, TipValjanostiKonstante.Trajno),
                Direktor(k[3], "Ahmed Ahmetović", new DateTime(2026, 6, 30), TipValjanostiKonstante.Vremenski),
                Direktor(k[4], "Nikola Nikolić", null, TipValjanostiKonstante.Trajno)
            );
        }

        private void SeedUgovori(Klijent[] k)
        {
            _db.Ugovori.AddRange(
                new Ugovor { KlijentId = k[0].Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2015, 4, 1) },
                new Ugovor { KlijentId = k[1].Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2018, 7, 1) },
                new Ugovor { KlijentId = k[2].Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2010, 2, 1) },
                new Ugovor { KlijentId = k[3].Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2020, 6, 1) },
                new Ugovor { KlijentId = k[4].Id, StatusUgovora = StatusUgovora.Potpisan, DatumUgovora = new DateTime(2019, 10, 1) }
            );
        }


        private static Klijent Klijent(
            string naziv, string idBroj, string adresa, string sifraDjelatnosti,
            DateTime datumOsnivanja, DateTime datumUspostave, VelicinaFirme velicina,
            string pepRizik, string uboRizik, string gotovinaRizik, string geografskiRizik,
            string ukupnaProcjena, DateTime datumProcjene) => new()
            {
                Naziv = naziv,
                IdBroj = idBroj,
                Adresa = adresa,
                SifraDjelatnosti = sifraDjelatnosti,
                VrstaKlijenta = VrstaKlijenta.PravnoLice,
                DatumOsnivanja = datumOsnivanja,
                DatumUspostave = datumUspostave,
                Velicina = velicina.ToString(),
                PepRizik = pepRizik,
                UboRizik = uboRizik,
                GotovinaRizik = gotovinaRizik,
                GeografskiRizik = geografskiRizik,
                UkupnaProcjena = ukupnaProcjena,
                DatumProcjene = datumProcjene,
                OvjeraCr = DaNeKonstante.Da,
                Status = StatusEntiteta.AKTIVAN,
            };

        private static Vlasnik Vlasnik(Klijent k, string ime, decimal procenat, DateTime datumUtvrdjivanja) => new()
        {
            KlijentId = k.Id,
            ImePrezime = ime,
            ProcenatVlasnistva = procenat,
            DatumUtvrdjivanja = datumUtvrdjivanja,
            IzvorPodatka = "SUDSKI REGISTAR",
            Status = StatusEntiteta.AKTIVAN,
        };

        private static Direktor Direktor(Klijent k, string ime, DateTime? datumValjanosti, string tipValjanosti) => new()
        {
            KlijentId = k.Id,
            ImePrezime = ime,
            DatumValjanosti = datumValjanosti,
            TipValjanosti = tipValjanosti,
            Status = StatusEntiteta.AKTIVAN,
        };
    }
}