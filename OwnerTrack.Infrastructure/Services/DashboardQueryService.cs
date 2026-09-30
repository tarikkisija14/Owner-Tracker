using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;

namespace OwnerTrack.Infrastructure.Services
{
    // Agregatni brojevi za Compliance Dashboard — isključivo iz postojećih
    // polja (Klijent.UkupnaProcjena, PepRizik, Ugovor), bez novog izračuna
    // rizika ili duplirane logike koju već ima WarningQueryService.
    public class DashboardQueryService
    {
        private readonly OwnerTrackDbContext _db;

        public DashboardQueryService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public int GetActiveClientCount() => _db.Klijenti.AsNoTracking().Count();

        public Dictionary<string, int> GetClientCountByRisk() =>
            _db.Klijenti
                .AsNoTracking()
                .GroupBy(k => string.IsNullOrWhiteSpace(k.UkupnaProcjena) ? "Nepoznato" : k.UkupnaProcjena!)
                .Select(g => new { Procjena = g.Key, Broj = g.Count() })
                .ToDictionary(x => x.Procjena, x => x.Broj);

        public int GetPepClientCount() =>
            _db.Klijenti.AsNoTracking().Count(k => k.PepRizik == DaNeConstants.Da);

        public int GetClientsWithoutContractCount() =>
            _db.Klijenti.AsNoTracking().Count(k =>
                k.Ugovor == null || k.Ugovor.StatusUgovora == ContractStatus.NemaUgovor);

        public int GetOwnerCount() => _db.Vlasnici.AsNoTracking().Count();

        public int GetDirectorCount() => _db.Direktori.AsNoTracking().Count();

        public int GetArchivedClientCount() =>
            _db.Klijenti.AsNoTracking().IgnoreQueryFilters().Count(k => k.Obrisan != null);

        public int GetUdruzenjaCount() => new KlijentQueryService(_db).GetUdruzenjaClients().Count;

        // Reuse KlijentQueryService.GetStecajClients umjesto duplog filtera:
        // SQLite-ov LOWER() ne obrađuje dijakritiku (Č→č), pa bi filter
        // direktno na SQL strani promašio nazive poput "U STEČAJU" — isti
        // problem koji GetStecajClients već rješava diacritic-safe pretragom
        // u memoriji.
        public int GetStecajCount() => new KlijentQueryService(_db).GetStecajClients().Count;

        public int GetAuditLogCount() => _db.AuditLogs.AsNoTracking().Count();

        public int GetActivityCodeCount() => _db.Djelatnosti.AsNoTracking().Count();
    }
}
