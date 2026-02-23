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

           
            cbVrstaKlijenta.Items.Clear();
            cbVrstaKlijenta.Items.AddRange(new[] { "PRAVNO LICE", "FIZIČKO LICE", "UDRUŽENJE", "OBRTNIK" });

           
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
            cbStatusUgovora.Items.AddRange(new[] { "", "POTPISAN", "ANEKS", "OTKAZAN", "NEMA UGOVOR", "NEAKTIVAN" });

            cbStatus.Items.Clear();
            cbStatus.Items.AddRange(new[] { "AKTIVAN", "NEAKTIVAN", "ARHIVIRAN" });


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


                if (!string.IsNullOrEmpty(klijent.SifraDjelatnosti))
                {
                    cbSifra.SelectedValue = klijent.SifraDjelatnosti;
                }

                dtDatumUspostave.Value = klijent.DatumUspostave ?? DateTime.Now;


                if (!string.IsNullOrEmpty(klijent.VrstaKlijenta))
                {
                    int index = cbVrstaKlijenta.FindStringExact(klijent.VrstaKlijenta);
                    if (index >= 0) cbVrstaKlijenta.SelectedIndex = index;
                }

                dtDatumOsnivanja.Value = klijent.DatumOsnivanja ?? DateTime.Now;


                if (!string.IsNullOrEmpty(klijent.Velicina))
                {
                    int index = cbVelicina.FindStringExact(klijent.Velicina);
                    if (index >= 0) cbVelicina.SelectedIndex = index;
                }


                SetComboValue(cbPepRizik, klijent.PepRizik);
                SetComboValue(cbUboRizik, klijent.UboRizik);
                SetComboValue(cbGotovinaRizik, klijent.GotovinaRizik);
                SetComboValue(cbGeografskiRizik, klijent.GeografskiRizik);

                txtUkupnaProcjena.Text = klijent.UkupnaProcjena ?? "";
                dtDatumProcjene.Value = klijent.DatumProcjene ?? DateTime.Now;
                txtOvjeraCr.Text = klijent.OvjeraCr ?? "";


                if (!string.IsNullOrEmpty(klijent.Status))
                {
                    int index = cbStatus.FindStringExact(klijent.Status);
                    if (index >= 0) cbStatus.SelectedIndex = index;
                }
                else
                {
                    cbStatus.SelectedIndex = 0;
                }


                txtNapomena.Text = klijent.Napomena ?? "";

                if (klijent.Ugovor != null)
                {
                    txtVrstaUgovora.Text = klijent.Ugovor.VrstaUgovora ?? "";
                    SetComboValue(cbStatusUgovora, klijent.Ugovor.StatusUgovora);
                    dtDatumUgovora.Value = klijent.Ugovor.DatumUgovora ?? DateTime.Now;
                }
            }
        }

        private void SetComboValue(ComboBox combo, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                combo.SelectedIndex = 0;
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

           
            string idBrojTrim = txtIdBroj.Text.Trim();
            bool idBrojValidan = idBrojTrim.Length == 13 && idBrojTrim.All(char.IsDigit);
            if (!idBrojValidan)
            {
                MessageBox.Show("ID broj mora sadržavati tačno 13 cifara!", "Greška validacije", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdBroj.Focus();
                return;
            }

          
            int trenutniId = _klijentId ?? 0;
            bool idBrojPostoji = _db.Klijenti.Any(k => k.IdBroj == idBrojTrim && k.Id != trenutniId);
            if (idBrojPostoji)
            {
                MessageBox.Show($"Klijent s ID brojem '{idBrojTrim}' već postoji u bazi!", "Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdBroj.Focus();
                return;
            }

           
            string nazivTrim = txtNaziv.Text.Trim();
            bool nazivPostoji = _db.Klijenti.Any(k => k.Naziv == nazivTrim && k.Id != trenutniId);
            if (nazivPostoji)
            {
                MessageBox.Show($"Klijent s nazivom '{nazivTrim}' već postoji u bazi!", "Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNaziv.Focus();
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
                        klijent.Status = cbStatus.Text ?? "AKTIVAN";
                        klijent.Napomena = txtNapomena.Text ?? "";
                        klijent.Azuriran = DateTime.Now;

                        _db.SaveChanges();

                        
                        var ugovor = _db.Ugovori.FirstOrDefault(u => u.KlijentId == klijent.Id);
                        if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
                        {
                            if (ugovor == null)
                            {
                                ugovor = new Ugovor { KlijentId = klijent.Id };
                                _db.Ugovori.Add(ugovor);
                            }
                            ugovor.VrstaUgovora = txtVrstaUgovora.Text ?? "";
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
                        Status = cbStatus.Text ?? "AKTIVAN",
                        Napomena = txtNapomena.Text ?? "",
                        Kreiran = DateTime.Now
                    };

                    _db.Klijenti.Add(klijent);
                    _db.SaveChanges();

                    if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
                    {
                        var ugovor = new Ugovor
                        {
                            KlijentId = klijent.Id,
                            VrstaUgovora = txtVrstaUgovora.Text ?? "",
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