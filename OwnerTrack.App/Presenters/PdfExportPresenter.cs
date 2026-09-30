using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using OwnerTrack.Infrastructure.ViewModels;

namespace OwnerTrack.App.Presenters
{
  
    public sealed class PdfExportPresenter
    {
        public async Task ExportTableAsync(DataGridView grid, Button button)
        {
            if (grid.Rows.Count == 0)
            {
                MessageBox.Show(UiMessages.PdfNoClientsToExport);
                return;
            }

            var ids = CollectVisibleIds(grid);

            using var dialog = DialogHelper.CreateSaveDialogPdf(
                UiMessages.PdfTableSaveTitle,
                $"{UiMessages.PdfTableFilePrefix}{DateTime.Now:yyyyMMdd}.pdf");
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string savedPath = dialog.FileName;
            await DialogHelper.ExecutePdfExport(
                button, button.Text,
                path =>
                {
                    using var db = DbContextFactory.Create();
                    return new PdfExportService(db).GenerateClientTable(ids, path);
                },
                savedPath);
        }

        public async Task ExportSingleClientAsync(DataGridView grid, Button button)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show(UiMessages.PdfNoClientSelected);
                return;
            }

            if (grid.SelectedRows[0].DataBoundItem is not KlijentViewModel row)
            {
                MessageBox.Show(UiMessages.PdfCannotReadSelected);
                return;
            }

            using var dialog = DialogHelper.CreateSaveDialogPdf(
                UiMessages.PdfReportSaveTitle,
                DialogHelper.BuildSafeFileName(row.Naziv ?? string.Empty));
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string savedPath = dialog.FileName;
            await DialogHelper.ExecutePdfExport(
                button, button.Text,
                path =>
                {
                    using var db = DbContextFactory.Create();
                    return new PdfExportService(db).GeneratePdf(row.Id, path);
                },
                savedPath);
        }

        /// <summary>
        /// Exports exactly what's shown in an evidencija grid (KYC, UBO,
        /// PEP, procjena rizika) — all currently visible rows, with that
        /// grid's own columns. No Klijent-specific PDF structure involved.
        /// </summary>
        public async Task ExportGenericTableAsync(DataGridView grid, Button button, string title, string filePrefix, string[]? onlyColumns = null)
        {
            if (grid.Rows.Count == 0)
            {
                MessageBox.Show(UiMessages.PdfNoDataToExport);
                return;
            }

            var (headers, weights, rows) = GridHelper.ExtractVisibleData(grid, selectedOnly: false, onlyColumns);

            using var dialog = DialogHelper.CreateSaveDialogPdf(
                UiMessages.PdfTableSaveTitle,
                $"{filePrefix}_{DateTime.Now:yyyyMMdd}.pdf");
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string savedPath = dialog.FileName;
            await DialogHelper.ExecutePdfExport(
                button, button.Text,
                path =>
                {
                    using var db = DbContextFactory.Create();
                    return new PdfExportService(db).GenerateGenericTable(title, headers, weights, rows, path);
                },
                savedPath);
        }

        /// <summary>
        /// Same as <see cref="ExportGenericTableAsync"/> but for just the
        /// selected row — a one-row PDF with that grid's own columns.
        /// </summary>
        public async Task ExportGenericSingleRowAsync(DataGridView grid, Button button, string title, string filePrefix, string[]? onlyColumns = null)
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show(UiMessages.PdfNoRowSelected);
                return;
            }

            var (headers, weights, rows) = GridHelper.ExtractVisibleData(grid, selectedOnly: true, onlyColumns);

            using var dialog = DialogHelper.CreateSaveDialogPdf(
                UiMessages.PdfReportSaveTitle,
                $"{filePrefix}_{DateTime.Now:yyyyMMdd}.pdf");
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string savedPath = dialog.FileName;
            await DialogHelper.ExecutePdfExport(
                button, button.Text,
                path =>
                {
                    using var db = DbContextFactory.Create();
                    return new PdfExportService(db).GenerateGenericTable(title, headers, weights, rows, path);
                },
                savedPath);
        }



        /// <summary>
        /// Compliance Summary — jedan agregatni PDF sa Dashboard brojevima
        /// (ne generički table export, jer podaci nisu red-po-red). Podaci se
        /// prikupljaju u pozivaocu (Form1) i prosljeđuju kao gotov DTO, da PDF
        /// servis ne pokreće drugi/paralelni upit.
        /// </summary>
        public async Task ExportComplianceSummaryAsync(Button button, OwnerTrack.Infrastructure.Models.ComplianceSummaryData data)
        {
            using var dialog = DialogHelper.CreateSaveDialogPdf(
                UiMessages.PdfReportSaveTitle,
                $"Compliance_Summary_{DateTime.Now:yyyyMMdd}.pdf");
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string savedPath = dialog.FileName;
            await DialogHelper.ExecutePdfExport(
                button, button.Text,
                path =>
                {
                    using var db = DbContextFactory.Create();
                    return new PdfExportService(db).GenerateComplianceSummaryPdf(data, path);
                },
                savedPath);
        }

        private static List<int> CollectVisibleIds(DataGridView grid) =>
            grid.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.DataBoundItem is not null)
                .Select(r => r.Cells["Id"].Value is int id ? id : 0)
                .Where(id => id > 0)
                .ToList();
    }
}