using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    // Lokalna prijava prije pristupa glavnoj formi. Uspjeh = DialogResult.OK i
    // popunjena UserSession; zatvaranje bez prijave = Cancel (aplikacija se završava).
    public partial class FrmLogin : Form
    {
        private readonly UserSession _session;

        public FrmLogin(UserSession session)
        {
            _session = session;
            InitializeComponent();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            AcceptButton = btnPrijava;
            CancelButton = btnIzlaz;
            txtKorisnik.Focus();
        }

        private void btnPrijava_Click(object sender, EventArgs e)
        {
            if (!btnPrijava.Enabled) return;

            string username = txtKorisnik.Text.Trim();
            string password = txtLozinka.Text;

            if (username.Length == 0)
            {
                MessageBox.Show(UiMessages.LoginUsernameRequired);
                txtKorisnik.Focus();
                return;
            }

            if (password.Length == 0)
            {
                MessageBox.Show(UiMessages.LoginPasswordRequired);
                txtLozinka.Focus();
                return;
            }

            btnPrijava.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                using var db = DbContextFactory.Create();
                var user = new AuthService(db).Authenticate(username, password);

                if (user is null)
                {
                    Cursor = Cursors.Default;
                    MessageBox.Show(
                        UiMessages.LoginFailed,
                        UiMessages.LoginFailedTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLozinka.Clear();
                    txtLozinka.Focus();
                    return;
                }

                txtLozinka.Clear();
                _session.SignIn(user);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri prijavi");
            }
            finally
            {
                Cursor = Cursors.Default;
                btnPrijava.Enabled = true;
            }
        }

        private void btnIzlaz_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
