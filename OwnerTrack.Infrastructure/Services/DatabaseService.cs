using Microsoft.EntityFrameworkCore;
using OwnerTrack.Infrastructure.Database;

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

        public string ResetDatabase()
        {
            string backupPath = CreateBackup();
            DeleteAllData();
            return backupPath;
        }

        public void RestoreBackup(string backupPath)
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

        private string CreateBackup()
        {
            if (!File.Exists(_dbPath))
                return string.Empty;

            string backupPath = BuildBackupPath();

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

        private string BuildBackupPath() =>
            $"{_dbPath}.backup_{DateTime.Now:yyyyMMdd_HHmmss}";

        private void DeleteAllData()
        {
            string tableList = string.Join(",", DataTables.Select(t => $"'{t}'"));

            using var db = DbContextFactory.Create();
            using var tx = db.Database.BeginTransaction();

            try
            {
                foreach (var table in DataTables)
                    db.Database.ExecuteSqlRaw($"DELETE FROM {table}");

                db.Database.ExecuteSqlRaw(
                    $"DELETE FROM sqlite_sequence WHERE name IN ({tableList})");

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