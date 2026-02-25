using OwnerTrack.Data.Entities;
using System;

namespace OwnerTrack.Infrastructure
{
    public class AuditService
    {
        private readonly OwnerTrackDbContext _db;

        public AuditService(OwnerTrackDbContext db)
        {
            _db = db;
        }

        
        public void Zabilježi(string tabela, int? entitetId, string akcija, string opis)
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
            => Zabilježi(tabela, id, "DODANO", opis);

        public void Izmijenjeno(string tabela, int id, string opis)
            => Zabilježi(tabela, id, "IZMIJENJENO", opis);

        
        public void SoftDelete(ISoftDeletable entitet, string tabela, int id, string opis)
        {
            entitet.Obrisan = DateTime.Now;
            Zabilježi(tabela, id, "OBRISANO", opis);
        }
    }
}