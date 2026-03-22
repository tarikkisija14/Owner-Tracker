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
                    $"Desila se neočekivana greška.\n\n" +
                    $"Detalji su sačuvani u:\n{AppLogger.GetLogPath()}\n\n" +
                    $"Greška: {AppLogger.FormatException(e.Exception)}",
                    "Neočekivana greška",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            };

            AppDomain.CurrentDomain.UnhandledException +=
                (_, e) => AppLogger.LogException(e.ExceptionObject as Exception);

            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        

        public static void LogException(Exception? ex) => AppLogger.LogException(ex);
        public static string FormatExceptionFull(Exception? ex) => AppLogger.FormatException(ex);
        public static string GetLogPath() => AppLogger.GetLogPath();
    }
}