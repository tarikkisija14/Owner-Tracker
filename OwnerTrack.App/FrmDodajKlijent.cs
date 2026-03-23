using Microsoft.EntityFrameworkCore;
using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using OwnerTrack.Infrastructure.Validators;

namespace OwnerTrack.App
{
    public partial class FrmDodajKlijent : Form
    {
        private readonly OwnerTrackDbContext _db;
        private readonly AuditService _audit;
        private readonly int? _klijentId;

        public FrmDodajKlijent(int? klijentId, OwnerTrackDbContext db)
        {
            InitializeComponent();
            _klijentId = klijentId;
            _db = db;
            _audit = new AuditService(db);
        }

       

        private void FrmDodajKlijent_Load(object sender, EventArgs e)
        {
            PopulateComboBoxes();
            LoadActivityCodes();

            if (_klijentId.HasValue)
            {
                LoadKlijent(_klijentId.Value);
                Text = UiMessages.KlijentEditTitle;
                btnSpremi.Text = UiMessages.KlijentSaveChangesButton;
            }
        }

        private void PopulateComboBoxes()
        {
            FormHelper.PopulateEnumCombo<VrstaKlijenta>(cbVrstaKlijenta);
            FormHelper.PopulateEnumCombo<VelicinaFirme>(cbVelicina);
            FormHelper.PopulateEnumComboWithEmpty<DaNe>(cbPepRizik);
            FormHelper.PopulateEnumComboWithEmpty<DaNe>(cbUboRizik);
            FormHelper.PopulateEnumComboWithEmpty<DaNe>(cbGotovinaRizik);
            FormHelper.PopulateEnumComboWithEmpty<DaNe>(cbGeografskiRizik);
            FormHelper.PopulateEnumCombo<StatusEntiteta>(cbStatus);

            cbStatusUgovora.Items.Clear();
            cbStatusUgovora.Items.AddRange(ContractStatus.Svi);
            cbStatusUgovora.Items.Insert(0, string.Empty);

            if (!_klijentId.HasValue)
                ResetComboBoxesToDefaults();
        }

        private void ResetComboBoxesToDefaults()
        {
            cbVrstaKlijenta.SelectedIndex = 0;
            cbVelicina.SelectedIndex = 0;
            cbPepRizik.SelectedIndex = 0;
            cbUboRizik.SelectedIndex = 0;
            cbGotovinaRizik.SelectedIndex = 0;
            cbGeografskiRizik.SelectedIndex = 0;
            cbStatusUgovora.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
        }

        private void LoadActivityCodes()
        {
            var codes = _db.Djelatnosti.OrderBy(d => d.Sifra).ToList();
            cbSifra.DataSource = codes;
            cbSifra.DisplayMember = "Naziv";
            cbSifra.ValueMember = "Sifra";

            if (codes.Count > 0 && !_klijentId.HasValue)
                cbSifra.SelectedIndex = 0;
        }

        private void LoadKlijent(int id)
        {
            var k = _db.Klijenti.Include(x => x.Ugovor).FirstOrDefault(x => x.Id == id);
            if (k is null) return;

            txtNaziv.Text = k.Naziv ?? string.Empty;
            txtIdBroj.Text = k.IdBroj ?? string.Empty;
            txtAdresa.Text = k.Adresa ?? string.Empty;
            txtEmail.Text = k.Email ?? string.Empty;
            txtTelefon.Text = k.Telefon ?? string.Empty;
            txtOvjeraCr.Text = k.OvjeraCr ?? string.Empty;
            txtUkupnaProcjena.Text = k.UkupnaProcjena ?? string.Empty;
            txtNapomena.Text = k.Napomena ?? string.Empty;

            if (!string.IsNullOrEmpty(k.SifraDjelatnosti))
                cbSifra.SelectedValue = k.SifraDjelatnosti;

            dtDatumUspostave.Value = k.DatumUspostave ?? DateTime.Now;
            dtDatumOsnivanja.Value = k.DatumOsnivanja ?? DateTime.Now;
            dtDatumProcjene.Value = k.DatumProcjene ?? DateTime.Now;

            FormHelper.SetCombo(cbVrstaKlijenta, k.VrstaKlijenta?.ToString());
            FormHelper.SetCombo(cbVelicina, k.Velicina);
            FormHelper.SetCombo(cbPepRizik, k.PepRizik);
            FormHelper.SetCombo(cbUboRizik, k.UboRizik);
            FormHelper.SetCombo(cbGotovinaRizik, k.GotovinaRizik);
            FormHelper.SetCombo(cbGeografskiRizik, k.GeografskiRizik);
            FormHelper.SetCombo(cbStatus, k.Status.ToString());

            if (k.Ugovor is not null)
            {
                txtVrstaUgovora.Text = k.Ugovor.VrstaUgovora ?? string.Empty;
                dtDatumUgovora.Value = k.Ugovor.DatumUgovora ?? DateTime.Now;
                FormHelper.SetCombo(cbStatusUgovora, k.Ugovor.StatusUgovora);
            }
        }

      
        private bool ValidateFields(string naziv, string idBroj)
        {
            string? jibError = JibValidator.GetValidationError(idBroj);
            if (jibError is not null)
            {
                MessageBox.Show(jibError, "Greška validacije", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdBroj.Focus();
                return false;
            }

            int currentId = _klijentId ?? 0;

            if (_db.Set<Klijent>().IgnoreQueryFilters()
                    .Any(k => k.IdBroj == idBroj && k.Id != currentId && k.Obrisan == null))
            {
                MessageBox.Show(
                    string.Format(UiMessages.KlijentDuplicateIdBrojFormat, idBroj),
                    UiMessages.KlijentDuplicateTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdBroj.Focus();
                return false;
            }

            if (_db.Set<Klijent>().IgnoreQueryFilters()
                    .Any(k => k.Naziv == naziv && k.Id != currentId && k.Obrisan == null))
            {
                MessageBox.Show(
                    string.Format(UiMessages.KlijentDuplicateNazivFormat, naziv),
                    UiMessages.KlijentDuplicateTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNaziv.Focus();
                return false;
            }

            return true;
        }

        

        private void ApplyFormFieldsToKlijent(Klijent k)
        {
            k.Adresa = txtAdresa.Text;
            k.SifraDjelatnosti = cbSifra.SelectedValue?.ToString() ?? string.Empty;
            k.DatumUspostave = dtDatumUspostave.Value;
            k.DatumOsnivanja = dtDatumOsnivanja.Value;
            k.Velicina = cbVelicina.Text;
            k.PepRizik = FormHelper.NullIfEmpty(cbPepRizik.Text);
            k.UboRizik = FormHelper.NullIfEmpty(cbUboRizik.Text);
            k.GotovinaRizik = FormHelper.NullIfEmpty(cbGotovinaRizik.Text);
            k.GeografskiRizik = FormHelper.NullIfEmpty(cbGeografskiRizik.Text);
            k.UkupnaProcjena = txtUkupnaProcjena.Text;
            k.DatumProcjene = dtDatumProcjene.Value;
            k.OvjeraCr = txtOvjeraCr.Text;
            k.Napomena = txtNapomena.Text;
            k.Email = FormHelper.NullIfEmpty(txtEmail.Text);
            k.Telefon = FormHelper.NullIfEmpty(txtTelefon.Text);
            k.VrstaKlijenta = Enum.TryParse<VrstaKlijenta>(cbVrstaKlijenta.Text, out var vk) ? vk : null;
            k.Status = Enum.TryParse<StatusEntiteta>(cbStatus.Text, out var se) ? se : StatusEntiteta.AKTIVAN;
        }

        private void ApplyFormFieldsToUgovor(Ugovor ugovor)
        {
            ugovor.VrstaUgovora = txtVrstaUgovora.Text;
            ugovor.StatusUgovora = cbStatusUgovora.Text;
            ugovor.DatumUgovora = dtDatumUgovora.Value;
        }

        

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNaziv.Text) || string.IsNullOrWhiteSpace(txtIdBroj.Text))
            {
                MessageBox.Show(UiMessages.KlijentRequiredFields);
                return;
            }

            string naziv = txtNaziv.Text.Trim();
            string idBroj = txtIdBroj.Text.Trim();

            if (!ValidateFields(naziv, idBroj)) return;

            try
            {
                if (_klijentId.HasValue)
                    SaveChanges(_klijentId.Value, naziv, idBroj);
                else
                    SaveNew(naziv, idBroj);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri snimanju");
            }
        }

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        

        private void SaveChanges(int id, string naziv, string idBroj)
        {
            var k = _db.Klijenti.Find(id);
            if (k is null) return;

            string previousName = k.Naziv;
            k.Naziv = naziv;
            k.IdBroj = idBroj;
            k.Azuriran = DateTime.Now;
            ApplyFormFieldsToKlijent(k);

            var ugovor = _db.Ugovori.FirstOrDefault(u => u.KlijentId == id);

            if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
            {
                if (ugovor is null)
                {
                    ugovor = new Ugovor { KlijentId = id };
                    _db.Ugovori.Add(ugovor);
                }
                ApplyFormFieldsToUgovor(ugovor);
            }
            else if (ugovor is not null)
            {
                _db.Ugovori.Remove(ugovor);
            }

            TransactionHelper.Execute(_db, db =>
            {
                db.SaveChanges();
                _audit.LogUpdated("Klijenti", id, $"'{previousName}' → '{naziv}'");
                db.SaveChanges();
            });

            MessageBox.Show(UiMessages.KlijentSavedUpdate);
        }

        private void SaveNew(string naziv, string idBroj)
        {
            var k = new Klijent { Naziv = naziv, IdBroj = idBroj, Kreiran = DateTime.Now };
            ApplyFormFieldsToKlijent(k);

            TransactionHelper.Execute(_db, db =>
            {
                db.Klijenti.Add(k);
                db.SaveChanges();

                _audit.LogAdded("Klijenti", k.Id, $"Novi klijent: '{naziv}' ({idBroj})");

                if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
                {
                    var ugovor = new Ugovor { KlijentId = k.Id };
                    ApplyFormFieldsToUgovor(ugovor);
                    db.Ugovori.Add(ugovor);
                }

                db.SaveChanges();
            });

            MessageBox.Show(UiMessages.KlijentSavedNew);
        }
    }
}