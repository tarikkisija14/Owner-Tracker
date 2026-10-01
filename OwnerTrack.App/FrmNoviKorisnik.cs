using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    // Modal za dodavanje novog lokalnog korisnika (dostupan svim prijavljenim korisnicima).
    public partial class FrmNoviKorisnik : Form
    {
        public FrmNoviKorisnik()
        {
            InitializeComponent();
        }

        private void FrmNoviKorisnik_Load(object sender, EventArgs e)
        {
            AcceptButton = btnSpremi;
            CancelButton = btnOtkazi;
            txtKorisnickoIme.Focus();
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (!btnSpremi.Enabled) return;

            string username = txtKorisnickoIme.Text.Trim();
            string password = txtLozinka.Text;

            if (!AuthService.IsValidUsername(username))
            {
                MessageBox.Show(UiMessages.NewUserUsernameInvalid);
                txtKorisnickoIme.Focus();
                return;
            }

            if (!AuthService.IsValidNewPassword(password))
            {
                MessageBox.Show(UiMessages.PasswordNewInvalid);
                txtLozinka.Focus();
                return;
            }

            if (password != txtPotvrda.Text)
            {
                MessageBox.Show(UiMessages.PasswordMismatch);
                txtPotvrda.Focus();
                return;
            }

            btnSpremi.Enabled = false;

            try
            {
                using var db = DbContextFactory.Create();
                if (!new AuthService(db).CreateUser(username, txtPrikaznoIme.Text, password))
                {
                    MessageBox.Show(
                        UiMessages.NewUserDuplicate,
                        UiMessages.NewUserTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKorisnickoIme.Focus();
                    return;
                }

                DialogHelper.ShowSaved(string.Format(UiMessages.NewUserCreatedFormat, username));
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri dodavanju korisnika");
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
