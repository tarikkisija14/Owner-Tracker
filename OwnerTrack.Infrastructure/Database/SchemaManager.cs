using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
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
            if (version < 11) ApplyV11(conn);
            if (version < 12) ApplyV12(conn);
            if (version < 13) ApplyV13(conn);
            if (version < 14) ApplyV14(conn);
            if (version < 15) ApplyV15(conn);
            if (version < 16) ApplyV16(conn);

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

        private void ApplyV11(SqliteConnection conn) =>
            ApplyMigration(conn, 11, "dodavanje PEP i indikatora rizika kolona na Klijenti", (c, tx) =>
            {
                AddColumnIfMissing(c, tx, "Klijenti", "PepImePrezime", "TEXT");
                AddColumnIfMissing(c, tx, "Klijenti", "PepFunkcija", "TEXT");
                AddColumnIfMissing(c, tx, "Klijenti", "PepPovezanost", "TEXT");
                AddColumnIfMissing(c, tx, "Klijenti", "PepMjerePoduzete", "TEXT");
                AddColumnIfMissing(c, tx, "Klijenti", "PepDatumProvjere", "TEXT");
                AddColumnIfMissing(c, tx, "Klijenti", "OpciIndikatoriRizika", "TEXT");
                AddColumnIfMissing(c, tx, "Klijenti", "IndikatoriIdentifikacijeRizika", "TEXT");
                AddColumnIfMissing(c, tx, "Klijenti", "IndikatoriTransakcijaRizika", "TEXT");
            });

        private void ApplyV12(SqliteConnection conn) =>
            ApplyMigration(conn, 12, "dodavanje RizikObrazacJson kolone na Klijenti", (c, tx) =>
            {
                AddColumnIfMissing(c, tx, "Klijenti", "RizikObrazacJson", "TEXT");
            });

        // Sprječava duplikate aktivnih klijenata na DB nivou (dosad je jedina
        // zaštita bila aplikacijska provjera u FrmDodajKlijent.ValidateFields,
        // koja ostavlja TOCTOU prozor između provjere i INSERT-a). Partial
        // unique index (WHERE Obrisan IS NULL) — namjerno isti obrazac kao
        // postojeća aplikacijska provjera (k.Obrisan == null) — tako da
        // arhiviranje i ponovno korištenje istog naziva/IdBroj-a i dalje rade
        // (arhivirani zapisi ne blokiraju novi aktivni zapis s istim poljem).
        //
        // Neke instalacije (baze kreirane prije nego što je ovaj SchemaManager
        // postao jedini put kreiranja sheme) imaju na Klijenti.Naziv/IdBroj
        // stari, ne-partial UNIQUE (inline column constraint, vidljiv kao
        // sqlite_autoindex_Klijenti_*), koji blokira upravo scenarij koji ova
        // migracija treba omogućiti — ponovno korištenje naziva/IdBroj-a nakon
        // arhiviranja. Takav constraint se u SQLite-u ne može ukloniti sa ALTER
        // TABLE, pa se tabela po potrebi rebuilda (isti obrazac kao ApplyV2 za
        // Direktori), a zatim se dodaje ispravan partial unique index.
        private void ApplyV13(SqliteConnection conn)
        {
            // PRAGMA foreign_keys ne smije se mijenjati unutar transakcije
            // (SQLite je tada tiho ignoriše), pa provjera i eventualno
            // isključivanje moraju biti ovdje, prije nego ApplyMigration
            // otvori transakciju. Rebuild u RebuildKlijentiWithoutInlineUnique
            // radi DROP TABLE Klijenti — bez ovoga bi SQLite (Microsoft.Data.Sqlite
            // ima foreign_keys uključen po defaultu) to protumačio kao brisanje
            // svakog Vlasnika/Direktora/Ugovora koji na taj red pokazuje (ON
            // DELETE CASCADE) i odbio operaciju/obrisao djecu.
            bool needsRebuild = HasLegacyFullUniqueConstraint(conn, null, "Klijenti", "Naziv") ||
                                 HasLegacyFullUniqueConstraint(conn, null, "Klijenti", "IdBroj");

            if (needsRebuild)
                ExecSql(conn, null, "PRAGMA foreign_keys = OFF");

            try
            {
                ApplyMigration(conn, 13, "dodavanje unique indexa na Klijenti.Naziv/IdBroj za aktivne zapise", (c, tx) =>
                {
                    if (needsRebuild)
                        RebuildKlijentiWithoutInlineUnique(c, tx);

                    ExecSql(c, tx,
                        "CREATE UNIQUE INDEX IF NOT EXISTS UX_Klijenti_Naziv_Active " +
                        "ON Klijenti(Naziv) WHERE Obrisan IS NULL");
                    ExecSql(c, tx,
                        "CREATE UNIQUE INDEX IF NOT EXISTS UX_Klijenti_IdBroj_Active " +
                        "ON Klijenti(IdBroj) WHERE Obrisan IS NULL");
                });

                if (needsRebuild)
                    VerifyNoOrphansAfterKlijentiRebuild(conn);
            }
            finally
            {
                if (needsRebuild)
                    ExecSql(conn, null, "PRAGMA foreign_keys = ON");
            }
        }

        // Optimistic-concurrency token za Klijenti/Vlasnici/Direktori — vidi
        // OwnerTrackDbContext.ConfigureConcurrencyTokens. DEFAULT 0 znači da
        // svi postojeći redovi kreću sa istom početnom vrijednošću (0), što je
        // sigurno: prvi sljedeći Save na bilo kojem od njih će EF-ov
        // "WHERE Version = 0" ispravno pogoditi jer ništa nije moglo promijeniti
        // taj red između migracije i tog Save-a.
        private void ApplyV14(SqliteConnection conn) =>
            ApplyMigration(conn, 14, "dodavanje Version kolone (optimistic concurrency) na Klijenti/Vlasnici/Direktori", (c, tx) =>
            {
                AddColumnIfMissing(c, tx, "Klijenti", "Version", "INTEGER NOT NULL DEFAULT 0");
                AddColumnIfMissing(c, tx, "Vlasnici", "Version", "INTEGER NOT NULL DEFAULT 0");
                AddColumnIfMissing(c, tx, "Direktori", "Version", "INTEGER NOT NULL DEFAULT 0");
            });

        private void ApplyV15(SqliteConnection conn) =>
            ApplyMigration(conn, 15, "kreiranje tabele WarningAcknowledgements (pregledana upozorenja)", (c, tx) =>
            {
                ExecSql(c, tx, @"
                    CREATE TABLE IF NOT EXISTS WarningAcknowledgements (
                        Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                        EntityType  TEXT NOT NULL,
                        EntityId    INTEGER NOT NULL,
                        DatumIsteka TEXT NOT NULL,
                        Napomena    TEXT,
                        Korisnik    TEXT,
                        Vrijeme     TEXT NOT NULL DEFAULT (datetime('now'))
                    )");

                ExecSql(c, tx, @"
                    CREATE UNIQUE INDEX IF NOT EXISTS UX_WarningAcknowledgements_Entity
                    ON WarningAcknowledgements(EntityType, EntityId, DatumIsteka)");
            });

        // EF Core enume (StatusEntiteta, VrstaKlijenta) čita i piše kao BROJEVE (0, 1, 2...),
        // a starije verzije aplikacije su u iste kolone upisivale tekst ('AKTIVAN', 'PRAVNO LICE').
        // Za takve redove EF upiti tipa Status == AKTIVAN (Status = 0) ne pogađaju ništa, pa iz
        // upozorenja i drugih upita tiho nestaju svi stari vlasnici/direktori. Ova migracija
        // pretvara poznata tekstualna imena u brojeve koje EF očekuje (kolone su TEXT affinity,
        // pa se broj sprema kao '0', isto kao što ga upisuje EF). Idempotentna je: dira samo
        // redove koji još sadrže tekstualno ime; nepoznate vrijednosti ostaju netaknute.
        private void ApplyV16(SqliteConnection conn) =>
            ApplyMigration(conn, 16, "pretvaranje tekstualnih enum vrijednosti (Status, VrstaKlijenta) u brojčane", (c, tx) =>
            {
                foreach (var table in new[] { "Klijenti", "Vlasnici", "Direktori" })
                {
                    if (!TableExists(c, tx, table)) continue;

                    ExecSql(c, tx, $@"
                        UPDATE {table}
                        SET Status = CASE UPPER(TRIM(Status))
                                         WHEN 'AKTIVAN'   THEN 0
                                         WHEN 'NEAKTIVAN' THEN 1
                                         WHEN 'ARHIVIRAN' THEN 2
                                     END
                        WHERE UPPER(TRIM(Status)) IN ('AKTIVAN', 'NEAKTIVAN', 'ARHIVIRAN')");
                }

                if (TableExists(c, tx, "Klijenti"))
                {
                    ExecSql(c, tx, @"
                        UPDATE Klijenti
                        SET VrstaKlijenta = CASE UPPER(REPLACE(REPLACE(TRIM(VrstaKlijenta), ' ', ''), '_', ''))
                                                WHEN 'PRAVNOLICE'    THEN 0
                                                WHEN 'FIZICKOLICE'   THEN 1
                                                WHEN 'FIZIČKOLICE'   THEN 1
                                                WHEN 'UDRUZENJE'     THEN 2
                                                WHEN 'UDRUŽENJE'     THEN 2
                                                WHEN 'OBRTNIK'       THEN 3
                                                WHEN 'JAVNAUSTANOVA' THEN 4
                                            END
                        WHERE UPPER(REPLACE(REPLACE(TRIM(VrstaKlijenta), ' ', ''), '_', '')) IN
                              ('PRAVNOLICE', 'FIZICKOLICE', 'FIZIČKOLICE', 'UDRUZENJE', 'UDRUŽENJE', 'OBRTNIK', 'JAVNAUSTANOVA')");
                }
            });

        // Nakon rebuilda Klijenti tabele (koji je rađen sa foreign_keys=OFF)
        // provjerava da nijedan Vlasnik/Direktor/Ugovor ne pokazuje na
        // nepostojeći KlijentId — čisto sigurnosna provjera, jer INSERT INTO
        // Klijenti_new SELECT ... FROM Klijenti kopira retke 1:1 po Id-u pa do
        // orphan zapisa ne bi smjelo doći, ali greška ovdje mora prekinuti
        // migraciju umjesto da tiho ostavi nekonzistentnu bazu.
        private static void VerifyNoOrphansAfterKlijentiRebuild(SqliteConnection conn)
        {
            foreach (var (table, fk) in new[] { ("Vlasnici", "KlijentId"), ("Direktori", "KlijentId"), ("Ugovori", "KlijentId") })
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = $"SELECT COUNT(*) FROM {table} t WHERE NOT EXISTS (SELECT 1 FROM Klijenti k WHERE k.Id = t.{fk})";
                int orphanCount = Convert.ToInt32(cmd.ExecuteScalar());
                if (orphanCount > 0)
                    throw new InvalidOperationException(
                        $"Rebuild Klijenti tabele je proizveo {orphanCount} orphan zapis(a) u {table} — migracija prekinuta.");
            }
        }

        // Otkriva UNIQUE definisan direktno na koloni (npr. "Naziv TEXT NOT
        // NULL UNIQUE" u CREATE TABLE), koji SQLite predstavlja kao automatski
        // indeks "sqlite_autoindex_<tabela>_<n>" čiji je jedini sadržaj ta
        // kolona — za razliku od imenovanih indexa koje ova klasa inače pravi.
        private static bool HasLegacyFullUniqueConstraint(SqliteConnection conn, SqliteTransaction? tx, string table, string column)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='index' AND tbl_name=@t AND name LIKE 'sqlite_autoindex_%'";
            cmd.Parameters.AddWithValue("@t", table);

            var autoIndexNames = new List<string>();
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    autoIndexNames.Add(reader.GetString(0));

            foreach (var indexName in autoIndexNames)
            {
                using var infoCmd = conn.CreateCommand();
                infoCmd.Transaction = tx;
                infoCmd.CommandText = $"PRAGMA index_info(\"{indexName}\")";
                using var infoReader = infoCmd.ExecuteReader();

                var columns = new List<string>();
                while (infoReader.Read())
                    columns.Add(infoReader.GetString(2));

                if (columns.Count == 1 && columns[0].Equals(column, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        // Rebuilda Klijenti tabelu bez inline UNIQUE na Naziv/IdBroj, čuvajući
        // sve ostale kolone, tipove, NOT NULL, default vrijednosti, FK i sve
        // postojeće podatke. Kolone se čitaju iz PRAGMA table_info umjesto da
        // se ručno prepisuju, da rebuild ne zavisi od ručno održavane liste od
        // 30+ kolona koja bi lako izašla iz sinhronizacije sa stvarnom šemom.
        private void RebuildKlijentiWithoutInlineUnique(SqliteConnection conn, SqliteTransaction tx)
        {
            var columns = new List<(string Name, string Type, bool NotNull, string? Default)>();
            using (var cmd = conn.CreateCommand())
            {
                cmd.Transaction = tx;
                cmd.CommandText = "PRAGMA table_info(Klijenti)";
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string name = reader.GetString(1);
                    string type = reader.GetString(2);
                    bool notNull = reader.GetInt32(3) != 0;
                    string? dflt = reader.IsDBNull(4) ? null : reader.GetString(4);
                    columns.Add((name, type, notNull, dflt));
                }
            }

            var columnDefs = new List<string>();
            foreach (var col in columns)
            {
                if (col.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    columnDefs.Add("Id INTEGER PRIMARY KEY AUTOINCREMENT");
                    continue;
                }

                string def = $"{col.Name} {col.Type}";
                if (col.NotNull) def += " NOT NULL";
                if (col.Default != null) def += $" DEFAULT {col.Default}";
                columnDefs.Add(def);
            }
            columnDefs.Add("FOREIGN KEY (SifraDjelatnosti) REFERENCES Djelatnosti(Sifra) ON DELETE RESTRICT");

            string columnList = string.Join(", ", columns.ConvertAll(c => c.Name));

            ExecSql(conn, tx, "DROP TABLE IF EXISTS Klijenti_new");
            ExecSql(conn, tx, $"CREATE TABLE Klijenti_new ({string.Join(", ", columnDefs)})");
            ExecSql(conn, tx, $"INSERT INTO Klijenti_new ({columnList}) SELECT {columnList} FROM Klijenti");
            ExecSql(conn, tx, "DROP TABLE Klijenti");
            ExecSql(conn, tx, "ALTER TABLE Klijenti_new RENAME TO Klijenti");

            // Neimenovani indexi (IX_Klijenti_Naziv/IdBroj) se ne prave ovdje
            // jer ih zamjenjuje partial unique index koji ApplyV13 pravi odmah
            // nakon poziva ove metode.
        }

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