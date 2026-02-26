using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using OwnerTrack.Infrastructure.Validators;
using System;
using System.Linq;
using System.Windows.Forms;

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
            LoadComboValues();
            LoadSifre();
            if (_klijentId.HasValue)
            {
                LoadKlijent(_klijentId.Value);
                Text = "Izmijeni firmu";
                btnSpremi.Text = "💾 Spremi izmjene";
            }
        }

        private void LoadComboValues()
        {
            cbVrstaKlijenta.Items.Clear();
            foreach (VrstaKlijenta v in Enum.GetValues(typeof(VrstaKlijenta)))
                cbVrstaKlijenta.Items.Add(v.ToString());

            cbVelicina.Items.Clear();
            foreach (VelicinaFirme v in Enum.GetValues(typeof(VelicinaFirme)))
                cbVelicina.Items.Add(v.ToString());

            cbPepRizik.Items.Clear();
            cbPepRizik.Items.Add("");
            foreach (DaNe v in Enum.GetValues(typeof(DaNe)))
                cbPepRizik.Items.Add(v.ToString());

            cbUboRizik.Items.Clear();
            cbUboRizik.Items.Add("");
            foreach (DaNe v in Enum.GetValues(typeof(DaNe)))
                cbUboRizik.Items.Add(v.ToString());

            cbGotovinaRizik.Items.Clear();
            cbGotovinaRizik.Items.Add("");
            foreach (DaNe v in Enum.GetValues(typeof(DaNe)))
                cbGotovinaRizik.Items.Add(v.ToString());

            cbGeografskiRizik.Items.Clear();
            cbGeografskiRizik.Items.Add("");
            foreach (DaNe v in Enum.GetValues(typeof(DaNe)))
                cbGeografskiRizik.Items.Add(v.ToString());

            cbStatusUgovora.Items.Clear();
            cbStatusUgovora.Items.AddRange(new[] { "", "POTPISAN", "ANEKS", "OTKAZAN", "NEMA UGOVOR", "NEAKTIVAN" });

            cbStatus.Items.Clear();
            foreach (StatusEntiteta v in Enum.GetValues(typeof(StatusEntiteta)))
                cbStatus.Items.Add(v.ToString());

            if (!_klijentId.HasValue)
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
        }

        private void LoadSifre()
        {
            var sifre = _db.Djelatnosti.OrderBy(d => d.Sifra).ToList();
            cbSifra.DataSource = sifre;
            cbSifra.DisplayMember = "Naziv";
            cbSifra.ValueMember = "Sifra";
            if (sifre.Count > 0 && !_klijentId.HasValue)
                cbSifra.SelectedIndex = 0;
        }

        private void LoadKlijent(int id)
        {
            var k = _db.Klijenti.Include(x => x.Ugovor).FirstOrDefault(x => x.Id == id);
            if (k == null) return;

            txtNaziv.Text = k.Naziv ?? "";
            txtIdBroj.Text = k.IdBroj ?? "";
            txtAdresa.Text = k.Adresa ?? "";
            if (!string.IsNullOrEmpty(k.SifraDjelatnosti))
                cbSifra.SelectedValue = k.SifraDjelatnosti;
            dtDatumUspostave.Value = k.DatumUspostave ?? DateTime.Now;
            SetCombo(cbVrstaKlijenta, k.VrstaKlijenta);
            dtDatumOsnivanja.Value = k.DatumOsnivanja ?? DateTime.Now;
            SetCombo(cbVelicina, k.Velicina);
            SetCombo(cbPepRizik, k.PepRizik);
            SetCombo(cbUboRizik, k.UboRizik);
            SetCombo(cbGotovinaRizik, k.GotovinaRizik);
            SetCombo(cbGeografskiRizik, k.GeografskiRizik);
            txtUkupnaProcjena.Text = k.UkupnaProcjena ?? "";
            dtDatumProcjene.Value = k.DatumProcjene ?? DateTime.Now;
            txtOvjeraCr.Text = k.OvjeraCr ?? "";
            SetCombo(cbStatus, k.Status);
            txtNapomena.Text = k.Napomena ?? "";
            txtEmail.Text = k.Email ?? "";
            txtTelefon.Text = k.Telefon ?? "";

            if (k.Ugovor != null)
            {
                txtVrstaUgovora.Text = k.Ugovor.VrstaUgovora ?? "";
                SetCombo(cbStatusUgovora, k.Ugovor.StatusUgovora);
                dtDatumUgovora.Value = k.Ugovor.DatumUgovora ?? DateTime.Now;
            }
        }

        private void SetCombo(ComboBox cb, string? value)
        {
            if (string.IsNullOrEmpty(value)) { cb.SelectedIndex = 0; return; }
            int idx = cb.FindStringExact(value);

            cb.SelectedIndex = idx >= 0 ? idx : 0;
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNaziv.Text) || string.IsNullOrWhiteSpace(txtIdBroj.Text))
            {
                MessageBox.Show("Popuni obavezna polja: Naziv i ID Broj!");
                return;
            }


            string idBroj = txtIdBroj.Text.Trim();
            string naziv = txtNaziv.Text.Trim();

            string? jibGreska = JibValidator.GreškaValidacije(idBroj);
            if (jibGreska != null)
            {
                MessageBox.Show(jibGreska, "Greška validacije",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdBroj.Focus();
                return;
            }

            int trenutniId = _klijentId ?? 0;

            if (_db.Set<OwnerTrack.Data.Entities.Klijent>().IgnoreQueryFilters()
                    .Any(k => k.IdBroj == idBroj && k.Id != trenutniId && k.Obrisan == null))
            {
                MessageBox.Show($"Klijent s ID brojem '{idBroj}' već postoji!",
                    "Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdBroj.Focus();
                return;
            }

            
            if (_db.Set<OwnerTrack.Data.Entities.Klijent>().IgnoreQueryFilters()
                    .Any(k => k.Naziv == naziv && k.Id != trenutniId && k.Obrisan == null))
            {
                MessageBox.Show($"Klijent s nazivom '{naziv}' već postoji!",
                    "Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNaziv.Focus();
                return;
            }

            try
            {
                if (_klijentId.HasValue)
                    SpremiIzmjenu(_klijentId.Value, naziv, idBroj);
                else
                    SpremiNovog(naziv, idBroj);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        private void SpremiIzmjenu(int id, string naziv, string idBroj)
        {
            var k = _db.Klijenti.Find(id);
            if (k == null) return;

            string stariNaziv = k.Naziv;
            k.Naziv = naziv;
            k.IdBroj = idBroj;
            k.Adresa = txtAdresa.Text;
            k.SifraDjelatnosti = cbSifra.SelectedValue?.ToString() ?? "";
            k.DatumUspostave = dtDatumUspostave.Value;
            k.VrstaKlijenta = cbVrstaKlijenta.Text;
            k.DatumOsnivanja = dtDatumOsnivanja.Value;
            k.Velicina = cbVelicina.Text;
            k.PepRizik = Null(cbPepRizik.Text);
            k.UboRizik = Null(cbUboRizik.Text);
            k.GotovinaRizik = Null(cbGotovinaRizik.Text);
            k.GeografskiRizik = Null(cbGeografskiRizik.Text);
            k.UkupnaProcjena = txtUkupnaProcjena.Text;
            k.DatumProcjene = dtDatumProcjene.Value;
            k.OvjeraCr = txtOvjeraCr.Text;
            k.Status = cbStatus.Text;
            k.Napomena = txtNapomena.Text;
            k.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            k.Telefon = string.IsNullOrWhiteSpace(txtTelefon.Text) ? null : txtTelefon.Text.Trim();
            k.Azuriran = DateTime.Now;

            var ugovor = _db.Ugovori.FirstOrDefault(u => u.KlijentId == id);
            if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
            {
                if (ugovor == null) { ugovor = new Ugovor { KlijentId = id }; _db.Ugovori.Add(ugovor); }
                ugovor.VrstaUgovora = txtVrstaUgovora.Text;
                ugovor.StatusUgovora = cbStatusUgovora.Text;
                ugovor.DatumUgovora = dtDatumUgovora.Value;
            }
            else if (ugovor != null)
            {
                _db.Ugovori.Remove(ugovor);
            }

            _audit.Izmijenjeno("Klijenti", id, $"'{stariNaziv}' → '{naziv}'");

            using var tx = _db.Database.BeginTransaction();
            try
            {
                _db.SaveChanges();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }

            MessageBox.Show("Klijent ažuriran!");
        }

        private void SpremiNovog(string naziv, string idBroj)
        {
            var k = new Klijent
            {
                Naziv = naziv,
                IdBroj = idBroj,
                Adresa = txtAdresa.Text,
                SifraDjelatnosti = cbSifra.SelectedValue?.ToString() ?? "",
                DatumUspostave = dtDatumUspostave.Value,
                VrstaKlijenta = cbVrstaKlijenta.Text,
                DatumOsnivanja = dtDatumOsnivanja.Value,
                Velicina = cbVelicina.Text,
                PepRizik = Null(cbPepRizik.Text),
                UboRizik = Null(cbUboRizik.Text),
                GotovinaRizik = Null(cbGotovinaRizik.Text),
                GeografskiRizik = Null(cbGeografskiRizik.Text),
                UkupnaProcjena = txtUkupnaProcjena.Text,
                DatumProcjene = dtDatumProcjene.Value,
                OvjeraCr = txtOvjeraCr.Text,
                Status = cbStatus.Text,
                Napomena = txtNapomena.Text,
                Kreiran = DateTime.Now,
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                Telefon = string.IsNullOrWhiteSpace(txtTelefon.Text) ? null : txtTelefon.Text.Trim(),
            };

            using var tx = _db.Database.BeginTransaction();
            try
            {
                _db.Klijenti.Add(k);
                _db.SaveChanges();

                _audit.Dodano("Klijenti", k.Id, $"Novi klijent: '{naziv}' ({idBroj})");

                if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
                {
                    _db.Ugovori.Add(new Ugovor
                    {
                        KlijentId = k.Id,
                        VrstaUgovora = txtVrstaUgovora.Text,
                        StatusUgovora = cbStatusUgovora.Text,
                        DatumUgovora = dtDatumUgovora.Value
                    });
                }

                _db.SaveChanges();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }

            MessageBox.Show("Klijent dodan!");
        }

        private static string? Null(string s) =>
            string.IsNullOrWhiteSpace(s) ? null : s;

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtTelefon_TextChanged(object sender, EventArgs e)
        {

        }
    }
}