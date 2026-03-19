using Microsoft.EntityFrameworkCore;
using OwnerTrack.Infrastructure.Database;
using System;
using System.IO;

namespace OwnerTrack.Infrastructure.Services
{
    public class DatabaseService
    {
        private static readonly string[] DataTables =
            { "AuditLogs", "Ugovori", "Vlasnici", "Direktori", "Klijenti" };

        private readonly string _dbPath;
        private readonly string _connectionString;

        public DatabaseService(string dbPath, string connectionString)
        {
            _dbPath = dbPath;
            _connectionString = connectionString;
        }

        
        public string ResetirajBazu()
        {
            string backupPath = KreirajBackup();
            ObrisiSvePodatke();
            return backupPath;
        }

        public void VratiBackup(string backupPath)
        {
            if (string.IsNullOrEmpty(backupPath) || !File.Exists(backupPath))
                return;

            try
            {
                File.Copy(backupPath, _dbPath, overwrite: true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Vraćanje backupa nije uspjelo: {ex.Message}\n\n" +
                    $"Backup se nalazi na:\n{backupPath}\n\n" +
                    "Ručno kopiraj taj fajl i preimenuj ga u 'Firme.db'.", ex);
            }
        }

      
        private string KreirajBackup()
        {
            if (!File.Exists(_dbPath))
                return string.Empty;

            string backupPath = $"{_dbPath}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";
            try
            {
                File.Copy(_dbPath, backupPath, overwrite: true);
                return backupPath;
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
                foreach (var tabela in DataTables)
                    db.Database.ExecuteSqlRaw($"DELETE FROM {tabela}");

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

            new SchemaManager(_connectionString).ReseedDjelatnosti();
        }
    }
}