using Microsoft.EntityFrameworkCore;
using OwnerTrack.Infrastructure.Database;
using System;
using System.IO;

namespace OwnerTrack.Infrastructure
{

    public class DatabaseService
    {
        private readonly string _dbPath;
        private readonly string _connectionString;

        public DatabaseService(string dbPath, string connectionString)
        {
            _dbPath = dbPath;
            _connectionString = connectionString;
        }


        public void ResetirajBazu()
        {
            NapraviBackup();
            ObrisiSvePodatke();
        }

        private void NapraviBackup()
        {
            if (!File.Exists(_dbPath))
                return;

            string backupPath = _dbPath + $".backup_{DateTime.Now:yyyyMMdd_HHmmss}";
            try
            {
                File.Copy(_dbPath, backupPath, overwrite: true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Backup baze nije uspio: {ex.Message}\n" +
                    "Reset je otkazan radi sigurnosti podataka.", ex);
            }
        }

        private void ObrisiSvePodatke()
        {
            using var db = DbContextFactory.Kreiraj();
            using var tx = db.Database.BeginTransaction();
            try
            {
                
                db.Database.ExecuteSqlRaw("DELETE FROM AuditLogs");
                db.Database.ExecuteSqlRaw("DELETE FROM Ugovori");
                db.Database.ExecuteSqlRaw("DELETE FROM Vlasnici");
                db.Database.ExecuteSqlRaw("DELETE FROM Direktori");
                db.Database.ExecuteSqlRaw("DELETE FROM Klijenti");

                db.Database.ExecuteSqlRaw(
                    "DELETE FROM sqlite_sequence WHERE name IN " +
                    "('AuditLogs','Ugovori','Vlasnici','Direktori','Klijenti')");

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }

            
            var schema = new SchemaManager(_connectionString);
            schema.ReseedDjelatnosti();
        }
    }
}