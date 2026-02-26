using OwnerTrack.App;
using OwnerTrack.Infrastructure.Database;
using System.IO;

namespace OwnerTrack.App
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (s, e) =>
            {
                LogException(e.Exception);
                MessageBox.Show(
                    $"Desila se neočekivana greška.\n\n" +
                    $"Detalji su sačuvani u:\n{GetLogPath()}\n\n" +
                    $"Greška: {e.Exception.Message}",
                    "Neočekivana greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                LogException(e.ExceptionObject as Exception);
            };

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static void LogException(Exception? ex)
        {
            try
            {
                string logPath = GetLogPath();
                string poruka =
                    $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]\n" +
                    $"Tip: {ex?.GetType().FullName ?? "nepoznat"}\n" +
                    $"Poruka: {ex?.Message ?? "nema poruke"}\n" +
                    $"Stack:\n{ex?.StackTrace ?? "nema stacka"}\n" +
                    $"Inner: {ex?.InnerException?.Message ?? "-"}\n" +
                    $"{new string('-', 80)}\n";

                File.AppendAllText(logPath, poruka);
            }
            catch
            {
                
            }
        }

        public static string GetLogPath()
        {
            string dir = Path.GetDirectoryName(DbContextFactory.DbPath)
                         ?? AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(dir, "ownertrack_errors.log");
        }
    }
}