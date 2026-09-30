using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    // Mali modal koji korisniku dozvoljava da upozorenje označi kao
    // pregledano/riješeno, sa opcionalnom napomenom — bez mijenjanja
    // stvarnog datuma isteka na Vlasniku/Direktoru koji je upozorenje
    // izazvao. Warning i dalje ostaje izveden iz stvarnog stanja podataka;
    // ovo samo evidentira da je korisnik obradio taj konkretan slučaj.
    public partial class FrmOznaciUpozorenje : Form
    {
        private readonly OwnerTrackDbContext _db;
        private readonly WarningDetail _warning;

        public FrmOznaciUpozorenje(OwnerTrackDbContext db, WarningDetail warning)
        {
            _db = db;
            _warning = warning;
            InitializeComponent();
        }

        private void FrmOznaciUpozorenje_Load(object sender, EventArgs e)
        {
            AcceptButton = btnSpremi;
            CancelButton = btnOtkazi;

            lblOpis.Text = $"{_warning.Tip}: {_warning.ImePrezime}\nKlijent: {_warning.NazivFirme}\nDatum isteka: {_warning.DatumIsteka:dd.MM.yyyy.}";
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (!btnSpremi.Enabled) return;
            btnSpremi.Enabled = false;

            try
            {
                new WarningAcknowledgementService(_db).Acknowledge(
                    _warning.Tip, _warning.EntityId, _warning.DatumIsteka,
                    txtNapomena.Text, _warning.NazivFirme, _warning.ImePrezime);

                DialogHelper.ShowSaved(UiMessages.WarningAcknowledgedSaved);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex);
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
