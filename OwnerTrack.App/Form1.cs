using DocumentFormat.OpenXml.Packaging;
using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.App.Presenters;
using OwnerTrack.App.ViewModels;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using OwnerTrack.Infrastructure.ViewModels;
using OpenXmlSheet = DocumentFormat.OpenXml.Spreadsheet.Sheet;

namespace OwnerTrack.App
{
    public partial class Form1 : Form
    {
        private readonly System.Windows.Forms.Timer _searchDebounceTimer;
        private readonly ArchivePresenter _archivePresenter;
        private readonly PdfExportPresenter _pdfPresenter;

        public Form1()
        {
            InitializeComponent();

            _searchDebounceTimer = new System.Windows.Forms.Timer
            {
                Interval = UiConstants.SearchDebounceMs
            };
            _searchDebounceTimer.Tick += (_, _) =>
            {
                _searchDebounceTimer.Stop();
                ApplyCurrentFilters();
            };

            _archivePresenter = new ArchivePresenter();
            _pdfPresenter = new PdfExportPresenter();

            Load += Form1_Load;
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                new SchemaManager(DbContextFactory.ConnectionString).ApplyMigrations();

                LoadActivityCodeFilter();
                LoadSizeFilter();
                LoadClients();
                RefreshWarningsBadge();

                Text = $"OwnerTrack v{Application.ProductVersion}";
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                MessageBox.Show(
                    string.Format(UiMessages.StartupErrorFormat,
                        ex.Message,
                        DbContextFactory.DbPath,
                        AppLogger.GetLogPath()),
                    UiMessages.StartupErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _searchDebounceTimer.Dispose();
            base.OnFormClosed(e);
        }

        

        private void LoadSizeFilter()
        {
            cmbFilterVelicina.Items.Clear();
            cmbFilterVelicina.Items.Add(new { Value = UiConstants.FilterAllValue, Display = UiConstants.FilterAllDisplay });

            foreach (VelicinaFirme v in Enum.GetValues(typeof(VelicinaFirme)))
                cmbFilterVelicina.Items.Add(new { Value = v.ToString(), Display = v.ToString() });

            cmbFilterVelicina.DisplayMember = "Display";
            cmbFilterVelicina.ValueMember = "Value";
            cmbFilterVelicina.SelectedIndex = 0;
        }

        private void LoadActivityCodeFilter()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var activityCodes = db.Djelatnosti.OrderBy(d => d.Naziv).ToList();

                cmbFilterDjelatnost.Items.Clear();
                cmbFilterDjelatnost.Items.Add(new { Sifra = UiConstants.FilterAllValue, Naziv = UiConstants.FilterAllDjelatnostDisplay });

                foreach (var d in activityCodes)
                    cmbFilterDjelatnost.Items.Add(new { d.Sifra, Naziv = $"{d.Sifra} - {d.Naziv}" });

                cmbFilterDjelatnost.DisplayMember = "Naziv";
                cmbFilterDjelatnost.ValueMember = "Sifra";
                cmbFilterDjelatnost.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju djelatnosti");
            }
        }

        

        private string GetSelectedActivityCode()
        {
            if (cmbFilterDjelatnost.SelectedItem is null) return string.Empty;
            dynamic item = cmbFilterDjelatnost.SelectedItem;
            return item.Sifra ?? string.Empty;
        }

        private string GetSelectedSize()
        {
            if (cmbFilterVelicina.SelectedItem is null) return string.Empty;
            dynamic item = cmbFilterVelicina.SelectedItem;
            return item.Value ?? string.Empty;
        }

        private void ApplyCurrentFilters() =>
            LoadClients(txtSearchKlijent.Text, GetSelectedActivityCode(), GetSelectedSize());

        

        private void txtSearchKlijent_TextChanged(object sender, EventArgs e)
        {
            _searchDebounceTimer.Stop();
            _searchDebounceTimer.Start();
        }

        private void cmbFilterDjelatnost_SelectedIndexChanged(object sender, EventArgs e) => ApplyCurrentFilters();
        private void cmbFilterVelicina_SelectedIndexChanged(object sender, EventArgs e) => ApplyCurrentFilters();

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            txtSearchKlijent.Text = string.Empty;
            cmbFilterDjelatnost.SelectedIndex = 0;
            cmbFilterVelicina.SelectedIndex = 0;
            LoadClients();
        }

        

        private void LoadClients(string searchText = "", string sifraDjelatnosti = "", string velicina = "")
        {
            try
            {
                var clients = KlijentQueryService.GetClients(searchText, sifraDjelatnosti, velicina);
                GridHelper.BindWithoutEvent(dataGridKlijenti, dataGridKlijenti_SelectionChanged, clients);
                GridHelper.ApplyColumns(dataGridKlijenti, GridColumns.Klijenti);
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju klijenata");
            }
        }

        private void LoadOwners(int klijentId)
        {
            try
            {
                dataGridVlasnici.DataSource = KlijentQueryService.GetOwners(klijentId);
                GridHelper.ApplyColumns(dataGridVlasnici, GridColumns.Vlasnici);
                dataGridVlasnici.ClearSelection();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju vlasnika");
            }
        }

        private void LoadDirectors(int klijentId)
        {
            try
            {
                dataGridDirektori.DataSource = KlijentQueryService.GetDirectors(klijentId);
                GridHelper.ApplyColumns(dataGridDirektori, GridColumns.Direktori);
                dataGridDirektori.ClearSelection();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju direktora");
            }
        }

        private void dataGridKlijenti_SelectionChanged(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int id)) return;
            LoadOwners(id);
            LoadDirectors(id);
        }

        

        private void btnDodajKlijent_Click(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Create();
            if (new FrmDodajKlijent(klijentId: null, db).ShowDialog() == DialogResult.OK)
                RefreshAfterChange();
        }

        private void btnIzmijeniKlijent_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int id, UiMessages.SelectFirm)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajKlijent(id, db).ShowDialog() == DialogResult.OK)
                RefreshAfterChange();
        }

        private void btnObrisiKlijent_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int id, UiMessages.SelectFirm)) return;
            if (!DialogHelper.ConfirmArchive(UiMessages.ArchiveKlijentPrompt)) return;

            _archivePresenter.ArchiveKlijent(id, onSuccess: RefreshAfterChange);
        }

        

        private void btnDodajVlasnika_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirmFirst)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajVlasnika(klijentId, vlasnikId: null, db).ShowDialog() == DialogResult.OK)
                LoadOwners(klijentId);
        }

        private void btnIzmijeniVlasnika_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridVlasnici, out int vlasnikId, UiMessages.SelectVlasnik)) return;
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirm)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajVlasnika(klijentId, vlasnikId, db).ShowDialog() == DialogResult.OK)
                LoadOwners(klijentId);
        }

        private void btnObrisiVlasnika_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridVlasnici, out int vlasnikId, UiMessages.SelectVlasnik)) return;
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirm)) return;
            if (!DialogHelper.ConfirmArchive(UiMessages.ArchiveVlasnikPrompt)) return;

            _archivePresenter.ArchiveVlasnik(vlasnikId, onSuccess: () => LoadOwners(klijentId));
        }

       

        private void btnDodajDirektora_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirmFirst)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajDirektora(klijentId, direktorId: null, db).ShowDialog() == DialogResult.OK)
                LoadDirectors(klijentId);
        }

        private void btnIzmijeniDirektora_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridDirektori, out int direktorId, UiMessages.SelectDirektor)) return;
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirm)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajDirektora(klijentId, direktorId, db).ShowDialog() == DialogResult.OK)
                LoadDirectors(klijentId);
        }

        private void btnObrisiDirektora_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridDirektori, out int direktorId, UiMessages.SelectDirektor)) return;
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirm)) return;
            if (!DialogHelper.ConfirmArchive(UiMessages.ArchiveDirektorPrompt)) return;

            _archivePresenter.ArchiveDirektor(direktorId, onSuccess: () => LoadDirectors(klijentId));
        }

        

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            using var dialog = DialogHelper.CreateOpenDialogExcel();
            if (dialog.ShowDialog() != DialogResult.OK) return;

            new ImportHelper(DbContextFactory.ConnectionString)
                .RunImport(dialog.FileName, this, RefreshAfterChange);
        }

        private void btnResetImport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    UiConstants.ResetImportConfirmMessage,
                    UiConstants.ResetImportTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            using var dialog = DialogHelper.CreateOpenDialogExcel(UiMessages.ResetImportDialogTitle);
            if (dialog.ShowDialog() != DialogResult.OK) return;

            if (!File.Exists(dialog.FileName))
            {
                MessageBox.Show(
                    UiMessages.ResetImportFileNotFound,
                    UiMessages.ResetImportFileNotFoundTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidateExcelFile(dialog.FileName)) return;

            try
            {
                var dbService = new DatabaseService(DbContextFactory.DbPath, DbContextFactory.ConnectionString);
                string backupPath = dbService.ResetDatabase();

                new ImportHelper(DbContextFactory.ConnectionString).RunImport(
                    dialog.FileName, this,
                    onCompleted: RefreshAfterChange,
                    onError: () => OfferBackupRestore(dbService, backupPath));
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri resetu baze");
            }
        }

        private static bool ValidateExcelFile(string filePath)
        {
            try
            {
                using var doc = SpreadsheetDocument.Open(filePath, isEditable: false);
                var workbookPart = doc.WorkbookPart
                    ?? throw new InvalidOperationException("Fajl nema validan WorkbookPart.");

                _ = workbookPart.Workbook.Sheets
                      ?.Cast<OpenXmlSheet>()
                      .FirstOrDefault(s => s.Name?.Value?.Contains(UiConstants.ExcelZirvanaKeyword) == true)
                    ?? throw new InvalidOperationException(
                        $"Fajl ne sadrži list sa '{UiConstants.ExcelZirvanaKeyword}'.");

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(UiMessages.ExcelValidationErrorFormat, ex.Message),
                    UiMessages.ExcelValidationErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void OfferBackupRestore(DatabaseService dbService, string backupPath)
        {
            if (string.IsNullOrEmpty(backupPath)) return;

            var answer = MessageBox.Show(
                string.Format(UiMessages.BackupRestorePromptFormat, backupPath),
                UiMessages.BackupRestoreTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (answer != DialogResult.Yes)
            {
                MessageBox.Show(
                    string.Format(UiMessages.BackupKeptFormat, backupPath),
                    UiMessages.BackupKeptTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                dbService.RestoreBackup(backupPath);
                RefreshAfterChange();
                MessageBox.Show(
                    UiMessages.BackupRestoredSuccess,
                    UiMessages.BackupRestoredSuccessTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                MessageBox.Show(
                    string.Format(UiMessages.BackupRestoreFailedFormat, backupPath),
                    UiMessages.BackupRestoreFailedTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void RefreshWarningsBadge()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var stats = new WarningQueryService(db).GetStats();
                ApplyBadgeState(BadgeState.FromStats(stats.Count, stats.HasExpired));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BADGE] Greška: {ex.Message}");
                AppLogger.LogException(ex);
            }
        }

        private void ApplyBadgeState(BadgeState state)
        {
            btnUpozorenja.Text = state.Label;
            btnUpozorenja.BackColor = state.BackColor;
            btnUpozorenja.ForeColor = state.ForeColor;

            var oldFont = btnUpozorenja.Font;
            if (oldFont.Style != state.FontStyle)
            {
                btnUpozorenja.Font = new Font(oldFont.FontFamily, oldFont.Size, state.FontStyle);
                oldFont.Dispose();
            }
        }

        private void btnUpozorenja_Click(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Create();
            new FrmUpozorenja(db).ShowDialog(this);
        }

        

        private async void btnExportTabelaPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportTableAsync(dataGridKlijenti, btnExportTabelaPdf);

        private async void btnSacuvajPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportSingleClientAsync(dataGridKlijenti, btnSacuvajPdf);

        

        private void RefreshAfterChange()
        {
            LoadActivityCodeFilter();
            LoadClients();
            RefreshWarningsBadge();
        }
    }
}