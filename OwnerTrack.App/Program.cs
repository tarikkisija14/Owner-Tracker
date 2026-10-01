using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.ThreadException += (_, e) =>
            {
                AppLogger.LogException(e.Exception);
                MessageBox.Show(
                    string.Format(UiMessages.UnhandledExceptionFormat,
                        AppLogger.GetLogPath(),
                        AppLogger.FormatException(e.Exception)),
                    UiMessages.UnhandledExceptionTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException +=
                (_, e) => AppLogger.LogException(e.ExceptionObject as Exception);

            ApplicationConfiguration.Initialize();

            // Ako je baza na serveru, a server nije dostupan, aplikacija se NE vraća
            // tiho na lokalnu bazu (korisnik bi radio u pogrešnoj bazi) nego čeka.
            if (!EnsureDatabaseReachable())
                return;

            // Migracije (uključujući tabelu Korisnici i seed) moraju proći prije
            // prijave, jer login čita korisnike iz baze.
            try
            {
                new SchemaManager(DbContextFactory.ConnectionString).ApplyMigrations();
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                MessageBox.Show(
                    string.Format(UiMessages.StartupErrorFormat,
                        ex.Message,
                        DbContextFactory.DbPath,
                        AppLogger.GetLogPath()),
                    UiMessages.StartupErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var session = new UserSession();

            // Login → Form1 → (odjava) → ponovo Login. Form1 se kreira tek nakon
            // uspješne prijave; zatvaranje logina ili Form1 bez odjave završava aplikaciju.
            while (true)
            {
                using (var login = new FrmLogin(session))
                {
                    if (login.ShowDialog() != DialogResult.OK || !session.IsAuthenticated)
                        break;
                }

                var main = new Form1(session);
                Application.Run(main);
                bool logoutRequested = main.LogoutRequested;
                main.Dispose();

                if (!logoutRequested)
                    break;
            }

            session.SignOut();
        }

        private static bool EnsureDatabaseReachable()
        {
            while (!DatabaseLocationService.IsConfiguredLocationReachable())
            {
                var answer = MessageBox.Show(
                    string.Format(UiMessages.DbUnreachableFormat, DbContextFactory.DbPath),
                    UiMessages.DbUnreachableTitle,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (answer == DialogResult.Cancel)
                    return false;

                if (answer == DialogResult.No)
                {
                    using var settings = new FrmPostavkeBaze();
                    settings.ShowDialog();
                }
            }

            return true;
        }
    }
}