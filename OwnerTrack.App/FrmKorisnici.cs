using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    // Popis lokalnih korisnika s deaktivacijom/aktivacijom (soft delete — račun ostaje u bazi).
    public partial class FrmKorisnici : Form
    {
        private readonly int _currentUserId;

        public FrmKorisnici(int currentUserId)
        {
            _currentUserId = currentUserId;
            InitializeComponent();
        }

        private void FrmKorisnici_Load(object sender, EventArgs e)
        {
            CancelButton = btnZatvori;
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var users = new AuthService(db).GetUsers();

                dataGridKorisnici.DataSource = users;
                GridHelper.ConfigureColumn(dataGridKorisnici, nameof(KorisnikInfo.Username), "Korisničko ime", 130);
                GridHelper.ConfigureColumn(dataGridKorisnici, nameof(KorisnikInfo.DisplayName), "Ime i prezime", 150);
                GridHelper.ConfigureColumn(dataGridKorisnici, nameof(KorisnikInfo.IsActive), "Aktivan", 60);
                GridHelper.ConfigureColumn(dataGridKorisnici, nameof(KorisnikInfo.LastLogin), "Zadnja prijava", 110, "dd.MM.yyyy HH:mm");
                dataGridKorisnici.Columns[nameof(KorisnikInfo.Id)].Visible = false;
                dataGridKorisnici.Columns[nameof(KorisnikInfo.Password)].Visible = false;

                // Lozinka je skrivena dok se ne klikne "Prikaži" u istom redu.
                if (!dataGridKorisnici.Columns.Contains(PasswordColumn))
                {
                    dataGridKorisnici.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        Name = PasswordColumn, HeaderText = "Lozinka", FillWeight = 70, ReadOnly = true,
                    });
                    dataGridKorisnici.Columns.Add(new DataGridViewButtonColumn
                    {
                        Name = ShowColumn, HeaderText = string.Empty, FillWeight = 60,
                        UseColumnTextForButtonValue = false, FlatStyle = FlatStyle.Flat,
                    });
                }

                foreach (DataGridViewRow row in dataGridKorisnici.Rows)
                    SetPasswordVisible(row, false);

                dataGridKorisnici.ClearSelection();
                UpdateButton();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju korisnika");
            }
        }

        private const string PasswordColumn = "LozinkaPrikaz";
        private const string ShowColumn = "PrikaziLozinku";

        private static void SetPasswordVisible(DataGridViewRow row, bool visible)
        {
            string? password = (row.DataBoundItem as KorisnikInfo)?.Password;
            row.Cells[PasswordColumn].Value = string.IsNullOrEmpty(password)
                ? "—"
                : visible ? password : "••••";
            row.Cells[ShowColumn].Value = string.IsNullOrEmpty(password)
                ? string.Empty
                : visible ? "Sakrij" : "Prikaži";
            row.Tag = visible;
        }

        private void dataGridKorisnici_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dataGridKorisnici.Columns[e.ColumnIndex].Name != ShowColumn) return;

            var row = dataGridKorisnici.Rows[e.RowIndex];
            SetPasswordVisible(row, row.Tag is not true);
        }

        private KorisnikInfo? SelectedUser() =>
            dataGridKorisnici.SelectedRows.Count > 0
                ? dataGridKorisnici.SelectedRows[0].DataBoundItem as KorisnikInfo
                : null;

        private void dataGridKorisnici_SelectionChanged(object? sender, EventArgs e) => UpdateButton();

        private void UpdateButton()
        {
            var user = SelectedUser();
            btnPromijeniStatus.Text = user is { IsActive: false } ? "Aktiviraj" : "Deaktiviraj";
        }

        private void btnPromijeniStatus_Click(object sender, EventArgs e)
        {
            var user = SelectedUser();
            if (user is null)
            {
                MessageBox.Show(UiMessages.UsersSelectUser);
                return;
            }

            bool activate = !user.IsActive;
            if (!activate && MessageBox.Show(
                    string.Format(UiMessages.UsersDeactivateConfirmFormat, user.Username),
                    "Potvrda", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using var db = DbContextFactory.Create();
                var result = new AuthService(db).SetActive(_currentUserId, user.Id, activate);

                switch (result)
                {
                    case AuthService.SetActiveResult.CannotDeactivateSelf:
                        MessageBox.Show(UiMessages.UsersCannotDeactivateSelf);
                        return;
                    case AuthService.SetActiveResult.CannotDeactivateLastActive:
                        MessageBox.Show(UiMessages.UsersCannotDeactivateLast);
                        return;
                    case AuthService.SetActiveResult.NotFound:
                        MessageBox.Show(UiMessages.UsersNotFound);
                        break;
                }

                LoadUsers();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri promjeni statusa korisnika");
            }
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
