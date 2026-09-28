namespace OwnerTrack.Data.Enums
{
    // Tačan tekst pitanja iz "OBRAZAC ZA PROCJENU RIZIKA" — pitanja su fiksna
    // (dio zvaničnog AML/CFT obrasca), pa žive u kodu, a odgovori po klijentu
    // u Klijent.RizikObrazacJson.
    public static class RizikObrazacKriteriji
    {
        public static readonly string[] RizikStranke =
        {
            "Da li se stranka bavi gotovinski intenzivnom djelatnošću?",
            "Da li stranka ima prebivalište/uobičajeno boravište izvan BiH?",
            "Da li je stranka posrednik ili obavlja profesionalnu djelatnost (npr. advokat), a koja nastupa u ime i za račun stranke za koju identitet stvarnog vlasnika nije moguće utvrditi?",
            "Da li je stranka zaklada, humanitarna organizacija ili slična neprofitna organizacija, naročito ako posluje na prekograničnoj osnovi?",
            "Da li stranka ima prebivalište/uobičajeno boravište na području poznatom po visokoj stopi kriminaliteta?",
            "Da li je stranka poznata kao pripadnik krim-miljea ili ima veze s organiziranim kriminalom?",
            "Da li priroda posla stranke otežava utvrđivanje stvarnog vlasnika?",
            "Da li je stranka strana ili domaća politički izloženo lice ili istaknuti funkcioner međunarodne organizacije?",
            "Da li stranka nema adresu ili ima nekoliko adresa bez vidljivog razloga?",
        };

        public static readonly string[] RizikPoslovnogOdnosa =
        {
            "Uključuje li poslovni odnos transakcije za koje se utvrđuje i provjera identitet bez nazočnosti stranke i/ili uspostavlja poslovni odnos bez nazočnosti stranke?",
            "Uključuje li poslovni odnos složene finansijske transakcije?",
            "Uključuje li poslovni odnos plaćanja trećih lica ili prema trećim licima kao i prekogranična plaćanja?",
            "Uključuje li poslovni odnos provođenje transakcija u ime i za račun stranke?",
            "Uključuje li poslovni odnos višestruke i/ili rizične transakcije nekretninama?",
            "Uključuje li poslovni odnos gotovinske transakcije?",
        };

        public static readonly string[] GeografskiRizik =
        {
            "Da li poslovna aktivnost uključuje zemlju koja je poznata kao porezna oaza ili finansijski off-shore centar?",
            "Da li poslovna aktivnost uključuje zemlju koja podržava terorističke aktivnosti?",
            "Da li poslovna aktivnost uključuje zemlju koju je FATF identificirao kao nekooperativnu u borbi protiv pranja novca ili finansiranja terorizma?",
            "Da li poslovna aktivnost uključuje zemlju u kojoj se prema procjeni relevantnih međunarodnih organizacija ne provode odgovarajuće mjere SPNFT?",
            "Da li poslovna aktivnost uključuje zemlju koja je poznata po značajnom stupnju korupcije ili drugih kriminalnih aktivnosti?",
        };

        public const string Da = "DA";
        public const string Ne = "NE";
        public const string Np = "N/P";
        public const string Vise = "VIŠE";
        public const string Nize = "NIŽE";
    }
}
