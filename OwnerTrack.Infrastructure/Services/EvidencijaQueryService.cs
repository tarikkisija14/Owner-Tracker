using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.ViewModels;

namespace OwnerTrack.Infrastructure.Services
{
    /// <summary>
    /// Izvještajni prikazi (KYC, UBO, PEP, procjena rizika) koji čitaju iz
    /// postojećih tabela (Klijenti, Vlasnici, Direktori) — bez novog unosa
    /// podataka, samo drugačiji raspored/podskup postojećih kolona.
    /// </summary>
    public class EvidencijaQueryService
    {
        private readonly OwnerTrackDbContext _db;

        public EvidencijaQueryService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public List<AuditEntryViewModel> GetAuditLogEvidencija()
        {
            return _db.AuditLogs
                .AsNoTracking()
                .OrderByDescending(a => a.Vrijeme)
                .Select(a => new AuditEntryViewModel
                {
                    Vrijeme = a.Vrijeme,
                    Tabela = a.Tabela,
                    EntitetId = a.EntitetId,
                    Akcija = a.Akcija,
                    Opis = a.Opis,
                })
                .ToList();
        }

        public List<KycViewModel> GetKycEvidencija()
        {
            var klijenti = _db.Klijenti
                .Include(k => k.Djelatnost)
                .Include(k => k.Vlasnici)
                .Include(k => k.Direktori)
                .AsNoTracking()
                .OrderBy(k => k.Naziv)
                .ToList();

            return klijenti.Select((k, i) => new KycViewModel
            {
                Id = k.Id,
                Redni = i + 1,
                Naziv = k.Naziv,
                IdBroj = k.IdBroj,
                Adresa = k.Adresa,
                SifraDjelatnosti = k.SifraDjelatnosti,
                Djelatnost = k.Djelatnost?.Naziv,
                DatumUspostaveOdnosa = k.DatumUspostave,
                VrstaKlijenta = k.VrstaKlijenta.ToDisplay(),
                VlasnikImena = string.Join(", ", k.Vlasnici.Select(v => v.ImePrezime)),
                DirektorImena = string.Join(", ", k.Direktori.Select(d => d.ImePrezime)),
                Velicina = k.Velicina,
                PepRizik = k.PepRizik,
                UboRizik = k.UboRizik,
                UkupnaProcjena = k.UkupnaProcjena,
            }).ToList();
        }

        public List<VlasnikSaFirmomViewModel> GetUboEvidencija()
        {
            var result = _db.Vlasnici
                .Include(v => v.Klijent)
                .AsNoTracking()
                .OrderBy(v => v.Klijent!.Naziv).ThenBy(v => v.ImePrezime)
                .Select(v => new VlasnikSaFirmomViewModel
                {
                    Id = v.KlijentId,
                    KlijentNaziv = v.Klijent!.Naziv,
                    KlijentIdBroj = v.Klijent!.IdBroj,
                    ImePrezime = v.ImePrezime,
                    ProcenatVlasnistva = v.ProcenatVlasnistva,
                    DatumUtvrdjivanja = v.DatumUtvrdjivanja,
                    IzvorPodatka = v.IzvorPodatka,
                })
                .ToList();

            for (int i = 0; i < result.Count; i++)
                result[i].Redni = i + 1;

            return result;
        }

        public List<PepViewModel> GetPepEvidencija()
        {
            var result = _db.Klijenti
                .Where(k => k.PepRizik == "DA")
                .AsNoTracking()
                .OrderBy(k => k.Naziv)
                .Select(k => new PepViewModel
                {
                    Id = k.Id,
                    NazivKlijenta = k.Naziv,
                    PepImePrezime = k.PepImePrezime,
                    PepFunkcija = k.PepFunkcija,
                    PepPovezanost = k.PepPovezanost,
                    PepMjerePoduzete = k.PepMjerePoduzete,
                    PepDatumProvjere = k.PepDatumProvjere,
                })
                .ToList();

            for (int i = 0; i < result.Count; i++)
                result[i].Redni = i + 1;

            return result;
        }

        public List<RizikProcjenaViewModel> GetRizikEvidencija()
        {
            var klijenti = _db.Klijenti
                .Include(k => k.Djelatnost)
                .Include(k => k.Ugovor)
                .Include(k => k.Vlasnici)
                .Include(k => k.Direktori)
                .AsNoTracking()
                .OrderBy(k => k.Naziv)
                .ToList();

            return klijenti.Select((k, i) => new RizikProcjenaViewModel
            {
                Id = k.Id,
                Redni = i + 1,
                Naziv = k.Naziv,
                IdBroj = k.IdBroj,
                Djelatnost = k.Djelatnost?.Naziv,
                VrstaKlijenta = k.VrstaKlijenta.ToDisplay(),
                VlasnikImena = string.Join(", ", k.Vlasnici.Select(v => v.ImePrezime)),
                DirektorImena = string.Join(", ", k.Direktori.Select(d => d.ImePrezime)),
                Velicina = k.Velicina,
                OpciIndikatoriRizika = k.OpciIndikatoriRizika,
                IndikatoriIdentifikacijeRizika = k.IndikatoriIdentifikacijeRizika,
                IndikatoriTransakcijaRizika = k.IndikatoriTransakcijaRizika,
                GeografskiRizik = k.GeografskiRizik,
                UkupnaProcjena = k.UkupnaProcjena,
                DatumProcjeneRizika = k.DatumProcjene,
                OvjeraCr = k.OvjeraCr,
                VrstaUgovora = k.Ugovor?.VrstaUgovora,
                DatumUgovora = k.Ugovor?.DatumUgovora,
            }).ToList();
        }
    }
}
