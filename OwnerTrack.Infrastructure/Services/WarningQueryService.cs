using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;

namespace OwnerTrack.Infrastructure.Services
{
    public class WarningQueryService
    {
        private readonly OwnerTrackDbContext _db;

        public WarningQueryService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public WarningStats GetStats()
        {
            var today = DateTime.Today;
            var threshold = today.AddDays(AppConstants.DanaUpozerenja);

            // "Pregledana" (acknowledged) upozorenja i dalje postoje kao stvarna
            // upozorenja (vidljiva u FrmUpozorenja), ali se ne broje u badge/count
            // logici — korisnik ih je već obradio. Isključuju se anti-joinom na
            // WarningAcknowledgements po (EntityType, EntityId, DatumIsteka).
            var acknowledgedOwnerIds = _db.WarningAcknowledgements
                .AsNoTracking()
                .Where(a => a.EntityType == "Vlasnik")
                .Select(a => new { a.EntityId, a.DatumIsteka });
            var acknowledgedDirectorIds = _db.WarningAcknowledgements
                .AsNoTracking()
                .Where(a => a.EntityType == "Direktor")
                .Select(a => new { a.EntityId, a.DatumIsteka });

            bool hasExpired =
                _db.Vlasnici.AsNoTracking().Any(v =>
                    v.DatumValjanostiDokumenta < today
                    && v.Status == StatusEntiteta.AKTIVAN
                    && v.Klijent.Status != StatusEntiteta.ARHIVIRAN
                    && !acknowledgedOwnerIds.Any(a => a.EntityId == v.Id && a.DatumIsteka == v.DatumValjanostiDokumenta))
                || _db.Direktori.AsNoTracking().Any(d =>
                    d.DatumValjanosti < today
                    && d.Status == StatusEntiteta.AKTIVAN
                    && d.Klijent.Status != StatusEntiteta.ARHIVIRAN
                    && !acknowledgedDirectorIds.Any(a => a.EntityId == d.Id && a.DatumIsteka == d.DatumValjanosti));

            int count =
                _db.Vlasnici.AsNoTracking().Count(v =>
                    v.DatumValjanostiDokumenta <= threshold
                    && v.Status == StatusEntiteta.AKTIVAN
                    && v.Klijent.Status != StatusEntiteta.ARHIVIRAN
                    && !acknowledgedOwnerIds.Any(a => a.EntityId == v.Id && a.DatumIsteka == v.DatumValjanostiDokumenta))
                + _db.Direktori.AsNoTracking().Count(d =>
                    d.DatumValjanosti <= threshold
                    && d.Status == StatusEntiteta.AKTIVAN
                    && d.Klijent.Status != StatusEntiteta.ARHIVIRAN
                    && !acknowledgedDirectorIds.Any(a => a.EntityId == d.Id && a.DatumIsteka == d.DatumValjanosti));

            return new WarningStats(count, hasExpired);
        }

        public List<WarningDetail> GetWarnings()
        {
            var threshold = DateTime.Today.AddDays(AppConstants.DanaUpozerenja);

            return GetOwnerWarnings(threshold)
                .Concat(GetDirectorWarnings(threshold))
                .OrderBy(x => x.DatumIsteka)
                .ToList();
        }

        private List<WarningDetail> GetOwnerWarnings(DateTime threshold) =>
            _db.Vlasnici
                .AsNoTracking()
                .Where(v => v.DatumValjanostiDokumenta != null
                         && v.DatumValjanostiDokumenta <= threshold
                         && v.Status == StatusEntiteta.AKTIVAN
                         && v.Klijent.Status != StatusEntiteta.ARHIVIRAN)
                .Include(v => v.Klijent)
                .Select(v => new WarningDetail
                {
                    KlijentId = v.KlijentId,
                    NazivFirme = v.Klijent.Naziv,
                    EntityId = v.Id,
                    ImePrezime = v.ImePrezime,
                    Tip = "Vlasnik",
                    DatumIsteka = v.DatumValjanostiDokumenta!.Value,
                })
                .ToList();

        private List<WarningDetail> GetDirectorWarnings(DateTime threshold) =>
            _db.Direktori
                .AsNoTracking()
                .Where(d => d.DatumValjanosti != null
                         && d.DatumValjanosti <= threshold
                         && d.Status == StatusEntiteta.AKTIVAN
                         && d.Klijent.Status != StatusEntiteta.ARHIVIRAN)
                .Include(d => d.Klijent)
                .Select(d => new WarningDetail
                {
                    KlijentId = d.KlijentId,
                    NazivFirme = d.Klijent.Naziv,
                    EntityId = d.Id,
                    ImePrezime = d.ImePrezime,
                    Tip = "Direktor",
                    DatumIsteka = d.DatumValjanosti!.Value,
                })
                .ToList();
    }
}