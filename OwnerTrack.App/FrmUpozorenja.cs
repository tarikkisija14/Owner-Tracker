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
        private int _hoverRowFirme = -1;
        private int _hoverRowDetalji = -1;

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

            lblStatFirmi.Text = firmCount.ToString();

            lblStatIsteklo.Text = expired.ToString();
            lblStatIsteklo.ForeColor = expired > 0 ? UiTheme.AlertRedOnDark : Color.White;

            lblStatKriticno.Text = critical.ToString();
            lblStatKriticno.ForeColor = critical > 0 ? UiTheme.AlertAmberOnDark : Color.White;

            lblStatUskoro.Text = upcoming.ToString();
        }

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
            lblEmptyFirme.Visible = grouped.Count == 0;

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
                // Ako nema nijedne firme, gornji prazan-prostor tekst (lblEmptyFirme)
                // već objašnjava situaciju — ne treba i dupli "izaberi firmu" ispod.
                lblEmptyDetalji.Visible = gridFirme.Rows.Count > 0;
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
            lblEmptyDetalji.Visible = false;

            if (gridDetalji.Columns.Count == 0) return;

            GridHelper.ConfigureColumn(gridDetalji, "Tip", "Tip", 15);
            GridHelper.ConfigureColumn(gridDetalji, "ImePrezime", "Ime i prezime", 35);
            GridHelper.ConfigureColumn(gridDetalji, "DatumIsteka", "Datum isteka", 20, "dd.MM.yyyy");
            GridHelper.ConfigureColumn(gridDetalji, "DanaDoIsteka", "Dana do isteka", 15);
            GridHelper.ConfigureColumn(gridDetalji, "Status", "Status", 15);
        }

        // ── Row colourisation ─────────────────────────────────────────────────

        private void gridFirme_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => ColorizeRow(gridFirme, e, _hoverRowFirme);

        private void gridDetalji_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
            => ColorizeRow(gridDetalji, e, _hoverRowDetalji);

        private static void ColorizeRow(DataGridView grid, DataGridViewCellFormattingEventArgs e, int hoverRow)
        {
            if (e.RowIndex < 0 || grid.Rows[e.RowIndex].DataBoundItem is null) return;
            dynamic item = grid.Rows[e.RowIndex].DataBoundItem;
            Color baseColor = RowColorForDays(item.DanaDoIsteka);
            grid.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                e.RowIndex == hoverRow ? ControlPaint.Dark(baseColor, 0.06f) : baseColor;
        }

        // ── Hover feedback (potamni trenutnu boju reda umjesto da je zamijeni) ──

        private void gridFirme_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            _hoverRowFirme = e.RowIndex;
            if (e.RowIndex >= 0) gridFirme.InvalidateRow(e.RowIndex);
        }

        private void gridFirme_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == _hoverRowFirme) _hoverRowFirme = -1;
            if (e.RowIndex >= 0) gridFirme.InvalidateRow(e.RowIndex);
        }

        private void gridDetalji_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            _hoverRowDetalji = e.RowIndex;
            if (e.RowIndex >= 0) gridDetalji.InvalidateRow(e.RowIndex);
        }

        private void gridDetalji_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == _hoverRowDetalji) _hoverRowDetalji = -1;
            if (e.RowIndex >= 0) gridDetalji.InvalidateRow(e.RowIndex);
        }

        private void btnZatvori_Click(object sender, EventArgs e) => Close();

        

        private static int DaysUntilExpiry(DateTime expiryDate, DateTime today) =>
            (int)(expiryDate - today).TotalDays;

        private static string WarningStatusText(DateTime expiryDate, DateTime today) =>
            expiryDate < today ? UiMessages.WarningStatusExpired :
            expiryDate <= today.AddDays(AppConstants.DanaKriticnoUpozorenje) ? UiMessages.WarningStatusCritical :
                                                                               UiMessages.WarningStatusUpcoming;

        private static Color RowColorForDays(int days) =>
            days < 0 ? UiColors.RowExpired :
            days <= AppConstants.DanaKriticnoUpozorenje ? UiColors.RowCritical :
                                                          UiColors.RowUpcoming;
    }
}