using OwnerTrack.Infrastructure.Database;
using System.Text;

namespace OwnerTrack.Infrastructure.Services
{
  
    public static class AppLogger
    {
        
        private const string LogFileName = "ownertrack_errors.log";

        

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
            catch
            {
                // Never let logging crash the application.
            }
        }

      
        public static string FormatException(Exception? ex)
        {
            if (ex is null)
                return "Nepoznata greška";

            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            AppendInnerExceptions(sb, ex.InnerException, indent: "  ");
            return sb.ToString();
        }

        public static string GetLogPath()
        {
            string directory = Path.GetDirectoryName(DbContextFactory.DbPath)
                               ?? AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(directory, LogFileName);
        }

       

        private static void AppendInnerExceptions(StringBuilder sb, Exception? inner, string indent = "")
        {
            int depth = 1;
            while (inner is not null)
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