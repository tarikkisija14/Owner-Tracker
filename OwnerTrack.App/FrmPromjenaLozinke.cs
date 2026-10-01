using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    // Modal za promjenu lozinke trenutno prijavljenog korisnika.
    public partial class FrmPromjenaLozinke : Form
    {
        private readonly int _userId;

        public FrmPromjenaLozinke(int userId)
        {
            _userId = userId;
            InitializeComponent();
        }

        private void FrmPromjenaLozinke_Load(object sender, EventArgs e)
        {
            AcceptButton = btnSpremi;
            CancelButton = btnOtkazi;
            txtTrenutna.Focus();
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (!btnSpremi.Enabled) return;

            string current = txtTrenutna.Text;
            string next = txtNova.Text;

            if (current.Length == 0)
            {
                MessageBox.Show(UiMessages.PasswordCurrentRequired);
                txtTrenutna.Focus();
                return;
            }

            if (!AuthService.IsValidNewPassword(next))
            {
                MessageBox.Show(UiMessages.PasswordNewInvalid);
                txtNova.Focus();
                return;
            }

            if (next != txtPotvrda.Text)
            {
                MessageBox.Show(UiMessages.PasswordMismatch);
                txtPotvrda.Focus();
                return;
            }

            btnSpremi.Enabled = false;

            try
            {
                using var db = DbContextFactory.Create();
                if (!new AuthService(db).ChangePassword(_userId, current, next))
                {
                    MessageBox.Show(
                        UiMessages.PasswordCurrentWrong,
                        UiMessages.PasswordChangeTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTrenutna.Clear();
                    txtTrenutna.Focus();
                    return;
                }

                DialogHelper.ShowSaved(UiMessages.PasswordChanged);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri promjeni lozinke");
            }
            finally
            {
                btnSpremi.Enabled = true;
            }
        }

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
