using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using System;

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
                Vrijeme = DateTime.Now
            });
        }

        public void Dodano(string tabela, int id, string opis)
            => Log(tabela, id, AuditKonstante.Dodano, opis);

        public void Izmijenjeno(string tabela, int id, string opis)
            => Log(tabela, id, AuditKonstante.Izmijenjeno, opis);

        
        public void SoftDelete(ISoftDeletable entitet, string tabela, int id, string opis)
        {
            entitet.Obrisan = DateTime.Now;
            Log(tabela, id, AuditKonstante.Obrisano, opis);
        }
    }
}