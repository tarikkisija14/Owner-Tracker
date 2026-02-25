using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Database;
using System;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class FrmDodajVlasnika : Form
    {
        private OwnerTrackDbContext _db;
        private int _klijentId;
        private int? _vlasnikId;

        public FrmDodajVlasnika(int klijentId, int? vlasnikId, OwnerTrackDbContext db)
        {
            InitializeComponent();
            _klijentId = klijentId;
            _vlasnikId = vlasnikId;
            _db = db;
        }

        private void FrmDodajVlasnika_Load(object sender, EventArgs e)
        {
            if (_vlasnikId.HasValue)
            {
                LoadVlasnik(_vlasnikId.Value);
                this.Text = "Izmijeni vlasnika";
                btnSpremi.Text = "💾 Spremi izmjene";
            }
            else
            {
                this.Text = "Dodaj novog vlasnika";
                btnSpremi.Text = "💾 Dodaj";
            }
        }

        private void LoadVlasnik(int vlasnikId)
        {
            var vlasnik = _db.Vlasnici.Find(vlasnikId);
            if (vlasnik != null)
            {
                txtImePrezime.Text = vlasnik.ImePrezime ?? "";
                dtDatumValjanosti.Value = vlasnik.DatumValjanostiDokumenta ?? DateTime.Now;
                txtProcetat.Text = vlasnik.ProcenatVlasnistva.ToString("F2");
                dtDatumUtvrdjivanja.Value = vlasnik.DatumUtvrdjivanja ?? DateTime.Now;
                txtIzvorPodatka.Text = vlasnik.IzvorPodatka ?? "";
            }
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImePrezime.Text))
            {
                MessageBox.Show("Upiši ime i prezime!");
                return;
            }

            if (!decimal.TryParse(
                    txtProcetat.Text.Replace(",", ".").Trim(),
                    System.Globalization.NumberStyles.Number,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out decimal procetat))
            {
                MessageBox.Show("Procenat mora biti broj (npr. 25 ili 33.5)!");
                return;
            }

            if (procetat < 0 || procetat > 100)
            {
                MessageBox.Show("Procenat vlasništva mora biti između 0 i 100!", "Greška validacije",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProcetat.Focus();
                return;
            }

            try
            {
                string imePrezime = txtImePrezime.Text.Trim();
                int trenutniId = _vlasnikId ?? 0;

                
                if (_db.Set<OwnerTrack.Data.Entities.Vlasnik>().IgnoreQueryFilters()
                        .Any(v => v.KlijentId == _klijentId
                               && v.ImePrezime == imePrezime
                               && v.Id != trenutniId
                               && v.Obrisan == null))
                {
                    MessageBox.Show($"Vlasnik '{imePrezime}' već postoji za ovu firmu!",
                        "Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtImePrezime.Focus();
                    return;
                }

                if (_vlasnikId.HasValue)
                {
                    var vlasnik = _db.Vlasnici.Find(_vlasnikId.Value);
                    if (vlasnik != null)
                    {
                        vlasnik.ImePrezime = imePrezime;
                        vlasnik.DatumValjanostiDokumenta = dtDatumValjanosti.Value;
                        vlasnik.ProcenatVlasnistva = procetat;
                        vlasnik.DatumUtvrdjivanja = dtDatumUtvrdjivanja.Value;
                        vlasnik.IzvorPodatka = txtIzvorPodatka.Text ?? "";
                        _db.SaveChanges();
                        MessageBox.Show("Ažurirano!");
                    }
                }
                else
                {
                    var vlasnik = new Vlasnik
                    {
                        KlijentId = _klijentId,
                        ImePrezime = imePrezime,
                        DatumValjanostiDokumenta = dtDatumValjanosti.Value,
                        ProcenatVlasnistva = procetat,
                        DatumUtvrdjivanja = dtDatumUtvrdjivanja.Value,
                        IzvorPodatka = txtIzvorPodatka.Text ?? "",
                        Status = StatusKonstante.Aktivan
                    };

                    _db.Vlasnici.Add(vlasnik);
                    _db.SaveChanges();
                    MessageBox.Show("Dodano!");
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