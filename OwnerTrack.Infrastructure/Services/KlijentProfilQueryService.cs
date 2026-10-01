using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.ViewModels;

namespace OwnerTrack.Infrastructure.Services
{
    public class KlijentProfilQueryService
    {
        private readonly OwnerTrackDbContext _db;

        public KlijentProfilQueryService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        // Otisak stanja jedne firme (verzije firme, vlasnika i direktora, uključujući
        // arhivirane) — mijenja se kad god bilo ko izmijeni ili arhivira bilo šta od toga.
        public string GetProfileToken(int klijentId)
        {
            var firma = _db.Klijenti.IgnoreQueryFilters().AsNoTracking()
                .Where(k => k.Id == klijentId)
                .Select(k => new { k.Version })
                .FirstOrDefault();
            if (firma is null) return "obrisano";

            var vlasnici = _db.Vlasnici.IgnoreQueryFilters().AsNoTracking()
                .Where(v => v.KlijentId == klijentId)
                .Select(v => v.Version).ToList();
            var direktori = _db.Direktori.IgnoreQueryFilters().AsNoTracking()
                .Where(d => d.KlijentId == klijentId)
                .Select(d => d.Version).ToList();

            return $"{firma.Version}|{vlasnici.Count}:{vlasnici.Sum()}|{direktori.Count}:{direktori.Sum()}";
        }

        public KlijentProfilViewModel? GetProfile(int klijentId)
        {
            var k = _db.Klijenti
                .IgnoreQueryFilters()
                .Include(x => x.Djelatnost)
                .Include(x => x.Ugovor)
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == klijentId);

            if (k == null) return null;

            var vlasnici = _db.Vlasnici
                .IgnoreQueryFilters()
                .Where(v => v.KlijentId == klijentId)
                .AsNoTracking()
                .OrderByDescending(v => v.Status == StatusEntiteta.AKTIVAN)
                .ThenBy(v => v.ImePrezime)
                .ToList();

            var direktori = _db.Direktori
                .IgnoreQueryFilters()
                .Where(d => d.KlijentId == klijentId)
                .AsNoTracking()
                .OrderByDescending(d => d.Status == StatusEntiteta.AKTIVAN)
                .ThenBy(d => d.ImePrezime)
                .ToList();

            var vlasnikIds = vlasnici.Select(v => v.Id).ToList();
            var direktorIds = direktori.Select(d => d.Id).ToList();

            var historija = _db.AuditLogs
                .Where(a =>
                    (a.Tabela == "Klijenti" && a.EntitetId == klijentId) ||
                    (a.Tabela == "Vlasnici" && a.EntitetId != null && vlasnikIds.Contains(a.EntitetId.Value)) ||
                    (a.Tabela == "Direktori" && a.EntitetId != null && direktorIds.Contains(a.EntitetId.Value)))
                .AsNoTracking()
                .OrderByDescending(a => a.Vrijeme)
                .Select(a => new AuditEntryViewModel
                {
                    Vrijeme = a.Vrijeme,
                    Tabela = a.Tabela,
                    Akcija = a.Akcija,
                    Opis = a.Opis,
                })
                .ToList();

            return new KlijentProfilViewModel
            {
                Id = k.Id,
                Naziv = k.Naziv,
                IdBroj = k.IdBroj,
                Adresa = k.Adresa,
                Djelatnost = k.Djelatnost?.Naziv,
                DatumUspostave = k.DatumUspostave,
                VrstaKlijenta = k.VrstaKlijenta.ToDisplay(),
                DatumOsnivanja = k.DatumOsnivanja,
                Velicina = k.Velicina,
                Email = k.Email,
                Telefon = k.Telefon,
                StatusKlijenta = k.Status.ToString(),
                Napomena = k.Napomena,

                PepRizik = k.PepRizik,
                UboRizik = k.UboRizik,
                GotovinaRizik = k.GotovinaRizik,
                GeografskiRizik = k.GeografskiRizik,
                UkupnaProcjena = k.UkupnaProcjena,
                DatumProcjeneRizika = k.DatumProcjene,
                OvjeraCr = k.OvjeraCr,
                PepImePrezime = k.PepImePrezime,
                PepFunkcija = k.PepFunkcija,
                PepPovezanost = k.PepPovezanost,
                PepMjerePoduzete = k.PepMjerePoduzete,
                PepDatumProvjere = k.PepDatumProvjere,
                OpciIndikatoriRizika = k.OpciIndikatoriRizika,
                IndikatoriIdentifikacijeRizika = k.IndikatoriIdentifikacijeRizika,
                IndikatoriTransakcijaRizika = k.IndikatoriTransakcijaRizika,

                VrstaUgovora = k.Ugovor?.VrstaUgovora,
                StatusUgovora = k.Ugovor?.StatusUgovora,
                DatumUgovora = k.Ugovor?.DatumUgovora,
                NapomenaUgovora = k.Ugovor?.Napomena,

                Vlasnici = vlasnici.Select(v => new VlasnikProfilViewModel
                {
                    ImePrezime = v.ImePrezime,
                    ProcenatVlasnistva = v.ProcenatVlasnistva,
                    DatumValjanostiDokumenta = v.DatumValjanostiDokumenta,
                    Status = v.Status.ToString(),
                }).ToList(),

                Direktori = direktori.Select(d => new DirektorProfilViewModel
                {
                    ImePrezime = d.ImePrezime,
                    DatumValjanosti = d.DatumValjanosti,
                    TipValjanosti = d.TipValjanosti,
                    Status = d.Status.ToString(),
                }).ToList(),

                Historija = historija,
            };
        }
    }
}
