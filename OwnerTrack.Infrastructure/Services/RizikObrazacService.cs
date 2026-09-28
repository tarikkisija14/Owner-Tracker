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

        public RizikObrazacPodaci Load(int klijentId) => Load(klijentId, out _);

        // out version — Klijent.Version u trenutku učitavanja, da ga pozivalac
        // (FrmRizikObrazac) može čuvati i proslijediti nazad u Save kao
        // expectedVersion za optimistic-concurrency provjeru.
        public RizikObrazacPodaci Load(int klijentId, out int version)
        {
            var row = _db.Klijenti
                .Where(k => k.Id == klijentId)
                .Select(k => new { k.RizikObrazacJson, k.Version })
                .FirstOrDefault();

            version = row?.Version ?? 0;

            if (row is null || string.IsNullOrWhiteSpace(row.RizikObrazacJson))
                return new RizikObrazacPodaci();

            return JsonSerializer.Deserialize<RizikObrazacPodaci>(row.RizikObrazacJson) ?? new RizikObrazacPodaci();
        }

        // expectedVersion je Klijent.Version onako kako ga je vidio pozivalac
        // (npr. FrmRizikObrazac pri otvaranju forme) — postavlja se na entitetu
        // prije SaveChanges da EF-ov optimistic-concurrency check ("WHERE
        // Version = expectedVersion") stvarno uporedi sa onim što je korisnik
        // vidio, a ne sa vrijednošću koju upravo pročita ovaj isti poziv (što bi
        // učinilo provjeru beskorisnom — uvijek bi se poklapala sa samom sobom).
        // Vraća novu verziju da pozivalac može ažurirati svoje lokalno stanje za
        // sljedeći Save u istoj sesiji forme.
        public int Save(int klijentId, RizikObrazacPodaci podaci, int expectedVersion)
        {
            var klijent = _db.Klijenti.FirstOrDefault(k => k.Id == klijentId)
                ?? throw new InvalidOperationException($"Klijent ID={klijentId} nije pronađen.");

            _db.Entry(klijent).Property(k => k.Version).OriginalValue = expectedVersion;

            klijent.RizikObrazacJson = JsonSerializer.Serialize(podaci);
            klijent.Azuriran = DateTime.Now;
            klijent.Version = expectedVersion + 1;
            new AuditService(_db).LogUpdated("Klijenti", klijentId, "Ažuriran obrazac za procjenu rizika");
            _db.SaveChanges();

            return klijent.Version;
        }
    }
}
