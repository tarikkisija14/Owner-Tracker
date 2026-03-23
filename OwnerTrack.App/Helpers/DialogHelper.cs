using OwnerTrack.App.Constants;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App.Helpers
{
    public static class DialogHelper
    {
        public static bool ConfirmArchive(string message) =>
            MessageBox.Show(
                message + "\n\nNastavi?",
                "Potvrda",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes;

        public static SaveFileDialog CreateSaveDialogPdf(string title, string fileName) =>
            new SaveFileDialog
            {
                Title = title,
                Filter = UiConstants.PdfFilter,
                DefaultExt = UiConstants.PdfExt,
                FileName = fileName,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            };

        public static OpenFileDialog CreateOpenDialogExcel(string? title = null)
        {
            var dialog = new OpenFileDialog { Filter = UiConstants.ExcelFilter };
            if (title is not null)
                dialog.Title = title;
            return dialog;
        }

        public static async Task ExecutePdfExport(
            Button button,
            string originalButtonText,
            Func<string, string> pdfGenerator,
            string outputPath)
        {
            button.Enabled = false;
            button.Text = UiConstants.PdfExportBusyLabel;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string savedPath = await Task.Run(() => pdfGenerator(outputPath));

                if (MessageBox.Show(
                        string.Format(UiMessages.PdfSavedPromptFormat, savedPath),
                        UiMessages.PdfSavedTitle,
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo(savedPath) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                MessageBox.Show(string.Format(UiMessages.PdfErrorFormat, AppLogger.GetLogPath()));
            }
            finally
            {
                button.Enabled = true;
                button.Text = originalButtonText;
                Cursor.Current = Cursors.Default;
            }
        }

        public static void LogAndShowError(Exception ex, string? prefix = null)
        {
            AppLogger.LogException(ex);
            string message = prefix is not null
                ? string.Format(UiMessages.GenericErrorWithPrefixFormat, prefix, ex.Message)
                : string.Format(UiMessages.GenericErrorFormat, ex.Message);
            MessageBox.Show(message);
        }

        public static string BuildSafeFileName(string naziv)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string safeName = string.Concat(naziv.Select(c => invalidChars.Contains(c) ? '_' : c));
            return $"{safeName.Trim('_', ' ')}_{DateTime.Now:yyyyMMdd}.pdf";
        }
    }
}