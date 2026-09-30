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
        private bool _dirty;

        public FrmDodajKlijent(int? klijentId, OwnerTrackDbContext db)
        {
            InitializeComponent();
            _klijentId = klijentId;
            _db = db;
            _audit = new AuditService(db);
        }



        private void FrmDodajKlijent_Load(object sender, EventArgs e)
        {
            AcceptButton = btnSpremi;
            CancelButton = btnOtkazi;

            PopulateComboBoxes();
            LoadActivityCodes();

            // Datum nije unesen dok korisnik ne označi checkbox (novi klijent);
            // za izmjenu LoadKlijent postavlja stvarne vrijednosti.
            foreach (var dt in new[] { dtDatumUspostave, dtDatumOsnivanja, dtDatumProcjene, dtPepDatumProvjere, dtDatumUgovora })
                dt.Checked = false;

            if (_klijentId.HasValue)
            {
                LoadKlijent(_klijentId.Value);
                Text = UiMessages.KlijentEditTitle;
                btnSpremi.Text = UiMessages.KlijentSaveChangesButton;
            }

            // Prati promjene tek OD OVDJE — podaci upravo učitani u polja
            // (LoadKlijent, ResetComboBoxesToDefaults) ne smiju formu odmah
            // označiti kao "nesačuvanu".
            FormHelper.AttachDirtyTracking(scrollPanel, () => _dirty = true);
            FormClosing += FrmDodajKlijent_FormClosing;
        }

        private void FrmDodajKlijent_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK) return; // uspješno sačuvano — ne pitaj ništa
            if (!_dirty) return;

            if (MessageBox.Show(
                    UiMessages.UnsavedChangesPrompt,
                    UiMessages.UnsavedChangesTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void PopulateComboBoxes()
        {
            cbVrstaKlijenta.DataSource = Enum.GetValues<VrstaKlijenta>()
                .Select(v => new { Value = v.ToString(), Display = v.ToDisplay() })
                .ToList();
            cbVrstaKlijenta.DisplayMember = "Display";
            cbVrstaKlijenta.ValueMember = "Value";

            cbVelicina.DataSource = Enum.GetValues<VelicinaFirme>()
                .Select(v => new { Value = v.ToString(), Display = v.ToDisplay() })
                .ToList();
            cbVelicina.DisplayMember = "Display";
            cbVelicina.ValueMember = "Value";
            FormHelper.PopulateEnumComboWithEmpty<DaNe>(cbPepRizik);
            FormHelper.PopulateEnumComboWithEmpty<DaNe>(cbUboRizik);
            FormHelper.PopulateEnumComboWithEmpty<DaNe>(cbGotovinaRizik);
            FormHelper.PopulateEnumComboWithEmpty<DaNe>(cbGeografskiRizik);
            FormHelper.PopulateEnumCombo<StatusEntiteta>(cbStatus);
            // Arhiviranje ide isključivo kroz postojeći archive workflow (ArchivePresenter):
            // ručno postavljen status ARHIVIRAN ne postavlja Obrisan niti kaskadira na djecu.
            cbStatus.Items.Remove(StatusEntiteta.ARHIVIRAN.ToString());

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
            var codes = _db.Djelatnosti
                .OrderBy(d => d.Sifra)
                .Select(d => new { d.Sifra, Display = d.Sifra + " - " + d.Naziv })
                .ToList();
            cbSifra.DataSource = codes;
            cbSifra.DisplayMember = "Display";
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
            txtPepImePrezime.Text = k.PepImePrezime ?? string.Empty;
            txtPepFunkcija.Text = k.PepFunkcija ?? string.Empty;
            txtPepPovezanost.Text = k.PepPovezanost ?? string.Empty;
            txtPepMjerePoduzete.Text = k.PepMjerePoduzete ?? string.Empty;
            txtOpciIndikatoriRizika.Text = k.OpciIndikatoriRizika ?? string.Empty;
            txtIndikatoriIdentifikacijeRizika.Text = k.IndikatoriIdentifikacijeRizika ?? string.Empty;
            txtIndikatoriTransakcijaRizika.Text = k.IndikatoriTransakcijaRizika ?? string.Empty;

            if (!string.IsNullOrEmpty(k.SifraDjelatnosti))
                cbSifra.SelectedValue = k.SifraDjelatnosti;

            FormHelper.SetNullableDate(dtDatumUspostave, k.DatumUspostave);
            FormHelper.SetNullableDate(dtDatumOsnivanja, k.DatumOsnivanja);
            FormHelper.SetNullableDate(dtDatumProcjene, k.DatumProcjene);
            FormHelper.SetNullableDate(dtPepDatumProvjere, k.PepDatumProvjere);

            if (k.VrstaKlijenta.HasValue)
                cbVrstaKlijenta.SelectedValue = k.VrstaKlijenta.Value.ToString();
            if (!string.IsNullOrEmpty(k.Velicina))
                cbVelicina.SelectedValue = k.Velicina;
            FormHelper.SetCombo(cbPepRizik, k.PepRizik);
            FormHelper.SetCombo(cbUboRizik, k.UboRizik);
            FormHelper.SetCombo(cbGotovinaRizik, k.GotovinaRizik);
            FormHelper.SetCombo(cbGeografskiRizik, k.GeografskiRizik);
            // Klijent koji je već u statusu ARHIVIRAN (npr. ranije ručno postavljen) zadržava tu
            // stavku, da ga spremanje ne "vrati" tiho u AKTIVAN.
            if (k.Status == StatusEntiteta.ARHIVIRAN)
                cbStatus.Items.Add(StatusEntiteta.ARHIVIRAN.ToString());
            FormHelper.SetCombo(cbStatus, k.Status.ToString());

            if (k.Ugovor is not null)
            {
                txtVrstaUgovora.Text = k.Ugovor.VrstaUgovora ?? string.Empty;
                FormHelper.SetNullableDate(dtDatumUgovora, k.Ugovor.DatumUgovora);
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
            k.DatumUspostave = FormHelper.GetNullableDate(dtDatumUspostave);
            k.DatumOsnivanja = FormHelper.GetNullableDate(dtDatumOsnivanja);
            k.Velicina = cbVelicina.SelectedValue?.ToString() ?? string.Empty;
            k.PepRizik = FormHelper.NullIfEmpty(cbPepRizik.Text);
            k.UboRizik = FormHelper.NullIfEmpty(cbUboRizik.Text);
            k.GotovinaRizik = FormHelper.NullIfEmpty(cbGotovinaRizik.Text);
            k.GeografskiRizik = FormHelper.NullIfEmpty(cbGeografskiRizik.Text);
            k.UkupnaProcjena = txtUkupnaProcjena.Text;
            k.DatumProcjene = FormHelper.GetNullableDate(dtDatumProcjene);
            k.OvjeraCr = txtOvjeraCr.Text;
            k.Napomena = txtNapomena.Text;
            k.PepImePrezime = txtPepImePrezime.Text;
            k.PepFunkcija = txtPepFunkcija.Text;
            k.PepPovezanost = txtPepPovezanost.Text;
            k.PepMjerePoduzete = txtPepMjerePoduzete.Text;
            k.PepDatumProvjere = FormHelper.GetNullableDate(dtPepDatumProvjere);
            k.OpciIndikatoriRizika = txtOpciIndikatoriRizika.Text;
            k.IndikatoriIdentifikacijeRizika = txtIndikatoriIdentifikacijeRizika.Text;
            k.IndikatoriTransakcijaRizika = txtIndikatoriTransakcijaRizika.Text;
            k.Email = FormHelper.NullIfEmpty(txtEmail.Text);
            k.Telefon = FormHelper.NullIfEmpty(txtTelefon.Text);
            k.VrstaKlijenta = cbVrstaKlijenta.SelectedValue is string vName && Enum.TryParse<VrstaKlijenta>(vName, out var vk) ? vk : null;
            k.Status = Enum.TryParse<StatusEntiteta>(cbStatus.Text, out var se) ? se : StatusEntiteta.AKTIVAN;
        }

        private void ApplyFormFieldsToUgovor(Ugovor ugovor)
        {
            ugovor.VrstaUgovora = txtVrstaUgovora.Text;
            ugovor.StatusUgovora = cbStatusUgovora.Text;
            ugovor.DatumUgovora = FormHelper.GetNullableDate(dtDatumUgovora);
        }

       

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            // Guards against a double-click (or Enter-key repeat) firing this
            // handler twice before the first SaveChanges/SaveNew call returns
            // and the dialog closes — without this, a fast double-click could
            // insert the same client twice (or run SaveChanges concurrently)
            // since nothing else disables the button while the DB call runs.
            if (!btnSpremi.Enabled) return;

            if (string.IsNullOrWhiteSpace(txtNaziv.Text))
            {
                MessageBox.Show(UiMessages.KlijentNazivRequired);
                txtNaziv.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtIdBroj.Text))
            {
                MessageBox.Show(UiMessages.KlijentIdBrojRequired);
                txtIdBroj.Focus();
                return;
            }

            string naziv = txtNaziv.Text.Trim();
            string idBroj = txtIdBroj.Text.Trim();

            if (!ValidateFields(naziv, idBroj)) return;

            // Prazan status ugovora pri izmjeni briše postojeći Ugovor — traži potvrdu
            // (isti confirm helper kao za arhiviranje); "Ne" ostavlja formu otvorenom.
            if (_klijentId.HasValue &&
                string.IsNullOrWhiteSpace(cbStatusUgovora.Text) &&
                _db.Ugovori.Any(u => u.KlijentId == _klijentId.Value) &&
                !DialogHelper.ConfirmArchive(UiMessages.KlijentUgovorDeleteConfirm))
            {
                cbStatusUgovora.Focus();
                return;
            }

            btnSpremi.Enabled = false;
            try
            {
                if (_klijentId.HasValue)
                    SaveChanges(_klijentId.Value, naziv, idBroj);
                else
                    SaveNew(naziv, idBroj);

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
                DialogHelper.LogAndShowError(ex, "Greška pri snimanju");
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



        private void SaveChanges(int id, string naziv, string idBroj)
        {
            var k = _db.Klijenti.Find(id);
            if (k is null) return;

            string previousName = k.Naziv;
            var before = SnapshotKlijentFields(k);
            var ugovorBefore = _db.Ugovori.FirstOrDefault(u => u.KlijentId == id);
            string? previousVrstaUgovora = ugovorBefore?.VrstaUgovora;
            string? previousStatusUgovora = ugovorBefore?.StatusUgovora;
            string? previousDatumUgovora = FormatDate(ugovorBefore?.DatumUgovora);

            k.Naziv = naziv;
            k.IdBroj = idBroj;
            k.Azuriran = DateTime.Now;
            ApplyFormFieldsToKlijent(k);
            k.Version++;

            var ugovor = ugovorBefore;

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
                ugovor = null;
            }

            string opis = AuditService.DescribeFieldChanges(naziv,
                ("Naziv", previousName, naziv),
                ("ID broj", before.IdBroj, idBroj),
                ("Adresa", before.Adresa, k.Adresa),
                ("Šifra djelatnosti", before.SifraDjelatnosti, k.SifraDjelatnosti),
                ("Datum uspostave", before.DatumUspostave, FormatDate(k.DatumUspostave)),
                ("Datum osnivanja", before.DatumOsnivanja, FormatDate(k.DatumOsnivanja)),
                ("Veličina", before.Velicina, k.Velicina),
                ("Email", before.Email, k.Email),
                ("Telefon", before.Telefon, k.Telefon),
                ("Vrsta klijenta", before.VrstaKlijenta, k.VrstaKlijenta?.ToString()),
                ("Status", before.Status, k.Status.ToString()),
                ("PEP rizik", before.PepRizik, k.PepRizik),
                ("UBO rizik", before.UboRizik, k.UboRizik),
                ("Gotovina rizik", before.GotovinaRizik, k.GotovinaRizik),
                ("Geografski rizik", before.GeografskiRizik, k.GeografskiRizik),
                ("Ukupna procjena", before.UkupnaProcjena, k.UkupnaProcjena),
                ("Datum procjene", before.DatumProcjene, FormatDate(k.DatumProcjene)),
                ("Ovjera CR", before.OvjeraCr, k.OvjeraCr),
                ("PEP ime i prezime", before.PepImePrezime, k.PepImePrezime),
                ("PEP funkcija", before.PepFunkcija, k.PepFunkcija),
                ("PEP povezanost", before.PepPovezanost, k.PepPovezanost),
                ("PEP datum provjere", before.PepDatumProvjere, FormatDate(k.PepDatumProvjere)),
                ("PEP mjere poduzete", before.PepMjerePoduzete, k.PepMjerePoduzete),
                ("Opći indikatori rizika", before.OpciIndikatoriRizika, k.OpciIndikatoriRizika),
                ("Indikatori identifikacije rizika", before.IndikatoriIdentifikacijeRizika, k.IndikatoriIdentifikacijeRizika),
                ("Indikatori transakcija rizika", before.IndikatoriTransakcijaRizika, k.IndikatoriTransakcijaRizika),
                ("Napomena", before.Napomena, k.Napomena),
                ("Vrsta ugovora", previousVrstaUgovora, ugovor?.VrstaUgovora),
                ("Status ugovora", previousStatusUgovora, ugovor?.StatusUgovora),
                ("Datum ugovora", previousDatumUgovora, FormatDate(ugovor?.DatumUgovora)));

            TransactionHelper.SaveWithAudit(_db, () => _audit.LogUpdated("Klijenti", id, opis));

            DialogHelper.ShowSaved(UiMessages.KlijentSavedUpdate);
        }

        private static string? FormatDate(DateTime? d) => d?.ToString("dd.MM.yyyy");

        private readonly record struct KlijentFieldSnapshot(
            string? IdBroj, string? Adresa, string? SifraDjelatnosti, string? DatumUspostave, string? DatumOsnivanja,
            string? Velicina, string? Email, string? Telefon, string? VrstaKlijenta, string? Status,
            string? PepRizik, string? UboRizik, string? GotovinaRizik, string? GeografskiRizik,
            string? UkupnaProcjena, string? DatumProcjene, string? OvjeraCr,
            string? PepImePrezime, string? PepFunkcija, string? PepPovezanost, string? PepDatumProvjere,
            string? PepMjerePoduzete, string? OpciIndikatoriRizika, string? IndikatoriIdentifikacijeRizika,
            string? IndikatoriTransakcijaRizika, string? Napomena);

        private static KlijentFieldSnapshot SnapshotKlijentFields(Klijent k) => new(
            k.IdBroj, k.Adresa, k.SifraDjelatnosti, FormatDate(k.DatumUspostave), FormatDate(k.DatumOsnivanja),
            k.Velicina, k.Email, k.Telefon, k.VrstaKlijenta?.ToString(), k.Status.ToString(),
            k.PepRizik, k.UboRizik, k.GotovinaRizik, k.GeografskiRizik,
            k.UkupnaProcjena, FormatDate(k.DatumProcjene), k.OvjeraCr,
            k.PepImePrezime, k.PepFunkcija, k.PepPovezanost, FormatDate(k.PepDatumProvjere),
            k.PepMjerePoduzete, k.OpciIndikatoriRizika, k.IndikatoriIdentifikacijeRizika,
            k.IndikatoriTransakcijaRizika, k.Napomena);

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

            DialogHelper.ShowSaved(UiMessages.KlijentSavedNew);
        }
    }
}