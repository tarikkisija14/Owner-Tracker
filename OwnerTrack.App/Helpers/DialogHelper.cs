using OwnerTrack.App.Constants;

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
            var dlg = new OpenFileDialog { Filter = UiConstants.ExcelFilter };
            if (title != null) dlg.Title = title;
            return dlg;
        }

        

       
        public static async Task IzvrsiPdfExport(
            Button btn,
            string originalniTekst,
            Func<string, string> generatorPdf,
            string outputPath)
        {
            btn.Enabled = false;
            btn.Text = UiConstants.PdfExportBusyLabel;
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                string path = await Task.Run(() => generatorPdf(outputPath));

                if (MessageBox.Show(
                        $"PDF je sačuvan:\n{path}\n\nŽeliš li ga otvoriti?",
                        "PDF kreiran",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show(
                    $"Greška pri generisanju PDF-a. Detalji su sačuvani u logu:\n{Program.GetLogPath()}");
            }
            finally
            {
                btn.Enabled = true;
                btn.Text = originalniTekst;
                Cursor.Current = Cursors.Default;
            }
        }

        

        public static void LogirajIPokaziGresku(Exception ex, string? prefiks = null)
        {
            Program.LogException(ex);
            string poruka = prefiks != null ? $"{prefiks}: {ex.Message}" : $"Greška: {ex.Message}";
            MessageBox.Show(poruka);
        }

        
        public static string SigurnoIme(string naziv)
        {
            var invalid = Path.GetInvalidFileNameChars();
            string safe = string.Concat(naziv.Select(c => invalid.Contains(c) ? '_' : c));
            return $"{safe.Trim('_', ' ')}_{DateTime.Now:yyyyMMdd}.pdf";
        }
    }
}