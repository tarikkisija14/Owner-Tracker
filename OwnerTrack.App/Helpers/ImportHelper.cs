using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Models;
using OwnerTrack.Infrastructure.Services;
using System.Threading;
using System.Windows.Forms;

namespace OwnerTrack.App.Helpers
{
    
    public class ImportHelper
    {
        private readonly string _connectionString;

        public ImportHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        

        public void PokreniImport(
            string filePath,
            Form owner,
            Action? onZavrsetak = null,
            Action? onGreska = null)
        {
            using var cts = new CancellationTokenSource();
            bool importZavrsen = false;

            using var frm = ImportProgressFormFactory.Kreiraj(
                out var progressBar,
                out var lblStatus,
                out var btnZatvori,
                out var btnOtkazi);

            frm.FormClosing += (s, args) =>
            {
                if (!importZavrsen)
                {
                    args.Cancel = true;
                    MessageBox.Show(
                        "Import je u toku. Molimo sačekajte da se završi ili pritisnite Otkaži.",
                        "Import u toku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            btnOtkazi.Click += (s, args) =>
            {
                if (!importZavrsen)
                {
                    btnOtkazi.Enabled = false;
                    btnOtkazi.Text = "Otkazivanje...";
                    cts.Cancel();
                }
            };

            var progress = new Progress<ImportProgress>(p =>
            {
                if (!frm.IsHandleCreated || frm.IsDisposed) return;
                frm.Invoke(() =>
                {
                    if (p.TotalRows > 0)
                    {
                        progressBar.Maximum = p.TotalRows;
                        progressBar.Value = Math.Min(p.ProcessedRows, p.TotalRows);
                    }
                    lblStatus.Text = FormatProgressLabel(p);
                    frm.Refresh();
                });
            });

            frm.Shown += async (s, args) =>
            {
                try
                {
                    var svc = new ExcelImportService(_connectionString);
                    var result = await Task.Run(
                        () => svc.ImportFromExcel(filePath, progress, cts.Token),
                        cts.Token);

                    importZavrsen = true;
                    OmoguciBtnZatvori(btnZatvori, btnOtkazi);
                    lblStatus.Text = FormatResultLabel(result, cts.IsCancellationRequested);

                    if (result.Errors.Count > 0)
                        MessageBox.Show(
                            $"Greške tokom importa:\n{string.Join("\n", result.Errors.Take(10))}",
                            "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (OperationCanceledException)
                {
                    importZavrsen = true;
                    lblStatus.Text = "Import je otkazan od strane korisnika.";
                    OmoguciBtnZatvori(btnZatvori, btnOtkazi);
                }
                catch (Exception ex)
                {
                    importZavrsen = true;
                    Program.LogException(ex);
                    MessageBox.Show($"Greška: {ex.Message}", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    frm.Close();
                    onGreska?.Invoke();
                    return;
                }

                void ZatvoriHandler(object? bs, EventArgs be)
                {
                    btnZatvori.Click -= ZatvoriHandler;
                    frm.Close();
                    if (!cts.IsCancellationRequested)
                        onZavrsetak?.Invoke();
                }
                btnZatvori.Click += ZatvoriHandler;
            };

            frm.ShowDialog(owner);
        }

        

        private static void OmoguciBtnZatvori(Button btnZatvori, Button btnOtkazi)
        {
            btnZatvori.Enabled = true;
            btnOtkazi.Enabled = false;
        }

        private static string FormatProgressLabel(ImportProgress p) =>
            $"Obrađeno: {p.ProcessedRows}/{p.TotalRows}\n" +
            $"Dodato: {p.SuccessCount}  |  Greške: {p.ErrorCount}\n" +
            $"{p.CurrentRow}";

        private static string FormatResultLabel(ImportResult result, bool cancelled) =>
            cancelled
                ? $"Import otkazan.\n" +
                  $"Dodano: {result.SuccessCount}  |  Preskočeno: {result.SkipCount}\n" +
                  $"Greške: {result.ErrorCount}"
                : $"Import završen!\n" +
                  $"Dodano: {result.SuccessCount}  |  Preskočeno (duplikati): {result.SkipCount}\n" +
                  $"Greške: {result.ErrorCount}  |  Vlasnici: {result.VlasnikCount}";
    }
}