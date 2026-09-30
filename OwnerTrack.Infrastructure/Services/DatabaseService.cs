using Microsoft.Data.Sqlite;
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
                throw new FileNotFoundException(
                    "Backup fajl nije pronađen, pa vraćanje nije izvršeno.", backupPath);

            try
            {
                // Idle pooled connections keep the current Firme.db file open; overwriting
                // it while they still hold it would leave them (and their now-stale
                // -wal/-shm files, which describe the file we're about to replace) pointing
                // at pages that no longer match the restored file, risking "malformed
                // database" errors on the next query.
                SqliteConnection.ClearAllPools();
                File.Copy(backupPath, _dbPath, overwrite: true);
                DeleteStaleWalSidecarFiles();
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
                // In WAL mode, recently committed data can still live only in the
                // -wal file. Copying Firme.db alone would silently drop that data from
                // the backup, so force it back into the main file first.
                CheckpointWal();
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

        private void CheckpointWal()
        {
            using var db = DbContextFactory.Create();
            db.Database.ExecuteSqlRaw("PRAGMA wal_checkpoint(TRUNCATE);");
        }

        private void DeleteStaleWalSidecarFiles()
        {
            foreach (var suffix in new[] { "-wal", "-shm" })
            {
                string path = _dbPath + suffix;
                if (File.Exists(path))
                    File.Delete(path);
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