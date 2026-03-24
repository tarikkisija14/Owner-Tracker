using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    public partial class FrmDodajDirektora : Form
    {
        private readonly OwnerTrackDbContext _db;
        private readonly AuditService _audit;
        private readonly int _klijentId;
        private readonly int? _direktorId;

        public FrmDodajDirektora(int klijentId, int? direktorId, OwnerTrackDbContext db)
        {
            InitializeComponent();
            _klijentId = klijentId;
            _direktorId = direktorId;
            _db = db;
            _audit = new AuditService(db);
        }

       

        private void FrmDodajDirektora_Load(object sender, EventArgs e)
        {
            cbTipValjanosti.Items.Clear();
            cbTipValjanosti.Items.Add(ValidityTypeConstants.Trajno);
            cbTipValjanosti.Items.Add(ValidityTypeConstants.Vremenski);
            cbTipValjanosti.SelectedIndex = 0;

            bool isEditMode = _direktorId.HasValue;
            Text = isEditMode ? UiMessages.DirektorEditTitle : UiMessages.DirektorAddTitle;
            btnSpremi.Text = isEditMode ? UiMessages.KlijentSaveChangesButton : UiMessages.KlijentSaveNewButton;

            if (isEditMode)
                LoadDirektor(_direktorId!.Value);
        }

        private void LoadDirektor(int direktorId)
        {
            var d = _db.Direktori.Find(direktorId);
            if (d is null) return;

            txtImePrezime.Text = d.ImePrezime ?? string.Empty;
            txtJmbg.Text = d.Jmbg ?? string.Empty;
            dtDatumValjanosti.Value = d.DatumValjanosti ?? DateTime.Now;
            cbTipValjanosti.Text = d.TipValjanosti ?? ValidityTypeConstants.Trajno;

            dtDatumValjanosti.Enabled = d.TipValjanosti == ValidityTypeConstants.Vremenski;
        }


        private void cbTipValjanosti_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtDatumValjanosti.Enabled = cbTipValjanosti.Text == ValidityTypeConstants.Vremenski;
        }
 

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImePrezime.Text))
            {
                MessageBox.Show(UiMessages.DirektorNameRequired);
                return;
            }

            bool isPermanent = cbTipValjanosti.Text == ValidityTypeConstants.Trajno;
            DateTime? dateOfValidity = isPermanent ? null : dtDatumValjanosti.Value;

            try
            {
                if (_direktorId.HasValue)
                    SaveChanges(_direktorId.Value, dateOfValidity);
                else
                    SaveNew(dateOfValidity);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex);
            }
        }

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

       

        private void ApplyFormFieldsToDirektor(Direktor d, DateTime? dateOfValidity)
        {
            d.ImePrezime = txtImePrezime.Text.Trim();
            d.DatumValjanosti = dateOfValidity;
            d.TipValjanosti = cbTipValjanosti.Text;
            d.Jmbg = FormHelper.NullIfEmpty(txtJmbg.Text);
        }

        

        private void SaveChanges(int direktorId, DateTime? dateOfValidity)
        {
            var d = _db.Direktori.Find(direktorId);
            if (d is null) return;

            string previousName = d.ImePrezime ?? string.Empty;
            ApplyFormFieldsToDirektor(d, dateOfValidity);

            TransactionHelper.Execute(_db, db =>
            {
                db.SaveChanges();
                _audit.LogUpdated("Direktori", direktorId, $"'{previousName}' → '{d.ImePrezime}'");
                db.SaveChanges();
            });

            MessageBox.Show(UiMessages.DirektorSavedUpdate);
        }

        private void SaveNew(DateTime? dateOfValidity)
        {
            var d = new Direktor { KlijentId = _klijentId, Status = StatusEntiteta.AKTIVAN };
            ApplyFormFieldsToDirektor(d, dateOfValidity);

            TransactionHelper.Execute(_db, db =>
            {
                db.Direktori.Add(d);
                db.SaveChanges();
                _audit.LogAdded("Direktori", d.Id, $"Novi direktor: '{d.ImePrezime}'");
                db.SaveChanges();
            });

            MessageBox.Show(UiMessages.DirektorSavedNew);
        }
    }
}