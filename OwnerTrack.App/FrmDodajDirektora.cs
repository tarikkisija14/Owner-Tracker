using Microsoft.EntityFrameworkCore;
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
        private bool _dirty;

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
            AcceptButton = btnSpremi;
            CancelButton = btnOtkazi;

            cbTipValjanosti.Items.Clear();
            cbTipValjanosti.Items.Add(ValidityTypeConstants.Trajno);
            cbTipValjanosti.Items.Add(ValidityTypeConstants.Vremenski);
            cbTipValjanosti.SelectedIndex = 0;

            bool isEditMode = _direktorId.HasValue;
            FormHelper.ApplyEditModeTitle(this, btnSpremi, isEditMode,
                UiMessages.DirektorEditTitle, UiMessages.DirektorAddTitle);

            dtDatumValjanosti.Checked = false;

            if (isEditMode)
                LoadDirektor(_direktorId!.Value);

            FormHelper.AttachDirtyTracking(this, () => _dirty = true);
            FormClosing += FrmDodajDirektora_FormClosing;
        }

        private void FrmDodajDirektora_FormClosing(object? sender, FormClosingEventArgs e)
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

        private void LoadDirektor(int direktorId)
        {
            var d = _db.Direktori.Find(direktorId);
            if (d is null) return;

            txtImePrezime.Text = d.ImePrezime ?? string.Empty;
            txtJmbg.Text = d.Jmbg ?? string.Empty;

            if (d.DatumValjanosti.HasValue)
            {
                dtDatumValjanosti.Value = d.DatumValjanosti.Value;
                dtDatumValjanosti.Checked = true;
            }
            else
            {
                dtDatumValjanosti.Checked = false;
            }

            cbTipValjanosti.Text = d.TipValjanosti ?? ValidityTypeConstants.Trajno;

        }


        private void cbTipValjanosti_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        private void btnSpremi_Click(object sender, EventArgs e)
        {
            // See FrmDodajKlijent.btnSpremi_Click for why this guard exists:
            // prevents a double-click from inserting/saving the same director twice.
            if (!btnSpremi.Enabled) return;

            if (string.IsNullOrWhiteSpace(txtImePrezime.Text))
            {
                MessageBox.Show(UiMessages.DirektorNameRequired);
                txtImePrezime.Focus();
                return;
            }

            string imePrezime = txtImePrezime.Text.Trim();
            int currentId = _direktorId ?? 0;

            if (_db.Set<Direktor>().IgnoreQueryFilters()
                    .Any(d => d.KlijentId == _klijentId
                           && d.ImePrezime == imePrezime
                           && d.Id != currentId
                           && d.Obrisan == null))
            {
                MessageBox.Show(
                    string.Format(UiMessages.DirektorDuplicateFormat, imePrezime),
                    UiMessages.DirektorDuplicateTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtImePrezime.Focus();
                return;
            }

            DateTime? dateOfValidity = dtDatumValjanosti.Checked ? dtDatumValjanosti.Value : null;

            btnSpremi.Enabled = false;
            try
            {
                if (_direktorId.HasValue)
                    SaveChanges(_direktorId.Value, dateOfValidity);
                else
                    SaveNew(dateOfValidity);

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
            string? previousDatumValjanosti = FormatDate(d.DatumValjanosti);
            string? previousTipValjanosti = d.TipValjanosti;
            string? previousJmbg = d.Jmbg;

            ApplyFormFieldsToDirektor(d, dateOfValidity);
            d.Version++;

            string opis = AuditService.DescribeFieldChanges(d.ImePrezime ?? previousName,
                ("Ime i prezime", previousName, d.ImePrezime),
                ("Datum važenja", previousDatumValjanosti, FormatDate(d.DatumValjanosti)),
                ("Tip valjanosti", previousTipValjanosti, d.TipValjanosti),
                ("JMBG", previousJmbg, d.Jmbg));

            TransactionHelper.SaveWithAudit(_db, () => _audit.LogUpdated("Direktori", direktorId, opis));

            DialogHelper.ShowSaved(UiMessages.DirektorSavedUpdate);
        }

        private static string? FormatDate(DateTime? d) => d?.ToString("dd.MM.yyyy");

        private void SaveNew(DateTime? dateOfValidity)
        {
            var d = new Direktor { KlijentId = _klijentId, Status = StatusEntiteta.AKTIVAN };
            ApplyFormFieldsToDirektor(d, dateOfValidity);

            _db.Direktori.Add(d);
            TransactionHelper.SaveWithAudit(_db,
                () => _audit.LogAdded("Direktori", d.Id, $"Novi direktor: '{d.ImePrezime}'"));

            DialogHelper.ShowSaved(UiMessages.DirektorSavedNew);
        }
    }
}