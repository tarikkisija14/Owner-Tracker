using Microsoft.Data.Sqlite;
using OwnerTrack.Infrastructure.Database;

namespace OwnerTrack.Infrastructure.Services
{
    // Pomoćne operacije za postavljanje baze u dijeljeni folder na serveru.
    public static class DatabaseLocationService
    {
        public const string SuggestedSharedFolder = @"\\SERVER\KnjigeMAHIR\OwnerTrackerDatabase";

        public static bool HasLocalDatabase => File.Exists(DbContextFactory.LocalDbPath);

        public static bool HasDatabase(string folder) =>
            File.Exists(Path.Combine(folder, DbContextFactory.DbFileName));

        /// <summary>Je li trenutno podešena baza dostupna (lokalna je uvijek).</summary>
        public static bool IsConfiguredLocationReachable() =>
            !DbContextFactory.IsShared || Directory.Exists(DbContextFactory.SharedFolder);

        /// <summary>
        /// Provjerava da se u folderu može praviti, mijenjati, preimenovati i brisati
        /// fajl (SQLite to radi uz bazu). Vraća poruku greške ili null ako je sve u redu.
        /// </summary>
        public static string? CheckFolderAccess(string folder)
        {
            string probe = Path.Combine(folder, $".ownertrack_test_{Guid.NewGuid():N}.tmp");
            string moved = probe + ".moved";

            try
            {
                if (!Directory.Exists(folder))
                    return "Folder ne postoji ili nije dostupan.";

                File.WriteAllText(probe, "test");
                File.AppendAllText(probe, "x");
                File.Move(probe, moved);
                File.Delete(moved);
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                TryDelete(probe);
                TryDelete(moved);
            }
        }

        /// <summary>
        /// Kopira lokalnu bazu u folder na serveru. Prije kopiranja se WAL zapisi
        /// vraćaju u glavni fajl i baza prebacuje u obični journal mod (na lokalnom
        /// disku), da se na server ne kopira baza koja očekuje WAL fajlove.
        /// Postojeća baza u odredišnom folderu se nikad ne prepisuje.
        /// </summary>
        public static void CopyLocalDatabaseTo(string folder)
        {
            string target = Path.Combine(folder, DbContextFactory.DbFileName);
            if (File.Exists(target))
                throw new InvalidOperationException("U odabranom folderu već postoji baza, pa kopiranje nije izvršeno.");

            string source = DbContextFactory.LocalDbPath;
            if (!File.Exists(source))
                throw new FileNotFoundException("Lokalna baza nije pronađena.", source);

            SqliteConnection.ClearAllPools();
            using (var conn = new SqliteConnection($"Data Source={source}"))
            {
                conn.Open();
                Exec(conn, "PRAGMA wal_checkpoint(TRUNCATE)");
                Exec(conn, "PRAGMA journal_mode = DELETE");
            }
            SqliteConnection.ClearAllPools();

            // Kopira se pod privremenim imenom, pa preimenuje: ako kopiranje prekine
            // mrežna greška, na serveru ne ostaje napola prepisana "Firme.db".
            string temp = target + ".copying";
            try
            {
                File.Copy(source, temp, overwrite: true);

                long sourceSize = new FileInfo(source).Length;
                long copiedSize = new FileInfo(temp).Length;
                if (sourceSize != copiedSize)
                    throw new IOException($"Kopija nije potpuna ({copiedSize} od {sourceSize} bajtova).");

                File.Move(temp, target);
            }
            finally
            {
                TryDelete(temp);
            }
        }

        private static void Exec(SqliteConnection conn, string sql)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch
            {
                // Čišćenje probnih fajlova ne smije prekinuti glavnu operaciju.
            }
        }
    }
}
