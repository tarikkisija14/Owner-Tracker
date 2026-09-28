namespace OwnerTrack.App.Constants
{
    
    internal static class GridColumns
    {
        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] Klijenti =
        {
            ("Id",                    40, "ID",                     null),
            ("Naziv",                220, "Naziv preduzeća",        null),
            ("IdBroj",               130, "ID broj",                null),
            ("Adresa",               200, "Adresa",                 null),
            ("SifraDjelatnosti",      70, "Šifra",                  null),
            ("Djelatnost",           220, "Djelatnost",             null),
            ("DatumUspostaveOdnosa", 120, "Datum uspostave odnosa", "dd.MM.yyyy"),
            ("VrstaKlijenta",        110, "Vrsta klijenta",         null),
            ("DatumOsnivanjaFirme",  120, "Datum osnivanja",        "dd.MM.yyyy"),
            ("Velicina",              80, "Veličina",               null),
            ("PepRizik",              70, "PEP",                    null),
            ("UboRizik",              70, "UBO",                    null),
            ("GotovinaRizik",         90, "Gotovina rizik",         null),
            ("GeografskiRizik",      100, "Geografski rizik",       null),
            ("UkupnaProcjena",       120, "Ukupna procjena",        null),
            ("DatumProcjeneRizika",  120, "Datum procjene rizika",  "dd.MM.yyyy"),
            ("OvjeraCr",             150, "Ovjera/CR",              null),
            ("StatusUgovora",        110, "Status ugovora",         null),
            ("DatumPotpisaUgovora",  120, "Datum potpisa ugovora",  "dd.MM.yyyy"),
            ("BrojVlasnika",          80, "Vlasnici",               null),
            ("BrojDirektora",         80, "Direktori",              null),
            ("StatusKlijenta",        90, "Status klijenta",        null),
            ("Napomena",             200, "Napomena",               null),
            ("PepImePrezime",        180, "PEP - ime i prezime",    null),
            ("PepFunkcija",          150, "PEP funkcija",           null),
            ("PepPovezanost",        180, "PEP povezanost",         null),
            ("PepMjerePoduzete",     200, "PEP mjere poduzete",     null),
            ("PepDatumProvjere",     120, "PEP datum provjere",     "dd.MM.yyyy"),
            ("OpciIndikatoriRizika", 180, "Opći indikatori",        null),
            ("IndikatoriIdentifikacijeRizika", 200, "Indikatori identifikacije", null),
            ("IndikatoriTransakcijaRizika",    200, "Indikatori transakcija",    null),
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
            ("Redni",                 50,  "Red.",                   null),
            ("Naziv",                220,  "Naziv preduzeća",        null),
            ("IdBroj",                130,  "ID broj",                null),
            ("Adresa",                200,  "Adresa",                 null),
            ("SifraDjelatnosti",       70,  "Šifra",                  null),
            ("Djelatnost",            220,  "Djelatnost",             null),
            ("DatumUspostaveOdnosa",  120,  "Datum uspostave odnosa", "dd.MM.yyyy"),
            ("VrstaKlijenta",         110,  "Vrsta klijenta",         null),
            ("VlasnikImena",          200,  "Vlasnik",                null),
            ("DirektorImena",         200,  "Direktor",               null),
            ("Velicina",               80,  "Veličina",               null),
            ("PepRizik",               70,  "PEP",                    null),
            ("UboRizik",               70,  "UBO",                    null),
            ("UkupnaProcjena",        120,  "Ukupna procjena",        null),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] UboSveFirme =
        {
            ("Redni",              50,  "Red.",                null),
            ("KlijentNaziv",      220,  "Naziv preduzeća",     null),
            ("KlijentIdBroj",     130,  "ID broj",             null),
            ("ImePrezime",        200,  "Ime i prezime",       null),
            ("ProcenatVlasnistva",110,  "% vlasništva",        null),
            ("DatumUtvrdjivanja", 140,  "Datum utvrđivanja",   "dd.MM.yyyy"),
            ("IzvorPodatka",      200,  "Izvor podatka",       null),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] Pep =
        {
            ("Redni",             50,  "Red.",                              null),
            ("NazivKlijenta",    220,  "Naziv klijenta",                    null),
            ("PepImePrezime",    200,  "Ime i prezime PEP osobe",           null),
            ("PepFunkcija",      180,  "Funkcija",                          null),
            ("PepPovezanost",    200,  "Povezanost s klijentom",            null),
            ("PepMjerePoduzete", 220,  "Mjere koje su poduzete",            null),
            ("PepDatumProvjere", 120,  "Datum provjere",                    "dd.MM.yyyy"),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] RizikProcjena =
        {
            ("Redni",                          50,  "Red.",                                        null),
            ("Naziv",                         220,  "Naziv preduzeća",                             null),
            ("IdBroj",                        130,  "ID broj",                                      null),
            ("Djelatnost",                    220,  "Djelatnost",                                   null),
            ("VrstaKlijenta",                 110,  "Vrsta klijenta",                               null),
            ("VlasnikImena",                  200,  "Vlasnik",                                      null),
            ("DirektorImena",                 200,  "Direktor",                                     null),
            ("Velicina",                       80,  "Veličina",                                     null),
            ("OpciIndikatoriRizika",          220,  "Opći indikatori",                              null),
            ("IndikatoriIdentifikacijeRizika",220,  "Indikatori identifikacije",                    null),
            ("IndikatoriTransakcijaRizika",   220,  "Indikatori transakcija",                       null),
            ("GeografskiRizik",               100,  "Geografski rizik",                             null),
            ("UkupnaProcjena",                120,  "Ukupna procjena",                              null),
            ("DatumProcjeneRizika",           120,  "Datum procjene rizika",                        "dd.MM.yyyy"),
            ("OvjeraCr",                      150,  "Ovjera/CR",                                    null),
            ("VrstaUgovora",                  130,  "Ugovor",                                       null),
            ("DatumUgovora",                  120,  "Datum ugovora",                                "dd.MM.yyyy"),
        };

        public static readonly (string Ime, int Sirina, string Zaglavlje, string? Format)[] AuditLog =
        {
            ("Vrijeme",   140, "Vrijeme",   "dd.MM.yyyy HH:mm"),
            ("Tabela",    120, "Tabela",    null),
            ("EntitetId",  80, "ID zapisa", null),
            ("Akcija",    110, "Akcija",    null),
            ("Opis",      400, "Opis",      null),
        };
    }
}