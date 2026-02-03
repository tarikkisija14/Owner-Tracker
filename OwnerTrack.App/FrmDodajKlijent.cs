using OwnerTrack.Data.Entities;
using OwnerTrack.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class FrmDodajKlijent : Form
    {
        private OwnerTrackDbContext _db;
        private int? _klijentId;

        public FrmDodajKlijent(int? klijentId, OwnerTrackDbContext db)
        {
            InitializeComponent();
            _klijentId = klijentId;
            _db = db;
        }

        private void FrmDodajKlijent_Load(object sender, EventArgs e)
        {
            LoadComboValues();
            LoadSifre();

            if (_klijentId.HasValue)
            {
                LoadKlijent(_klijentId.Value);
                this.Text = "Izmijeni firmu";
                btnSpremi.Text = "💾 Spremi izmjene";
            }
        }

        private void LoadComboValues()
        {
            // Popunjavanje ComboBox-ova sa vrijednostima
            cbVrstaKlijenta.Items.Clear();
            cbVrstaKlijenta.Items.AddRange(new[] { "PRAVNO LICE", "FIZIČKA OSOBA" });

            cbVelicina.Items.Clear();
            cbVelicina.Items.AddRange(new[] { "MIKRO", "MALO", "SREDNJE", "VELIKA" });

            cbPepRizik.Items.Clear();
            cbPepRizik.Items.AddRange(new[] { "", "DA", "NE" });

            cbUboRizik.Items.Clear();
            cbUboRizik.Items.AddRange(new[] { "", "DA", "NE" });

            cbGotovinaRizik.Items.Clear();
            cbGotovinaRizik.Items.AddRange(new[] { "", "DA", "NE" });

            cbGeografskiRizik.Items.Clear();
            cbGeografskiRizik.Items.AddRange(new[] { "", "DA", "NE" });

            cbStatusUgovora.Items.Clear();
            cbStatusUgovora.Items.AddRange(new[] { "", "POTPISAN", "NEPOTPISAN", "ANEKS" });

            // Postavi default vrijednosti za nove unose
            if (!_klijentId.HasValue)
            {
                cbVrstaKlijenta.SelectedIndex = 0;
                cbVelicina.SelectedIndex = 0;
                cbPepRizik.SelectedIndex = 0;
                cbUboRizik.SelectedIndex = 0;
                cbGotovinaRizik.SelectedIndex = 0;
                cbGeografskiRizik.SelectedIndex = 0;
                cbStatusUgovora.SelectedIndex = 0;
            }
        }

        private void LoadSifre()
        {
            var sifre = _db.Djelatnosti.OrderBy(d => d.Sifra).ToList();

            cbSifra.DataSource = sifre;
            cbSifra.DisplayMember = "Naziv";
            cbSifra.ValueMember = "Sifra";

            if (sifre.Count > 0 && !_klijentId.HasValue)
            {
                cbSifra.SelectedIndex = 0;
            }
        }

        private void LoadKlijent(int klijentId)
        {
            var klijent = _db.Klijenti
                .Include(k => k.Ugovor)
                .FirstOrDefault(k => k.Id == klijentId);

            if (klijent != null)
            {
                txtNaziv.Text = klijent.Naziv ?? "";
                txtIdBroj.Text = klijent.IdBroj ?? "";
                txtAdresa.Text = klijent.Adresa ?? "";

                // Postavi šifru djelatnosti
                if (!string.IsNullOrEmpty(klijent.SifraDjelatnosti))
                {
                    cbSifra.SelectedValue = klijent.SifraDjelatnosti;
                }

                dtDatumUspostave.Value = klijent.DatumUspostave ?? DateTime.Now;

                // Postavi vrstu klijenta
                if (!string.IsNullOrEmpty(klijent.VrstaKlijenta))
                {
                    int index = cbVrstaKlijenta.FindStringExact(klijent.VrstaKlijenta);
                    if (index >= 0) cbVrstaKlijenta.SelectedIndex = index;
                }

                dtDatumOsnivanja.Value = klijent.DatumOsnivanja ?? DateTime.Now;

                // Postavi veličinu
                if (!string.IsNullOrEmpty(klijent.Velicina))
                {
                    int index = cbVelicina.FindStringExact(klijent.Velicina);
                    if (index >= 0) cbVelicina.SelectedIndex = index;
                }

                // Postavi rizike
                SetComboValue(cbPepRizik, klijent.PepRizik);
                SetComboValue(cbUboRizik, klijent.UboRizik);
                SetComboValue(cbGotovinaRizik, klijent.GotovinaRizik);
                SetComboValue(cbGeografskiRizik, klijent.GeografskiRizik);

                txtUkupnaProcjena.Text = klijent.UkupnaProcjena ?? "";
                dtDatumProcjene.Value = klijent.DatumProcjene ?? DateTime.Now;
                txtOvjeraCr.Text = klijent.OvjeraCr ?? "";

                if (klijent.Ugovor != null)
                {
                    SetComboValue(cbStatusUgovora, klijent.Ugovor.StatusUgovora);
                    dtDatumUgovora.Value = klijent.Ugovor.DatumUgovora ?? DateTime.Now;
                }
            }
        }

        private void SetComboValue(ComboBox combo, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                combo.SelectedIndex = 0; // Prazna opcija
            }
            else
            {
                int index = combo.FindStringExact(value);
                if (index >= 0)
                {
                    combo.SelectedIndex = index;
                }
            }
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNaziv.Text) || string.IsNullOrWhiteSpace(txtIdBroj.Text))
            {
                MessageBox.Show("Popuni obavezna polja: Naziv i ID Broj!");
                return;
            }

            try
            {
                if (_klijentId.HasValue)
                {
                    var klijent = _db.Klijenti.Find(_klijentId.Value);
                    if (klijent != null)
                    {
                        klijent.Naziv = txtNaziv.Text.Trim();
                        klijent.IdBroj = txtIdBroj.Text.Trim();
                        klijent.Adresa = txtAdresa.Text ?? "";
                        klijent.SifraDjelatnosti = cbSifra.SelectedValue?.ToString() ?? "";
                        klijent.DatumUspostave = dtDatumUspostave.Value;
                        klijent.VrstaKlijenta = cbVrstaKlijenta.Text;
                        klijent.DatumOsnivanja = dtDatumOsnivanja.Value;
                        klijent.Velicina = cbVelicina.Text;
                        klijent.PepRizik = cbPepRizik.Text == "" ? null : cbPepRizik.Text;
                        klijent.UboRizik = cbUboRizik.Text == "" ? null : cbUboRizik.Text;
                        klijent.GotovinaRizik = cbGotovinaRizik.Text == "" ? null : cbGotovinaRizik.Text;
                        klijent.GeografskiRizik = cbGeografskiRizik.Text == "" ? null : cbGeografskiRizik.Text;
                        klijent.UkupnaProcjena = txtUkupnaProcjena.Text ?? "";
                        klijent.DatumProcjene = dtDatumProcjene.Value;
                        klijent.OvjeraCr = txtOvjeraCr.Text ?? "";
                        klijent.Azuriran = DateTime.Now;

                        _db.SaveChanges();

                        // Ugovor
                        var ugovor = _db.Ugovori.FirstOrDefault(u => u.KlijentId == klijent.Id);
                        if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
                        {
                            if (ugovor == null)
                            {
                                ugovor = new Ugovor { KlijentId = klijent.Id };
                                _db.Ugovori.Add(ugovor);
                            }
                            ugovor.StatusUgovora = cbStatusUgovora.Text;
                            ugovor.DatumUgovora = dtDatumUgovora.Value;
                        }
                        else if (ugovor != null)
                        {
                            _db.Ugovori.Remove(ugovor);
                        }

                        _db.SaveChanges();
                        MessageBox.Show("Klijent ažuriran!");
                    }
                }
                else
                {
                    var klijent = new Klijent
                    {
                        Naziv = txtNaziv.Text.Trim(),
                        IdBroj = txtIdBroj.Text.Trim(),
                        Adresa = txtAdresa.Text ?? "",
                        SifraDjelatnosti = cbSifra.SelectedValue?.ToString() ?? "",
                        DatumUspostave = dtDatumUspostave.Value,
                        VrstaKlijenta = cbVrstaKlijenta.Text,
                        DatumOsnivanja = dtDatumOsnivanja.Value,
                        Velicina = cbVelicina.Text,
                        PepRizik = cbPepRizik.Text == "" ? null : cbPepRizik.Text,
                        UboRizik = cbUboRizik.Text == "" ? null : cbUboRizik.Text,
                        GotovinaRizik = cbGotovinaRizik.Text == "" ? null : cbGotovinaRizik.Text,
                        GeografskiRizik = cbGeografskiRizik.Text == "" ? null : cbGeografskiRizik.Text,
                        UkupnaProcjena = txtUkupnaProcjena.Text ?? "",
                        DatumProcjene = dtDatumProcjene.Value,
                        OvjeraCr = txtOvjeraCr.Text ?? "",
                        Status = "AKTIVAN"
                    };

                    _db.Klijenti.Add(klijent);
                    _db.SaveChanges();

                    if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
                    {
                        var ugovor = new Ugovor
                        {
                            KlijentId = klijent.Id,
                            StatusUgovora = cbStatusUgovora.Text,
                            DatumUgovora = dtDatumUgovora.Value
                        };
                        _db.Ugovori.Add(ugovor);
                        _db.SaveChanges();
                    }

                    MessageBox.Show("Klijent dodan!");
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}