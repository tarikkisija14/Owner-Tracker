using OwnerTrack.Infrastructure.Models;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App.Helpers
{

    public class ImportHelper
    {
        private const int MaxErrorsShownInDialog = 10;

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
            bool importFinished = false;

            using var progressForm = ImportProgressFormFactory.Kreiraj(
                out var progressBar,
                out var lblStatus,
                out var btnClose,
                out var btnCancel);

            progressForm.FormClosing += (_, args) =>
            {
                if (!importFinished)
                {
                    args.Cancel = true;
                    MessageBox.Show(
                        "Import je u toku. Molimo sačekajte da se završi ili pritisnite Otkaži.",
                        "Import u toku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            btnCancel.Click += (_, _) =>
            {
                if (!importFinished)
                {
                    btnCancel.Enabled = false;
                    btnCancel.Text = "Otkazivanje...";
                    cts.Cancel();
                }
            };

            var progress = new Progress<ImportProgress>(p =>
            {
                if (!progressForm.IsHandleCreated || progressForm.IsDisposed)
                    return;

                progressForm.Invoke(() =>
                {
                    if (p.TotalRows > 0)
                    {
                        progressBar.Maximum = p.TotalRows;
                        progressBar.Value = Math.Min(p.ProcessedRows, p.TotalRows);
                    }
                    lblStatus.Text = FormatProgressLabel(p);
                    progressForm.Refresh();
                });
            });

            progressForm.Shown += async (_, _) =>
            {
                try
                {
                    var service = new ExcelImportService(_connectionString);
                    var result = await Task.Run(
                        () => service.ImportFromExcel(filePath, progress, cts.Token),
                        cts.Token);

                    importFinished = true;
                    EnableCloseButton(btnClose, btnCancel);
                    lblStatus.Text = FormatResultLabel(result, cts.IsCancellationRequested);

                    if (result.Errors.Count > 0)
                        MessageBox.Show(
                            $"Greške tokom importa:\n{string.Join("\n", result.Errors.Take(MaxErrorsShownInDialog))}",
                            "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (OperationCanceledException)
                {
                    importFinished = true;
                    lblStatus.Text = "Import je otkazan od strane korisnika.";
                    EnableCloseButton(btnClose, btnCancel);
                }
                catch (Exception ex)
                {
                    importFinished = true;
                    AppLogger.LogException(ex);
                    MessageBox.Show($"Greška: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressForm.Close();
                    onGreska?.Invoke();
                    return;
                }

                void OnCloseClicked(object? s, EventArgs e)
                {
                    btnClose.Click -= OnCloseClicked;
                    progressForm.Close();
                    if (!cts.IsCancellationRequested)
                        onZavrsetak?.Invoke();
                }
                btnClose.Click += OnCloseClicked;
            };

            progressForm.ShowDialog(owner);
        }


        private static void EnableCloseButton(Button btnClose, Button btnCancel)
        {
            btnClose.Enabled = true;
            btnCancel.Enabled = false;
        }

        private static string FormatProgressLabel(ImportProgress p) =>
            $"Obrađeno: {p.ProcessedRows}/{p.TotalRows}\n" +
            $"Dodato: {p.SuccessCount}  |  Greške: {p.ErrorCount}\n" +
            $"{p.CurrentRow}";

        private static string FormatResultLabel(ImportResult result, bool wasCancelled) =>
            wasCancelled
                ? $"Import otkazan.\nDodano: {result.SuccessCount}  |  Preskočeno: {result.SkipCount}\nGreške: {result.ErrorCount}"
                : $"Import završen!\nDodano: {result.SuccessCount}  |  Preskočeno (duplikati): {result.SkipCount}\nGreške: {result.ErrorCount}  |  Vlasnici: {result.VlasnikCount}";
    }
}