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

      

        private static List<int> CollectVisibleIds(DataGridView grid) =>
            grid.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.DataBoundItem is not null)
                .Select(r => r.Cells["Id"].Value is int id ? id : 0)
                .Where(id => id > 0)
                .ToList();
    }
}