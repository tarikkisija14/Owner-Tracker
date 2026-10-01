using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace OwnerTrack.Infrastructure.Database
{
    public static class DbContextFactory
    {
        public const string DbFileName = "Firme.db";
        private const string ConfigFileName = "database.path";

        // Lokalni folder korisnika — tu ostaju postavke, log i zadnje korisničko ime
        // čak i kad je baza na serveru (ne smiju se miješati između računara).
        public static string LocalDataDirectory { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "OwnerTrack");

        public static string LocalDbPath => Path.Combine(LocalDataDirectory, DbFileName);

        private static string ConfigPath => Path.Combine(LocalDataDirectory, ConfigFileName);

        // Folder dijeljene baze na serveru (iz database.path) ili null = lokalna baza.
        public static string? SharedFolder { get; private set; }

        public static bool IsShared => SharedFolder is not null;

        public static string DbPath { get; private set; } = string.Empty;

        public static string ConnectionString { get; private set; } = string.Empty;

        static DbContextFactory()
        {
            Directory.CreateDirectory(LocalDataDirectory);
            Reload();
        }

        public static void UseSharedFolder(string folder)
        {
            File.WriteAllText(ConfigPath, folder.Trim());
            Reload();
        }

        public static void UseLocalDatabase()
        {
            if (File.Exists(ConfigPath))
                File.Delete(ConfigPath);
            Reload();
        }

        private static void Reload()
        {
            SharedFolder = ReadConfiguredFolder();
            DbPath = Path.Combine(SharedFolder ?? LocalDataDirectory, DbFileName);
            ConnectionString = $"Data Source={DbPath}";

            // Idle konekcije drže staru bazu otvorenom; nakon promjene lokacije
            // moraju se zatvoriti da se ništa ne piše u pogrešan fajl.
            SqliteConnection.ClearAllPools();
        }

        private static string? ReadConfiguredFolder()
        {
            try
            {
                if (!File.Exists(ConfigPath)) return null;
                string value = File.ReadAllText(ConfigPath).Trim();
                return value.Length == 0 ? null : value;
            }
            catch
            {
                return null;
            }
        }

        public static OwnerTrackDbContext Create()
        {
            var options = new DbContextOptionsBuilder<OwnerTrackDbContext>()
                .UseSqlite(ConnectionString, sqliteOpts =>
                    sqliteOpts.CommandTimeout(30))
                .Options;

            var ctx = new OwnerTrackDbContext(options);

            // WAL ne radi pouzdano preko mrežnog dijeljenja (traži zajedničku memoriju
            // koju SMB ne daje), pa se za bazu na serveru koristi obični journal.
            if (IsShared)
            {
                ctx.Database.ExecuteSqlRaw(@"
                    PRAGMA foreign_keys = ON;
                    PRAGMA journal_mode = DELETE;
                    PRAGMA synchronous  = FULL;
                    PRAGMA cache_size   = 5000;
                    PRAGMA temp_store   = memory;
                ");
            }
            else
            {
                ctx.Database.ExecuteSqlRaw(@"
                    PRAGMA foreign_keys = ON;
                    PRAGMA journal_mode = WAL;
                    PRAGMA synchronous  = NORMAL;
                    PRAGMA cache_size   = 5000;
                    PRAGMA temp_store   = memory;
                ");
            }

            return ctx;
        }
    }
}
