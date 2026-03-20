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

            ExecSql(conn, null, @"
                CREATE TABLE IF NOT EXISTS __SchemaVersion (
                    Version   INTEGER NOT NULL,
                    AppliedAt TEXT    NOT NULL
                )");

            int version = GetCurrentVersion(conn);
            Debug.WriteLine($"[SCHEMA] Trenutna verzija: {version}");

            if (version >= 1 && !TableExists(conn, null, "Klijenti"))
            {
                Debug.WriteLine("[SCHEMA] Verzija >= 1 ali tabele nedostaju — resetujem i kreiram od nule.");
                ResetVersionHistory(conn);
                version = 0;
            }

            if (version < 1) ApplyV1(conn);
            if (version < 2) ApplyV2(conn);
            if (version < 3) ApplyV3(conn);
            if (version < 4) ApplyV4(conn);
            if (version < 5) ApplyV5(conn);
            if (version < 6) ApplyV6(conn);
            if (version < 7) ApplyV7(conn);
            if (version < 8) ApplyV8(conn);
            if (version < 9) ApplyV9(conn);
            if (version < 10) ApplyV10(conn);

            Debug.WriteLine($"[SCHEMA] Gotovo. Verzija: {GetCurrentVersion(conn)}");
        }

        

        private void ApplyV1(SqliteConnection conn) =>
            ApplyMigration(conn, 1, "kreiranje svih osnovnih tabela", (c, tx) =>
            {
                ExecSql(c, tx, @"
                    CREATE TABLE IF NOT EXISTS Djelatnosti (
                        Sifra TEXT PRIMARY KEY NOT NULL,
                        Naziv TEXT NOT NULL
                    )");

                ExecSql(c, tx, @"
                    CREATE TABLE IF NOT EXISTS Klijenti (
                        Id               INTEGER PRIMARY KEY AUTOINCREMENT,
                        Naziv            TEXT NOT NULL,
                        IdBroj           TEXT,
                        Adresa           TEXT,
                        SifraDjelatnosti TEXT,
                        VrstaKlijenta    TEXT,
                        DatumOsnivanja   TEXT,
                        DatumUspostave   TEXT,
                        Velicina         TEXT,
                        PepRizik         TEXT,
                        UboRizik         TEXT,
                        GotovinaRizik    TEXT,
                        GeografskiRizik  TEXT,
                        UkupnaProcjena   TEXT,
                        DatumProcjene    TEXT,
                        OvjeraCr         TEXT,
                        Status           TEXT NOT NULL DEFAULT 'AKTIVAN',
                        Napomena         TEXT,
                        Email            TEXT,
                        Telefon          TEXT,
                        Kreiran          TEXT NOT NULL DEFAULT (datetime('now')),
                        Azuriran         TEXT,
                        Obrisan          TEXT,
                        FOREIGN KEY (SifraDjelatnosti) REFERENCES Djelatnosti(Sifra) ON DELETE RESTRICT
                    )");

                ExecSql(c, tx, "CREATE INDEX IF NOT EXISTS IX_Klijenti_Naziv  ON Klijenti(Naziv)");
                ExecSql(c, tx, "CREATE INDEX IF NOT EXISTS IX_Klijenti_IdBroj ON Klijenti(IdBroj)");

                ExecSql(c, tx, @"
                    CREATE TABLE IF NOT EXISTS Vlasnici (
                        Id                       INTEGER PRIMARY KEY AUTOINCREMENT,
                        KlijentId                INTEGER NOT NULL,
                        ImePrezime               TEXT NOT NULL,
                        DatumValjanostiDokumenta TEXT,
                        ProcenatVlasnistva       REAL,
                        DatumUtvrdjivanja        TEXT,
                        IzvorPodatka             TEXT,
                        Status                   TEXT NOT NULL DEFAULT 'AKTIVAN',
                        Kreiran                  TEXT NOT NULL DEFAULT (datetime('now')),
                        Obrisan                  TEXT,
                        FOREIGN KEY (KlijentId) REFERENCES Klijenti(Id) ON DELETE CASCADE
                    )");

                ExecSql(c, tx, "CREATE INDEX IF NOT EXISTS IX_Vlasnici_KlijentId_ImePrezime ON Vlasnici(KlijentId, ImePrezime)");

                ExecSql(c, tx, @"
                    CREATE TABLE IF NOT EXISTS Direktori (
                        Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                        KlijentId       INTEGER NOT NULL,
                        ImePrezime      TEXT NOT NULL,
                        DatumValjanosti TEXT,
                        TipValjanosti   TEXT,
                        Jmbg            TEXT,
                        Status          TEXT NOT NULL DEFAULT 'AKTIVAN',
                        Kreiran         TEXT NOT NULL DEFAULT (datetime('now')),
                        Obrisan         TEXT,
                        FOREIGN KEY (KlijentId) REFERENCES Klijenti(Id) ON DELETE CASCADE
                    )");

                ExecSql(c, tx, "CREATE INDEX IF NOT EXISTS IX_Direktori_KlijentId ON Direktori(KlijentId)");

                ExecSql(c, tx, @"
                    CREATE TABLE IF NOT EXISTS Ugovori (
                        Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                        KlijentId     INTEGER NOT NULL UNIQUE,
                        VrstaUgovora  TEXT,
                        StatusUgovora TEXT NOT NULL,
                        DatumUgovora  TEXT,
                        Napomena      TEXT,
                        Kreiran       TEXT NOT NULL DEFAULT (datetime('now')),
                        Obrisan       TEXT,
                        FOREIGN KEY (KlijentId) REFERENCES Klijenti(Id) ON DELETE CASCADE
                    )");

                ExecSql(c, tx, "CREATE UNIQUE INDEX IF NOT EXISTS IX_Ugovori_KlijentId ON Ugovori(KlijentId)");

                ExecSql(c, tx, @"
                    CREATE TABLE IF NOT EXISTS AuditLogs (
                        Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                        Tabela    TEXT    NOT NULL,
                        EntitetId INTEGER,
                        Akcija    TEXT    NOT NULL,
                        Opis      TEXT,
                        Vrijeme   TEXT    NOT NULL
                    )");

                InsertDjelatnosti(c, tx);
            });

        private void ApplyV2(SqliteConnection conn) =>
            ApplyMigration(conn, 2, "uklanjanje unique constrainta na Direktori", (c, tx) =>
            {
                bool hasUnique;
                using (var cmd = c.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT COUNT(*) FROM sqlite_master
                        WHERE type='index' AND tbl_name='Direktori'
                        AND sql LIKE '%UNIQUE%' AND sql LIKE '%KlijentId%'";
                    cmd.Transaction = tx;
                    hasUnique = Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }

                if (!hasUnique) return;

                ExecSql(c, tx, "DROP TABLE IF EXISTS Direktori_new");
                ExecSql(c, tx, @"
                    CREATE TABLE Direktori_new (
                        Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                        KlijentId       INTEGER NOT NULL,
                        ImePrezime      TEXT NOT NULL,
                        DatumValjanosti TEXT,
                        TipValjanosti   TEXT,
                        Status          TEXT,
                        Kreiran         TEXT NOT NULL DEFAULT (datetime('now')),
                        FOREIGN KEY (KlijentId) REFERENCES Klijenti(Id) ON DELETE CASCADE
                    )");
                ExecSql(c, tx, @"
                    INSERT INTO Direktori_new
                        (Id, KlijentId, ImePrezime, DatumValjanosti, TipValjanosti, Status, Kreiran)
                    SELECT Id, KlijentId, ImePrezime, DatumValjanosti, TipValjanosti, Status, Kreiran
                    FROM Direktori");
                ExecSql(c, tx, "DROP TABLE Direktori");
                ExecSql(c, tx, "ALTER TABLE Direktori_new RENAME TO Direktori");
                ExecSql(c, tx, "CREATE INDEX IF NOT EXISTS IX_Direktori_KlijentId ON Direktori(KlijentId)");
            });

        private void ApplyV3(SqliteConnection conn) =>
            ApplyMigration(conn, 3, "rename ProcetatVlasnistva → ProcenatVlasnistva", (c, tx) =>
            {
                bool oldExists = ColumnExists(c, tx, "Vlasnici", "ProcetatVlasnistva");
                bool newExists = ColumnExists(c, tx, "Vlasnici", "ProcenatVlasnistva");

                if (oldExists && !newExists)
                    ExecSql(c, tx, "ALTER TABLE Vlasnici RENAME COLUMN ProcetatVlasnistva TO ProcenatVlasnistva");
            });

        private void ApplyV4(SqliteConnection conn) =>
            ApplyMigration(conn, 4, "dodavanje Obrisan kolona", (c, tx) =>
            {
                foreach (var table in new[] { "Klijenti", "Vlasnici", "Direktori" })
                    if (TableExists(c, tx, table))
                        AddColumnIfMissing(c, tx, table, "Obrisan", "TEXT");
            });

        private void ApplyV5(SqliteConnection conn) =>
            ApplyMigration(conn, 5, "kreiranje AuditLogs tabele", (c, tx) =>
            {
                ExecSql(c, tx, @"
                    CREATE TABLE IF NOT EXISTS AuditLogs (
                        Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                        Tabela    TEXT    NOT NULL,
                        EntitetId INTEGER,
                        Akcija    TEXT    NOT NULL,
                        Opis      TEXT,
                        Vrijeme   TEXT    NOT NULL
                    )");
            });

        private void ApplyV6(SqliteConnection conn) =>
            ApplyMigration(conn, 6, "dodavanje Jmbg, Email, Telefon kolona", (c, tx) =>
            {
                if (TableExists(c, tx, "Direktori"))
                    AddColumnIfMissing(c, tx, "Direktori", "Jmbg", "TEXT");

                if (TableExists(c, tx, "Klijenti"))
                {
                    AddColumnIfMissing(c, tx, "Klijenti", "Email", "TEXT");
                    AddColumnIfMissing(c, tx, "Klijenti", "Telefon", "TEXT");
                }
            });

        private void ApplyV7(SqliteConnection conn) =>
            ApplyMigration(conn, 7, "uklanjanje unique indexa koji blokiraju soft-delete", (c, tx) =>
            {
                DropIndexIfExists(c, tx, "IX_Klijenti_Naziv");
                DropIndexIfExists(c, tx, "IX_Klijenti_IdBroj");
                DropIndexIfExists(c, tx, "IX_Vlasnici_KlijentId_ImePrezime");
            });

        private void ApplyV8(SqliteConnection conn) =>
            ApplyMigration(conn, 8, "kreiranje Djelatnosti tabele i seed KD BiH šifara", (c, tx) =>
            {
                ExecSql(c, tx, @"
                    CREATE TABLE IF NOT EXISTS Djelatnosti (
                        Sifra TEXT PRIMARY KEY NOT NULL,
                        Naziv TEXT NOT NULL
                    )");

                InsertDjelatnosti(c, tx);
            });

        private void ApplyV9(SqliteConnection conn) =>
            ApplyMigration(conn, 9, "dodavanje Azuriran kolone na Klijenti", (c, tx) =>
            {
                AddColumnIfMissing(c, tx, "Klijenti", "Azuriran", "TEXT");
            });

        private void ApplyV10(SqliteConnection conn) =>
            ApplyMigration(conn, 10, "dodavanje VrstaUgovora, Napomena, Obrisan na Ugovori", (c, tx) =>
            {
                AddColumnIfMissing(c, tx, "Ugovori", "VrstaUgovora", "TEXT");
                AddColumnIfMissing(c, tx, "Ugovori", "Napomena", "TEXT");
                AddColumnIfMissing(c, tx, "Ugovori", "Obrisan", "TEXT");
            });

        

        public void ReseedDjelatnosti()
        {
            using var conn = new SqliteConnection(_connectionString);
            conn.Open();
            ApplyMigration(conn, version: -1, "reseed Djelatnosti", (c, tx) =>
                InsertDjelatnosti(c, tx),
            recordVersion: false);
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
                ("96.03", "Pogrebne i srodne djelatnosti"),
            };

            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "INSERT OR IGNORE INTO Djelatnosti (Sifra, Naziv) VALUES (@s, @n)";
            cmd.Parameters.Add(new SqliteParameter("@s", ""));
            cmd.Parameters.Add(new SqliteParameter("@n", ""));

            foreach (var (sifra, naziv) in djelatnosti)
            {
                cmd.Parameters["@s"].Value = sifra;
                cmd.Parameters["@n"].Value = naziv;
                cmd.ExecuteNonQuery();
            }

            Debug.WriteLine($"[SCHEMA] InsertDjelatnosti — obrađeno {djelatnosti.Length} šifara.");
        }

        // ── Migration runner ──────────────────────────────────────────────────────

        /// <summary>
        /// Wraps <paramref name="work"/> in a transaction. Rolls back and rethrows on failure.
        /// When <paramref name="recordVersion"/> is true, records <paramref name="version"/>
        /// in __SchemaVersion before committing.
        /// </summary>
        private void ApplyMigration(
            SqliteConnection conn,
            int version,
            string description,
            Action<SqliteConnection, SqliteTransaction> work,
            bool recordVersion = true)
        {
            Debug.WriteLine($"[SCHEMA] V{version} — {description}...");
            using var tx = conn.BeginTransaction();
            try
            {
                work(conn, tx);
                if (recordVersion)
                    SetVersion(conn, version, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                tx.Rollback();
                throw new InvalidOperationException($"V{version} neuspješna: {ex.Message}", ex);
            }
        }

        

        private void ExecSql(SqliteConnection conn, SqliteTransaction? tx, string sql)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private bool TableExists(SqliteConnection conn, SqliteTransaction? tx, string table)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@t";
            cmd.Parameters.AddWithValue("@t", table);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
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
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
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

        private void ResetVersionHistory(SqliteConnection conn)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM __SchemaVersion";
            cmd.ExecuteNonQuery();
        }
    }
}