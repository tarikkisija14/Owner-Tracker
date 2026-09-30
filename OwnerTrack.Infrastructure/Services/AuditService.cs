using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;

namespace OwnerTrack.Infrastructure.Services
{
    public class AuditService
    {
        private readonly OwnerTrackDbContext _db;

        public AuditService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        public void Log(string tabela, int? entitetId, string akcija, string opis)
        {
            _db.AuditLogs.Add(new AuditLog
            {
                Tabela = tabela,
                EntitetId = entitetId,
                Akcija = akcija,
                Opis = opis,
                Vrijeme = DateTime.Now,
            });
        }

        public void LogAdded(string tabela, int id, string opis)
            => Log(tabela, id, AuditConstants.Dodano, opis);

        public void LogUpdated(string tabela, int id, string opis)
            => Log(tabela, id, AuditConstants.Izmijenjeno, opis);

        public void Archive(IArchivable entity, string tabela, int id, string opis)
        {
            entity.Status = StatusEntiteta.ARHIVIRAN;
            entity.Obrisan = DateTime.Now;
            Log(tabela, id, AuditConstants.Obrisano, opis);
        }

        /// <summary>
        /// Gradi opis izmjene koji prikazuje samo polja koja su se stvarno
        /// promijenila (npr. samo datum, iako se ime nije mijenjalo) — umjesto
        /// dosadašnjeg fiksnog "'staro ime' → 'novo ime'" formata koji je
        /// zbunjujuće ispisivao isto ime na obje strane kad se mijenjalo neko
        /// drugo polje.
        /// </summary>
        public static string DescribeFieldChanges(string label, params (string Field, string? Before, string? After)[] fields)
        {
            var changes = fields
                .Where(f => (f.Before ?? string.Empty) != (f.After ?? string.Empty))
                .Select(f => $"{f.Field}: '{f.Before}' → '{f.After}'")
                .ToList();

            return changes.Count == 0
                ? $"'{label}' — snimljeno bez promjena na poljima"
                : $"'{label}' — " + string.Join("; ", changes);
        }
    }
}