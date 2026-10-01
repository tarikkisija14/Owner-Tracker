namespace OwnerTrack.Data.Enums
{
    // Tačan tekst pitanja iz "OBRAZAC ZA PROCJENU RIZIKA" (list "INDIKATORI RIZIKA")
    // — pitanja su fiksna (dio zvaničnog AML/CFT obrasca), pa žive u kodu, a
    // odgovori po klijentu u Klijent.RizikObrazacJson. Redni brojevi se dodaju
    // pri prikazu (forma/PDF), ne čuvaju se u tekstu.
    public static class RizikObrazacKriteriji
    {
        public static readonly string[] OpciIndikatori =
        {
            "Klijent insistira na hitnom izvršenju transakcije bez obzira na dodatne troškove.",
            "Klijent neopravdano mijenja instrukcije u vezi sa izvršenjem transakcije.",
            "Klijent pokazuje neobično poznavanje detalja transakcije ili nastoji da kontroliše svaki njen aspekt.",
            "Klijent nudi novac, poklone ili druge neuobičajene pogodnosti kao protuuslugu za izvođenje očito neuobičajenog ili sumnjivog posla.",
            "Stranka je pod istragom za kazneno djelo pranja novca ili finansiranja terorizma.",
            "Stranka pokušava uvjeriti radnika da ne popunjava neki od dokumenata koji je potreban za obavljanje transakcije.",
            "Stranka vrlo dobro poznaje pravila o obavještavanju o sumnjivim transakcijama.",
            "Stranka je vrlo dobro upoznata sa slučajevima koji se odnose na pranje novca i finansiranje terorizma.",
            "Stranka sama izjavljuje da su sredstva „čista“ i nisu „oprana“.",
            "Postojanje neuobičajenih okolnosti u vezi sa transakcijom, kao što su neuobičajena žurba klijenta, nastojanje da se transakcija obavi van radnog vremena ili na neobinom mjestu.",
            "Transakcije koje uključuju zemlje ili područja visokog rizika, one koji su podložni sankcijama ili one koji imaju slabu regulaciju u vezi sa sprečavanjem pranja novca i finansiranja terorizma.",
            "Poslovni odnosi ili transakcije sa fizičkim ili pravnim licima za koje se zna da su povezani sa kriminalnim aktivnostima ili su na listama sankcionisanih lica.",
            "Transakcije koje uključuju složene vlasničke strukture ili neobične organizacione strukture koje otežavaju utvrđivanje stvarnog vlasnika imovine.",
            "Postojanje neuobičajeno visokih ili niskih provizija ili naknada u vezi sa transakcijom.",
            "Transakcije koje su nekonzistentne sa uobičajenim poslovanjem klijenta ili sa poznatim izvorima njegovih prihoda.",
            "Klijent često mijenja bankovne račune ili koristi račune u više banaka bez jasnog razloga.",
            "Klijent izbjegava direktan kontakt sa zaposlenima i preferira obavljanje transakcija putem interneta ili telefona.",
            "Postojanje neuobičajenih tokova novca između povezanih lica ili kompanija koje nemaju očiglednu poslovnu svrhu.",
            "Klijent uplaćuje ili isplaćuje velike svote novca u gotovini, a djelatnost klijenta ne opravdava takve transakcije.",
            "Transakcije koje uključuju prijenos sredstava u ili iz jurisdikcija koje ne sarađuju u borbi protiv pranja novca i finansiranja terorizma.",
            "Klijent koristi usluge fiktivnih kompanija ili kompanija koje imaju nejasnu vlasničku strukturu.",
            "Postojanje neuobičajenih transakcija na računima neprofitnih organizacija ili dobrotvornih fondacija.",
            "Klijent odbija da pruži dodatne informacije ili objašnjenja u vezi sa transakcijom kada se to zatraži.",
            "Zaposlenik sumnja da klijent djeluje u ime treće osobe koja nije otkrivena.",
            "Transakcije koje uključuju kupovinu ili prodaju imovine po cijenama koje značajno odstupaju od tržišnih vrijednosti.",
        };

        public static readonly string[] IndikatoriIdentifikacije =
        {
            "Stranka podnosi na uvid neodgovarajuće lične dokumente, odnosno dokumente koji pokazuju da su krivotvoreni, preuređeni ili neispravni.",
            "Stranka se protivi davanju na uvid ličnih dokumenata.",
            "Stranka pokušava da prikaže samo kopije ličnih dokumenata.",
            "Stranka se pokušava identificirati pomoću drugih dokumenata koji nisu lični identifikacioni dokumenti.",
            "Svi lični dokumenti su izdati u inostranstvu i njihovu je vjerodostojnost teško provjeriti.",
            "Klijent daje netačne ili nepotpune informacije prilikom identifikacije.",
            "Postoje značajne nedosljednosti između informacija koje je klijent dao i informacija dobijenih iz drugih izvora.",
            "Klijent odbija da pruži informacije o izvoru sredstava ili svrsi transakcije.",
            "Klijent koristi lažni ili promijenjeni identitet.",
            "Klijent koristi adresu koja se razlikuje od njegove uobičajene adrese prebivališta ili boravišta.",
            "Klijent često mijenja lične podatke, kao što su adresa, broj telefona ili e-mail adresa, bez opravdanog razloga.",
            "Klijent je fizički ili pravno lice koje je već bilo uključeno u sumnjive transakcije ili aktivnosti pranja novca i finansiranja terorizma.",
        };

        public static readonly string[] IndikatoriTransakcija =
        {
            "Više povezanih gotovinskih transakcija u iznosima manjim od iznosa predviđenog za prijavljivanje gotovinskih transakcija, a koje ukupno prelaze taj iznos.",
            "Klijent u velikim iznosima vrši zamjenu novčanica u većim apoenima za novčanice malih apoena ili obrnuto, naročito ako takve transakcije nisu karakteristične za klijenta.",
            "Klijent vrši zamjenu oštećenih novčanica u značajnom iznosu.",
            "Klijent predaje na zamjenu neprebrojan novac, a nakon prebrojavanja smanjuje iznos transakcije na iznos koji je nešto ispod limita za obavezno prijavljivanje.",
            "Klijent vrši zamjenu veće količine novca iz jedne u drugu stranu valutu (konverzija većih iznosa novca).",
            "Klijent vrši učestale transakcije zamjene novca na iste i zaokružene iznose ili na iznose koji su malo ispod praga za prijavljivanje.",
            "Klijent koji vrši zamjenu veće količine novca zahtjeva od mjenjača usitnjavanje novca.",
            "Postojanje neuobičajenih oblika plaćanja, kao što su plaćanja od strane trećih lica koja nisu direktno povezana sa transakcijom.",
            "Česta plaćanja ili primanja iz inostranstva, posebno iz zemalja ili područja visokog rizika.",
            "Transakcije koje nemaju očiglednu ekonomsku ili pravnu svrhu.",
            "Transakcije koje uključuju neobične izvore finansiranja, kao što su zajmovi od nepoznatih lica ili kompanija.",
            "Klijent koristi složene finansijske instrumente ili proizvode koji nisu u skladu sa njegovim poslovnim profilom.",
            "Postojanje neuobičajenih transakcija na računima koji se koriste za isplatu plata ili drugih primanja zaposlenih.",
            "Transakcije koje uključuju velike svote novca koje se brzo prenose na druge račune ili se podižu u gotovini.",
            "Klijent često vrši transakcije sa inostranstvom bez jasne poslovne svrhe.",
        };

        public static readonly string[] GeografskiRizikOstali =
        {
            "Da li poslovna aktivnost uključuje zemlju koja je poznata kao porezna oaza ili finansijski off-shore centar?",
            "Da li poslovna aktivnost uključuje zemlju koja podržava terorističke aktivnosti?",
            "Da li poslovna aktivnost uključuje zemlju koju je FATF identificirao kao nekooperativnu u borbi protiv pranja novaca ili finansiranja terorizma?",
            "Da li poslovna aktivnost uključuje zemlju u kojoj se prema procjeni relevantnih međunarodnih organizacija ne provode odgovarajuće mjere SPNFT?",
            "Da li poslovna aktivnost uključuje zemlju koja je poznata po značajnom stupnju korupcije ili drugih kriminalnih aktivnosti?",
            "Da li je stranka politički izložena",
        };

        // Posljednja stavka bloka "Geografski rizik i ostali rizici" je slobodan
        // tekst (u Excelu linija za upis), a ne DA/NE/N-P kriterij.
        public const string OstalaSumnjivaZapazanja = "Ostala sumnjiva zapažanja";

        public const string Da = "DA";
        public const string Ne = "NE";
        public const string Np = "N/P";
        public const string Vise = "VIŠE";
        public const string Nize = "NIŽE";
    }
}
