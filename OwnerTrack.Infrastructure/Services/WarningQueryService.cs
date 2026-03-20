using Microsoft.EntityFrameworkCore;
using OwnerTrack.App.Helpers;
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

        
        public WarningStats DohvatiStats()
        {
            var danas = DateTime.Today;
            var granica = danas.AddDays(AppKonstante.DanaUpozerenja);

            bool imaIsteklih =
                _db.Vlasnici.AsNoTracking().Any(v =>
                    v.DatumValjanostiDokumenta < danas
                    && v.Status == StatusEntiteta.AKTIVAN
                    && v.Klijent.Status != StatusEntiteta.ARHIVIRAN)
                || _db.Direktori.AsNoTracking().Any(d =>
                    d.DatumValjanosti < danas
                    && d.TipValjanosti == TipValjanostiKonstante.Vremenski
                    && d.Status == StatusEntiteta.AKTIVAN
                    && d.Klijent.Status != StatusEntiteta.ARHIVIRAN);

            int count =
                _db.Vlasnici.AsNoTracking().Count(v =>
                    v.DatumValjanostiDokumenta <= granica
                    && v.Status == StatusEntiteta.AKTIVAN
                    && v.Klijent.Status != StatusEntiteta.ARHIVIRAN)
                + _db.Direktori.AsNoTracking().Count(d =>
                    d.DatumValjanosti <= granica
                    && d.TipValjanosti == TipValjanostiKonstante.Vremenski
                    && d.Status == StatusEntiteta.AKTIVAN
                    && d.Klijent.Status != StatusEntiteta.ARHIVIRAN);

            return new WarningStats(count, imaIsteklih);
        }

        
        public List<UpozorenjeDetalj> DohvatiUpozorenja()
        {
            var granica = DateTime.Today.AddDays(AppKonstante.DanaUpozerenja);

            return DohvatiUpozorenjaVlasnika(granica)
                .Concat(DohvatiUpozerenjaDirektora(granica))
                .OrderBy(x => x.DatumIsteka)
                .ToList();
        }

        

        private List<UpozorenjeDetalj> DohvatiUpozorenjaVlasnika(DateTime granica) =>
            _db.Vlasnici
                .AsNoTracking()
                .Where(v => v.DatumValjanostiDokumenta != null
                         && v.DatumValjanostiDokumenta <= granica
                         && v.Status == StatusEntiteta.AKTIVAN
                         && v.Klijent.Status != StatusEntiteta.ARHIVIRAN)
                .Include(v => v.Klijent)
                .Select(v => new UpozorenjeDetalj
                {
                    KlijentId = v.KlijentId,
                    NazivFirme = v.Klijent.Naziv,
                    ImePrezime = v.ImePrezime,
                    Tip = "Vlasnik",
                    DatumIsteka = v.DatumValjanostiDokumenta!.Value,
                })
                .ToList();

        private List<UpozorenjeDetalj> DohvatiUpozerenjaDirektora(DateTime granica) =>
            _db.Direktori
                .AsNoTracking()
                .Where(d => d.DatumValjanosti != null
                         && d.DatumValjanosti <= granica
                         && d.TipValjanosti == TipValjanostiKonstante.Vremenski
                         && d.Status == StatusEntiteta.AKTIVAN
                         && d.Klijent.Status != StatusEntiteta.ARHIVIRAN)
                .Include(d => d.Klijent)
                .Select(d => new UpozorenjeDetalj
                {
                    KlijentId = d.KlijentId,
                    NazivFirme = d.Klijent.Naziv,
                    ImePrezime = d.ImePrezime,
                    Tip = "Direktor",
                    DatumIsteka = d.DatumValjanosti!.Value,
                })
                .ToList();
    }
}