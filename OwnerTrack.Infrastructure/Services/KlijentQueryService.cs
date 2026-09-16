using Microsoft.EntityFrameworkCore;
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
            var result = _db.Klijenti
                .Where(k =>
                    (string.IsNullOrWhiteSpace(searchText) ||
                     k.Naziv.ToLower().Contains(searchText.ToLower()) ||
                     k.IdBroj.ToLower().Contains(searchText.ToLower()))
                    && (string.IsNullOrWhiteSpace(sifraDjelatnosti) || k.SifraDjelatnosti == sifraDjelatnosti)
                    && (string.IsNullOrWhiteSpace(velicina) || k.Velicina == velicina))
                .AsNoTracking()
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
