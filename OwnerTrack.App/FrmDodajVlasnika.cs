using OwnerTrack.Data.Entities;
using OwnerTrack.Infrastructure;
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
                txtProcetat.Text = vlasnik.ProcetatVlasnistva.ToString("F2");
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

            if (!decimal.TryParse(txtProcetat.Text, out decimal procetat))
            {
                MessageBox.Show("Procetat mora biti broj!");
                return;
            }

            try
            {
                if (_vlasnikId.HasValue)
                {
                    var vlasnik = _db.Vlasnici.Find(_vlasnikId.Value);
                    if (vlasnik != null)
                    {
                        vlasnik.ImePrezime = txtImePrezime.Text.Trim();
                        vlasnik.DatumValjanostiDokumenta = dtDatumValjanosti.Value;
                        vlasnik.ProcetatVlasnistva = procetat;
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
                        ImePrezime = txtImePrezime.Text.Trim(),
                        DatumValjanostiDokumenta = dtDatumValjanosti.Value,
                        ProcetatVlasnistva = procetat,
                        DatumUtvrdjivanja = dtDatumUtvrdjivanja.Value,
                        IzvorPodatka = txtIzvorPodatka.Text ?? "",
                        Status = "AKTIVAN"
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