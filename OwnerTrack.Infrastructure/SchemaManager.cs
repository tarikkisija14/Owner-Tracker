using Microsoft.Data.Sqlite;
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

            int ver = GetCurrentVersion(conn);
            Debug.WriteLine($"[SCHEMA] Trenutna verzija: {ver}");

            if (ver < 1) ApplyV1(conn);
            if (ver < 2) ApplyV2(conn);
            if (ver < 3) ApplyV3(conn);
            if (ver < 4) ApplyV4(conn);
            if (ver < 5) ApplyV5(conn);
            if (ver < 6) ApplyV6(conn);


            Debug.WriteLine($"[SCHEMA] Gotovo. Verzija: {GetCurrentVersion(conn)}");
        }

        private void ApplyV1(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] V1...");
            SetVersion(conn, 1);
        }

        private void ApplyV2(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] V2 — uklanjanje unique constrainta na Direktori...");
            using var tx = conn.BeginTransaction();
            try
            {
                bool hasUnique = false;
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT COUNT(*) FROM sqlite_master
                        WHERE type='index' AND tbl_name='Direktori'
                        AND sql LIKE '%UNIQUE%' AND sql LIKE '%KlijentId%'";
                    cmd.Transaction = tx;
                    hasUnique = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }

                if (hasUnique)
                {
                    ExecSql(conn, tx, @"
                        CREATE TABLE Direktori_new (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            KlijentId INTEGER NOT NULL,
                            ImePrezime TEXT NOT NULL,
                            DatumValjanosti TEXT,
                            TipValjanosti TEXT,
                            Status TEXT,
                            Kreiran TEXT NOT NULL DEFAULT (datetime('now')),
                            FOREIGN KEY (KlijentId) REFERENCES Klijenti(Id) ON DELETE CASCADE
                        )");
                    ExecSql(conn, tx, @"
                        INSERT INTO Direktori_new
                            (Id, KlijentId, ImePrezime, DatumValjanosti, TipValjanosti, Status, Kreiran)
                        SELECT Id, KlijentId, ImePrezime, DatumValjanosti, TipValjanosti, Status, Kreiran
                        FROM Direktori");
                    ExecSql(conn, tx, "DROP TABLE Direktori");
                    ExecSql(conn, tx, "ALTER TABLE Direktori_new RENAME TO Direktori");
                    ExecSql(conn, tx, "CREATE INDEX IX_Direktori_KlijentId ON Direktori(KlijentId)");
                }

                SetVersion(conn, 2, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new InvalidOperationException($"V2 neuspješna: {ex.Message}", ex);
            }
        }

        
        private void ApplyV3(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] V3 — rename ProcetatVlasnistva → ProcenatVlasnistva...");
            using var tx = conn.BeginTransaction();
            try
            {
                bool oldExists = ColumnExists(conn, tx, "Vlasnici", "ProcetatVlasnistva");
                bool newExists = ColumnExists(conn, tx, "Vlasnici", "ProcenatVlasnistva");

                if (oldExists && !newExists)
                    ExecSql(conn, tx,
                        "ALTER TABLE Vlasnici RENAME COLUMN ProcetatVlasnistva TO ProcenatVlasnistva");

                SetVersion(conn, 3, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new InvalidOperationException($"V3 neuspješna: {ex.Message}", ex);
            }
        }

       
        private void ApplyV4(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] V4 — dodavanje Obrisan kolona...");
            using var tx = conn.BeginTransaction();
            try
            {
                
                if (TableExists(conn, tx, "Klijenti"))
                    AddColumnIfMissing(conn, tx, "Klijenti", "Obrisan", "TEXT");
                if (TableExists(conn, tx, "Vlasnici"))
                    AddColumnIfMissing(conn, tx, "Vlasnici", "Obrisan", "TEXT");
                if (TableExists(conn, tx, "Direktori"))
                    AddColumnIfMissing(conn, tx, "Direktori", "Obrisan", "TEXT");

                SetVersion(conn, 4, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new InvalidOperationException($"V4 neuspješna: {ex.Message}", ex);
            }
        }

        private void ApplyV5(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] V5 — kreiranje AuditLogs tabele...");
            using var tx = conn.BeginTransaction();
            try
            {
                ExecSql(conn, tx, @"
            CREATE TABLE IF NOT EXISTS AuditLogs (
                Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                Tabela    TEXT    NOT NULL,
                EntitetId INTEGER,
                Akcija    TEXT    NOT NULL,
                Opis      TEXT,
                Vrijeme   TEXT    NOT NULL
            )");

                SetVersion(conn, 5, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new InvalidOperationException($"V5 neuspješna: {ex.Message}", ex);
            }
        }

        private void ApplyV6(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] V6 — dodavanje Jmbg, Email, Telefon kolona...");
            using var tx = conn.BeginTransaction();
            try
            {
                if (TableExists(conn, tx, "Direktori"))
                    AddColumnIfMissing(conn, tx, "Direktori", "Jmbg", "TEXT");
                if (TableExists(conn, tx, "Klijenti"))
                {
                    AddColumnIfMissing(conn, tx, "Klijenti", "Email", "TEXT");
                    AddColumnIfMissing(conn, tx, "Klijenti", "Telefon", "TEXT");
                }

                SetVersion(conn, 6, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new InvalidOperationException($"V6 neuspješna: {ex.Message}", ex);
            }
        }

        

        private void ExecSql(SqliteConnection conn, SqliteTransaction tx, string sql)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private bool ColumnExists(SqliteConnection conn, SqliteTransaction tx, string table, string column)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = $"PRAGMA table_info({table})";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                if (reader.GetString(1).Equals(column, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }
        private bool TableExists(SqliteConnection conn, SqliteTransaction tx, string table)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@t";
            cmd.Parameters.AddWithValue("@t", table);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private void AddColumnIfMissing(SqliteConnection conn, SqliteTransaction tx,
                                         string table, string column, string type)
        {
            if (!ColumnExists(conn, tx, table, column))
                ExecSql(conn, tx, $"ALTER TABLE {table} ADD COLUMN {column} {type}");
        }

        private int GetCurrentVersion(SqliteConnection conn)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT MAX(Version) FROM __SchemaVersion";
            var result = cmd.ExecuteScalar();
            return result == DBNull.Value || result == null ? 0 : Convert.ToInt32(result);
        }

        private void SetVersion(SqliteConnection conn, int version, SqliteTransaction? tx = null)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "INSERT INTO __SchemaVersion (Version, AppliedAt) VALUES (@v, @d)";
            cmd.Parameters.AddWithValue("@v", version);
            cmd.Parameters.AddWithValue("@d", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.ExecuteNonQuery();
        }
    }
}