using System.Text.Json;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;

namespace OwnerTrack.Infrastructure.Services
{
    public class RizikObrazacService
    {
        private readonly OwnerTrackDbContext _db;

        public RizikObrazacService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public RizikObrazacPodaci Load(int klijentId)
        {
            var json = _db.Klijenti
                .Where(k => k.Id == klijentId)
                .Select(k => k.RizikObrazacJson)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(json))
                return new RizikObrazacPodaci();

            return JsonSerializer.Deserialize<RizikObrazacPodaci>(json) ?? new RizikObrazacPodaci();
        }

        public void Save(int klijentId, RizikObrazacPodaci podaci)
        {
            var klijent = _db.Klijenti.FirstOrDefault(k => k.Id == klijentId)
                ?? throw new InvalidOperationException($"Klijent ID={klijentId} nije pronađen.");

            klijent.RizikObrazacJson = JsonSerializer.Serialize(podaci);
            klijent.Azuriran = DateTime.Now;
            new AuditService(_db).LogUpdated("Klijenti", klijentId, "Ažuriran obrazac za procjenu rizika");
            _db.SaveChanges();
        }
    }
}
