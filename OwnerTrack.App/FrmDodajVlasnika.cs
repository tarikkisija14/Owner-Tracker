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
        private bool _dirty;

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
            AcceptButton = btnSpremi;
            CancelButton = btnOtkazi;

            bool isEditMode = _vlasnikId.HasValue;
            FormHelper.ApplyEditModeTitle(this, btnSpremi, isEditMode,
                UiMessages.VlasnikEditTitle, UiMessages.VlasnikAddTitle);

            dtDatumValjanosti.Checked = false;

            if (isEditMode)
                LoadVlasnik(_vlasnikId!.Value);

            FormHelper.AttachDirtyTracking(this, () => _dirty = true);
            FormClosing += FrmDodajVlasnika_FormClosing;
        }

        private void FrmDodajVlasnika_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK) return;
            if (!_dirty) return;

            if (MessageBox.Show(
                    UiMessages.UnsavedChangesPrompt,
                    UiMessages.UnsavedChangesTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void LoadVlasnik(int vlasnikId)
        {
            var v = _db.Vlasnici.Find(vlasnikId);
            if (v is null) return;

            txtImePrezime.Text = v.ImePrezime ?? string.Empty;
            txtProcetat.Text = v.ProcenatVlasnistva.ToString("F2");
            txtIzvorPodatka.Text = v.IzvorPodatka ?? string.Empty;

            if (v.DatumValjanostiDokumenta.HasValue)
            {
                dtDatumValjanosti.Value = v.DatumValjanostiDokumenta.Value;
                dtDatumValjanosti.Checked = true;
            }
            else
            {
                dtDatumValjanosti.Checked = false;
            }

            dtDatumUtvrdjivanja.Value = v.DatumUtvrdjivanja ?? DateTime.Now;
        }

       

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            // See FrmDodajKlijent.btnSpremi_Click for why this guard exists:
            // prevents a double-click from inserting/saving the same owner twice.
            if (!btnSpremi.Enabled) return;

            if (string.IsNullOrWhiteSpace(txtImePrezime.Text))
            {
                MessageBox.Show(UiMessages.VlasnikNameRequired);
                txtImePrezime.Focus();
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

            btnSpremi.Enabled = false;
            try
            {
                if (_vlasnikId.HasValue)
                    SaveChanges(_vlasnikId.Value, imePrezime, percentage);
                else
                    SaveNew(imePrezime, percentage);

                _dirty = false;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (DbUpdateConcurrencyException)
            {
                DialogHelper.ShowConcurrencyConflict();
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
            v.DatumValjanostiDokumenta = dtDatumValjanosti.Checked ? dtDatumValjanosti.Value : null;
            v.ProcenatVlasnistva = percentage;
            v.DatumUtvrdjivanja = dtDatumUtvrdjivanja.Value;
            v.IzvorPodatka = txtIzvorPodatka.Text;
        }

       

        private void SaveChanges(int vlasnikId, string imePrezime, decimal percentage)
        {
            var v = _db.Vlasnici.Find(vlasnikId);
            if (v is null) return;

            string previousName = v.ImePrezime ?? string.Empty;
            string? previousDatumValjanosti = FormatDate(v.DatumValjanostiDokumenta);
            string previousPercentage = v.ProcenatVlasnistva.ToString("F2");
            string? previousDatumUtvrdjivanja = FormatDate(v.DatumUtvrdjivanja);
            string? previousIzvor = v.IzvorPodatka;

            ApplyFormFieldsToVlasnik(v, imePrezime, percentage);
            v.Version++;

            string opis = AuditService.DescribeFieldChanges(imePrezime,
                ("Ime i prezime", previousName, imePrezime),
                ("Datum važenja dokumenta", previousDatumValjanosti, FormatDate(v.DatumValjanostiDokumenta)),
                ("% vlasništva", previousPercentage, percentage.ToString("F2")),
                ("Datum utvrđivanja", previousDatumUtvrdjivanja, FormatDate(v.DatumUtvrdjivanja)),
                ("Izvor podatka", previousIzvor, v.IzvorPodatka));

            TransactionHelper.SaveWithAudit(_db, () => _audit.LogUpdated("Vlasnici", vlasnikId, opis));

            DialogHelper.ShowSaved(UiMessages.VlasnikSavedUpdate);
        }

        private static string? FormatDate(DateTime? d) => d?.ToString("dd.MM.yyyy");

        private void SaveNew(string imePrezime, decimal percentage)
        {
            var v = new Vlasnik { KlijentId = _klijentId, Status = StatusEntiteta.AKTIVAN };
            ApplyFormFieldsToVlasnik(v, imePrezime, percentage);

            _db.Vlasnici.Add(v);
            TransactionHelper.SaveWithAudit(_db,
                () => _audit.LogAdded("Vlasnici", v.Id, $"Novi vlasnik: '{imePrezime}'"));

            DialogHelper.ShowSaved(UiMessages.VlasnikSavedNew);
        }
    }
}