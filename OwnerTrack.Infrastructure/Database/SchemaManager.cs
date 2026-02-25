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
            if (ver < 7) ApplyV7(conn);
            if (ver < 8) ApplyV8(conn);

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


        /// <summary>
        /// Poziva se nakon reseta baze da vrati djelatnosti (INSERT OR IGNORE).
        /// </summary>
        public void ReseedDjelatnosti()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();
            try
            {
                InsertDjelatnosti(conn, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new InvalidOperationException($"ReseedDjelatnosti neuspješan: {ex.Message}", ex);
            }
        }

        private void ApplyV8(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] V8 — kreiranje Djelatnosti tabele i seed KD BiH šifara...");
            using var tx = conn.BeginTransaction();
            try
            {
                ExecSql(conn, tx, @"
                    CREATE TABLE IF NOT EXISTS Djelatnosti (
                        Sifra TEXT PRIMARY KEY NOT NULL,
                        Naziv TEXT NOT NULL
                    )");

                InsertDjelatnosti(conn, tx);

                SetVersion(conn, 8, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new InvalidOperationException($"V8 neuspješna: {ex.Message}", ex);
            }
        }

        private void InsertDjelatnosti(SqliteConnection conn, SqliteTransaction tx)
        {
            var djelatnosti = new (string Sifra, string Naziv)[]
                {
                    ("01.13", "Uzgoj povrća, dinja i lubenica, korjenastog i gomoljastog povrća"),
                    ("01.25", "Uzgoj bobičastog, orašastog i ostalog voća"),
                    ("01.41", "Uzgoj muznih krava"),
                    ("03.22", "Slatkovodna akvakultura"),
                    ("10.51", "Proizvodnja mlijeka, mliječnih proizvoda i sira"),
                    ("10.71", "Proizvodnja hljeba; svježih peciva i kolača"),
                    ("13.20", "Tkanje tekstila"),
                    ("13.92", "Proizvodnja gotovih tekstilnih proizvoda, osim odjeće"),
                    ("14.13", "Proizvodnja ostale vanjske odjeće"),
                    ("15.11", "Štavljenje i obrada kože; dorada i bojenje krzna"),
                    ("15.20", "Proizvodnja obuće"),
                    ("16.10", "Piljenje i blanjanje drva (proizvodnja rezane građe); impregnacija drveta"),
                    ("16.23", "Proizvodnja ostale građevne stolarije i elemenata"),
                    ("16.24", "Proizvodnja ambalaže od drva"),
                    ("20.42", "Proizvodnja parfema i toaletno-kozmetičkih preparata"),
                    ("22.22", "Proizvodnja ambalaže od plastičnih masa"),
                    ("22.23", "Proizvodnja proizvoda od plastičnih masa za građevinarstvo"),
                    ("25.11", "Proizvodnja metalnih konstrukcija i njihovih dijelova"),
                    ("25.62", "Mašinska obrada metala"),
                    ("35.11", "Proizvodnja električne energije"),
                    ("35.30", "Proizvodnja i snabdijevanje parom i klimatizacija"),
                    ("45.11", "Trgovina automobilima i motornim vozilima lake kategorije"),
                    ("45.20", "Održavanje i popravak motornih vozila"),
                    ("45.32", "Trgovina na malo dijelovima i priborom za motorna vozila"),
                    ("46.12", "Posredovanje u trgovini gorivima, rudama, metalima i industrijskim hemikalijama"),
                    ("46.39", "Nespecijalizirana trgovina na veliko hranom, pićima i duhanskim proizvodima"),
                    ("46.69", "Trgovina na veliko ostalim strojevima i opremom"),
                    ("46.73", "Trgovina na veliko drvom, građevinskim materijalom i sanitarnom opremom"),
                    ("46.74", "Trgovina na veliko željeznom robom, instalacijskim materijalom i opremom za vodovod i grijanje"),
                    ("46.90", "Nespecijalizirana trgovina na veliko"),
                    ("47.19", "Ostala trgovina na malo u nespecijaliziranim prodavaonicama"),
                    ("47.30", "Trgovina na malo motornim gorivima u specijaliziranim prodavnicama"),
                    ("47.52", "Trgovina na malo željeznom robom, bojama i staklom u specijaliziranim prodavaonicama"),
                    ("47.59", "Trgovina na malo namještajem, opremom za rasvjetu i ostalim proizvodima za domaćinstvo u specijaliziranim prodavnicama"),
                    ("47.73", "Ljekarne"),
                    ("47.76", "Trgovina na malo cvijećem, sadnicama, sjemenjem, gnojivom"),
                    ("47.78", "Ostala trgovina na malo novom robom u specijaliziranim prodavnicama"),
                    ("47.91", "Trgovina na malo putem pošte ili interneta"),
                    ("49.39", "Ostali kopneni prijevoz putnika, d. n."),
                    ("49.41", "Cestovni prijevoz robe"),
                    ("55.10", "Hoteli i sličan smještaj"),
                    ("56.10", "Djelatnosti restorana i ostalih objekata za pripremu i usluživanje hrane"),
                    ("62.01", "Računarsko programiranje"),
                    ("68.10", "Kupovina i prodaja vlastitih nekretnina"),
                    ("69.10", "PRAVNE DJELATNOSTI"),
                    ("69.20", "Računovodstvene, knjigovodstvene i revizijske djelatnosti; porezno savjetovanje"),
                    ("74.12", "Računovodstveni, knjigovodstveni poslovi, porezno savjetovanje"),
                    ("75.00", "Veterinarske djelatnosti"),
                    ("77.11", "Iznajmljivanje i davanje u zakup (leasing) automobila i motornih vozila lake kategorije"),
                    ("79.90", "Udruženje građana KOŠARKAŠKI KLUB"),
                    ("80.10", "Djelatnosti privatne zaštite"),
                    ("87.10", "Djelatnosti ustanova za njegu"),
                    ("93.19", "Udruženje građana"),
                    ("94.12", "Djelatnosti strukovnih članskih organizacija"),
                    ("94.91", "Djelatnosti vjerskih organizacija"),
                    ("94.99", "Okupljanje oboljelih od dijabetesa"),
                    ("96.03", "Pogrebne i srodne djelatnosti")
                };

            var insertCmd = conn.CreateCommand();
            insertCmd.Transaction = tx;
            insertCmd.CommandText = "INSERT OR IGNORE INTO Djelatnosti (Sifra, Naziv) VALUES (@s, @n)";
            insertCmd.Parameters.Add(new SqliteParameter("@s", ""));
            insertCmd.Parameters.Add(new SqliteParameter("@n", ""));

            foreach (var d in djelatnosti)
            {
                insertCmd.Parameters["@s"].Value = d.Sifra;
                insertCmd.Parameters["@n"].Value = d.Naziv;
                insertCmd.ExecuteNonQuery();
            }

            Debug.WriteLine($"[SCHEMA] InsertDjelatnosti — obrađeno {djelatnosti.Length} šifara.");
        }

        private void ApplyV7(SqliteConnection conn)
        {
            Debug.WriteLine("[SCHEMA] V7 — uklanjanje unique indexa koji blokiraju soft-delete...");
            using var tx = conn.BeginTransaction();
            try
            {

                DropIndexIfExists(conn, tx, "IX_Klijenti_Naziv");
                DropIndexIfExists(conn, tx, "IX_Klijenti_IdBroj");
                DropIndexIfExists(conn, tx, "IX_Vlasnici_KlijentId_ImePrezime");

                SetVersion(conn, 7, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new InvalidOperationException($"V7 neuspješna: {ex.Message}", ex);
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

        private void DropIndexIfExists(SqliteConnection conn, SqliteTransaction tx, string indexName)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='index' AND name=@n";
            cmd.Parameters.AddWithValue("@n", indexName);
            bool exists = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            if (exists)
                ExecSql(conn, tx, $"DROP INDEX IF EXISTS \"{indexName}\"");
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