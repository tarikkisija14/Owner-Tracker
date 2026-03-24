using OwnerTrack.App.Constants;
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

        public void RunImport(
            string filePath,
            Form owner,
            Action? onCompleted = null,
            Action? onError = null)
        {
            using var cts = new CancellationTokenSource();
            bool importDone = false;

            using var progressForm = ImportProgressFormFactory.Create(
                out var progressBar,
                out var lblStatus,
                out var btnClose,
                out var btnCancel);

            WireFormClosingGuard(progressForm, () => importDone);
            WireCancelButton(btnCancel, cts, () => importDone);

            var progress = BuildProgressReporter(progressForm, progressBar, lblStatus);

            progressForm.Shown += async (_, _) =>
            {
                ImportResult? result = null;

                try
                {
                    var service = new ExcelImportService(_connectionString);
                    result = await Task.Run(
                        () => service.ImportFromExcel(filePath, progress, cts.Token),
                        cts.Token);

                    importDone = true;
                    ActivateCloseButton(btnClose, btnCancel);
                    lblStatus.Text = ImportProgressFormatter.FormatResult(result, cts.IsCancellationRequested);

                    if (result.Errors.Count > 0)
                        ShowImportErrors(result.Errors);
                }
                catch (OperationCanceledException)
                {
                    importDone = true;
                    lblStatus.Text = UiMessages.ImportCancelledByUser;
                    ActivateCloseButton(btnClose, btnCancel);
                }
                catch (Exception ex)
                {
                    importDone = true;
                    AppLogger.LogException(ex);
                    MessageBox.Show(
                        string.Format(UiMessages.ImportErrorFormat, ex.Message),
                        UiMessages.ImportErrorTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    progressForm.Close();
                    onError?.Invoke();
                    return;
                }

                void OnCloseClicked(object? s, EventArgs e)
                {
                    btnClose.Click -= OnCloseClicked;
                    progressForm.Close();
                    if (!cts.IsCancellationRequested)
                        onCompleted?.Invoke();
                }

                btnClose.Click += OnCloseClicked;
            };

            progressForm.ShowDialog(owner);
        }

      

        private static void WireFormClosingGuard(Form form, Func<bool> isDone)
        {
            form.FormClosing += (_, args) =>
            {
                if (isDone()) return;
                args.Cancel = true;
                MessageBox.Show(
                    UiMessages.ImportInProgressMessage,
                    UiMessages.ImportInProgressTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            };
        }

        private static void WireCancelButton(Button btnCancel, CancellationTokenSource cts, Func<bool> isDone)
        {
            btnCancel.Click += (_, _) =>
            {
                if (isDone()) return;
                btnCancel.Enabled = false;
                btnCancel.Text = UiMessages.ImportCancellingText;
                cts.Cancel();
            };
        }

        private static IProgress<ImportProgress> BuildProgressReporter(
            Form progressForm, ProgressBar progressBar, Label lblStatus)
        {
            return new Progress<ImportProgress>(p =>
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
                    lblStatus.Text = ImportProgressFormatter.FormatProgress(p);
                    progressForm.Refresh();
                });
            });
        }

        private static void ShowImportErrors(List<string> errors)
        {
            MessageBox.Show(
                string.Format(UiMessages.ImportErrorsFormat,
                    string.Join("\n", errors.Take(MaxErrorsShownInDialog))),
                UiMessages.ImportErrorsTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static void ActivateCloseButton(Button btnClose, Button btnCancel)
        {
            btnClose.Enabled = true;
            btnCancel.Enabled = false;
        }
    }
}