using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                Filter = "PDF dokument (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                FileName = fileName,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            };

        public static OpenFileDialog KreirajOpenDialogExcel(string? title = null)
        {
            var dlg = new OpenFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx" };
            if (title != null) dlg.Title = title;
            return dlg;
        }

        /// <summary>
        /// Disables <paramref name="btn"/>, runs <paramref name="generatorPdf"/> on a background
        /// thread, then offers to open the result. Restores button state in all cases.
        /// </summary>
        public static async Task IzvrsiPdfExport(
            Button btn,
            string originalniTekst,
            Func<string, string> generatorPdf,
            string outputPath)
        {
            btn.Enabled = false;
            btn.Text = "Generišem...";
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;

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
                System.Windows.Forms.Cursor.Current = Cursors.Default;
            }
        }

        public static void LogirajIPokaziGresku(Exception ex, string? prefiks = null)
        {
            Program.LogException(ex);
            string poruka = prefiks != null ? $"{prefiks}: {ex.Message}" : $"Greška: {ex.Message}";
            MessageBox.Show(poruka);
        }

        /// <summary>Converts <paramref name="naziv"/> into a safe PDF filename with today's date.</summary>
        public static string SigurnoIme(string naziv)
        {
            var invalid = Path.GetInvalidFileNameChars();
            string safe = string.Concat(naziv.Select(c => invalid.Contains(c) ? '_' : c));
            return $"{safe.Trim('_', ' ')}_{DateTime.Now:yyyyMMdd}.pdf";
        }
    }
}