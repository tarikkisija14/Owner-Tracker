using Microsoft.EntityFrameworkCore;
using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

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
            bool jeIzmjena = _vlasnikId.HasValue;
            Text = jeIzmjena ? "Izmijeni vlasnika" : "Dodaj novog vlasnika";
            btnSpremi.Text = jeIzmjena ? "💾 Spremi izmjene" : "💾 Dodaj";

            if (jeIzmjena)
                UcitajVlasnika(_vlasnikId!.Value);
        }

        private void UcitajVlasnika(int vlasnikId)
        {
            var v = _db.Vlasnici.Find(vlasnikId);
            if (v == null) return;

            txtImePrezime.Text = v.ImePrezime ?? string.Empty;
            txtProcetat.Text = v.ProcenatVlasnistva.ToString("F2");
            txtIzvorPodatka.Text = v.IzvorPodatka ?? string.Empty;
            dtDatumValjanosti.Value = v.DatumValjanostiDokumenta ?? DateTime.Now;
            dtDatumUtvrdjivanja.Value = v.DatumUtvrdjivanja ?? DateTime.Now;
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
                    out decimal procenat)
                || procenat < 0 || procenat > 100)
            {
                MessageBox.Show("Procenat vlasništva mora biti broj između 0 i 100!",
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
                DialogHelper.LogirajIPokaziGresku(ex);
            }
        }

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        

        private void PrimijeniPolja(Vlasnik v, string imePrezime, decimal procenat)
        {
            v.ImePrezime = imePrezime;
            v.DatumValjanostiDokumenta = dtDatumValjanosti.Value;
            v.ProcenatVlasnistva = procenat;
            v.DatumUtvrdjivanja = dtDatumUtvrdjivanja.Value;
            v.IzvorPodatka = txtIzvorPodatka.Text;
        }

        private void SpremiIzmjenu(int vlasnikId, string imePrezime, decimal procenat)
        {
            var v = _db.Vlasnici.Find(vlasnikId);
            if (v == null) return;

            string stariNaziv = v.ImePrezime ?? string.Empty;
            PrimijeniPolja(v, imePrezime, procenat);

            TransactionHelper.Execute(_db, db =>
            {
                db.SaveChanges();
                _audit.Izmijenjeno("Vlasnici", vlasnikId, $"'{stariNaziv}' → '{imePrezime}'");
                db.SaveChanges();
            });

            MessageBox.Show("Ažurirano!");
        }

        private void SpremiNovog(string imePrezime, decimal procenat)
        {
            var v = new Vlasnik { KlijentId = _klijentId, Status = StatusEntiteta.AKTIVAN };
            PrimijeniPolja(v, imePrezime, procenat);

            TransactionHelper.Execute(_db, db =>
            {
                db.Vlasnici.Add(v);
                db.SaveChanges();
                _audit.Dodano("Vlasnici", v.Id, $"Novi vlasnik: '{imePrezime}'");
                db.SaveChanges();
            });

            MessageBox.Show("Dodano!");
        }
    }
}