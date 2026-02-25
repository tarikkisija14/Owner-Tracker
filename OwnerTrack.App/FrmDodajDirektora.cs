using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using System;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class FrmDodajDirektora : Form
    {
        private OwnerTrackDbContext _db;
        private int _klijentId;
        private int? _direktorId;

        public FrmDodajDirektora(int klijentId, int? direktorId, OwnerTrackDbContext db)
        {
            InitializeComponent();
            _klijentId = klijentId;
            _direktorId = direktorId;
            _db = db;
        }

        private void FrmDodajDirektora_Load(object sender, EventArgs e)
        {
            cbTipValjanosti.Items.AddRange(new[] { "TRAJNO", "VREMENSKI" });
            cbTipValjanosti.SelectedIndex = 0;



            if (_direktorId.HasValue)
            {
                LoadDirektor(_direktorId.Value);
                this.Text = "Izmijeni direktora";
                btnSpremi.Text = "💾 Spremi izmjene";
            }
            else
            {
                this.Text = "Dodaj novog direktora";
                btnSpremi.Text = "💾 Dodaj";
            }

        }

        private void LoadDirektor(int direktorId)
        {
            var direktor = _db.Direktori.Find(direktorId);
            if (direktor != null)
            {
                txtImePrezime.Text = direktor.ImePrezime ?? "";
                dtDatumValjanosti.Value = direktor.DatumValjanosti ?? DateTime.Now;
                cbTipValjanosti.Text = direktor.TipValjanosti ?? "TRAJNO";
                txtJmbg.Text = direktor.Jmbg ?? "";
            }
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImePrezime.Text))
            {
                MessageBox.Show("Upiši ime i prezime direktora!");
                return;
            }

            try
            {
                if (_direktorId.HasValue)
                {
                    var direktor = _db.Direktori.Find(_direktorId.Value);
                    if (direktor != null)
                    {
                        direktor.ImePrezime = txtImePrezime.Text.Trim();
                        direktor.DatumValjanosti = cbTipValjanosti.Text == "TRAJNO" ? null : dtDatumValjanosti.Value;
                        direktor.TipValjanosti = cbTipValjanosti.Text;
                        direktor.Jmbg = string.IsNullOrWhiteSpace(txtJmbg.Text) ? null : txtJmbg.Text.Trim();

                        _db.SaveChanges();
                        MessageBox.Show("Ažurirano!");
                    }
                }
                else
                {
                    var direktor = new Direktor
                    {
                        KlijentId = _klijentId,
                        ImePrezime = txtImePrezime.Text.Trim(),
                        DatumValjanosti = cbTipValjanosti.Text == "TRAJNO" ? null : dtDatumValjanosti.Value,
                        TipValjanosti = cbTipValjanosti.Text,
                        Status = StatusKonstante.Aktivan,
                        Jmbg = string.IsNullOrWhiteSpace(txtJmbg.Text) ? null : txtJmbg.Text.Trim(),

                    };

                    _db.Direktori.Add(direktor);
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

        private void cbTipValjanosti_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtDatumValjanosti.Enabled = true;
        }
    }
}