using OwnerTrack.App.Constants;
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
            Application.Run(new Form1());
        }
    }
}