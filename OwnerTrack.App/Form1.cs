using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class Form1 : Form
    {
        private OwnerTrackDbContext _db;
        private int? _selectedKlijentId = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicijalizuj DbContext
            var options = new DbContextOptionsBuilder<OwnerTrackDbContext>()
                .UseSqlite($"Data Source=C:\\Users\\tarik\\Desktop\\Job\\Firme.db")
                .Options;

            _db = new OwnerTrackDbContext(options);

            
            LoadSifre();

            
            LoadKlijenti();

            
            dataGridKlijenti.SelectionChanged += DataGridKlijenti_SelectionChanged;
        }

        

        private void LoadSifre()
        {
            var sifre = _db.Djelatnosti.ToList();
            cbSifra.DataSource = sifre;
            cbSifra.DisplayMember = "Naziv";
            cbSifra.ValueMember = "Sifra";
            cbSifra.SelectedIndex = -1;
        }

        private void LoadKlijenti()
        {
            try
            {
                var klijenti = _db.Klijenti
                    .Include(k => k.Djelatnost)
                    .Include(k => k.Vlasnici)
                    .Include(k => k.Direktori)
                    .Include(k => k.Ugovor)
                    .ToList();

                dataGridKlijenti.DataSource = klijenti.Select(k => new
                {
                    k.Id,
                    k.Naziv,
                    k.IdBroj,
                    k.Adresa,
                    Sifra = k.SifraDjelativnosti,
                    Djelatnost = k.Djelatnost?.Naziv,
                    k.DatumUspostave,
                    k.VrstaKlijenta,
                    k.DatumOsnivanja,
                    BrojVlasnika = k.Vlasnici.Count,
                    k.Velicina,
                    k.Status
                }).ToList();

                
                dataGridKlijenti.Columns["Id"].Width = 50;
                dataGridKlijenti.Columns["Naziv"].Width = 200;
                dataGridKlijenti.Columns["IdBroj"].Width = 120;
                dataGridKlijenti.Columns["BrojVlasnika"].Width = 80;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju: {ex.Message}");
            }
        }

        

        private void DataGridKlijenti_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count > 0)
            {
                _selectedKlijentId = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
                DisplayKlijentDetails(_selectedKlijentId.Value);
            }
        }

        private void DisplayKlijentDetails(int klijentId)
        {
            var klijent = _db.Klijenti
                .Include(k => k.Djelatnost)
                .Include(k => k.Vlasnici)
                .Include(k => k.Direktori)
                .Include(k => k.Ugovor)
                .FirstOrDefault(k => k.Id == klijentId);

            if (klijent != null)
            {
                
                txtNaziv.Text = klijent.Naziv;
                txtIdBroj.Text = klijent.IdBroj;
                txtAdresa.Text = klijent.Adresa;
                cbSifra.SelectedValue = klijent.SifraDjelativnosti;
                dtDatumUspostave.Value = klijent.DatumUspostave ?? DateTime.Now;
                cbVrstaKlijenta.Text = klijent.VrstaKlijenta;
                dtDatumOsnivanja.Value = klijent.DatumOsnivanja ?? DateTime.Now;
                cbVelicina.Text = klijent.Velicina;

                
                cbPepRizik.Text = klijent.PepRizik ?? "";
                cbUboRizik.Text = klijent.UboRizik ?? "";
                cbGotovinaRizik.Text = klijent.GotovinaRizik ?? "";
                cbGeografskiRizik.Text = klijent.GeografskiRizik ?? "";
                txtUkupnaProcjena.Text = klijent.UkupnaProcjena ?? "";
                dtDatumProcjene.Value = klijent.DatumProcjene ?? DateTime.Now;
                txtOvjeraCr.Text = klijent.OvjeraCr ?? "";

                
                if (klijent.Ugovor != null)
                {
                    cbStatusUgovora.Text = klijent.Ugovor.StatusUgovora ?? "";
                    dtDatumUgovora.Value = klijent.Ugovor.DatumUgovora ?? DateTime.Now;
                }

                
                LoadVlasnici(klijentId);

                
                LoadDirektori(klijentId);
            }
        }

        private void LoadVlasnici(int klijentId)
        {
            var vlasnici = _db.Vlasnici
                .Where(v => v.KlijentId == klijentId)
                .ToList();

            dataGridVlasnici.DataSource = vlasnici.Select(v => new
            {
                v.Id,
                v.ImePrezime,
                v.ProcetatVlasnistva,
                v.DatumUtvrdjivanja,
                v.IzvorPodatka,
                v.Status
            }).ToList();
        }

        private void LoadDirektori(int klijentId)
        {
            var direktori = _db.Direktori
                .Where(d => d.KlijentId == klijentId)
                .ToList();

            dataGridDirektori.DataSource = direktori.Select(d => new
            {
                d.Id,
                d.ImePrezime,
                d.DatumValjanosti,
                d.TipValjanosti,
                d.Status
            }).ToList();
        }

        

        private void btnDodajKlijent_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNaziv.Text) ||
                string.IsNullOrWhiteSpace(txtIdBroj.Text) ||
                cbSifra.SelectedValue == null)
            {
                MessageBox.Show("Popuni obavezna polja: Naziv, ID Broj, Šifra!");
                return;
            }

            try
            {
                var klijent = new Klijent
                {
                    Naziv = txtNaziv.Text.Trim(),
                    IdBroj = txtIdBroj.Text.Trim(),
                    Adresa = txtAdresa.Text,
                    SifraDjelativnosti = cbSifra.SelectedValue.ToString(),
                    DatumUspostave = dtDatumUspostave.Value,
                    VrstaKlijenta = cbVrstaKlijenta.Text,
                    DatumOsnivanja = dtDatumOsnivanja.Value,
                    Velicina = cbVelicina.Text,
                    PepRizik = cbPepRizik.Text == "" ? null : cbPepRizik.Text,
                    UboRizik = cbUboRizik.Text == "" ? null : cbUboRizik.Text,
                    GotovinaRizik = cbGotovinaRizik.Text == "" ? null : cbGotovinaRizik.Text,
                    GeografskiRizik = cbGeografskiRizik.Text == "" ? null : cbGeografskiRizik.Text,
                    UkupnaProcjena = txtUkupnaProcjena.Text,
                    DatumProcjene = dtDatumProcjene.Value,
                    OvjeraCr = txtOvjeraCr.Text,
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

                MessageBox.Show($"Klijent '{klijent.Naziv}' je dodan!");
                ClearForm();
                LoadKlijenti();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        

        private void btnIzmijeniKlijent_Click(object sender, EventArgs e)
        {
            if (_selectedKlijentId == null)
            {
                MessageBox.Show("Odaberi klijenta za izmjenu!");
                return;
            }

            try
            {
                var klijent = _db.Klijenti.Find(_selectedKlijentId);

                if (klijent != null)
                {
                    klijent.Naziv = txtNaziv.Text.Trim();
                    klijent.IdBroj = txtIdBroj.Text.Trim();
                    klijent.Adresa = txtAdresa.Text;
                    klijent.SifraDjelativnosti = cbSifra.SelectedValue.ToString();
                    klijent.DatumUspostave = dtDatumUspostave.Value;
                    klijent.VrstaKlijenta = cbVrstaKlijenta.Text;
                    klijent.DatumOsnivanja = dtDatumOsnivanja.Value;
                    klijent.Velicina = cbVelicina.Text;
                    klijent.PepRizik = cbPepRizik.Text == "" ? null : cbPepRizik.Text;
                    klijent.UboRizik = cbUboRizik.Text == "" ? null : cbUboRizik.Text;
                    klijent.GotovinaRizik = cbGotovinaRizik.Text == "" ? null : cbGotovinaRizik.Text;
                    klijent.GeografskiRizik = cbGeografskiRizik.Text == "" ? null : cbGeografskiRizik.Text;
                    klijent.UkupnaProcjena = txtUkupnaProcjena.Text;
                    klijent.DatumProcjene = dtDatumProcjene.Value;
                    klijent.OvjeraCr = txtOvjeraCr.Text;
                    klijent.Azuriran = DateTime.Now;

                    _db.SaveChanges();

                    MessageBox.Show($"Klijent '{klijent.Naziv}' je ažuriran!");
                    LoadKlijenti();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        

        private void btnObrisiKlijent_Click(object sender, EventArgs e)
        {
            if (_selectedKlijentId == null)
            {
                MessageBox.Show("Odaberi klijenta za brisanje!");
                return;
            }

            if (MessageBox.Show("Sigurno obrisati klijenta?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                var klijent = _db.Klijenti.Find(_selectedKlijentId);
                if (klijent != null)
                {
                    _db.Klijenti.Remove(klijent);
                    _db.SaveChanges();

                    MessageBox.Show($"Klijent je obrisan!");
                    ClearForm();
                    LoadKlijenti();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        

        private void btnDodajVlasnika_Click(object sender, EventArgs e)
        {
            if (_selectedKlijentId == null)
            {
                MessageBox.Show("Prvo odaberi klijenta!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtVlasnikIme.Text))
            {
                MessageBox.Show("Upiši ime vlasnika!");
                return;
            }

            try
            {
                var vlasnik = new Vlasnik
                {
                    KlijentId = _selectedKlijentId.Value,
                    ImePrezime = txtVlasnikIme.Text.Trim(),
                    ProcetatVlasnistva = decimal.TryParse(txtProcenat.Text, out var p) ? p : 0,
                    DatumUtvrdjivanja = dtDatumVlasnika.Value,
                    IzvorPodatka = txtIzvorPodatka.Text,
                    Status = "AKTIVAN"
                };

                _db.Vlasnici.Add(vlasnik);
                _db.SaveChanges();

                MessageBox.Show("Vlasnik je dodan!");
                txtVlasnikIme.Clear();
                txtProcenat.Clear();
                LoadVlasnici(_selectedKlijentId.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        private void btnObrisiVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridVlasnici.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi vlasnika za brisanje!");
                return;
            }

            int vlasnikId = (int)dataGridVlasnici.SelectedRows[0].Cells["Id"].Value;

            if (MessageBox.Show("Obrisati vlasnika?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                var vlasnik = _db.Vlasnici.Find(vlasnikId);
                if (vlasnik != null)
                {
                    _db.Vlasnici.Remove(vlasnik);
                    _db.SaveChanges();

                    MessageBox.Show("Vlasnik je obrisan!");
                    LoadVlasnici(_selectedKlijentId.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        

        private void btnDodajDirektora_Click(object sender, EventArgs e)
        {
            if (_selectedKlijentId == null)
            {
                MessageBox.Show("Prvo odaberi klijenta!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDirektorIme.Text))
            {
                MessageBox.Show("Upiši ime direktora!");
                return;
            }

            try
            {
                var direktor = new Direktor
                {
                    KlijentId = _selectedKlijentId.Value,
                    ImePrezime = txtDirektorIme.Text.Trim(),
                    DatumValjanosti = dtDatumDirektora.Value,
                    TipValjanosti = cbTipValjanosti.Text,
                    Status = "AKTIVAN"
                };

                _db.Direktori.Add(direktor);
                _db.SaveChanges();

                MessageBox.Show("Direktor je dodan!");
                txtDirektorIme.Clear();
                LoadDirektori(_selectedKlijentId.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        private void btnObrisiDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridDirektori.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi direktora za brisanje!");
                return;
            }

            int direktorId = (int)dataGridDirektori.SelectedRows[0].Cells["Id"].Value;

            if (MessageBox.Show("Obrisati direktora?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                var direktor = _db.Direktori.Find(direktorId);
                if (direktor != null)
                {
                    _db.Direktori.Remove(direktor);
                    _db.SaveChanges();

                    MessageBox.Show("Direktor je obrisan!");
                    LoadDirektori(_selectedKlijentId.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        

        private void ClearForm()
        {
            txtNaziv.Clear();
            txtIdBroj.Clear();
            txtAdresa.Clear();
            cbSifra.SelectedIndex = -1;
            cbVrstaKlijenta.SelectedIndex = -1;
            cbVelicina.SelectedIndex = -1;
            cbPepRizik.SelectedIndex = -1;
            cbUboRizik.SelectedIndex = -1;
            cbGotovinaRizik.SelectedIndex = -1;
            cbGeografskiRizik.SelectedIndex = -1;
            txtUkupnaProcjena.Clear();
            txtOvjeraCr.Clear();
            cbStatusUgovora.SelectedIndex = -1;
            _selectedKlijentId = null;
        }

        private void btnNovi_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Excel import - dolazi u idućoj verziji!");
            // TODO: Excel Import
        }
    }
}