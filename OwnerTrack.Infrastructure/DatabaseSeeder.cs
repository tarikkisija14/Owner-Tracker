using OwnerTrack.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwnerTrack.Infrastructure
{
    public class DatabaseSeeder
    {
        private OwnerTrackDbContext _db;

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

            
            var klijent1 = new Klijent
            {
                Naziv = "TEST RAČUNOVODSTVENA KUĆA DOO",
                IdBroj = "4512010570004",
                Adresa = "Travnik, Šumeće bb",
                SifraDjelatnosti = "69.20",
                VrstaKlijenta = "PRAVNO LICE",
                DatumOsnivanja = new DateTime(2015, 3, 15),
                DatumUspostave = new DateTime(2015, 4, 1),
                Velicina = "MIKRO",
                PepRizik = "NE",
                UboRizik = "NE",
                GotovinaRizik = "NE",
                GeografskiRizik = "NE",
                UkupnaProcjena = "NIZAK",
                DatumProcjene = new DateTime(2024, 1, 15),
                OvjeraCr = "DA",
                Status = "AKTIVAN"
            };

            var klijent2 = new Klijent
            {
                Naziv = "TRGOVINA BOSNA DOO",
                IdBroj = "4512020570005",
                Adresa = "Sarajevo, Titova 25",
                SifraDjelatnosti = "47.11",
                VrstaKlijenta = "PRAVNO LICE",
                DatumOsnivanja = new DateTime(2018, 6, 20),
                DatumUspostave = new DateTime(2018, 7, 1),
                Velicina = "MALI",
                PepRizik = "NE",
                UboRizik = "NE",
                GotovinaRizik = "DA",
                GeografskiRizik = "NE",
                UkupnaProcjena = "SREDNJI",
                DatumProcjene = new DateTime(2024, 2, 10),
                OvjeraCr = "DA",
                Status = "AKTIVAN"
            };

            var klijent3 = new Klijent
            {
                Naziv = "GRAĐEVINSKA FIRMA PROFESSIONAL DOO",
                IdBroj = "4512030570006",
                Adresa = "Zenica, Industrijska zona 10",
                SifraDjelatnosti = "41.20",
                VrstaKlijenta = "PRAVNO LICE",
                DatumOsnivanja = new DateTime(2010, 1, 10),
                DatumUspostave = new DateTime(2010, 2, 1),
                Velicina = "SREDNJI",
                PepRizik = "DA",
                UboRizik = "DA",
                GotovinaRizik = "DA",
                GeografskiRizik = "NE",
                UkupnaProcjena = "VISOK",
                DatumProcjene = new DateTime(2024, 1, 20),
                OvjeraCr = "DA",
                Status = "AKTIVAN"
            };

            var klijent4 = new Klijent
            {
                Naziv = "RESTORAN STARI GRAD",
                IdBroj = "4512040570007",
                Adresa = "Mostar, Kujundžiluk 5",
                SifraDjelatnosti = "56.10",
                VrstaKlijenta = "PRAVNO LICE",
                DatumOsnivanja = new DateTime(2020, 5, 1),
                DatumUspostave = new DateTime(2020, 6, 1),
                Velicina = "MIKRO",
                PepRizik = "NE",
                UboRizik = "NE",
                GotovinaRizik = "DA",
                GeografskiRizik = "NE",
                UkupnaProcjena = "SREDNJI",
                DatumProcjene = new DateTime(2024, 3, 5),
                OvjeraCr = "DA",
                Status = "AKTIVAN"
            };

            var klijent5 = new Klijent
            {
                Naziv = "IT SOLUTIONS DOO",
                IdBroj = "4512050570008",
                Adresa = "Banja Luka, Tech Park 12",
                SifraDjelatnosti = "62.01",
                VrstaKlijenta = "PRAVNO LICE",
                DatumOsnivanja = new DateTime(2019, 9, 15),
                DatumUspostave = new DateTime(2019, 10, 1),
                Velicina = "MALI",
                PepRizik = "NE",
                UboRizik = "NE",
                GotovinaRizik = "NE",
                GeografskiRizik = "NE",
                UkupnaProcjena = "NIZAK",
                DatumProcjene = new DateTime(2024, 2, 20),
                OvjeraCr = "DA",
                Status = "AKTIVAN"
            };

            _db.Klijenti.AddRange(klijent1, klijent2, klijent3, klijent4, klijent5);
            _db.SaveChanges();

            // Dodaj vlasnike
            _db.Vlasnici.AddRange(
                new Vlasnik
                {
                    KlijentId = klijent1.Id,
                    ImePrezime = "Marko Marković",
                    ProcetatVlasnistva = 60,
                    DatumUtvrdjivanja = new DateTime(2015, 3, 15),
                    IzvorPodatka = "SUDSKI REGISTAR",
                    Status = "AKTIVAN"
                },
                new Vlasnik
                {
                    KlijentId = klijent1.Id,
                    ImePrezime = "Ana Anić",
                    ProcetatVlasnistva = 40,
                    DatumUtvrdjivanja = new DateTime(2015, 3, 15),
                    IzvorPodatka = "SUDSKI REGISTAR",
                    Status = "AKTIVAN"
                },
                new Vlasnik
                {
                    KlijentId = klijent2.Id,
                    ImePrezime = "Petar Petrović",
                    ProcetatVlasnistva = 100,
                    DatumUtvrdjivanja = new DateTime(2018, 6, 20),
                    IzvorPodatka = "SUDSKI REGISTAR",
                    Status = "AKTIVAN"
                },
                new Vlasnik
                {
                    KlijentId = klijent3.Id,
                    ImePrezime = "Ivan Ivanović",
                    ProcetatVlasnistva = 51,
                    DatumUtvrdjivanja = new DateTime(2010, 1, 10),
                    IzvorPodatka = "SUDSKI REGISTAR",
                    Status = "AKTIVAN"
                },
                new Vlasnik
                {
                    KlijentId = klijent3.Id,
                    ImePrezime = "Jovana Jovanović",
                    ProcetatVlasnistva = 49,
                    DatumUtvrdjivanja = new DateTime(2010, 1, 10),
                    IzvorPodatka = "SUDSKI REGISTAR",
                    Status = "AKTIVAN"
                },
                new Vlasnik
                {
                    KlijentId = klijent4.Id,
                    ImePrezime = "Ahmed Ahmetović",
                    ProcetatVlasnistva = 100,
                    DatumUtvrdjivanja = new DateTime(2020, 5, 1),
                    IzvorPodatka = "SUDSKI REGISTAR",
                    Status = "AKTIVAN"
                },
                new Vlasnik
                {
                    KlijentId = klijent5.Id,
                    ImePrezime = "Nikola Nikolić",
                    ProcetatVlasnistva = 70,
                    DatumUtvrdjivanja = new DateTime(2019, 9, 15),
                    IzvorPodatka = "SUDSKI REGISTAR",
                    Status = "AKTIVAN"
                },
                new Vlasnik
                {
                    KlijentId = klijent5.Id,
                    ImePrezime = "Maja Majić",
                    ProcetatVlasnistva = 30,
                    DatumUtvrdjivanja = new DateTime(2019, 9, 15),
                    IzvorPodatka = "SUDSKI REGISTAR",
                    Status = "AKTIVAN"
                }
            );
            _db.SaveChanges();

            // Dodaj direktore
            _db.Direktori.AddRange(
                new Direktor
                {
                    KlijentId = klijent1.Id,
                    ImePrezime = "Marko Marković",
                    DatumValjanosti = new DateTime(2025, 12, 31),
                    TipValjanosti = "VREMENSKI",
                    Status = "AKTIVAN"
                },
                new Direktor
                {
                    KlijentId = klijent2.Id,
                    ImePrezime = "Petar Petrović",
                    DatumValjanosti = null,
                    TipValjanosti = "TRAJNO",
                    Status = "AKTIVAN"
                },
                new Direktor
                {
                    KlijentId = klijent3.Id,
                    ImePrezime = "Ivan Ivanović",
                    DatumValjanosti = null,
                    TipValjanosti = "TRAJNO",
                    Status = "AKTIVAN"
                },
                new Direktor
                {
                    KlijentId = klijent4.Id,
                    ImePrezime = "Ahmed Ahmetović",
                    DatumValjanosti = new DateTime(2026, 6, 30),
                    TipValjanosti = "VREMENSKI",
                    Status = "AKTIVAN"
                },
                new Direktor
                {
                    KlijentId = klijent5.Id,
                    ImePrezime = "Nikola Nikolić",
                    DatumValjanosti = null,
                    TipValjanosti = "TRAJNO",
                    Status = "AKTIVAN"
                }
            );
            _db.SaveChanges();

            // Dodaj ugovore
            _db.Ugovori.AddRange(
                new Ugovor
                {
                    KlijentId = klijent1.Id,
                    StatusUgovora = "AKTIVAN",
                    DatumUgovora = new DateTime(2015, 4, 1)
                },
                new Ugovor
                {
                    KlijentId = klijent2.Id,
                    StatusUgovora = "AKTIVAN",
                    DatumUgovora = new DateTime(2018, 7, 1)
                },
                new Ugovor
                {
                    KlijentId = klijent3.Id,
                    StatusUgovora = "AKTIVAN",
                    DatumUgovora = new DateTime(2010, 2, 1)
                },
                new Ugovor
                {
                    KlijentId = klijent4.Id,
                    StatusUgovora = "AKTIVAN",
                    DatumUgovora = new DateTime(2020, 6, 1)
                },
                new Ugovor
                {
                    KlijentId = klijent5.Id,
                    StatusUgovora = "AKTIVAN",
                    DatumUgovora = new DateTime(2019, 10, 1)
                }
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
