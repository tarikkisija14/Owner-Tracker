using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.ViewModels;
using OwnerTrack.Infrastructure.Database;

namespace OwnerTrack.Infrastructure.Services
{
    public class KlijentQueryService
    {
        private readonly OwnerTrackDbContext _db;

        public KlijentQueryService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public List<KlijentViewModel> GetClients(
            string searchText = "",
            string sifraDjelatnosti = "",
            string velicina = "")
        {
            var query = _db.Klijenti
                .Where(k =>
                    (string.IsNullOrWhiteSpace(sifraDjelatnosti) || k.SifraDjelatnosti == sifraDjelatnosti)
                    && (string.IsNullOrWhiteSpace(velicina) || k.Velicina == velicina));

            query = ApplyDiacriticSafeSearch(query, searchText);
            return ProjectClients(query);
        }

        // Arhivirani/otkazani klijenti — isti soft-delete mehanizam koji
        // AuditService.Arhiviraj već postavlja (Obrisan != null), samo
        // obrnut filter u odnosu na globalni query filter koji ih inače
        // isključuje iz svih ostalih upita.
        public List<KlijentViewModel> GetArchivedClients(string searchText = "")
        {
            var query = _db.Klijenti
                .IgnoreQueryFilters()
                .Where(k => k.Obrisan != null);

            query = ApplyDiacriticSafeSearch(query, searchText);
            return ProjectClients(query);
        }

        // Nema posebnog statusa/flaga za stečaj u bazi — jedini trag je da
        // firme kod kojih je pokrenut stečajni postupak dobiju to upisano
        // u sam naziv (npr. "TOM DD U STEČAJU"). Filtriramo i po
        // dijakritičkoj i po ne-dijakritičkoj varijanti riječi jer uvezeni
        // Excel podaci nisu uvijek dosljedni oko č/c.
        public List<KlijentViewModel> GetStecajClients(string searchText = "")
        {
            var matchingIds = SearchableRows(_db.Klijenti)
                .Where(k =>
                    k.Naziv?.Contains("stečaj", StringComparison.OrdinalIgnoreCase) == true ||
                    k.Naziv?.Contains("stecaj", StringComparison.OrdinalIgnoreCase) == true)
                .Select(k => k.Id)
                .ToList();

            var query = _db.Klijenti.Where(k => matchingIds.Contains(k.Id));
            query = ApplyDiacriticSafeSearch(query, searchText);
            return ProjectClients(query);
        }

        // "Udruženje" nije vrijednost VrstaKlijenta, nego se bilježi kroz polje
        // Veličina (VelicinaFirme.UDRUŽENJE) — isto polje koje inače nosi
        // MIKRO/MALO/SREDNJE/VELIKO/OBRTNIK. Poređenje je egzaktno (=), pa
        // dijakritika (Ž) ovdje ne pravi problem kakav je imao ILIKE/LOWER().
        public List<KlijentViewModel> GetUdruzenjaClients(string searchText = "")
        {
            string udruzenje = VelicinaFirme.UDRUŽENJE.ToString();

            var query = _db.Klijenti.Where(k => k.Velicina == udruzenje);
            query = ApplyDiacriticSafeSearch(query, searchText);
            return ProjectClients(query);
        }

        // "Bez ugovora" pokriva oba slučaja koja u praksi znače isto: klijent
        // nema uopšte Ugovor red (Ugovor == null), ili ima Ugovor red čiji je
        // StatusUgovora eksplicitno postavljen na "NEMA UGOVOR" (korisnik može
        // kreirati takav red umjesto da ga briše — vidi FrmDodajKlijent).
        public List<KlijentViewModel> GetClientsWithoutContract(string searchText = "")
        {
            var query = _db.Klijenti.Where(k =>
                k.Ugovor == null || k.Ugovor.StatusUgovora == ContractStatus.NemaUgovor);
            query = ApplyDiacriticSafeSearch(query, searchText);
            return ProjectClients(query);
        }

        // SQLite-ova ugrađena LOWER()/LIKE case-insensitivnost pokriva samo
        // ASCII a-z, pa npr. "Č".ToLower() na SQLite strani ostaje "Č" (ne
        // postaje "č") i search po "č"/"Č"/"ć"/"Ć"/"š"/"Š"/"ž"/"Ž"/"đ"/"Đ" ne bi
        // pronašao zapise čiji je naziv upisan u drugom "case"-u tog slova.
        // .NET-ov OrdinalIgnoreCase Contains to radi ispravno, pa se poklapanje
        // po Naziv/IdBroj radi u memoriji nad samo (Id, Naziv, IdBroj) —
        // jeftina projekcija, ne cijeli red — a zatim se pravi upit (sa
        // Include/Count za Djelatnost/Vlasnike/Direktore) izvršava kroz SQL
        // samo za te ID-ove. Ista tehnika koju je GetStecajClients već
        // koristio, sada primijenjena dosljedno na sve search-e u ovoj klasi.
        // Namjerno bez AsNoTracking/full-table-scan optimizacije preko toga —
        // tabela je mala (desetine do niske stotine redova za ovu vrstu
        // aplikacije) i identičan obrazac se već pokazao ispravnim u praksi za
        // GetStecajClients; ako tabela naraste na hiljade redova ovo bi
        // trebalo zamijeniti pravim collation-om u bazi.
        private IQueryable<Klijent> ApplyDiacriticSafeSearch(IQueryable<Klijent> query, string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return query;

            var matchingIds = SearchableRows(query)
                .Where(k =>
                    k.Naziv?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true ||
                    k.IdBroj?.Contains(searchText, StringComparison.OrdinalIgnoreCase) == true)
                .Select(k => k.Id)
                .ToList();

            return query.Where(k => matchingIds.Contains(k.Id));
        }

        private static IEnumerable<(int Id, string Naziv, string IdBroj)> SearchableRows(IQueryable<Klijent> query) =>
            query.AsNoTracking()
                 .Select(k => new { k.Id, k.Naziv, k.IdBroj })
                 .AsEnumerable()
                 .Select(k => (k.Id, k.Naziv, k.IdBroj));

        private static List<KlijentViewModel> ProjectClients(IQueryable<Klijent> query)
        {
            // Eksplicitan OrderBy — bez njega SQL ne garantuje nikakav
            // redoslijed rezultata (raniji "sortirano po Id" izgled je bio
            // slučajan, zavisan od fizičkog rasporeda redova u tabeli, a ne
            // stvaran garantovan poredak).
            var result = query
                .AsNoTracking()
                .OrderBy(k => k.Id)
                .Select(k => new KlijentViewModel
                {
                    Id = k.Id,
                    Naziv = k.Naziv,
                    IdBroj = k.IdBroj,
                    Adresa = k.Adresa,
                    SifraDjelatnosti = k.SifraDjelatnosti,
                    Djelatnost = k.Djelatnost != null ? k.Djelatnost.Naziv : string.Empty,
                    DatumUspostaveOdnosa = k.DatumUspostave,
                    VrstaKlijenta = k.VrstaKlijenta != null ? k.VrstaKlijenta.ToString() : null,
                    DatumOsnivanjaFirme = k.DatumOsnivanja,
                    Velicina = k.Velicina,
                    PepRizik = k.PepRizik,
                    UboRizik = k.UboRizik,
                    GotovinaRizik = k.GotovinaRizik,
                    GeografskiRizik = k.GeografskiRizik,
                    UkupnaProcjena = k.UkupnaProcjena,
                    DatumProcjeneRizika = k.DatumProcjene,
                    OvjeraCr = k.OvjeraCr,
                    StatusUgovora = k.Ugovor != null ? k.Ugovor.StatusUgovora : string.Empty,
                    DatumPotpisaUgovora = k.Ugovor != null ? k.Ugovor.DatumUgovora : null,
                    BrojVlasnika = k.Vlasnici.Count(),
                    BrojDirektora = k.Direktori.Count(),
                    StatusKlijenta = k.Status.ToString(),
                    Napomena = k.Napomena,
                    PepImePrezime = k.PepImePrezime,
                    PepFunkcija = k.PepFunkcija,
                    PepPovezanost = k.PepPovezanost,
                    PepMjerePoduzete = k.PepMjerePoduzete,
                    PepDatumProvjere = k.PepDatumProvjere,
                    OpciIndikatoriRizika = k.OpciIndikatoriRizika,
                    IndikatoriIdentifikacijeRizika = k.IndikatoriIdentifikacijeRizika,
                    IndikatoriTransakcijaRizika = k.IndikatoriTransakcijaRizika,
                })
                .ToList();

            foreach (var k in result)
                if (k.VrstaKlijenta != null && Enum.TryParse<VrstaKlijenta>(k.VrstaKlijenta, out var vk))
                    k.VrstaKlijenta = vk.ToDisplay();

            return result;
        }

        public int GetTotalCount() => _db.Klijenti.Count();

        public List<VlasnikViewModel> GetOwners(int klijentId)
        {
            return _db.Vlasnici
                .Where(v => v.KlijentId == klijentId)
                .AsNoTracking()
                .Select(v => new VlasnikViewModel
                {
                    Id = v.Id,
                    ImePrezime = v.ImePrezime,
                    DatumValjanostiDokumenta = v.DatumValjanostiDokumenta,
                    ProcenatVlasnistva = v.ProcenatVlasnistva,
                    DatumUtvrdjivanja = v.DatumUtvrdjivanja,
                    IzvorPodatka = v.IzvorPodatka,
                    StatusVlasnika = v.Status.ToString(),
                })
                .ToList();
        }

        public List<DirektorViewModel> GetDirectors(int klijentId)
        {
            return _db.Direktori
                .Where(d => d.KlijentId == klijentId)
                .AsNoTracking()
                .Select(d => new DirektorViewModel
                {
                    Id = d.Id,
                    ImePrezime = d.ImePrezime,
                    DatumValjanostiDokumenta = d.DatumValjanosti,
                    TipValjanosti = d.TipValjanosti,
                    StatusDirektora = d.Status.ToString(),
                })
                .ToList();
        }
    }
}
