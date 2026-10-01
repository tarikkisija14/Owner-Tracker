namespace OwnerTrack.App.Constants
{
    
    internal static class GridColumns
    {
        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] Klijenti =
        {
            ("Redni",                          50, "RED",                                   null),
            ("Naziv",                         220, "NAZIV PREDUZEĆA",                       null),
            ("IdBroj",                        130, "ID BROJ",                               null),
            ("Adresa",                        200, "ADRESA",                                null),
            ("SifraDjelatnosti",               90, "ŠIFRA DJELATNOSTI",                     null),
            ("Djelatnost",                    220, "DJELATNOST",                            null),
            ("DatumUspostaveOdnosa",          120, "DATUM USPOSTAVE ODNOSA",                "dd.MM.yyyy"),
            ("VrstaKlijenta",                 110, "VRSTA KLIJENTA",                        null),
            ("DatumOsnivanjaFirme",           120, "DATUM OSNIVANJA",                       "dd.MM.yyyy"),
            ("VlasnikImena",                  200, "VLASNIK",                               null),
            ("DatumVazenjaDokumentaVlasnika", 150, "DATUM VAŽENJA DOKUMENTA VLASNIKA",      null),
            ("ProcenatVlasnistva",             90, "% VLASNIŠTVA",                          null),
            ("DatumUtvrdjivanjaVlasnistva",   150, "DATUM UTVRĐIVANJA VLASNIŠTVA",          null),
            ("IzvorPodatkaVlasnistvo",        180, "IZVOR PODATKA ZA VLASNIŠTVO",           null),
            ("DirektorImena",                 200, "DIREKTOR",                              null),
            ("DatumVazenjaDokumentaDirektora",150, "DATUM VAŽENJA DOKUMENTA DIREKTORA",     null),
            ("Velicina",                       80, "VELIČINA",                              null),
            ("PepRizik",                       90, "PEP (DA/NE)",                           null),
            ("UboRizik",                       90, "UBO (DA/NE)",                           null),
            ("GotovinaRizik",                 100, "GOTOVINA (DA/NE)",                      null),
            ("GeografskiRizik",               130, "GEOGRAFSKI RIZIK (DA/NE)",              null),
            ("UkupnaProcjena",                120, "UKUPNA PROCJENA RIZIKA",                null),
            ("DatumProcjeneRizika",           120, "DATUM PROCJENE RIZIKA",                 "dd.MM.yyyy"),
            ("OvjeraCr",                      150, "OVJERA/CR",                             null),
            ("StatusUgovora",                 110, "UGOVOR",                                null),
            ("DatumPotpisaUgovora",           120, "DATUM UGOVORA",                         "dd.MM.yyyy"),
            ("Napomena",                     200, "NAPOMENA",                               null),
        };

        // Kolone koje PDF izvoz prikazuje za grid-ove sa GridColumns.Klijenti (Bez ugovora,
        // Udruženja, Stečaj) — isti skup koji koristi PdfExportService.GenerateClientTable.
        public static readonly string[] KlijentiPdf =
        {
            "Naziv", "IdBroj", "Djelatnost", "Velicina", "PepRizik", "UboRizik", "UkupnaProcjena",
            "StatusUgovora", "DatumUspostaveOdnosa", "DatumOsnivanjaFirme", "StatusKlijenta",
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] Vlasnici =
        {
            ("Id",                       40,  "ID",                 null),
            ("ImePrezime",              180,  "Ime i prezime",      null),
            ("DatumValjanostiDokumenta",140,  "Datum važenja dok.", "dd.MM.yyyy"),
            ("ProcenatVlasnistva",      100,  "% vlasništva",       null),
            ("DatumUtvrdjivanja",       130,  "Datum utvrđivanja",  "dd.MM.yyyy"),
            ("IzvorPodatka",            150,  "Izvor podatka",      null),
            ("StatusVlasnika",           90,  "Status",             null),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] Direktori =
        {
            ("Id",                       40,  "ID",                 null),
            ("ImePrezime",              200,  "Ime i prezime",      null),
            ("DatumValjanostiDokumenta",140,  "Datum važenja dok.", "dd.MM.yyyy"),
            ("TipValjanosti",           120,  "Tip valjanosti",     null),
            ("StatusDirektora",          90,  "Status",             null),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] Kyc =
        {
            ("Redni",                 50,  "RED",                     null),
            ("Naziv",                220,  "NAZIV PREDUZEĆA",         null),
            ("IdBroj",               130,  "ID BROJ",                 null),
            ("Adresa",               200,  "ADRESA",                  null),
            ("SifraDjelatnosti",      90,  "ŠIFRA DJELATNOSTI",       null),
            ("Djelatnost",           220,  "DJELATNOST",              null),
            ("DatumUspostaveOdnosa", 120,  "DATUM USPOSTAVE ODNOSA",  "dd.MM.yyyy"),
            ("VrstaKlijenta",        110,  "VRSTA KLIJENTA",          null),
            ("PepRizik",              90,  "PEP (DA/NE)",             null),
            ("UkupnaProcjena",       120,  "UKUPNA PROCJENA RIZIKA",  null),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] UboSveFirme =
        {
            ("Redni",              50,  "RED",                           null),
            ("KlijentNaziv",      220,  "NAZIV PREDUZEĆA",               null),
            ("KlijentIdBroj",     130,  "ID BROJ",                       null),
            ("ImePrezime",        200,  "VLASNIK",                       null),
            ("ProcenatVlasnistva",110,  "% VLASNIŠTVA",                  null),
            ("DatumUtvrdjivanja", 140,  "DATUM UTVRĐIVANJA VLASNIŠTVA",  "dd.MM.yyyy"),
            ("IzvorPodatka",      200,  "IZVOR PODATKA ZA VLASNIŠTVO",   null),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] Pep =
        {
            ("Redni",             50,  "RBR",                                   null),
            ("NazivKlijenta",    220,  "NAZIV KLIJENTA",                        null),
            ("PepImePrezime",    200,  "IME I PREZIME POLITIČKI IZLOŽENE OSOBE",null),
            ("PepFunkcija",      180,  "FUNKCIJA",                              null),
            ("PepPovezanost",    200,  "POVEZANOST S KLIJENTOM",                null),
            ("PepMjerePoduzete", 220,  "MJERE KOJE SU PODUZETE",                null),
            ("PepDatumProvjere", 120,  "DATUM PROVJERE",                        "dd.MM.yyyy"),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] RizikProcjena =
        {
            ("Redni",                          50,  "RED",                                              null),
            ("Naziv",                         220,  "NAZIV PREDUZEĆA",                                  null),
            ("IdBroj",                        130,  "ID BROJ",                                          null),
            ("Djelatnost",                    220,  "DJELATNOST",                                       null),
            ("VrstaKlijenta",                 150,  "VRSTA KLIJENTA (PRAVNO ILI FIZIČKO)",             null),
            ("VlasnikImena",                  200,  "VLASNIK",                                          null),
            ("DirektorImena",                 200,  "DIREKTOR",                                         null),
            ("OpciIndikatoriRizika",          120,  "OPĆI INDIKATORI",                                  null),
            ("IndikatoriIdentifikacijeRizika",200,  "INDIKATORI VEZANI ZA IDENTIFIKACIJA KLIJENTA",     null),
            ("IndikatoriTransakcijaRizika",   200,  "INDIKATORI VEZANI ZA TRANSAKCIJE",                 null),
            ("GeografskiRizik",               200,  "GEOGRAFSKI RIZIK I OSTALI RIZICI",                 null),
            ("UkupnaProcjena",                120,  "UKUPNA PROCJENA RIZIKA",                           null),
            ("DatumProcjeneRizika",           120,  "DATUM PROCJENE RIZIKA",                            "dd.MM.yyyy"),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] AuditLog =
        {
            ("Vrijeme",   140, "Vrijeme",   "dd.MM.yyyy HH:mm"),
            ("Tabela",    120, "Tabela",    null),
            ("EntitetId",  80, "ID zapisa", null),
            ("Akcija",    110, "Akcija",    null),
            ("Korisnik",  110, "Korisnik",  null),
            ("Opis",      400, "Opis",      null),
        };
    }
}