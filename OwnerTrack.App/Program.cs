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
                LogException(e.ExceptionObject as Exception);

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        public static void LogException(Exception? ex)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]");
                sb.AppendLine($"Tip:    {ex?.GetType().FullName ?? "nepoznat"}");
                sb.AppendLine($"Poruka: {ex?.Message ?? "nema poruke"}");
                sb.AppendLine($"Stack:\n{ex?.StackTrace ?? "nema stacka"}");

                AppendInnerExceptions(sb, ex?.InnerException);

                sb.AppendLine(new string('-', 80));
                File.AppendAllText(GetLogPath(), sb.ToString());
            }
            catch { /* never let logging crash the app */ }
        }

        public static string FormatExceptionFull(Exception? ex)
        {
            if (ex == null) return "Nepoznata greška";

            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            AppendInnerExceptions(sb, ex.InnerException, indent: "  ");
            return sb.ToString();
        }

        public static string GetLogPath()
        {
            string dir = Path.GetDirectoryName(DbContextFactory.DbPath)
                         ?? AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(dir, "ownertrack_errors.log");
        }

        
        private static void AppendInnerExceptions(StringBuilder sb, Exception? inner, string indent = "")
        {
            int depth = 1;
            while (inner != null)
            {
                if (string.IsNullOrEmpty(indent))
                {
                    sb.AppendLine($"--- Inner Exception [{depth}] ---");
                    sb.AppendLine($"Tip:    {inner.GetType().FullName}");
                    sb.AppendLine($"Poruka: {inner.Message}");
                    sb.AppendLine($"Stack:\n{inner.StackTrace}");
                }
                else
                {
                    sb.AppendLine($"{indent}[{depth}] {inner.GetType().Name}: {inner.Message}");
                }

                inner = inner.InnerException;
                depth++;
            }
        }
    }
}