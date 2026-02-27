using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using System;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class FrmDodajDirektora : Form
    {
        private readonly OwnerTrackDbContext _db;
        private readonly AuditService _audit;
        private readonly int _klijentId;
        private readonly int? _direktorId;

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

            cbTipValjanosti.Items.Clear();
            cbTipValjanosti.Items.Add(TipValjanostiKonstante.Trajno);
            cbTipValjanosti.Items.Add(TipValjanostiKonstante.Vremenski);
            cbTipValjanosti.SelectedIndex = 0;

            if (_direktorId.HasValue)
            {
                LoadDirektor(_direktorId.Value);
                Text = "Izmijeni direktora";
                btnSpremi.Text = "💾 Spremi izmjene";
            }
            else
            {
                Text = "Dodaj novog direktora";
                btnSpremi.Text = "💾 Dodaj";
            }
        }

        private void LoadDirektor(int direktorId)
        {
            var d = _db.Direktori.Find(direktorId);
            if (d == null) return;

            txtImePrezime.Text = d.ImePrezime ?? "";
            dtDatumValjanosti.Value = d.DatumValjanosti ?? DateTime.Now;

            cbTipValjanosti.Text = d.TipValjanosti ?? TipValjanostiKonstante.Trajno;
            txtJmbg.Text = d.Jmbg ?? "";


            dtDatumValjanosti.Enabled = d.TipValjanosti == TipValjanostiKonstante.Vremenski;
        }

        private void cbTipValjanosti_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtDatumValjanosti.Enabled = cbTipValjanosti.Text == TipValjanostiKonstante.Vremenski;
        }

        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtImePrezime.Text))
            {
                MessageBox.Show("Upiši ime i prezime direktora!");
                return;
            }


            bool jeTrajno = cbTipValjanosti.Text == TipValjanostiKonstante.Trajno;
            DateTime? datumValjanosti = jeTrajno ? null : dtDatumValjanosti.Value;

            try
            {
                if (_direktorId.HasValue)
                    SpremiIzmjenu(_direktorId.Value, datumValjanosti, jeTrajno);
                else
                    SpremiNovog(datumValjanosti, jeTrajno);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        private void SpremiIzmjenu(int direktorId, DateTime? datumValjanosti, bool jeTrajno)
        {
            var d = _db.Direktori.Find(direktorId);
            if (d == null) return;

            string stariNaziv = d.ImePrezime ?? "";

            d.ImePrezime = txtImePrezime.Text.Trim();
            d.DatumValjanosti = datumValjanosti;
            d.TipValjanosti = cbTipValjanosti.Text;
            d.Jmbg = string.IsNullOrWhiteSpace(txtJmbg.Text) ? null : txtJmbg.Text.Trim();

            using var tx = _db.Database.BeginTransaction();
            try
            {
                _db.SaveChanges();
                _audit.Izmijenjeno("Direktori", direktorId, $"'{stariNaziv}' → '{d.ImePrezime}'");
                _db.SaveChanges();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }

            MessageBox.Show("Ažurirano!");
        }

        private void SpremiNovog(DateTime? datumValjanosti, bool jeTrajno)
        {
            var d = new Direktor
            {
                KlijentId = _klijentId,
                ImePrezime = txtImePrezime.Text.Trim(),
                DatumValjanosti = datumValjanosti,
                TipValjanosti = cbTipValjanosti.Text,
                Status = StatusEntiteta.AKTIVAN,
                Jmbg = string.IsNullOrWhiteSpace(txtJmbg.Text) ? null : txtJmbg.Text.Trim(),
            };

            using var tx = _db.Database.BeginTransaction();
            try
            {
                _db.Direktori.Add(d);
                _db.SaveChanges();
                _audit.Dodano("Direktori", d.Id, $"Novi direktor: '{d.ImePrezime}'");
                _db.SaveChanges();
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }

            MessageBox.Show("Dodano!");
        }

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}