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
    }
}