using Microsoft.EntityFrameworkCore;
using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class FrmUpozorenja : Form
    {
        private readonly OwnerTrackDbContext _db;
        private readonly bool _dbOwned;

        private List<UpozorenjeDetalj> _svaUpozorenja = new();

        public FrmUpozorenja() : this(DbContextFactory.Kreiraj(), dbOwned: true) { }

        public FrmUpozorenja(OwnerTrackDbContext db) : this(db, dbOwned: false) { }

        private FrmUpozorenja(OwnerTrackDbContext db, bool dbOwned)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _dbOwned = dbOwned;
            InitializeComponent();
        }

        
        private void FrmUpozorenja_Load(object sender, EventArgs e) => UcitajUpozorenja();

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_dbOwned) _db?.Dispose();
            base.OnFormClosed(e);
        }

       
        private void UcitajUpozorenja()
        {
            try
            {
                var danas = DateTime.Today;
                var granica = danas.AddDays(AppKonstante.DanaUpozerenja);

                _svaUpozorenja = UcitajUpozorenjaVlasnika(granica)
                    .Concat(UcitajUpozerenjaDirektora(granica))
                    .OrderBy(x => x.DatumIsteka)
                    .ToList();

                PrikaziSumarij(danas);
                PrikaziGridFirme(danas);
            }
            catch (Exception ex)
            {
                DialogHelper.LogirajIPokaziGresku(ex, "Greška pri učitavanju upozorenja");
            }
        }

        private List<UpozorenjeDetalj> UcitajUpozorenjaVlasnika(DateTime granica) =>
            _db.Vlasnici
                .AsNoTracking()
                .Where(v => v.DatumValjanostiDokumenta != null
                         && v.DatumValjanostiDokumenta <= granica
                         && v.Status == StatusEntiteta.AKTIVAN
                         && v.Klijent.Status != StatusEntiteta.ARHIVIRAN)
                .Include(v => v.Klijent)
                .Select(v => new UpozorenjeDetalj
                {
                    KlijentId = v.KlijentId,
                    NazivFirme = v.Klijent.Naziv,
                    ImePrezime = v.ImePrezime,
                    Tip = "Vlasnik",
                    DatumIsteka = v.DatumValjanostiDokumenta!.Value,
                })
                .ToList();

        private List<UpozorenjeDetalj> UcitajUpozerenjaDirektora(DateTime granica) =>
            _db.Direktori
                .AsNoTracking()
                .Where(d => d.DatumValjanosti != null
                         && d.DatumValjanosti <= granica
                         && d.TipValjanosti == TipValjanostiKonstante.Vremenski
                         && d.Status == StatusEntiteta.AKTIVAN
                         && d.Klijent.Status != StatusEntiteta.ARHIVIRAN)
                .Include(d => d.Klijent)
                .Select(d => new UpozorenjeDetalj
                {
                    KlijentId = d.KlijentId,
                    NazivFirme = d.Klijent.Naziv,
                    ImePrezime = d.ImePrezime,
                    Tip = "Direktor",
                    DatumIsteka = d.DatumValjanosti!.Value,
                })
                .ToList();

       
        private void PrikaziSumarij(DateTime danas)
        {
            int istekli = _svaUpozorenja.Count(x => x.DatumIsteka < danas);
            int kriticni = _svaUpozorenja.Count(x =>
                x.DatumIsteka >= danas &&
                x.DatumIsteka <= danas.AddDays(AppKonstante.DanaKriticnoUpozorenje));
            int ostali = _svaUpozorenja.Count(x =>
                x.DatumIsteka > danas.AddDays(AppKonstante.DanaKriticnoUpozorenje));
            int firmi = _svaUpozorenja.Select(x => x.KlijentId).Distinct().Count();

            lblSumarij.Text =
                $"Ukupno {firmi} firma s upozorenjima   |   " +
                $" Isteklo: {istekli}   " +
                $" Kritično (≤{AppKonstante.DanaKriticnoUpozorenje} dana): {kriticni}   " +
                $" Uskoro ({AppKonstante.DanaKriticnoUpozorenje + 1}–{AppKonstante.DanaUpozerenja} dana): {ostali}";

            panelTop.BackColor = istekli > 0
                ? Color.FromArgb(160, 30, 30)
                : kriticni > 0
                    ? Color.FromArgb(180, 90, 20)
                    : Color.FromArgb(130, 110, 20);
        }

        private void PrikaziGridFirme(DateTime danas)
        {
            var poFirmama = _svaUpozorenja
                .GroupBy(x => new { x.KlijentId, x.NazivFirme })
                .Select(g => new
                {
                    g.Key.KlijentId,
                    Firma = g.Key.NazivFirme,
                    Upozorenja = g.Count(),
                    NajbliziDatum = g.Min(x => x.DatumIsteka),
                    DanaDoIsteka = (int)(g.Min(x => x.DatumIsteka) - danas).TotalDays,
                })
                .OrderBy(x => x.NajbliziDatum)
                .ToList();

            GridHelper.BindBezEventa(gridFirme, gridFirme_SelectionChanged, poFirmama);

            if (gridFirme.Columns.Count == 0) return;
            if (gridFirme.Columns.Contains("KlijentId")) gridFirme.Columns["KlijentId"].Visible = false;

            GridHelper.KonfigurirajKolonu(gridFirme, "Firma", "Naziv firme", 50);
            GridHelper.KonfigurirajKolonu(gridFirme, "Upozorenja", "Br. upozorenja", 15);
            GridHelper.KonfigurirajKolonu(gridFirme, "NajbliziDatum", "Najbliži datum isteka", 20, "dd.MM.yyyy");
            GridHelper.KonfigurirajKolonu(gridFirme, "DanaDoIsteka", "Dana do isteka", 15);
        }

       
        private void gridFirme_SelectionChanged(object sender, EventArgs e)
        {
            if (gridFirme.SelectedRows.Count == 0) { gridDetalji.DataSource = null; return; }

            dynamic row = gridFirme.SelectedRows[0].DataBoundItem;
            int klijentId = row.KlijentId;
            var danas = DateTime.Today;

            var detalji = _svaUpozorenja
                .Where(x => x.KlijentId == klijentId)
                .Select(x => new
                {
                    x.Tip,
                    x.ImePrezime,
                    x.DatumIsteka,
                    DanaDoIsteka = (int)(x.DatumIsteka - danas).TotalDays,
                    Status = StatusTekst(x.DatumIsteka, danas),
                })
                .OrderBy(x => x.DatumIsteka)
                .ToList();

            gridDetalji.DataSource = detalji;
            gridDetalji.ClearSelection();

            if (gridDetalji.Columns.Count == 0) return;
            GridHelper.KonfigurirajKolonu(gridDetalji, "Tip", "Tip", 15);
            GridHelper.KonfigurirajKolonu(gridDetalji, "ImePrezime", "Ime i prezime", 35);
            GridHelper.KonfigurirajKolonu(gridDetalji, "DatumIsteka", "Datum isteka", 20, "dd.MM.yyyy");
            GridHelper.KonfigurirajKolonu(gridDetalji, "DanaDoIsteka", "Dana do isteka", 15);
            GridHelper.KonfigurirajKolonu(gridDetalji, "Status", "Status", 15);
        }

        
        private void gridFirme_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => BojajRed(gridFirme, e);

        private void gridDetalji_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => BojajRed(gridDetalji, e);

        private static void BojajRed(DataGridView grid, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || grid.Rows[e.RowIndex].DataBoundItem == null) return;
            dynamic item = grid.Rows[e.RowIndex].DataBoundItem;
            grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = BojaZaDane(item.DanaDoIsteka);
        }

        private void btnZatvori_Click(object sender, EventArgs e) => Close();

        
        private static string StatusTekst(DateTime datumIsteka, DateTime danas) =>
            datumIsteka < danas ? "⛔ ISTEKLO"
            : datumIsteka <= danas.AddDays(AppKonstante.DanaKriticnoUpozorenje) ? "⚠ Kritično"
            : "🕐 Uskoro";

        private static Color BojaZaDane(int dana) =>
            dana < 0 ? Color.FromArgb(220, 80, 80)
            : dana <= AppKonstante.DanaKriticnoUpozorenje ? Color.FromArgb(255, 200, 120)
            : Color.FromArgb(255, 245, 150);
    }
}