using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App.Helpers
{
    // "Zapamti korisničko ime" na login formi. Čuva se samo korisničko ime (nikad
    // lozinka), u fajlu pored baze u profilu trenutnog Windows korisnika.
    public static class LoginPreferences
    {
        private static string FilePath =>
            Path.Combine(DbContextFactory.LocalDataDirectory, "lastuser.txt");

        public static string? LoadRememberedUsername()
        {
            try
            {
                if (!File.Exists(FilePath)) return null;
                string value = File.ReadAllText(FilePath).Trim();
                return value.Length == 0 ? null : value;
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                return null;
            }
        }

        public static void SaveRememberedUsername(string username)
        {
            try
            {
                File.WriteAllText(FilePath, username);
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
            }
        }

        public static void ClearRememberedUsername()
        {
            try
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
            }
        }
    }
}
