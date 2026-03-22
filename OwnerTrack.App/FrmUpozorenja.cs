using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    public partial class FrmUpozorenja : Form
    {

        private readonly OwnerTrackDbContext _db;
        private readonly bool _dbOwned;

        private List<UpozorenjeDetalj> _allWarnings = new();


        public FrmUpozorenja() : this(DbContextFactory.Kreiraj(), dbOwned: true) { }

        public FrmUpozorenja(OwnerTrackDbContext db) : this(db, dbOwned: false) { }

        private FrmUpozorenja(OwnerTrackDbContext db, bool dbOwned)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _dbOwned = dbOwned;
            InitializeComponent();
        }


        private void FrmUpozorenja_Load(object sender, EventArgs e) => LoadWarnings();

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_dbOwned) _db?.Dispose();
            base.OnFormClosed(e);
        }


        private void LoadWarnings()
        {
            try
            {
                _allWarnings = new WarningQueryService(_db).DohvatiUpozorenja();
                RenderSummaryPanel(DateTime.Today);
                RenderFirmsGrid(DateTime.Today);
            }
            catch (Exception ex)
            {
                DialogHelper.LogirajIPokaziGresku(ex, "Greška pri učitavanju upozorenja");
            }
        }


        private void RenderSummaryPanel(DateTime today)
        {
            int expired = _allWarnings.Count(x => x.DatumIsteka < today);
            int critical = _allWarnings.Count(x =>
                x.DatumIsteka >= today &&
                x.DatumIsteka <= today.AddDays(AppKonstante.DanaKriticnoUpozorenje));
            int upcoming = _allWarnings.Count(x =>
                x.DatumIsteka > today.AddDays(AppKonstante.DanaKriticnoUpozorenje));
            int firmCount = _allWarnings.Select(x => x.KlijentId).Distinct().Count();

            lblSumarij.Text =
                $"Ukupno {firmCount} firma s upozorenjima   |   " +
                $" Isteklo: {expired}   " +
                $" Kritično (≤{AppKonstante.DanaKriticnoUpozorenje} dana): {critical}   " +
                $" Uskoro ({AppKonstante.DanaKriticnoUpozorenje + 1}–{AppKonstante.DanaUpozerenja} dana): {upcoming}";

            panelTop.BackColor = SummaryPanelColor(expired, critical);
        }

        private static Color SummaryPanelColor(int expired, int critical) =>
            expired > 0 ? Color.FromArgb(160, 30, 30) :
            critical > 0 ? Color.FromArgb(180, 90, 20) :
                           Color.FromArgb(130, 110, 20);


        private void RenderFirmsGrid(DateTime today)
        {
            var grouped = _allWarnings
                .GroupBy(x => new { x.KlijentId, x.NazivFirme })
                .Select(g => new
                {
                    g.Key.KlijentId,
                    Firma = g.Key.NazivFirme,
                    Upozorenja = g.Count(),
                    NajbliziDatum = g.Min(x => x.DatumIsteka),
                    DanaDoIsteka = DaysUntilExpiry(g.Min(x => x.DatumIsteka), today),
                })
                .OrderBy(x => x.NajbliziDatum)
                .ToList();

            GridHelper.BindBezEventa(gridFirme, gridFirme_SelectionChanged, grouped);

            if (gridFirme.Columns.Count == 0) return;

            if (gridFirme.Columns.Contains("KlijentId"))
                gridFirme.Columns["KlijentId"].Visible = false;

            GridHelper.KonfigurirajKolonu(gridFirme, "Firma", "Naziv firme", 50);
            GridHelper.KonfigurirajKolonu(gridFirme, "Upozorenja", "Br. upozorenja", 15);
            GridHelper.KonfigurirajKolonu(gridFirme, "NajbliziDatum", "Najbliži datum isteka", 20, "dd.MM.yyyy");
            GridHelper.KonfigurirajKolonu(gridFirme, "DanaDoIsteka", "Dana do isteka", 15);
        }


        private void gridFirme_SelectionChanged(object sender, EventArgs e)
        {
            if (gridFirme.SelectedRows.Count == 0)
            {
                gridDetalji.DataSource = null;
                return;
            }

            dynamic selectedRow = gridFirme.SelectedRows[0].DataBoundItem;
            int klijentId = selectedRow.KlijentId;
            var today = DateTime.Today;

            var details = _allWarnings
                .Where(x => x.KlijentId == klijentId)
                .Select(x => new
                {
                    x.Tip,
                    x.ImePrezime,
                    x.DatumIsteka,
                    DanaDoIsteka = DaysUntilExpiry(x.DatumIsteka, today),
                    Status = WarningStatusText(x.DatumIsteka, today),
                })
                .OrderBy(x => x.DatumIsteka)
                .ToList();

            gridDetalji.DataSource = details;
            gridDetalji.ClearSelection();

            if (gridDetalji.Columns.Count == 0) return;

            GridHelper.KonfigurirajKolonu(gridDetalji, "Tip", "Tip", 15);
            GridHelper.KonfigurirajKolonu(gridDetalji, "ImePrezime", "Ime i prezime", 35);
            GridHelper.KonfigurirajKolonu(gridDetalji, "DatumIsteka", "Datum isteka", 20, "dd.MM.yyyy");
            GridHelper.KonfigurirajKolonu(gridDetalji, "DanaDoIsteka", "Dana do isteka", 15);
            GridHelper.KonfigurirajKolonu(gridDetalji, "Status", "Status", 15);
        }


        private void gridFirme_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => ColorizeRow(gridFirme, e);

        private void gridDetalji_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => ColorizeRow(gridDetalji, e);

        private static void ColorizeRow(DataGridView grid, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || grid.Rows[e.RowIndex].DataBoundItem is null) return;
            dynamic item = grid.Rows[e.RowIndex].DataBoundItem;
            grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = RowColorForDays(item.DanaDoIsteka);
        }

        private void btnZatvori_Click(object sender, EventArgs e) => Close();


        private static int DaysUntilExpiry(DateTime expiryDate, DateTime today) =>
            (int)(expiryDate - today).TotalDays;

        private static string WarningStatusText(DateTime expiryDate, DateTime today) =>
            expiryDate < today ? "⛔ ISTEKLO"
            : expiryDate <= today.AddDays(AppKonstante.DanaKriticnoUpozorenje) ? "⚠ Kritično"
            : "🕐 Uskoro";

        private static Color RowColorForDays(int days) =>
            days < 0 ? Color.FromArgb(220, 80, 80)
            : days <= AppKonstante.DanaKriticnoUpozorenje ? Color.FromArgb(255, 200, 120)
            : Color.FromArgb(255, 245, 150);
    }
}