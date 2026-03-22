using OwnerTrack.App.Constants;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App.Helpers
{

    public static class DialogHelper
    {


        public static bool PotvrdiArhiviranje(string poruka) =>
            MessageBox.Show(
                poruka + "\n\nNastavi?",
                "Potvrda",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.Yes;



        public static SaveFileDialog KreirajSaveDialogPdf(string title, string fileName) =>
            new SaveFileDialog
            {
                Title = title,
                Filter = UiConstants.PdfFilter,
                DefaultExt = UiConstants.PdfExt,
                FileName = fileName,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            };

        public static OpenFileDialog KreirajOpenDialogExcel(string? title = null)
        {
            var dialog = new OpenFileDialog { Filter = UiConstants.ExcelFilter };
            if (title is not null)
                dialog.Title = title;
            return dialog;
        }



        public static async Task IzvrsiPdfExport(
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
                        $"PDF je sačuvan:\n{savedPath}\n\nŽeliš li ga otvoriti?",
                        "PDF kreiran",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo(savedPath) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                MessageBox.Show(
                    $"Greška pri generisanju PDF-a. Detalji su sačuvani u logu:\n{AppLogger.GetLogPath()}");
            }
            finally
            {
                button.Enabled = true;
                button.Text = originalButtonText;
                Cursor.Current = Cursors.Default;
            }
        }



        public static void LogirajIPokaziGresku(Exception ex, string? prefix = null)
        {
            AppLogger.LogException(ex);
            string message = prefix is not null ? $"{prefix}: {ex.Message}" : $"Greška: {ex.Message}";
            MessageBox.Show(message);
        }


        public static string SigurnoIme(string naziv)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            string safeName = string.Concat(naziv.Select(c => invalidChars.Contains(c) ? '_' : c));
            return $"{safeName.Trim('_', ' ')}_{DateTime.Now:yyyyMMdd}.pdf";
        }
    }
}