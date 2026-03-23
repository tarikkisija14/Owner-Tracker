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

        /// <inheritdoc cref="Archive"/>
        [Obsolete("Use Archive() which also sets Status = ARHIVIRAN.")]
        public void SoftDelete(ISoftDeletable entity, string tabela, int id, string opis)
        {
            entity.Obrisan = DateTime.Now;
            Log(tabela, id, AuditConstants.Obrisano, opis);
        }
    }
}