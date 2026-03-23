using Microsoft.EntityFrameworkCore;
using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    public partial class FrmDodajVlasnika : Form
    {
        private readonly OwnerTrackDbContext _db;
        private readonly AuditService _audit;
        private readonly int _klijentId;
        private readonly int? _vlasnikId;

        public FrmDodajVlasnika(int klijentId, int? vlasnikId, OwnerTrackDbContext db)
        {
            InitializeComponent();
            _klijentId = klijentId;
            _vlasnikId = vlasnikId;
            _db = db;
            _audit = new AuditService(db);
        }

       

        private void FrmDodajVlasnika_Load(object sender, EventArgs e)
        {
            bool isEditMode = _vlasnikId.HasValue;
            Text = isEditMode ? UiMessages.VlasnikEditTitle : UiMessages.VlasnikAddTitle;
            btnSpremi.Text = isEditMode ? UiMessages.KlijentSaveChangesButton : UiMessages.KlijentSaveNewButton;

            if (isEditMode)
                LoadVlasnik(_vlasnikId!.Value);
        }

        private void LoadVlasnik(int vlasnikId)
        {
            var v = _db.Vlasnici.Find(vlasnikId);
            if (v is null) return;

            txtImePrezime.Text = v.ImePrezime ?? string.Empty;
            txtProcetat.Text = v.ProcenatVlasnistva.ToString("F2");
            txtIzvorPodatka.Text = v.IzvorPodatka ?? string.Empty;
            dtDatumValjanosti.Value = v.DatumValjanostiDokumenta ?? DateTime.Now;
            dtDatumUtvrdjivanja.Value = v.DatumUtvrdjivanja ?? DateTime.Now;
        }

        

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImePrezime.Text))
            {
                MessageBox.Show(UiMessages.VlasnikNameRequired);
                return;
            }

            if (!TryParsePercentage(out decimal percentage)) return;

            string imePrezime = txtImePrezime.Text.Trim();
            int currentId = _vlasnikId ?? 0;

            if (_db.Set<Vlasnik>().IgnoreQueryFilters()
                    .Any(v => v.KlijentId == _klijentId
                           && v.ImePrezime == imePrezime
                           && v.Id != currentId
                           && v.Obrisan == null))
            {
                MessageBox.Show(
                    string.Format(UiMessages.VlasnikDuplicateFormat, imePrezime),
                    UiMessages.VlasnikDuplicateTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtImePrezime.Focus();
                return;
            }

            try
            {
                if (_vlasnikId.HasValue)
                    SaveChanges(_vlasnikId.Value, imePrezime, percentage);
                else
                    SaveNew(imePrezime, percentage);

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

       

        private bool TryParsePercentage(out decimal percentage)
        {
            string normalised = txtProcetat.Text.Replace(",", ".").Trim();
            bool valid = decimal.TryParse(normalised,
                                    System.Globalization.NumberStyles.Number,
                                    System.Globalization.CultureInfo.InvariantCulture,
                                    out percentage)
                                && percentage is >= 0 and <= 100;

            if (!valid)
            {
                MessageBox.Show(
                    UiMessages.VlasnikPercentageError,
                    UiMessages.VlasnikPercentageErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProcetat.Focus();
                percentage = 0;
            }

            return valid;
        }

        

        private void ApplyFormFieldsToVlasnik(Vlasnik v, string imePrezime, decimal percentage)
        {
            v.ImePrezime = imePrezime;
            v.DatumValjanostiDokumenta = dtDatumValjanosti.Value;
            v.ProcenatVlasnistva = percentage;
            v.DatumUtvrdjivanja = dtDatumUtvrdjivanja.Value;
            v.IzvorPodatka = txtIzvorPodatka.Text;
        }

        

        private void SaveChanges(int vlasnikId, string imePrezime, decimal percentage)
        {
            var v = _db.Vlasnici.Find(vlasnikId);
            if (v is null) return;

            string previousName = v.ImePrezime ?? string.Empty;
            ApplyFormFieldsToVlasnik(v, imePrezime, percentage);

            TransactionHelper.Execute(_db, db =>
            {
                db.SaveChanges();
                _audit.LogUpdated("Vlasnici", vlasnikId, $"'{previousName}' → '{imePrezime}'");
                db.SaveChanges();
            });

            MessageBox.Show(UiMessages.VlasnikSavedUpdate);
        }

        private void SaveNew(string imePrezime, decimal percentage)
        {
            var v = new Vlasnik { KlijentId = _klijentId, Status = StatusEntiteta.AKTIVAN };
            ApplyFormFieldsToVlasnik(v, imePrezime, percentage);

            TransactionHelper.Execute(_db, db =>
            {
                db.Vlasnici.Add(v);
                db.SaveChanges();
                _audit.LogAdded("Vlasnici", v.Id, $"Novi vlasnik: '{imePrezime}'");
                db.SaveChanges();
            });

            MessageBox.Show(UiMessages.VlasnikSavedNew);
        }
    }
}