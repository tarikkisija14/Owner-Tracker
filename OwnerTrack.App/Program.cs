using OwnerTrack.App;
using OwnerTrack.Infrastructure.Database;
using System.IO;
using System.Text;

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
                    $"Greška: {FormatExceptionFull(e.Exception)}",
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
                var sb = new StringBuilder();
                sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                sb.AppendLine($"Tip: {ex?.GetType().FullName ?? "nepoznat"}");
                sb.AppendLine($"Poruka: {ex?.Message ?? "nema poruke"}");
                sb.AppendLine($"Stack:\n{ex?.StackTrace ?? "nema stacka"}");

                
                var inner = ex?.InnerException;
                int depth = 1;
                while (inner != null)
                {
                    sb.AppendLine($"--- Inner Exception [{depth}] ---");
                    sb.AppendLine($"Tip: {inner.GetType().FullName}");
                    sb.AppendLine($"Poruka: {inner.Message}");
                    sb.AppendLine($"Stack:\n{inner.StackTrace}");
                    inner = inner.InnerException;
                    depth++;
                }

                sb.AppendLine(new string('-', 80));
                File.AppendAllText(logPath, sb.ToString());
            }
            catch { }
        }

        public static string FormatExceptionFull(Exception? ex)
        {
            if (ex == null) return "Nepoznata greška";
            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            var inner = ex.InnerException;
            int depth = 1;
            while (inner != null)
            {
                sb.AppendLine($"  [{depth}] {inner.GetType().Name}: {inner.Message}");
                inner = inner.InnerException;
                depth++;
            }
            return sb.ToString();
        }

        public static string GetLogPath()
        {
            string dir = Path.GetDirectoryName(DbContextFactory.DbPath)
                         ?? AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(dir, "ownertrack_errors.log");
        }
    }
}