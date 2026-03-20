using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;

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

            bool jeIzmjena = _direktorId.HasValue;
            Text = jeIzmjena ? "Izmijeni direktora" : "Dodaj novog direktora";
            btnSpremi.Text = jeIzmjena ? "💾 Spremi izmjene" : "💾 Dodaj";

            if (jeIzmjena)
                UcitajDirektora(_direktorId!.Value);
        }

        private void UcitajDirektora(int direktorId)
        {
            var d = _db.Direktori.Find(direktorId);
            if (d == null) return;

            txtImePrezime.Text = d.ImePrezime ?? string.Empty;
            txtJmbg.Text = d.Jmbg ?? string.Empty;
            dtDatumValjanosti.Value = d.DatumValjanosti ?? DateTime.Now;
            cbTipValjanosti.Text = d.TipValjanosti ?? TipValjanostiKonstante.Trajno;
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
                    SpremiIzmjenu(_direktorId.Value, datumValjanosti);
                else
                    SpremiNovog(datumValjanosti);

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

        

        private void PrimijeniPolja(Direktor d, DateTime? datumValjanosti)
        {
            d.ImePrezime = txtImePrezime.Text.Trim();
            d.DatumValjanosti = datumValjanosti;
            d.TipValjanosti = cbTipValjanosti.Text;
            d.Jmbg = FormHelper.NullAkoJePrazno(txtJmbg.Text);
        }

        private void SpremiIzmjenu(int direktorId, DateTime? datumValjanosti)
        {
            var d = _db.Direktori.Find(direktorId);
            if (d == null) return;

            string stariNaziv = d.ImePrezime ?? string.Empty;
            PrimijeniPolja(d, datumValjanosti);

            TransactionHelper.Execute(_db, db =>
            {
                db.SaveChanges();
                _audit.Izmijenjeno("Direktori", direktorId, $"'{stariNaziv}' → '{d.ImePrezime}'");
                db.SaveChanges();
            });

            MessageBox.Show("Ažurirano!");
        }

        private void SpremiNovog(DateTime? datumValjanosti)
        {
            var d = new Direktor { KlijentId = _klijentId, Status = StatusEntiteta.AKTIVAN };
            PrimijeniPolja(d, datumValjanosti);

            TransactionHelper.Execute(_db, db =>
            {
                db.Direktori.Add(d);
                db.SaveChanges();
                _audit.Dodano("Direktori", d.Id, $"Novi direktor: '{d.ImePrezime}'");
                db.SaveChanges();
            });

            MessageBox.Show("Dodano!");
        }
    }
}