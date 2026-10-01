using Microsoft.Data.Sqlite;
using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    // Postavke lokacije baze: lokalna (samo ovaj računar) ili dijeljena u folderu na serveru.
    // Dostupno s login forme i pri pokretanju kad baza na serveru nije dostupna.
    public partial class FrmPostavkeBaze : Form
    {
        public FrmPostavkeBaze()
        {
            InitializeComponent();
        }

        public static string DescribeCurrentLocation() =>
            DbContextFactory.IsShared
                ? string.Format(UiMessages.DbLocationSharedFormat, DbContextFactory.SharedFolder)
                : UiMessages.DbLocationLocal;

        private void FrmPostavkeBaze_Load(object sender, EventArgs e)
        {
            CancelButton = btnZatvori;
            txtFolder.Text = DbContextFactory.SharedFolder ?? DatabaseLocationService.SuggestedSharedFolder;
            RefreshCurrent();
        }

        private void RefreshCurrent()
        {
            lblTrenutnoValue.Text = DescribeCurrentLocation();
            btnLokalna.Visible = DbContextFactory.IsShared;
        }

        private void btnOdaberi_Click(object sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog { ShowNewFolderButton = false };
            if (Directory.Exists(txtFolder.Text.Trim()))
                dialog.InitialDirectory = txtFolder.Text.Trim();

            if (dialog.ShowDialog(this) == DialogResult.OK)
                txtFolder.Text = dialog.SelectedPath;
        }

        private void btnPrimijeni_Click(object sender, EventArgs e)
        {
            string folder = txtFolder.Text.Trim();
            if (folder.Length > 3)
                folder = folder.TrimEnd('\\', '/');

            if (folder.Length == 0)
            {
                MessageBox.Show(UiMessages.DbFolderRequired);
                txtFolder.Focus();
                return;
            }

            Cursor = Cursors.WaitCursor;
            try
            {
                string? accessError = DatabaseLocationService.CheckFolderAccess(folder);
                if (accessError is not null)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show(
                        string.Format(UiMessages.DbFolderUnavailableFormat, folder, accessError),
                        UiMessages.DbSettingsTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool copyLocal = false;
                if (!DatabaseLocationService.HasDatabase(folder))
                {
                    Cursor = Cursors.Default;
                    if (DatabaseLocationService.HasLocalDatabase && !DbContextFactory.IsShared)
                    {
                        var answer = MessageBox.Show(
                            UiMessages.DbNoDatabaseWithLocalPrompt,
                            UiMessages.DbSettingsTitle,
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (answer == DialogResult.Cancel) return;
                        copyLocal = answer == DialogResult.Yes;
                    }
                    else if (MessageBox.Show(
                                 UiMessages.DbNoDatabaseNoLocalPrompt,
                                 UiMessages.DbSettingsTitle,
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }
                    Cursor = Cursors.WaitCursor;
                }

                ApplyFolder(folder, copyLocal);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ApplyFolder(string folder, bool copyLocal)
        {
            string? previousFolder = DbContextFactory.SharedFolder;

            try
            {
                if (copyLocal)
                    DatabaseLocationService.CopyLocalDatabaseTo(folder);

                DbContextFactory.UseSharedFolder(folder);
                new SchemaManager(DbContextFactory.ConnectionString).ApplyMigrations();
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                RestoreSetting(previousFolder);
                MessageBox.Show(
                    string.Format(UiMessages.DbSwitchFailedFormat, ex.Message),
                    UiMessages.DbSettingsTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                RefreshCurrent();
                return;
            }

            RefreshCurrent();
            DialogHelper.ShowSaved(string.Format(UiMessages.DbSwitchedFormat, folder));
        }

        private void btnLokalna_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    UiMessages.DbSwitchLocalPrompt,
                    UiMessages.DbSettingsTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            string? previousFolder = DbContextFactory.SharedFolder;

            try
            {
                DbContextFactory.UseLocalDatabase();
                new SchemaManager(DbContextFactory.ConnectionString).ApplyMigrations();
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                RestoreSetting(previousFolder);
                MessageBox.Show(
                    string.Format(UiMessages.DbSwitchFailedFormat, ex.Message),
                    UiMessages.DbSettingsTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                RefreshCurrent();
                return;
            }

            RefreshCurrent();
            DialogHelper.ShowSaved(UiMessages.DbSwitchedLocal);
        }

        private static void RestoreSetting(string? previousFolder)
        {
            if (previousFolder is null)
                DbContextFactory.UseLocalDatabase();
            else
                DbContextFactory.UseSharedFolder(previousFolder);
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
