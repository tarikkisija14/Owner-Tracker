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

        
        public void ZabiljeziBesSave(string tabela, int? entitetId, string akcija, string opis)
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

        public void ZabiljeziBesSave(ISoftDeletable entitet, string tabela, int? entitetId, string opis,
            string akcija = AuditKonstante.Obrisano)
            => ZabiljeziBesSave(tabela, entitetId, akcija, opis);

        
        public void Dodano(string tabela, int id, string opis)
            => ZabiljeziBesSave(tabela, id, AuditKonstante.Dodano, opis);

        public void Izmijenjeno(string tabela, int id, string opis)
            => ZabiljeziBesSave(tabela, id, AuditKonstante.Izmijenjeno, opis);

        public void SoftDelete(ISoftDeletable entitet, string tabela, int id, string opis)
        {
            entitet.Obrisan = DateTime.Now;
            ZabiljeziBesSave(tabela, id, AuditKonstante.Obrisano, opis);
        }
    }
}