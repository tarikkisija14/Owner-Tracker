using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;

namespace OwnerTrack.Infrastructure
{
   
    public class SchemaManager
    {
        private readonly string _connectionString;

        public SchemaManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ApplyMigrations()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();

            
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS __SchemaVersion (
                        Version INTEGER NOT NULL,
                        AppliedAt TEXT NOT NULL
                    )";
                cmd.ExecuteNonQuery();
            }

            int currentVersion = GetCurrentVersion(conn);
            Debug.WriteLine($"[SCHEMA] Trenutna verzija: {currentVersion}");

            if (currentVersion < 1) ApplyV1(conn);
            if (currentVersion < 2) ApplyV2(conn);

            Debug.WriteLine($"[SCHEMA] Migracije završene. Nova verzija: {GetCurrentVersion(conn)}");
        }

       
        private void ApplyV1(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] Primjenjujem V1...");
            SetVersion(conn, 1);
        }

        
        private void ApplyV2(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] Primjenjujem V2 — uklanjanje unique constrainta na Direktori...");

            using var transaction = conn.BeginTransaction();
            try
            {
                
                bool uniqueIndexExists = false;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT COUNT(*) FROM sqlite_master 
                        WHERE type='index' 
                        AND tbl_name='Direktori' 
                        AND sql LIKE '%UNIQUE%'
                        AND sql LIKE '%KlijentId%'";
                    cmd.Transaction = transaction;
                    uniqueIndexExists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }

                if (uniqueIndexExists)
                {
                    Debug.WriteLine("[SCHEMA] Pronađen unique index na Direktori.KlijentId — krećem s migracijom...");

                   
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = @"
                            CREATE TABLE Direktori_new (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                KlijentId INTEGER NOT NULL,
                                ImePrezime TEXT NOT NULL,
                                DatumValjanosti TEXT,
                                TipValjanosti TEXT,
                                Status TEXT,
                                Kreiran TEXT NOT NULL DEFAULT (datetime('now')),
                                FOREIGN KEY (KlijentId) REFERENCES Klijenti(Id) ON DELETE CASCADE
                            )";
                        cmd.ExecuteNonQuery();
                    }

                    
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = @"
                            INSERT INTO Direktori_new (Id, KlijentId, ImePrezime, DatumValjanosti, TipValjanosti, Status, Kreiran)
                            SELECT Id, KlijentId, ImePrezime, DatumValjanosti, TipValjanosti, Status, Kreiran FROM Direktori";
                        cmd.ExecuteNonQuery();
                    }

                    
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = "DROP TABLE Direktori";
                        cmd.ExecuteNonQuery();
                    }

                    
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = "ALTER TABLE Direktori_new RENAME TO Direktori";
                        cmd.ExecuteNonQuery();
                    }

                    
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = transaction;
                        cmd.CommandText = "CREATE INDEX IX_Direktori_KlijentId ON Direktori(KlijentId)";
                        cmd.ExecuteNonQuery();
                    }

                    Debug.WriteLine("[SCHEMA] V2 migracija tabele Direktori uspješna.");
                }
                else
                {
                    Debug.WriteLine("[SCHEMA] Unique index ne postoji — V2 nije potrebna.");
                }

                SetVersion(conn, 2, transaction);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Debug.WriteLine($"[SCHEMA] GREŠKA u V2 migraciji: {ex.Message}");
                throw new InvalidOperationException($"Migracija baze podataka nije uspjela (V2): {ex.Message}", ex);
            }
        }

        private int GetCurrentVersion(SqliteConnection conn)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(Version) FROM __SchemaVersion";
            var result = cmd.ExecuteScalar();
            return result == DBNull.Value || result == null ? 0 : Convert.ToInt32(result);
        }

        private void SetVersion(SqliteConnection conn, int version, SqliteTransaction transaction = null)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = "INSERT INTO __SchemaVersion (Version, AppliedAt) VALUES (@v, @d)";
            cmd.Parameters.AddWithValue("@v", version);
            cmd.Parameters.AddWithValue("@d", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }
    }
}