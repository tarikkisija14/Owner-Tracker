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

            var clients = SeedKlijenti();
            _db.SaveChanges();

            SeedVlasnici(clients);
            _db.SaveChanges();

            SeedDirektori(clients);
            _db.SaveChanges();

            SeedUgovori(clients);
            _db.SaveChanges();

            Console.WriteLine("Test podaci uspješno dodati!");
            Console.WriteLine($"Dodato klijenata: {_db.Klijenti.Count()}");
            Console.WriteLine($"Dodato vlasnika:  {_db.Vlasnici.Count()}");
            Console.WriteLine($"Dodato direktora: {_db.Direktori.Count()}");
            Console.WriteLine($"Dodato ugovora:   {_db.Ugovori.Count()}");
        }

        private void SeedDjelatnosti()
        {
            var codes = new[]
            {
                (AppConstants.DefaultSifraDjelatnosti, "Računovodstvene, knjigovodstvene i revizorske djelatnosti"),
                ("47.11", "Maloprodaja u nespecijaliziranim prodavnicama pretežno hrane"),
                ("41.20", "Gradnja stambenih i nestambenih zgrada"),
                ("56.10", "Restorani i ostali objekti za pripremu hrane"),
                ("62.01", "Računalno programiranje"),
            };

            foreach (var (sifra, naziv) in codes)
                if (!_db.Djelatnosti.Any(d => d.Sifra == sifra))
                    _db.Djelatnosti.Add(new Djelatnost { Sifra = sifra, Naziv = naziv });
        }

        private Klijent[] SeedKlijenti()
        {
            var k1 = CreateKlijent("TEST RAČUNOVODSTVENA KUĆA DOO", "4512010570004", "Travnik, Šumeće bb",
                AppConstants.DefaultSifraDjelatnosti, new DateTime(2015, 3, 15), new DateTime(2015, 4, 1),
                VelicinaFirme.MIKRO, DaNeConstants.Ne, DaNeConstants.Ne, DaNeConstants.Ne, DaNeConstants.Ne,
                "NIZAK", new DateTime(2024, 1, 15));

            var k2 = CreateKlijent("TRGOVINA BOSNA DOO", "4512020570005", "Sarajevo, Titova 25",
                "47.11", new DateTime(2018, 6, 20), new DateTime(2018, 7, 1),
                VelicinaFirme.MALO, DaNeConstants.Ne, DaNeConstants.Ne, DaNeConstants.Da, DaNeConstants.Ne,
                "SREDNJI", new DateTime(2024, 2, 10));

            var k3 = CreateKlijent("GRAĐEVINSKA FIRMA PROFESSIONAL DOO", "4512030570006", "Zenica, Industrijska zona 10",
                "41.20", new DateTime(2010, 1, 10), new DateTime(2010, 2, 1),
                VelicinaFirme.SREDNJE, DaNeConstants.Da, DaNeConstants.Da, DaNeConstants.Da, DaNeConstants.Ne,
                "VISOK", new DateTime(2024, 1, 20));

            var k4 = CreateKlijent("RESTORAN STARI GRAD", "4512040570007", "Mostar, Kujundžiluk 5",
                "56.10", new DateTime(2020, 5, 1), new DateTime(2020, 6, 1),
                VelicinaFirme.MIKRO, DaNeConstants.Ne, DaNeConstants.Ne, DaNeConstants.Da, DaNeConstants.Ne,
                "SREDNJI", new DateTime(2024, 3, 5));

            var k5 = CreateKlijent("IT SOLUTIONS DOO", "4512050570008", "Banja Luka, Tech Park 12",
                "62.01", new DateTime(2019, 9, 15), new DateTime(2019, 10, 1),
                VelicinaFirme.MALO, DaNeConstants.Ne, DaNeConstants.Ne, DaNeConstants.Ne, DaNeConstants.Ne,
                "NIZAK", new DateTime(2024, 2, 20));

            _db.Klijenti.AddRange(k1, k2, k3, k4, k5);
            return new[] { k1, k2, k3, k4, k5 };
        }

        private void SeedVlasnici(Klijent[] clients)
        {
            _db.Vlasnici.AddRange(
                CreateVlasnik(clients[0], "Marko Marković", 60, new DateTime(2015, 3, 15)),
                CreateVlasnik(clients[0], "Ana Anić", 40, new DateTime(2015, 3, 15)),
                CreateVlasnik(clients[1], "Petar Petrović", 100, new DateTime(2018, 6, 20)),
                CreateVlasnik(clients[2], "Ivan Ivanović", 51, new DateTime(2010, 1, 10)),
                CreateVlasnik(clients[2], "Jovana Jovanović", 49, new DateTime(2010, 1, 10)),
                CreateVlasnik(clients[3], "Ahmed Ahmetović", 100, new DateTime(2020, 5, 1)),
                CreateVlasnik(clients[4], "Nikola Nikolić", 70, new DateTime(2019, 9, 15)),
                CreateVlasnik(clients[4], "Maja Majić", 30, new DateTime(2019, 9, 15))
            );
        }

        private void SeedDirektori(Klijent[] clients)
        {
            _db.Direktori.AddRange(
                CreateDirektor(clients[0], "Marko Marković", new DateTime(2025, 12, 31), ValidityTypeConstants.Vremenski),
                CreateDirektor(clients[1], "Petar Petrović", null, ValidityTypeConstants.Trajno),
                CreateDirektor(clients[2], "Ivan Ivanović", null, ValidityTypeConstants.Trajno),
                CreateDirektor(clients[3], "Ahmed Ahmetović", new DateTime(2026, 6, 30), ValidityTypeConstants.Vremenski),
                CreateDirektor(clients[4], "Nikola Nikolić", null, ValidityTypeConstants.Trajno)
            );
        }

        private void SeedUgovori(Klijent[] clients)
        {
            _db.Ugovori.AddRange(
                new Ugovor { KlijentId = clients[0].Id, StatusUgovora = ContractStatus.Potpisan, DatumUgovora = new DateTime(2015, 4, 1) },
                new Ugovor { KlijentId = clients[1].Id, StatusUgovora = ContractStatus.Potpisan, DatumUgovora = new DateTime(2018, 7, 1) },
                new Ugovor { KlijentId = clients[2].Id, StatusUgovora = ContractStatus.Potpisan, DatumUgovora = new DateTime(2010, 2, 1) },
                new Ugovor { KlijentId = clients[3].Id, StatusUgovora = ContractStatus.Potpisan, DatumUgovora = new DateTime(2020, 6, 1) },
                new Ugovor { KlijentId = clients[4].Id, StatusUgovora = ContractStatus.Potpisan, DatumUgovora = new DateTime(2019, 10, 1) }
            );
        }

        private static Klijent CreateKlijent(
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
                OvjeraCr = DaNeConstants.Da,
                Status = StatusEntiteta.AKTIVAN,
            };

        private static Vlasnik CreateVlasnik(Klijent client, string name, decimal percentage, DateTime datumUtvrdjivanja) => new()
        {
            KlijentId = client.Id,
            ImePrezime = name,
            ProcenatVlasnistva = percentage,
            DatumUtvrdjivanja = datumUtvrdjivanja,
            IzvorPodatka = "SUDSKI REGISTAR",
            Status = StatusEntiteta.AKTIVAN,
        };

        private static Direktor CreateDirektor(Klijent client, string name, DateTime? datumValjanosti, string tipValjanosti) => new()
        {
            KlijentId = client.Id,
            ImePrezime = name,
            DatumValjanosti = datumValjanosti,
            TipValjanosti = tipValjanosti,
            Status = StatusEntiteta.AKTIVAN,
        };
    }
}