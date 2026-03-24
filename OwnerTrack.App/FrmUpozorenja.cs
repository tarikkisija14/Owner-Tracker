using OwnerTrack.App.Constants;
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
        private List<WarningDetail> _allWarnings = new();

        // ── Constructors ──────────────────────────────────────────────────────

        public FrmUpozorenja() : this(DbContextFactory.Create(), dbOwned: true) { }
        public FrmUpozorenja(OwnerTrackDbContext db) : this(db, dbOwned: false) { }

        private FrmUpozorenja(OwnerTrackDbContext db, bool dbOwned)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _dbOwned = dbOwned;
            InitializeComponent();
        }

        // ── Lifecycle ─────────────────────────────────────────────────────────

        private void FrmUpozorenja_Load(object sender, EventArgs e) => LoadWarnings();

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_dbOwned) _db?.Dispose();
            base.OnFormClosed(e);
        }

        // ── Data loading ──────────────────────────────────────────────────────

        private void LoadWarnings()
        {
            try
            {
                _allWarnings = new WarningQueryService(_db).GetWarnings();
                RenderSummaryPanel(DateTime.Today);
                RenderFirmsGrid(DateTime.Today);
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju upozorenja");
            }
        }

        // ── Summary panel ─────────────────────────────────────────────────────

        private void RenderSummaryPanel(DateTime today)
        {
            int expired = _allWarnings.Count(x => x.DatumIsteka < today);
            int critical = _allWarnings.Count(x =>
                x.DatumIsteka >= today &&
                x.DatumIsteka <= today.AddDays(AppConstants.DanaKriticnoUpozorenje));
            int upcoming = _allWarnings.Count(x =>
                x.DatumIsteka > today.AddDays(AppConstants.DanaKriticnoUpozorenje));
            int firmCount = _allWarnings.Select(x => x.KlijentId).Distinct().Count();

            lblSumarij.Text = string.Format(
                UiMessages.WarningSummaryFormat,
                firmCount,
                expired,
                AppConstants.DanaKriticnoUpozorenje,
                critical,
                AppConstants.DanaKriticnoUpozorenje + 1,
                AppConstants.DanaUpozerenja,
                upcoming);

            panelTop.BackColor = SummaryPanelColor(expired, critical);
        }

        private static Color SummaryPanelColor(int expired, int critical) =>
            expired > 0 ? Color.FromArgb(160, 30, 30) :
            critical > 0 ? Color.FromArgb(180, 90, 20) :
                           Color.FromArgb(130, 110, 20);

        // ── Firms grid ────────────────────────────────────────────────────────

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

            GridHelper.BindWithoutEvent(gridFirme, gridFirme_SelectionChanged, grouped);

            if (gridFirme.Columns.Count == 0) return;

            if (gridFirme.Columns.Contains("KlijentId"))
                gridFirme.Columns["KlijentId"].Visible = false;

            GridHelper.ConfigureColumn(gridFirme, "Firma", "Naziv firme", 50);
            GridHelper.ConfigureColumn(gridFirme, "Upozorenja", "Br. upozorenja", 15);
            GridHelper.ConfigureColumn(gridFirme, "NajbliziDatum", "Najbliži datum isteka", 20, "dd.MM.yyyy");
            GridHelper.ConfigureColumn(gridFirme, "DanaDoIsteka", "Dana do isteka", 15);
        }

        // ── Detail grid ───────────────────────────────────────────────────────

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

            GridHelper.ConfigureColumn(gridDetalji, "Tip", "Tip", 15);
            GridHelper.ConfigureColumn(gridDetalji, "ImePrezime", "Ime i prezime", 35);
            GridHelper.ConfigureColumn(gridDetalji, "DatumIsteka", "Datum isteka", 20, "dd.MM.yyyy");
            GridHelper.ConfigureColumn(gridDetalji, "DanaDoIsteka", "Dana do isteka", 15);
            GridHelper.ConfigureColumn(gridDetalji, "Status", "Status", 15);
        }

        // ── Row colourisation ─────────────────────────────────────────────────

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
            expiryDate < today ? UiMessages.WarningStatusExpired :
            expiryDate <= today.AddDays(AppConstants.DanaKriticnoUpozorenje) ? UiMessages.WarningStatusCritical :
                                                                               UiMessages.WarningStatusUpcoming;

        private static Color RowColorForDays(int days) =>
            days < 0 ? Color.FromArgb(220, 80, 80) :
            days <= AppConstants.DanaKriticnoUpozorenje ? Color.FromArgb(255, 200, 120) :
                                                          Color.FromArgb(255, 245, 150);
    }
}