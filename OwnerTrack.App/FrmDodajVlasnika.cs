using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using System;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class FrmDodajVlasnika : Form
    {
        private readonly OwnerTrackDbContext _db;
        private readonly AuditService _audit;
        private readonly int _klijentId;
        private readonly int? _vlasnikId;

        public FrmDodajVlasnika(int klijentId, int? vlasnikId, OwnerTrackDbContext db)
        {
            InitializeComponent();
            _klijentId = klijentId;
            _vlasnikId = vlasnikId;
            _db = db;
            _audit = new AuditService(db);
        }

        private void FrmDodajVlasnika_Load(object sender, EventArgs e)
        {
            if (_vlasnikId.HasValue)
            {
                LoadVlasnik(_vlasnikId.Value);
                Text = "Izmijeni vlasnika";
                btnSpremi.Text = "💾 Spremi izmjene";
            }
            else
            {
                Text = "Dodaj novog vlasnika";
                btnSpremi.Text = "💾 Dodaj";
            }
        }

        private void LoadVlasnik(int vlasnikId)
        {
            var v = _db.Vlasnici.Find(vlasnikId);
            if (v == null) return;

            txtImePrezime.Text = v.ImePrezime ?? "";
            dtDatumValjanosti.Value = v.DatumValjanostiDokumenta ?? DateTime.Now;
            txtProcetat.Text = v.ProcenatVlasnistva.ToString("F2");
            dtDatumUtvrdjivanja.Value = v.DatumUtvrdjivanja ?? DateTime.Now;
            txtIzvorPodatka.Text = v.IzvorPodatka ?? "";
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
                    out decimal procenat))
            {
                MessageBox.Show("Procenat mora biti broj (npr. 25 ili 33.5)!");
                return;
            }

            if (procenat < 0 || procenat > 100)
            {
                MessageBox.Show("Procenat vlasništva mora biti između 0 i 100!",
                    "Greška validacije", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProcetat.Focus();
                return;
            }

            string imePrezime = txtImePrezime.Text.Trim();
            int trenutniId = _vlasnikId ?? 0;

            if (_db.Set<Vlasnik>().IgnoreQueryFilters()
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

            try
            {
                if (_vlasnikId.HasValue)
                    SpremiIzmjenu(_vlasnikId.Value, imePrezime, procenat);
                else
                    SpremiNovog(imePrezime, procenat);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        private void SpremiIzmjenu(int vlasnikId, string imePrezime, decimal procenat)
        {
            var v = _db.Vlasnici.Find(vlasnikId);
            if (v == null) return;

            string stariNaziv = v.ImePrezime ?? "";

            v.ImePrezime = imePrezime;
            v.DatumValjanostiDokumenta = dtDatumValjanosti.Value;
            v.ProcenatVlasnistva = procenat;
            v.DatumUtvrdjivanja = dtDatumUtvrdjivanja.Value;
            v.IzvorPodatka = txtIzvorPodatka.Text ?? "";

            using var tx = _db.Database.BeginTransaction();
            try
            {
                _db.SaveChanges();
                _audit.Izmijenjeno("Vlasnici", vlasnikId, $"'{stariNaziv}' → '{imePrezime}'");
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

        private void SpremiNovog(string imePrezime, decimal procenat)
        {
            var v = new Vlasnik
            {
                KlijentId = _klijentId,
                ImePrezime = imePrezime,
                DatumValjanostiDokumenta = dtDatumValjanosti.Value,
                ProcenatVlasnistva = procenat,
                DatumUtvrdjivanja = dtDatumUtvrdjivanja.Value,
                IzvorPodatka = txtIzvorPodatka.Text ?? "",
                Status = StatusEntiteta.AKTIVAN
            };

            using var tx = _db.Database.BeginTransaction();
            try
            {
                _db.Vlasnici.Add(v);
                _db.SaveChanges();
                _audit.Dodano("Vlasnici", v.Id, $"Novi vlasnik: '{imePrezime}'");
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