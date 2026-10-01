using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

namespace OwnerTrack.App
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitMain;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelMainContent = new Panel();
            panelViewKlijenti = new Panel();
            panelSidebar = new Panel();
            btnToggleSidebar = new IconButton();
            panelToggleDivider = new Panel();
            panelSidebarBrand = new Panel();
            lblSidebarBrand = new Label();
            panelSidebarBrandDivider = new Panel();
            btnNavDashboard = new IconButton();
            btnNavKlijenti = new IconButton();
            btnNavKyc = new IconButton();
            btnNavUbo = new IconButton();
            btnNavPep = new IconButton();
            btnNavRizik = new IconButton();
            btnNavBezUgovora = new IconButton();
            btnNavOtkazani = new IconButton();
            btnNavUdruzenja = new IconButton();
            btnNavStecaj = new IconButton();
            btnNavAuditLog = new IconButton();
            btnNavProfil = new IconButton();
            btnNavLogout = new IconButton();
            panelSidebarBottomDivider = new Panel();
            panelSidebarLogoutDivider = new Panel();
            panelViewProfil = new Panel();
            panelProfilHeader = new Panel();
            lblProfilTitle = new Label();
            groupBoxProfil = new GroupBox();
            lblProfilKorisnickoIme = new Label();
            lblProfilKorisnickoImeValue = new Label();
            lblProfilPrikaznoIme = new Label();
            lblProfilPrikaznoImeValue = new Label();
            lblProfilStatus = new Label();
            lblProfilStatusValue = new Label();
            lblProfilZadnjaPrijava = new Label();
            lblProfilZadnjaPrijavaValue = new Label();
            groupBoxAktivnost = new GroupBox();
            lblAktivnostSazetak = new Label();
            dataGridAktivnost = new DataGridView();
            lblEmptyAktivnost = new Label();
            btnPromijeniLozinku = new IconButton();
            btnNoviKorisnik = new IconButton();
            btnKorisnici = new IconButton();
            picProfil = new PictureBox();
            btnOdaberiSliku = new IconButton();
            btnUkloniSliku = new IconButton();
            panelSidebarDivider = new Panel();
            panelViewDashboard = new Panel();
            panelDashboardHeader = new Panel();
            lblDashboardTitle = new Label();
            lblConfidiaBrand = new Label();
            panelDashboardTiles = new TableLayoutPanel();
            tileAktivniKlijenti = new StatTile();
            tileKyc = new StatTile();
            tileVlasnici = new StatTile();
            tilePep = new StatTile();
            tileRizik = new StatTile();
            tileBezUgovora = new StatTile();
            tileUpozorenja = new StatTile();
            tileArhivirani = new StatTile();
            tileUdruzenja = new StatTile();
            tileStecaj = new StatTile();
            tileAuditLog = new StatTile();
            tileDodajKlijenta = new StatTile();
            tileOtkaziKlijenta = new StatTile();
            tileExportPdf = new StatTile();
            tileDjelatnosti = new StatTile();
            tileOsvjezi = new StatTile();
            btnDashboardExportPdfProxy = new Button();
            panelViewBezUgovora = new Panel();
            panelBezUgovoraHeader = new Panel();
            lblBezUgovoraTitle = new Label();
            btnBezUgovoraSacuvajPdf = new IconButton();
            btnBezUgovoraExportPdf = new IconButton();
            dataGridBezUgovora = new DataGridView();
            lblEmptyBezUgovora = new Label();
            panelViewKyc = new Panel();
            panelKycHeader = new Panel();
            lblKycTitle = new Label();
            btnKycSacuvajPdf = new IconButton();
            btnKycExportPdf = new IconButton();
            dataGridKyc = new DataGridView();
            lblEmptyKyc = new Label();
            panelViewUbo = new Panel();
            panelUboHeader = new Panel();
            lblUboTitle = new Label();
            btnUboSacuvajPdf = new IconButton();
            btnUboExportPdf = new IconButton();
            dataGridUbo = new DataGridView();
            lblEmptyUbo = new Label();
            panelViewPep = new Panel();
            panelPepHeader = new Panel();
            lblPepTitle = new Label();
            btnPepSacuvajPdf = new IconButton();
            btnPepExportPdf = new IconButton();
            dataGridPep = new DataGridView();
            lblEmptyPep = new Label();
            panelViewRizik = new Panel();
            panelRizikHeader = new Panel();
            lblRizikTitle = new Label();
            btnRizikSacuvajPdf = new IconButton();
            btnRizikExportPdf = new IconButton();
            dataGridRizik = new DataGridView();
            lblEmptyRizik = new Label();
            panelViewOtkazani = new Panel();
            panelOtkazaniHeader = new Panel();
            lblOtkazaniTitle = new Label();
            dataGridOtkazani = new DataGridView();
            lblEmptyOtkazani = new Label();
            panelViewUdruzenja = new Panel();
            panelUdruzenjaHeader = new Panel();
            lblUdruzenjaTitle = new Label();
            btnUdruzenjaSacuvajPdf = new IconButton();
            btnUdruzenjaExportPdf = new IconButton();
            dataGridUdruzenja = new DataGridView();
            lblEmptyUdruzenja = new Label();
            panelViewStecaj = new Panel();
            panelStecajHeader = new Panel();
            lblStecajTitle = new Label();
            btnStecajSacuvajPdf = new IconButton();
            btnStecajExportPdf = new IconButton();
            dataGridStecaj = new DataGridView();
            lblEmptyStecaj = new Label();
            panelViewAuditLog = new Panel();
            panelAuditLogHeader = new Panel();
            lblAuditLogTitle = new Label();
            dataGridAuditLog = new DataGridView();
            lblEmptyAuditLog = new Label();
            panelToolbar = new Panel();
            btnResetImport = new IconButton();
            btnDodajKlijent = new IconButton();
            btnIzmijeniKlijent = new IconButton();
            btnObrisiKlijent = new IconButton();
            btnImportExcel = new IconButton();
            btnUpozorenja = new IconButton();
            btnExportTabelaPdf = new IconButton();
            btnSacuvajPdf = new IconButton();
            panelSearch = new Panel();
            lblSearchKlijent = new Label();
            txtSearchKlijent = new TextBox();
            lblFilterDjelatnost = new Label();
            cmbFilterDjelatnost = new ComboBox();
            lblFilterVelicina = new Label();
            cmbFilterVelicina = new ComboBox();
            btnResetFilters = new IconButton();
            lblKlijentiCount = new Label();
            dataGridKlijenti = new DataGridView();
            dataGridVlasnici = new DataGridView();
            dataGridDirektori = new DataGridView();
            lblEmptyKlijenti = new Label();
            lblEmptyVlasnici = new Label();
            lblEmptyDirektori = new Label();
            btnDodajVlasnika = new IconButton();
            btnIzmijeniVlasnika = new IconButton();
            btnObrisiVlasnika = new IconButton();
            btnDodajDirektora = new IconButton();
            btnIzmijeniDirektora = new IconButton();
            btnObrisiDirektora = new IconButton();
            splitMain = new SplitContainer();
            splitBottom = new SplitContainer();
            panelVlasnici = new Panel();
            panelVlasniciBtns = new Panel();
            panelDirektori = new Panel();
            panelDirektoriBtns = new Panel();
            panelToolbar.SuspendLayout();
            panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridKlijenti).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridVlasnici).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridDirektori).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridBezUgovora).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridKyc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridUbo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridPep).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridRizik).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridOtkazani).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridUdruzenja).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridStecaj).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridAuditLog).BeginInit();
            panelViewDashboard.SuspendLayout();
            panelViewBezUgovora.SuspendLayout();
            panelViewKyc.SuspendLayout();
            panelViewUbo.SuspendLayout();
            panelViewPep.SuspendLayout();
            panelViewRizik.SuspendLayout();
            panelViewOtkazani.SuspendLayout();
            panelViewUdruzenja.SuspendLayout();
            panelViewStecaj.SuspendLayout();
            panelViewAuditLog.SuspendLayout();
            panelDashboardHeader.SuspendLayout();
            panelBezUgovoraHeader.SuspendLayout();
            panelKycHeader.SuspendLayout();
            panelUboHeader.SuspendLayout();
            panelPepHeader.SuspendLayout();
            panelRizikHeader.SuspendLayout();
            panelOtkazaniHeader.SuspendLayout();
            panelUdruzenjaHeader.SuspendLayout();
            panelStecajHeader.SuspendLayout();
            panelAuditLogHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitBottom).BeginInit();
            splitBottom.Panel1.SuspendLayout();
            splitBottom.Panel2.SuspendLayout();
            splitBottom.SuspendLayout();
            panelVlasnici.SuspendLayout();
            panelVlasniciBtns.SuspendLayout();
            panelDirektori.SuspendLayout();
            panelDirektoriBtns.SuspendLayout();
            SuspendLayout();

            // ── panelToolbar ──────────────────────────────────────
            panelToolbar.BackColor = UiTheme.PanelLight;
            panelToolbar.Controls.Add(btnResetImport);
            panelToolbar.Controls.Add(btnDodajKlijent);
            panelToolbar.Controls.Add(btnIzmijeniKlijent);
            panelToolbar.Controls.Add(btnObrisiKlijent);
            panelToolbar.Controls.Add(btnImportExcel);
            panelToolbar.Controls.Add(btnUpozorenja);
            panelToolbar.Controls.Add(btnExportTabelaPdf);
            panelToolbar.Controls.Add(btnSacuvajPdf);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Location = new Point(0, 0);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new Size(1450, 52);
            panelToolbar.TabIndex = 2;
            panelToolbar.Padding = new Padding(6, 0, 6, 0);

            // btnDodajKlijent
            btnDodajKlijent.Location = new Point(10, 8);
            btnDodajKlijent.Name = "btnDodajKlijent";
            btnDodajKlijent.Size = new Size(150, 36);
            btnDodajKlijent.TabIndex = 0;
            btnDodajKlijent.Text = "Dodaj klijenta";
            btnDodajKlijent.IconGlyph = "\uE710";
            UiTheme.StyleAccentButton(btnDodajKlijent, UiTheme.Green, 10f);
            btnDodajKlijent.Click += btnDodajKlijent_Click;

            // btnIzmijeniKlijent
            btnIzmijeniKlijent.Location = new Point(172, 8);
            btnIzmijeniKlijent.Name = "btnIzmijeniKlijent";
            btnIzmijeniKlijent.Size = new Size(125, 36);
            btnIzmijeniKlijent.TabIndex = 1;
            btnIzmijeniKlijent.Text = "Izmijeni";
            btnIzmijeniKlijent.IconGlyph = "\uE70F";
            UiTheme.StyleAccentButton(btnIzmijeniKlijent, UiTheme.Blue, 10f);
            btnIzmijeniKlijent.Click += btnIzmijeniKlijent_Click;

            // btnObrisiKlijent
            btnObrisiKlijent.Location = new Point(309, 8);
            btnObrisiKlijent.Name = "btnObrisiKlijent";
            btnObrisiKlijent.Size = new Size(125, 36);
            btnObrisiKlijent.TabIndex = 2;
            btnObrisiKlijent.Text = "Obriši";
            btnObrisiKlijent.IconGlyph = "\uE74D";
            UiTheme.StyleAccentButton(btnObrisiKlijent, UiTheme.Red, 10f);
            btnObrisiKlijent.Click += btnObrisiKlijent_Click;

            // btnImportExcel
            btnImportExcel.Location = new Point(448, 8);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(155, 36);
            btnImportExcel.TabIndex = 3;
            btnImportExcel.Text = "Import Excel";
            btnImportExcel.IconGlyph = "\uE896";
            UiTheme.StyleAccentButton(btnImportExcel, UiTheme.ImportAccent, 10f);
            btnImportExcel.Click += btnImportExcel_Click;

            // btnResetImport
            btnResetImport.Location = new Point(615, 8);
            btnResetImport.Name = "btnResetImport";
            btnResetImport.Size = new Size(210, 36);
            btnResetImport.TabIndex = 4;
            btnResetImport.Text = "Resetuj i reimportuj";
            btnResetImport.IconGlyph = "\uE777";
            UiTheme.StyleAccentButton(btnResetImport, UiTheme.ResetAccent, 10f);
            btnResetImport.Click += btnResetImport_Click;

            // btnSacuvajPdf
            btnSacuvajPdf.Location = new Point(857, 8);
            btnSacuvajPdf.Name = "btnSacuvajPdf";
            btnSacuvajPdf.Size = new Size(168, 36);
            btnSacuvajPdf.TabIndex = 6;
            btnSacuvajPdf.Text = "Sačuvaj kao PDF";
            btnSacuvajPdf.IconGlyph = "\uE8A5";
            UiTheme.StyleAccentButton(btnSacuvajPdf, UiTheme.PdfSaveAccent, 10f);
            btnSacuvajPdf.Click += btnSacuvajPdf_Click;

            // btnExportTabelaPdf
            btnExportTabelaPdf.Location = new Point(1037, 8);
            btnExportTabelaPdf.Name = "btnExportTabelaPdf";
            btnExportTabelaPdf.Size = new Size(215, 36);
            btnExportTabelaPdf.TabIndex = 7;
            btnExportTabelaPdf.Text = "Sačuvaj tabelu kao PDF";
            btnExportTabelaPdf.IconGlyph = "\uE71D";
            UiTheme.StyleAccentButton(btnExportTabelaPdf, UiTheme.PdfExportAccent, 10f);
            btnExportTabelaPdf.Click += btnExportTabelaPdf_Click;

            // btnUpozorenja
            btnUpozorenja.Location = new Point(1277, 8);
            btnUpozorenja.Name = "btnUpozorenja";
            btnUpozorenja.Size = new Size(168, 36);
            btnUpozorenja.TabIndex = 5;
            btnUpozorenja.Text = "Upozorenja (0)";
            btnUpozorenja.IconGlyph = "\uE7E7";
            UiTheme.StyleAccentButton(btnUpozorenja, UiTheme.WarningsAccent, 10f);
            btnUpozorenja.Click += btnUpozorenja_Click;

            // ── panelSearch ───────────────────────────────────────
            panelSearch.BackColor = UiTheme.PanelLight;
            panelSearch.BorderStyle = BorderStyle.None;
            panelSearch.Controls.Add(lblSearchKlijent);
            panelSearch.Controls.Add(lblFilterDjelatnost);
            panelSearch.Controls.Add(cmbFilterDjelatnost);
            panelSearch.Controls.Add(lblFilterVelicina);
            panelSearch.Controls.Add(cmbFilterVelicina);
            panelSearch.Controls.Add(btnResetFilters);
            panelSearch.Controls.Add(lblKlijentiCount);
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Location = new Point(0, 52);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(1450, 44);
            panelSearch.TabIndex = 1;

            var searchFont = new Font("Segoe UI", 9F);

            lblSearchKlijent.AutoSize = true;
            lblSearchKlijent.Font = searchFont;
            lblSearchKlijent.ForeColor = Color.FromArgb(50, 60, 80);
            lblSearchKlijent.Location = new Point(12, 13);
            lblSearchKlijent.Name = "lblSearchKlijent";
            lblSearchKlijent.TabIndex = 0;
            lblSearchKlijent.Text = "🔍 Pretraži firmu po nazivu ili ID:";

            txtSearchKlijent.Name = "txtSearchKlijent";
            txtSearchKlijent.Font = searchFont;
            txtSearchKlijent.TabIndex = 1;
            txtSearchKlijent.TextChanged += txtSearchKlijent_TextChanged;
            panelSearch.Controls.Add(UiTheme.WrapWithFocusBorder(txtSearchKlijent, new Point(195, 10), new Size(230, 23)));

            lblFilterDjelatnost.AutoSize = true;
            lblFilterDjelatnost.Font = searchFont;
            lblFilterDjelatnost.ForeColor = Color.FromArgb(50, 60, 80);
            lblFilterDjelatnost.Location = new Point(468, 13);
            lblFilterDjelatnost.Name = "lblFilterDjelatnost";
            lblFilterDjelatnost.TabIndex = 2;
            lblFilterDjelatnost.Text = "🏭 Djelatnost:";

            cmbFilterDjelatnost.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterDjelatnost.Font = searchFont;
            cmbFilterDjelatnost.Location = new Point(560, 10);
            cmbFilterDjelatnost.Name = "cmbFilterDjelatnost";
            cmbFilterDjelatnost.Size = new Size(300, 23);
            cmbFilterDjelatnost.TabIndex = 3;
            cmbFilterDjelatnost.SelectedIndexChanged += cmbFilterDjelatnost_SelectedIndexChanged;

            lblFilterVelicina.AutoSize = true;
            lblFilterVelicina.Font = searchFont;
            lblFilterVelicina.ForeColor = Color.FromArgb(50, 60, 80);
            lblFilterVelicina.Location = new Point(876, 13);
            lblFilterVelicina.Name = "lblFilterVelicina";
            lblFilterVelicina.TabIndex = 5;
            lblFilterVelicina.Text = "📏 Veličina:";

            cmbFilterVelicina.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterVelicina.Font = searchFont;
            cmbFilterVelicina.Location = new Point(948, 10);
            cmbFilterVelicina.Name = "cmbFilterVelicina";
            cmbFilterVelicina.Size = new Size(130, 23);
            cmbFilterVelicina.TabIndex = 6;
            cmbFilterVelicina.SelectedIndexChanged += cmbFilterVelicina_SelectedIndexChanged;

            btnResetFilters.Location = new Point(1092, 7);
            btnResetFilters.Name = "btnResetFilters";
            btnResetFilters.Size = new Size(115, 30);
            btnResetFilters.TabIndex = 4;
            btnResetFilters.Text = "Resetuj";
            btnResetFilters.IconGlyph = "\uE72C";
            UiTheme.StyleAccentButton(btnResetFilters, UiTheme.Blue, 8.5f);
            btnResetFilters.Click += btnResetFilters_Click;

            lblKlijentiCount.AutoSize = true;
            lblKlijentiCount.Font = searchFont;
            lblKlijentiCount.ForeColor = UiTheme.MutedText;
            lblKlijentiCount.Location = new Point(1230, 13);
            lblKlijentiCount.Name = "lblKlijentiCount";
            lblKlijentiCount.TabIndex = 8;
            lblKlijentiCount.Text = string.Empty;

            // ── dataGridKlijenti ──────────────────────────────────
            dataGridKlijenti.AllowUserToAddRows = false;
            dataGridKlijenti.AllowUserToDeleteRows = false;
            dataGridKlijenti.Dock = DockStyle.Fill;
            dataGridKlijenti.Location = new Point(0, 0);
            dataGridKlijenti.MultiSelect = false;
            dataGridKlijenti.Name = "dataGridKlijenti";
            dataGridKlijenti.ReadOnly = true;
            dataGridKlijenti.RowHeadersVisible = false;
            dataGridKlijenti.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridKlijenti.Size = new Size(1450, 536);
            dataGridKlijenti.TabIndex = 0;
            UiTheme.StyleGrid(dataGridKlijenti);
            dataGridKlijenti.SelectionChanged += dataGridKlijenti_SelectionChanged;

            // ── lblEmptyKlijenti ──────────────────────────────────
            UiTheme.StyleEmptyState(lblEmptyKlijenti, "Nema klijenata za prikaz.");

            // ── dataGridVlasnici ──────────────────────────────────
            dataGridVlasnici.AllowUserToAddRows = false;
            dataGridVlasnici.AllowUserToDeleteRows = false;
            dataGridVlasnici.Dock = DockStyle.Fill;
            dataGridVlasnici.Location = new Point(0, 38);
            dataGridVlasnici.MultiSelect = false;
            dataGridVlasnici.Name = "dataGridVlasnici";
            dataGridVlasnici.ReadOnly = true;
            dataGridVlasnici.RowHeadersVisible = false;
            dataGridVlasnici.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridVlasnici.Size = new Size(656, 177);
            dataGridVlasnici.TabIndex = 0;
            UiTheme.StyleGrid(dataGridVlasnici);

            // ── lblEmptyVlasnici ──────────────────────────────────
            UiTheme.StyleEmptyState(lblEmptyVlasnici, "Nema evidentiranih vlasnika.");

            // ── dataGridDirektori ─────────────────────────────────
            dataGridDirektori.AllowUserToAddRows = false;
            dataGridDirektori.AllowUserToDeleteRows = false;
            dataGridDirektori.Dock = DockStyle.Fill;
            dataGridDirektori.Location = new Point(0, 38);
            dataGridDirektori.MultiSelect = false;
            dataGridDirektori.Name = "dataGridDirektori";
            dataGridDirektori.ReadOnly = true;
            dataGridDirektori.RowHeadersVisible = false;
            dataGridDirektori.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridDirektori.Size = new Size(790, 177);
            dataGridDirektori.TabIndex = 0;
            UiTheme.StyleGrid(dataGridDirektori);

            // ── lblEmptyDirektori ─────────────────────────────────
            UiTheme.StyleEmptyState(lblEmptyDirektori, "Nema evidentiranih direktora.");

            // btnDodajVlasnika
            btnDodajVlasnika.Location = new Point(9, 5);
            btnDodajVlasnika.Name = "btnDodajVlasnika";
            btnDodajVlasnika.Size = new Size(138, 30);
            btnDodajVlasnika.TabIndex = 0;
            btnDodajVlasnika.Text = "Dodaj vlasnika";
            btnDodajVlasnika.IconGlyph = "\uE710";
            UiTheme.StyleAccentButton(btnDodajVlasnika, UiTheme.Green, 8.5f);
            btnDodajVlasnika.Click += btnDodajVlasnika_Click;

            btnIzmijeniVlasnika.Location = new Point(159, 5);
            btnIzmijeniVlasnika.Name = "btnIzmijeniVlasnika";
            btnIzmijeniVlasnika.Size = new Size(110, 30);
            btnIzmijeniVlasnika.TabIndex = 1;
            btnIzmijeniVlasnika.Text = "Izmijeni";
            btnIzmijeniVlasnika.IconGlyph = "\uE70F";
            UiTheme.StyleAccentButton(btnIzmijeniVlasnika, UiTheme.Blue, 8.5f);
            btnIzmijeniVlasnika.Click += btnIzmijeniVlasnika_Click;

            btnObrisiVlasnika.Location = new Point(281, 5);
            btnObrisiVlasnika.Name = "btnObrisiVlasnika";
            btnObrisiVlasnika.Size = new Size(110, 30);
            btnObrisiVlasnika.TabIndex = 2;
            btnObrisiVlasnika.Text = "Obriši";
            btnObrisiVlasnika.IconGlyph = "\uE74D";
            UiTheme.StyleAccentButton(btnObrisiVlasnika, UiTheme.Red, 8.5f);
            btnObrisiVlasnika.Click += btnObrisiVlasnika_Click;

            btnDodajDirektora.Location = new Point(9, 5);
            btnDodajDirektora.Name = "btnDodajDirektora";
            btnDodajDirektora.Size = new Size(158, 30);
            btnDodajDirektora.TabIndex = 0;
            btnDodajDirektora.Text = "Dodaj direktora";
            btnDodajDirektora.IconGlyph = "\uE710";
            UiTheme.StyleAccentButton(btnDodajDirektora, UiTheme.Green, 8.5f);
            btnDodajDirektora.Click += btnDodajDirektora_Click;

            btnIzmijeniDirektora.Location = new Point(179, 5);
            btnIzmijeniDirektora.Name = "btnIzmijeniDirektora";
            btnIzmijeniDirektora.Size = new Size(110, 30);
            btnIzmijeniDirektora.TabIndex = 1;
            btnIzmijeniDirektora.Text = "Izmijeni";
            btnIzmijeniDirektora.IconGlyph = "\uE70F";
            UiTheme.StyleAccentButton(btnIzmijeniDirektora, UiTheme.Blue, 8.5f);
            btnIzmijeniDirektora.Click += btnIzmijeniDirektora_Click;

            btnObrisiDirektora.Location = new Point(301, 5);
            btnObrisiDirektora.Name = "btnObrisiDirektora";
            btnObrisiDirektora.Size = new Size(110, 30);
            btnObrisiDirektora.TabIndex = 2;
            btnObrisiDirektora.Text = "Obriši";
            btnObrisiDirektora.IconGlyph = "\uE74D";
            UiTheme.StyleAccentButton(btnObrisiDirektora, UiTheme.Red, 8.5f);
            btnObrisiDirektora.Click += btnObrisiDirektora_Click;

            // ── panelVlasniciBtns ─────────────────────────────────
            panelVlasniciBtns.BackColor = UiTheme.PanelLight;
            panelVlasniciBtns.Controls.Add(btnDodajVlasnika);
            panelVlasniciBtns.Controls.Add(btnIzmijeniVlasnika);
            panelVlasniciBtns.Controls.Add(btnObrisiVlasnika);
            panelVlasniciBtns.Dock = DockStyle.Top;
            panelVlasniciBtns.Location = new Point(0, 0);
            panelVlasniciBtns.Name = "panelVlasniciBtns";
            panelVlasniciBtns.Size = new Size(656, 40);
            panelVlasniciBtns.TabIndex = 1;

            // ── panelVlasnici ─────────────────────────────────────
            panelVlasnici.Controls.Add(dataGridVlasnici);
            panelVlasnici.Controls.Add(lblEmptyVlasnici);
            panelVlasnici.Controls.Add(panelVlasniciBtns);
            panelVlasnici.Dock = DockStyle.Fill;
            panelVlasnici.Location = new Point(0, 0);
            panelVlasnici.Name = "panelVlasnici";
            panelVlasnici.Size = new Size(656, 215);
            panelVlasnici.TabIndex = 0;

            // ── panelDirektoriBtns ────────────────────────────────
            panelDirektoriBtns.BackColor = UiTheme.PanelLight;
            panelDirektoriBtns.Controls.Add(btnDodajDirektora);
            panelDirektoriBtns.Controls.Add(btnIzmijeniDirektora);
            panelDirektoriBtns.Controls.Add(btnObrisiDirektora);
            panelDirektoriBtns.Dock = DockStyle.Top;
            panelDirektoriBtns.Location = new Point(0, 0);
            panelDirektoriBtns.Name = "panelDirektoriBtns";
            panelDirektoriBtns.Size = new Size(790, 40);
            panelDirektoriBtns.TabIndex = 1;

            // ── panelDirektori ────────────────────────────────────
            panelDirektori.Controls.Add(dataGridDirektori);
            panelDirektori.Controls.Add(lblEmptyDirektori);
            panelDirektori.Controls.Add(panelDirektoriBtns);
            panelDirektori.Dock = DockStyle.Fill;
            panelDirektori.Location = new Point(0, 0);
            panelDirektori.Name = "panelDirektori";
            panelDirektori.Size = new Size(790, 215);
            panelDirektori.TabIndex = 0;

            // ── splitBottom ───────────────────────────────────────
            splitBottom.Dock = DockStyle.Fill;
            splitBottom.Location = new Point(0, 0);
            splitBottom.Name = "splitBottom";
            splitBottom.Panel1.Controls.Add(panelVlasnici);
            splitBottom.Panel1MinSize = 50;
            splitBottom.Panel2.Controls.Add(panelDirektori);
            splitBottom.Panel2MinSize = 50;
            splitBottom.Size = new Size(1450, 215);
            splitBottom.SplitterDistance = 656;
            splitBottom.TabIndex = 0;

            // ── splitMain ─────────────────────────────────────────
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 96);
            splitMain.Name = "splitMain";
            splitMain.Orientation = Orientation.Horizontal;
            splitMain.Panel1.Controls.Add(dataGridKlijenti);
            splitMain.Panel1.Controls.Add(lblEmptyKlijenti);
            splitMain.Panel1MinSize = 100;
            splitMain.Panel2.Controls.Add(splitBottom);
            splitMain.Panel2MinSize = 100;
            splitMain.Size = new Size(1450, 755);
            splitMain.SplitterDistance = 536;
            splitMain.TabIndex = 0;

            // ── panelViewDashboard ─────────────────────────────────
            lblDashboardTitle.Dock = DockStyle.Left;
            lblDashboardTitle.AutoSize = true;
            lblDashboardTitle.BackColor = UiTheme.PanelLight;
            lblDashboardTitle.ForeColor = UiTheme.Navy;
            lblDashboardTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblDashboardTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblDashboardTitle.Padding = new Padding(12, 0, 0, 0);
            lblDashboardTitle.Text = "Početni ekran";
            lblDashboardTitle.Name = "lblDashboardTitle";

            lblConfidiaBrand.Dock = DockStyle.Fill;
            lblConfidiaBrand.BackColor = UiTheme.PanelLight;
            lblConfidiaBrand.ForeColor = UiTheme.Navy;
            lblConfidiaBrand.Font = UiTheme.Base(14f, FontStyle.Bold);
            lblConfidiaBrand.TextAlign = ContentAlignment.MiddleCenter;
            lblConfidiaBrand.Text = "CONFIDIA BH";
            lblConfidiaBrand.Name = "lblConfidiaBrand";

            panelDashboardHeader.Dock = DockStyle.Top;
            panelDashboardHeader.Height = 48;
            panelDashboardHeader.BackColor = UiTheme.PanelLight;
            panelDashboardHeader.Name = "panelDashboardHeader";
            panelDashboardHeader.Controls.Add(lblConfidiaBrand);

            // ── akcijske kartice (bez brojčane statistike, samo ikona) ──
            tileDodajKlijenta.Title = "Dodaj klijenta";
            tileDodajKlijenta.AccentColor = UiTheme.Green;
            tileDodajKlijenta.IconGlyph = "";
            tileDodajKlijenta.Name = "tileDodajKlijenta";
            tileDodajKlijenta.TileClick += tileDodajKlijenta_TileClick;

            tileOtkaziKlijenta.Title = "Otkaži klijenta";
            tileOtkaziKlijenta.AccentColor = UiTheme.Red;
            tileOtkaziKlijenta.IconGlyph = "";
            tileOtkaziKlijenta.Name = "tileOtkaziKlijenta";
            tileOtkaziKlijenta.TileClick += tileOtkaziKlijenta_TileClick;

            tileExportPdf.Title = "Izvoz PDF izvještaja";
            tileExportPdf.AccentColor = UiTheme.PdfExportAccent;
            tileExportPdf.IconGlyph = "";
            tileExportPdf.Name = "tileExportPdf";
            tileExportPdf.TileClick += tileExportPdf_TileClick;

            btnDashboardExportPdfProxy.Visible = false;
            btnDashboardExportPdfProxy.Text = "Sačuvaj izvještaj kao PDF";
            btnDashboardExportPdfProxy.Name = "btnDashboardExportPdfProxy";

            tileDjelatnosti.Title = "Djelatnosti";
            tileDjelatnosti.AccentColor = Color.White;
            tileDjelatnosti.Name = "tileDjelatnosti";
            tileDjelatnosti.TileClick += tileDjelatnosti_TileClick;

            tileOsvjezi.Title = "Osvježi podatke";
            tileOsvjezi.AccentColor = UiTheme.Blue;
            tileOsvjezi.IconGlyph = "";
            tileOsvjezi.Name = "tileOsvjezi";
            tileOsvjezi.TileClick += tileOsvjezi_TileClick;

            // ── kartice — jedna po svakoj sidebar stavci, plus Upozorenja ──
            tileAktivniKlijenti.Title = "Klijenti";
            tileAktivniKlijenti.AccentColor = Color.White;
            tileAktivniKlijenti.Name = "tileAktivniKlijenti";
            tileAktivniKlijenti.TileClick += tileAktivniKlijenti_TileClick;

            tileKyc.Title = "KYC evidencija";
            tileKyc.AccentColor = Color.White;
            tileKyc.Name = "tileKyc";
            tileKyc.TileClick += tileKyc_TileClick;

            tileVlasnici.Title = "UBO / Vlasništvo (vlasnika)";
            tileVlasnici.AccentColor = Color.White;
            tileVlasnici.Name = "tileVlasnici";
            tileVlasnici.TileClick += tileVlasnici_TileClick;

            tilePep.Title = "PEP evidencija";
            tilePep.AccentColor = UiTheme.AlertAmberOnDark;
            tilePep.Name = "tilePep";
            tilePep.TileClick += tilePep_TileClick;

            tileRizik.Title = "Evidencija procjena rizika";
            tileRizik.AccentColor = Color.White;
            tileRizik.Name = "tileRizik";
            tileRizik.TileClick += tileRizik_TileClick;

            tileBezUgovora.Title = "Klijenti bez ugovora";
            tileBezUgovora.AccentColor = UiTheme.AlertAmberOnDark;
            tileBezUgovora.Name = "tileBezUgovora";
            tileBezUgovora.TileClick += tileBezUgovora_TileClick;

            tileUpozorenja.Title = "Upozorenja";
            tileUpozorenja.AccentColor = UiTheme.AlertRedOnDark;
            tileUpozorenja.Name = "tileUpozorenja";
            tileUpozorenja.TileClick += tileUpozorenja_TileClick;

            tileArhivirani.Title = "Otkazani klijenti";
            tileArhivirani.AccentColor = Color.White;
            tileArhivirani.Name = "tileArhivirani";
            tileArhivirani.TileClick += tileArhivirani_TileClick;

            tileUdruzenja.Title = "Udruženja";
            tileUdruzenja.AccentColor = Color.White;
            tileUdruzenja.Name = "tileUdruzenja";
            tileUdruzenja.TileClick += tileUdruzenja_TileClick;

            tileStecaj.Title = "Klijenti u stečaju";
            tileStecaj.AccentColor = Color.White;
            tileStecaj.Name = "tileStecaj";
            tileStecaj.TileClick += tileStecaj_TileClick;

            tileAuditLog.Title = "Historija promjena";
            tileAuditLog.AccentColor = Color.White;
            tileAuditLog.Name = "tileAuditLog";
            tileAuditLog.TileClick += tileAuditLog_TileClick;

            panelDashboardTiles.Dock = DockStyle.Fill;
            panelDashboardTiles.BackColor = UiTheme.FormBackgroundMain;
            panelDashboardTiles.Padding = new Padding(16);
            panelDashboardTiles.Name = "panelDashboardTiles";
            panelDashboardTiles.ColumnCount = 4;
            panelDashboardTiles.RowCount = 4;
            for (int i = 0; i < 4; i++)
                panelDashboardTiles.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            for (int i = 0; i < 4; i++)
                panelDashboardTiles.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            panelDashboardTiles.Controls.Add(tileAktivniKlijenti, 0, 0);
            panelDashboardTiles.Controls.Add(tileKyc, 1, 0);
            panelDashboardTiles.Controls.Add(tileVlasnici, 2, 0);
            panelDashboardTiles.Controls.Add(tilePep, 3, 0);
            panelDashboardTiles.Controls.Add(tileRizik, 0, 1);
            panelDashboardTiles.Controls.Add(tileBezUgovora, 1, 1);
            panelDashboardTiles.Controls.Add(tileUpozorenja, 2, 1);
            panelDashboardTiles.Controls.Add(tileArhivirani, 3, 1);
            panelDashboardTiles.Controls.Add(tileUdruzenja, 0, 2);
            panelDashboardTiles.Controls.Add(tileStecaj, 1, 2);
            panelDashboardTiles.Controls.Add(tileAuditLog, 2, 2);
            panelDashboardTiles.Controls.Add(tileDjelatnosti, 3, 2);
            panelDashboardTiles.Controls.Add(tileDodajKlijenta, 0, 3);
            panelDashboardTiles.Controls.Add(tileOtkaziKlijenta, 1, 3);
            panelDashboardTiles.Controls.Add(tileExportPdf, 2, 3);
            panelDashboardTiles.Controls.Add(tileOsvjezi, 3, 3);

            panelViewDashboard.Dock = DockStyle.Fill;
            panelViewDashboard.Name = "panelViewDashboard";
            panelViewDashboard.BackColor = UiTheme.FormBackgroundMain;
            panelViewDashboard.Visible = false;
            panelViewDashboard.Controls.Add(panelDashboardTiles);
            panelViewDashboard.Controls.Add(panelDashboardHeader);
            panelViewDashboard.Controls.Add(btnDashboardExportPdfProxy);

            // ── dataGridBezUgovora / panelViewBezUgovora ──────────
            dataGridBezUgovora.AllowUserToAddRows = false;
            dataGridBezUgovora.AllowUserToDeleteRows = false;
            dataGridBezUgovora.Dock = DockStyle.Fill;
            dataGridBezUgovora.MultiSelect = false;
            dataGridBezUgovora.Name = "dataGridBezUgovora";
            dataGridBezUgovora.ReadOnly = true;
            dataGridBezUgovora.RowHeadersVisible = false;
            dataGridBezUgovora.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridBezUgovora.TabIndex = 0;
            UiTheme.StyleGrid(dataGridBezUgovora);
            UiTheme.StyleEmptyState(lblEmptyBezUgovora, "Nema klijenata bez ugovora.");

            lblBezUgovoraTitle.Dock = DockStyle.Left;
            lblBezUgovoraTitle.AutoSize = true;
            lblBezUgovoraTitle.BackColor = UiTheme.PanelLight;
            lblBezUgovoraTitle.ForeColor = UiTheme.Navy;
            lblBezUgovoraTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblBezUgovoraTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblBezUgovoraTitle.Padding = new Padding(12, 0, 0, 0);
            lblBezUgovoraTitle.Text = "Klijenti bez ugovora";
            lblBezUgovoraTitle.Name = "lblBezUgovoraTitle";

            btnBezUgovoraSacuvajPdf.Location = new Point(220, 6);
            btnBezUgovoraSacuvajPdf.Name = "btnBezUgovoraSacuvajPdf";
            btnBezUgovoraSacuvajPdf.Size = new Size(168, 36);
            btnBezUgovoraSacuvajPdf.Text = "Sačuvaj kao PDF";
            btnBezUgovoraSacuvajPdf.IconGlyph = "";
            UiTheme.StyleAccentButton(btnBezUgovoraSacuvajPdf, UiTheme.PdfSaveAccent, 10f);
            btnBezUgovoraSacuvajPdf.Click += btnBezUgovoraSacuvajPdf_Click;

            btnBezUgovoraExportPdf.Location = new Point(398, 6);
            btnBezUgovoraExportPdf.Name = "btnBezUgovoraExportPdf";
            btnBezUgovoraExportPdf.Size = new Size(180, 36);
            btnBezUgovoraExportPdf.Text = "Sačuvaj tabelu kao PDF";
            btnBezUgovoraExportPdf.IconGlyph = "";
            UiTheme.StyleAccentButton(btnBezUgovoraExportPdf, UiTheme.PdfExportAccent, 10f);
            btnBezUgovoraExportPdf.Click += btnBezUgovoraExportPdf_Click;

            panelBezUgovoraHeader.Dock = DockStyle.Top;
            panelBezUgovoraHeader.Height = 48;
            panelBezUgovoraHeader.BackColor = UiTheme.PanelLight;
            panelBezUgovoraHeader.Name = "panelBezUgovoraHeader";
            panelBezUgovoraHeader.Controls.Add(lblBezUgovoraTitle);
            panelBezUgovoraHeader.Controls.Add(btnBezUgovoraSacuvajPdf);
            panelBezUgovoraHeader.Controls.Add(btnBezUgovoraExportPdf);

            panelViewBezUgovora.Dock = DockStyle.Fill;
            panelViewBezUgovora.Name = "panelViewBezUgovora";
            panelViewBezUgovora.Visible = false;
            panelViewBezUgovora.Controls.Add(dataGridBezUgovora);
            panelViewBezUgovora.Controls.Add(lblEmptyBezUgovora);
            panelViewBezUgovora.Controls.Add(panelBezUgovoraHeader);

            // ── dataGridKyc / panelViewKyc ────────────────────────
            dataGridKyc.AllowUserToAddRows = false;
            dataGridKyc.AllowUserToDeleteRows = false;
            dataGridKyc.Dock = DockStyle.Fill;
            dataGridKyc.MultiSelect = false;
            dataGridKyc.Name = "dataGridKyc";
            dataGridKyc.ReadOnly = true;
            dataGridKyc.RowHeadersVisible = false;
            dataGridKyc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridKyc.TabIndex = 0;
            UiTheme.StyleGrid(dataGridKyc);
            UiTheme.StyleEmptyState(lblEmptyKyc, "Nema klijenata za prikaz.");

            lblKycTitle.Dock = DockStyle.Left;
            lblKycTitle.AutoSize = true;
            lblKycTitle.BackColor = UiTheme.PanelLight;
            lblKycTitle.ForeColor = UiTheme.Navy;
            lblKycTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblKycTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblKycTitle.Padding = new Padding(12, 0, 0, 0);
            lblKycTitle.Text = "KYC evidencija";
            lblKycTitle.Name = "lblKycTitle";

            btnKycSacuvajPdf.Location = new Point(220, 6);
            btnKycSacuvajPdf.Name = "btnKycSacuvajPdf";
            btnKycSacuvajPdf.Size = new Size(168, 36);
            btnKycSacuvajPdf.Text = "Sačuvaj kao PDF";
            btnKycSacuvajPdf.IconGlyph = "\uE8A5";
            UiTheme.StyleAccentButton(btnKycSacuvajPdf, UiTheme.PdfSaveAccent, 10f);
            btnKycSacuvajPdf.Click += btnKycSacuvajPdf_Click;

            btnKycExportPdf.Location = new Point(398, 6);
            btnKycExportPdf.Name = "btnKycExportPdf";
            btnKycExportPdf.Size = new Size(180, 36);
            btnKycExportPdf.Text = "Sačuvaj tabelu kao PDF";
            btnKycExportPdf.IconGlyph = "\uE71D";
            UiTheme.StyleAccentButton(btnKycExportPdf, UiTheme.PdfExportAccent, 10f);
            btnKycExportPdf.Click += btnKycExportPdf_Click;

            panelKycHeader.Dock = DockStyle.Top;
            panelKycHeader.Height = 48;
            panelKycHeader.BackColor = UiTheme.PanelLight;
            panelKycHeader.Name = "panelKycHeader";
            panelKycHeader.Controls.Add(lblKycTitle);
            panelKycHeader.Controls.Add(btnKycSacuvajPdf);
            panelKycHeader.Controls.Add(btnKycExportPdf);

            panelViewKyc.Dock = DockStyle.Fill;
            panelViewKyc.Name = "panelViewKyc";
            panelViewKyc.Visible = false;
            panelViewKyc.Controls.Add(dataGridKyc);
            panelViewKyc.Controls.Add(lblEmptyKyc);
            panelViewKyc.Controls.Add(panelKycHeader);

            // ── dataGridUbo / panelViewUbo ────────────────────────
            dataGridUbo.AllowUserToAddRows = false;
            dataGridUbo.AllowUserToDeleteRows = false;
            dataGridUbo.Dock = DockStyle.Fill;
            dataGridUbo.MultiSelect = false;
            dataGridUbo.Name = "dataGridUbo";
            dataGridUbo.ReadOnly = true;
            dataGridUbo.RowHeadersVisible = false;
            dataGridUbo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridUbo.TabIndex = 0;
            UiTheme.StyleGrid(dataGridUbo);
            UiTheme.StyleEmptyState(lblEmptyUbo, "Nema evidentiranih vlasnika.");

            lblUboTitle.Dock = DockStyle.Left;
            lblUboTitle.AutoSize = true;
            lblUboTitle.BackColor = UiTheme.PanelLight;
            lblUboTitle.ForeColor = UiTheme.Navy;
            lblUboTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblUboTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblUboTitle.Padding = new Padding(12, 0, 0, 0);
            lblUboTitle.Text = "UBO / Vlasništvo";
            lblUboTitle.Name = "lblUboTitle";

            btnUboSacuvajPdf.Location = new Point(220, 6);
            btnUboSacuvajPdf.Name = "btnUboSacuvajPdf";
            btnUboSacuvajPdf.Size = new Size(168, 36);
            btnUboSacuvajPdf.Text = "Sačuvaj kao PDF";
            btnUboSacuvajPdf.IconGlyph = "\uE8A5";
            UiTheme.StyleAccentButton(btnUboSacuvajPdf, UiTheme.PdfSaveAccent, 10f);
            btnUboSacuvajPdf.Click += btnUboSacuvajPdf_Click;

            btnUboExportPdf.Location = new Point(398, 6);
            btnUboExportPdf.Name = "btnUboExportPdf";
            btnUboExportPdf.Size = new Size(180, 36);
            btnUboExportPdf.Text = "Sačuvaj tabelu kao PDF";
            btnUboExportPdf.IconGlyph = "\uE71D";
            UiTheme.StyleAccentButton(btnUboExportPdf, UiTheme.PdfExportAccent, 10f);
            btnUboExportPdf.Click += btnUboExportPdf_Click;

            panelUboHeader.Dock = DockStyle.Top;
            panelUboHeader.Height = 48;
            panelUboHeader.BackColor = UiTheme.PanelLight;
            panelUboHeader.Name = "panelUboHeader";
            panelUboHeader.Controls.Add(lblUboTitle);
            panelUboHeader.Controls.Add(btnUboSacuvajPdf);
            panelUboHeader.Controls.Add(btnUboExportPdf);

            panelViewUbo.Dock = DockStyle.Fill;
            panelViewUbo.Name = "panelViewUbo";
            panelViewUbo.Visible = false;
            panelViewUbo.Controls.Add(dataGridUbo);
            panelViewUbo.Controls.Add(lblEmptyUbo);
            panelViewUbo.Controls.Add(panelUboHeader);

            // ── dataGridPep / panelViewPep ────────────────────────
            dataGridPep.AllowUserToAddRows = false;
            dataGridPep.AllowUserToDeleteRows = false;
            dataGridPep.Dock = DockStyle.Fill;
            dataGridPep.MultiSelect = false;
            dataGridPep.Name = "dataGridPep";
            dataGridPep.ReadOnly = true;
            dataGridPep.RowHeadersVisible = false;
            dataGridPep.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridPep.TabIndex = 0;
            UiTheme.StyleGrid(dataGridPep);
            UiTheme.StyleEmptyState(lblEmptyPep, "Nema klijenata za prikaz.");

            lblPepTitle.Dock = DockStyle.Left;
            lblPepTitle.AutoSize = true;
            lblPepTitle.BackColor = UiTheme.PanelLight;
            lblPepTitle.ForeColor = UiTheme.Navy;
            lblPepTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblPepTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblPepTitle.Padding = new Padding(12, 0, 0, 0);
            lblPepTitle.Text = "PEP evidencija";
            lblPepTitle.Name = "lblPepTitle";

            btnPepSacuvajPdf.Location = new Point(220, 6);
            btnPepSacuvajPdf.Name = "btnPepSacuvajPdf";
            btnPepSacuvajPdf.Size = new Size(168, 36);
            btnPepSacuvajPdf.Text = "Sačuvaj kao PDF";
            btnPepSacuvajPdf.IconGlyph = "\uE8A5";
            UiTheme.StyleAccentButton(btnPepSacuvajPdf, UiTheme.PdfSaveAccent, 10f);
            btnPepSacuvajPdf.Click += btnPepSacuvajPdf_Click;

            btnPepExportPdf.Location = new Point(398, 6);
            btnPepExportPdf.Name = "btnPepExportPdf";
            btnPepExportPdf.Size = new Size(180, 36);
            btnPepExportPdf.Text = "Sačuvaj tabelu kao PDF";
            btnPepExportPdf.IconGlyph = "\uE71D";
            UiTheme.StyleAccentButton(btnPepExportPdf, UiTheme.PdfExportAccent, 10f);
            btnPepExportPdf.Click += btnPepExportPdf_Click;

            panelPepHeader.Dock = DockStyle.Top;
            panelPepHeader.Height = 48;
            panelPepHeader.BackColor = UiTheme.PanelLight;
            panelPepHeader.Name = "panelPepHeader";
            panelPepHeader.Controls.Add(lblPepTitle);
            panelPepHeader.Controls.Add(btnPepSacuvajPdf);
            panelPepHeader.Controls.Add(btnPepExportPdf);

            panelViewPep.Dock = DockStyle.Fill;
            panelViewPep.Name = "panelViewPep";
            panelViewPep.Visible = false;
            panelViewPep.Controls.Add(dataGridPep);
            panelViewPep.Controls.Add(lblEmptyPep);
            panelViewPep.Controls.Add(panelPepHeader);

            // ── dataGridRizik / panelViewRizik ────────────────────
            dataGridRizik.AllowUserToAddRows = false;
            dataGridRizik.AllowUserToDeleteRows = false;
            dataGridRizik.Dock = DockStyle.Fill;
            dataGridRizik.MultiSelect = false;
            dataGridRizik.Name = "dataGridRizik";
            dataGridRizik.ReadOnly = true;
            dataGridRizik.RowHeadersVisible = false;
            dataGridRizik.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridRizik.TabIndex = 0;
            UiTheme.StyleGrid(dataGridRizik);
            UiTheme.StyleEmptyState(lblEmptyRizik, "Nema klijenata za prikaz.");

            lblRizikTitle.Dock = DockStyle.Left;
            lblRizikTitle.AutoSize = true;
            lblRizikTitle.BackColor = UiTheme.PanelLight;
            lblRizikTitle.ForeColor = UiTheme.Navy;
            lblRizikTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblRizikTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblRizikTitle.Padding = new Padding(12, 0, 0, 0);
            lblRizikTitle.Text = "Evidencija procjena rizika klijenata";
            lblRizikTitle.Name = "lblRizikTitle";

            btnRizikSacuvajPdf.Location = new Point(400, 6);
            btnRizikSacuvajPdf.Name = "btnRizikSacuvajPdf";
            btnRizikSacuvajPdf.Size = new Size(168, 36);
            btnRizikSacuvajPdf.Text = "Sačuvaj kao PDF";
            btnRizikSacuvajPdf.IconGlyph = "\uE8A5";
            UiTheme.StyleAccentButton(btnRizikSacuvajPdf, UiTheme.PdfSaveAccent, 10f);
            btnRizikSacuvajPdf.Click += btnRizikSacuvajPdf_Click;

            btnRizikExportPdf.Location = new Point(578, 6);
            btnRizikExportPdf.Name = "btnRizikExportPdf";
            btnRizikExportPdf.Size = new Size(180, 36);
            btnRizikExportPdf.Text = "Sačuvaj tabelu kao PDF";
            btnRizikExportPdf.IconGlyph = "\uE71D";
            UiTheme.StyleAccentButton(btnRizikExportPdf, UiTheme.PdfExportAccent, 10f);
            btnRizikExportPdf.Click += btnRizikExportPdf_Click;

            panelRizikHeader.Dock = DockStyle.Top;
            panelRizikHeader.Height = 48;
            panelRizikHeader.BackColor = UiTheme.PanelLight;
            panelRizikHeader.Name = "panelRizikHeader";
            panelRizikHeader.Controls.Add(lblRizikTitle);
            panelRizikHeader.Controls.Add(btnRizikSacuvajPdf);
            panelRizikHeader.Controls.Add(btnRizikExportPdf);

            panelViewRizik.Dock = DockStyle.Fill;
            panelViewRizik.Name = "panelViewRizik";
            panelViewRizik.Visible = false;
            panelViewRizik.Controls.Add(dataGridRizik);
            panelViewRizik.Controls.Add(lblEmptyRizik);
            panelViewRizik.Controls.Add(panelRizikHeader);

            // ── dataGridOtkazani / panelViewOtkazani ──────────────
            dataGridOtkazani.AllowUserToAddRows = false;
            dataGridOtkazani.AllowUserToDeleteRows = false;
            dataGridOtkazani.Dock = DockStyle.Fill;
            dataGridOtkazani.MultiSelect = false;
            dataGridOtkazani.Name = "dataGridOtkazani";
            dataGridOtkazani.ReadOnly = true;
            dataGridOtkazani.RowHeadersVisible = false;
            dataGridOtkazani.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridOtkazani.TabIndex = 0;
            UiTheme.StyleGrid(dataGridOtkazani);
            UiTheme.StyleEmptyState(lblEmptyOtkazani, "Nema otkazanih klijenata.");

            lblOtkazaniTitle.Dock = DockStyle.Left;
            lblOtkazaniTitle.AutoSize = true;
            lblOtkazaniTitle.BackColor = UiTheme.PanelLight;
            lblOtkazaniTitle.ForeColor = UiTheme.Navy;
            lblOtkazaniTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblOtkazaniTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblOtkazaniTitle.Padding = new Padding(12, 0, 0, 0);
            lblOtkazaniTitle.Text = "Otkazani klijenti";
            lblOtkazaniTitle.Name = "lblOtkazaniTitle";

            panelOtkazaniHeader.Dock = DockStyle.Top;
            panelOtkazaniHeader.Height = 48;
            panelOtkazaniHeader.BackColor = UiTheme.PanelLight;
            panelOtkazaniHeader.Name = "panelOtkazaniHeader";
            panelOtkazaniHeader.Controls.Add(lblOtkazaniTitle);

            panelViewOtkazani.Dock = DockStyle.Fill;
            panelViewOtkazani.Name = "panelViewOtkazani";
            panelViewOtkazani.Visible = false;
            panelViewOtkazani.Controls.Add(dataGridOtkazani);
            panelViewOtkazani.Controls.Add(lblEmptyOtkazani);
            panelViewOtkazani.Controls.Add(panelOtkazaniHeader);

            // ── dataGridUdruzenja / panelViewUdruzenja ────────────
            dataGridUdruzenja.AllowUserToAddRows = false;
            dataGridUdruzenja.AllowUserToDeleteRows = false;
            dataGridUdruzenja.Dock = DockStyle.Fill;
            dataGridUdruzenja.MultiSelect = false;
            dataGridUdruzenja.Name = "dataGridUdruzenja";
            dataGridUdruzenja.ReadOnly = true;
            dataGridUdruzenja.RowHeadersVisible = false;
            dataGridUdruzenja.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridUdruzenja.TabIndex = 0;
            UiTheme.StyleGrid(dataGridUdruzenja);
            UiTheme.StyleEmptyState(lblEmptyUdruzenja, "Nema evidentiranih udruženja.");

            lblUdruzenjaTitle.Dock = DockStyle.Left;
            lblUdruzenjaTitle.AutoSize = true;
            lblUdruzenjaTitle.BackColor = UiTheme.PanelLight;
            lblUdruzenjaTitle.ForeColor = UiTheme.Navy;
            lblUdruzenjaTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblUdruzenjaTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblUdruzenjaTitle.Padding = new Padding(12, 0, 0, 0);
            lblUdruzenjaTitle.Text = "Udruženja";
            lblUdruzenjaTitle.Name = "lblUdruzenjaTitle";

            btnUdruzenjaSacuvajPdf.Location = new Point(220, 6);
            btnUdruzenjaSacuvajPdf.Name = "btnUdruzenjaSacuvajPdf";
            btnUdruzenjaSacuvajPdf.Size = new Size(168, 36);
            btnUdruzenjaSacuvajPdf.Text = "Sačuvaj kao PDF";
            btnUdruzenjaSacuvajPdf.IconGlyph = "";
            UiTheme.StyleAccentButton(btnUdruzenjaSacuvajPdf, UiTheme.PdfSaveAccent, 10f);
            btnUdruzenjaSacuvajPdf.Click += btnUdruzenjaSacuvajPdf_Click;

            btnUdruzenjaExportPdf.Location = new Point(398, 6);
            btnUdruzenjaExportPdf.Name = "btnUdruzenjaExportPdf";
            btnUdruzenjaExportPdf.Size = new Size(180, 36);
            btnUdruzenjaExportPdf.Text = "Sačuvaj tabelu kao PDF";
            btnUdruzenjaExportPdf.IconGlyph = "";
            UiTheme.StyleAccentButton(btnUdruzenjaExportPdf, UiTheme.PdfExportAccent, 10f);
            btnUdruzenjaExportPdf.Click += btnUdruzenjaExportPdf_Click;

            panelUdruzenjaHeader.Dock = DockStyle.Top;
            panelUdruzenjaHeader.Height = 48;
            panelUdruzenjaHeader.BackColor = UiTheme.PanelLight;
            panelUdruzenjaHeader.Name = "panelUdruzenjaHeader";
            panelUdruzenjaHeader.Controls.Add(lblUdruzenjaTitle);
            panelUdruzenjaHeader.Controls.Add(btnUdruzenjaSacuvajPdf);
            panelUdruzenjaHeader.Controls.Add(btnUdruzenjaExportPdf);

            panelViewUdruzenja.Dock = DockStyle.Fill;
            panelViewUdruzenja.Name = "panelViewUdruzenja";
            panelViewUdruzenja.Visible = false;
            panelViewUdruzenja.Controls.Add(dataGridUdruzenja);
            panelViewUdruzenja.Controls.Add(lblEmptyUdruzenja);
            panelViewUdruzenja.Controls.Add(panelUdruzenjaHeader);

            // ── dataGridStecaj / panelViewStecaj ──────────────────
            dataGridStecaj.AllowUserToAddRows = false;
            dataGridStecaj.AllowUserToDeleteRows = false;
            dataGridStecaj.Dock = DockStyle.Fill;
            dataGridStecaj.MultiSelect = false;
            dataGridStecaj.Name = "dataGridStecaj";
            dataGridStecaj.ReadOnly = true;
            dataGridStecaj.RowHeadersVisible = false;
            dataGridStecaj.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridStecaj.TabIndex = 0;
            UiTheme.StyleGrid(dataGridStecaj);
            UiTheme.StyleEmptyState(lblEmptyStecaj, "Nema klijenata u stečaju.");

            lblStecajTitle.Dock = DockStyle.Left;
            lblStecajTitle.AutoSize = true;
            lblStecajTitle.BackColor = UiTheme.PanelLight;
            lblStecajTitle.ForeColor = UiTheme.Navy;
            lblStecajTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblStecajTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblStecajTitle.Padding = new Padding(12, 0, 0, 0);
            lblStecajTitle.Text = "Klijenti u stečaju";
            lblStecajTitle.Name = "lblStecajTitle";

            btnStecajSacuvajPdf.Location = new Point(220, 6);
            btnStecajSacuvajPdf.Name = "btnStecajSacuvajPdf";
            btnStecajSacuvajPdf.Size = new Size(168, 36);
            btnStecajSacuvajPdf.Text = "Sačuvaj kao PDF";
            btnStecajSacuvajPdf.IconGlyph = "";
            UiTheme.StyleAccentButton(btnStecajSacuvajPdf, UiTheme.PdfSaveAccent, 10f);
            btnStecajSacuvajPdf.Click += btnStecajSacuvajPdf_Click;

            btnStecajExportPdf.Location = new Point(398, 6);
            btnStecajExportPdf.Name = "btnStecajExportPdf";
            btnStecajExportPdf.Size = new Size(180, 36);
            btnStecajExportPdf.Text = "Sačuvaj tabelu kao PDF";
            btnStecajExportPdf.IconGlyph = "";
            UiTheme.StyleAccentButton(btnStecajExportPdf, UiTheme.PdfExportAccent, 10f);
            btnStecajExportPdf.Click += btnStecajExportPdf_Click;

            panelStecajHeader.Dock = DockStyle.Top;
            panelStecajHeader.Height = 48;
            panelStecajHeader.BackColor = UiTheme.PanelLight;
            panelStecajHeader.Name = "panelStecajHeader";
            panelStecajHeader.Controls.Add(lblStecajTitle);
            panelStecajHeader.Controls.Add(btnStecajSacuvajPdf);
            panelStecajHeader.Controls.Add(btnStecajExportPdf);

            panelViewStecaj.Dock = DockStyle.Fill;
            panelViewStecaj.Name = "panelViewStecaj";
            panelViewStecaj.Visible = false;
            panelViewStecaj.Controls.Add(dataGridStecaj);
            panelViewStecaj.Controls.Add(lblEmptyStecaj);
            panelViewStecaj.Controls.Add(panelStecajHeader);

            // ── dataGridAuditLog / panelViewAuditLog ──────────────
            dataGridAuditLog.AllowUserToAddRows = false;
            dataGridAuditLog.AllowUserToDeleteRows = false;
            dataGridAuditLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridAuditLog.Dock = DockStyle.Fill;
            dataGridAuditLog.MultiSelect = false;
            dataGridAuditLog.Name = "dataGridAuditLog";
            dataGridAuditLog.ReadOnly = true;
            dataGridAuditLog.RowHeadersVisible = false;
            dataGridAuditLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridAuditLog.TabIndex = 0;
            UiTheme.StyleGrid(dataGridAuditLog);
            UiTheme.StyleEmptyState(lblEmptyAuditLog, "Nema zabilježenih promjena.");

            lblAuditLogTitle.Dock = DockStyle.Left;
            lblAuditLogTitle.AutoSize = true;
            lblAuditLogTitle.BackColor = UiTheme.PanelLight;
            lblAuditLogTitle.ForeColor = UiTheme.Navy;
            lblAuditLogTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblAuditLogTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblAuditLogTitle.Padding = new Padding(12, 0, 0, 0);
            lblAuditLogTitle.Text = "Historija promjena";
            lblAuditLogTitle.Name = "lblAuditLogTitle";

            panelAuditLogHeader.Dock = DockStyle.Top;
            panelAuditLogHeader.Height = 48;
            panelAuditLogHeader.BackColor = UiTheme.PanelLight;
            panelAuditLogHeader.Name = "panelAuditLogHeader";
            panelAuditLogHeader.Controls.Add(lblAuditLogTitle);

            panelViewAuditLog.Dock = DockStyle.Fill;
            panelViewAuditLog.Name = "panelViewAuditLog";
            panelViewAuditLog.Visible = false;
            panelViewAuditLog.Controls.Add(dataGridAuditLog);
            panelViewAuditLog.Controls.Add(lblEmptyAuditLog);
            panelViewAuditLog.Controls.Add(panelAuditLogHeader);

            // ── panelViewProfil ───────────────────────────────────
            lblProfilTitle.Dock = DockStyle.Left;
            lblProfilTitle.AutoSize = true;
            lblProfilTitle.BackColor = UiTheme.PanelLight;
            lblProfilTitle.ForeColor = UiTheme.Navy;
            lblProfilTitle.Font = UiTheme.Base(11f, FontStyle.Bold);
            lblProfilTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblProfilTitle.Padding = new Padding(12, 0, 0, 0);
            lblProfilTitle.Text = "Profil";
            lblProfilTitle.Name = "lblProfilTitle";

            panelProfilHeader.Dock = DockStyle.Top;
            panelProfilHeader.Height = 48;
            panelProfilHeader.BackColor = UiTheme.PanelLight;
            panelProfilHeader.Name = "panelProfilHeader";
            panelProfilHeader.Controls.Add(lblProfilTitle);

            UiTheme.StyleGroupBox(groupBoxProfil, "Prijavljeni korisnik");
            groupBoxProfil.Location = new Point(24, 216);
            groupBoxProfil.Size = new Size(480, 190);
            groupBoxProfil.Name = "groupBoxProfil";

            lblProfilKorisnickoIme.Text = "Korisničko ime:";
            lblProfilKorisnickoIme.Location = new Point(12, 36);
            lblProfilKorisnickoIme.Name = "lblProfilKorisnickoIme";
            UiTheme.StyleLabel(lblProfilKorisnickoIme);
            lblProfilKorisnickoImeValue.Location = new Point(165, 36);
            lblProfilKorisnickoImeValue.Name = "lblProfilKorisnickoImeValue";
            UiTheme.StyleLabel(lblProfilKorisnickoImeValue);
            lblProfilKorisnickoImeValue.Font = UiTheme.Base(9.5f, FontStyle.Bold);

            lblProfilPrikaznoIme.Text = "Ime:";
            lblProfilPrikaznoIme.Location = new Point(12, 72);
            lblProfilPrikaznoIme.Name = "lblProfilPrikaznoIme";
            UiTheme.StyleLabel(lblProfilPrikaznoIme);
            lblProfilPrikaznoImeValue.Location = new Point(165, 72);
            lblProfilPrikaznoImeValue.Name = "lblProfilPrikaznoImeValue";
            UiTheme.StyleLabel(lblProfilPrikaznoImeValue);
            lblProfilPrikaznoImeValue.Font = UiTheme.Base(9.5f, FontStyle.Bold);

            lblProfilStatus.Text = "Status:";
            lblProfilStatus.Location = new Point(12, 108);
            lblProfilStatus.Name = "lblProfilStatus";
            UiTheme.StyleLabel(lblProfilStatus);
            lblProfilStatusValue.Location = new Point(165, 108);
            lblProfilStatusValue.Name = "lblProfilStatusValue";
            UiTheme.StyleLabel(lblProfilStatusValue);
            lblProfilStatusValue.Font = UiTheme.Base(9.5f, FontStyle.Bold);

            groupBoxProfil.Controls.Add(lblProfilKorisnickoIme);
            groupBoxProfil.Controls.Add(lblProfilKorisnickoImeValue);
            groupBoxProfil.Controls.Add(lblProfilPrikaznoIme);
            groupBoxProfil.Controls.Add(lblProfilPrikaznoImeValue);
            groupBoxProfil.Controls.Add(lblProfilStatus);
            groupBoxProfil.Controls.Add(lblProfilStatusValue);

            lblProfilZadnjaPrijava.Text = "Zadnja prijava:";
            lblProfilZadnjaPrijava.Location = new Point(12, 144);
            lblProfilZadnjaPrijava.Name = "lblProfilZadnjaPrijava";
            UiTheme.StyleLabel(lblProfilZadnjaPrijava);
            lblProfilZadnjaPrijavaValue.Location = new Point(165, 144);
            lblProfilZadnjaPrijavaValue.Name = "lblProfilZadnjaPrijavaValue";
            UiTheme.StyleLabel(lblProfilZadnjaPrijavaValue);
            lblProfilZadnjaPrijavaValue.Font = UiTheme.Base(9.5f, FontStyle.Bold);
            groupBoxProfil.Controls.Add(lblProfilZadnjaPrijava);
            groupBoxProfil.Controls.Add(lblProfilZadnjaPrijavaValue);

            // ── Moja zadnja aktivnost ─────────────────────────────
            UiTheme.StyleGroupBox(groupBoxAktivnost, "Moja zadnja aktivnost");
            groupBoxAktivnost.Location = new Point(528, 68);
            groupBoxAktivnost.Size = new Size(760, 346);
            groupBoxAktivnost.Name = "groupBoxAktivnost";

            lblAktivnostSazetak.Dock = DockStyle.Top;
            lblAktivnostSazetak.Height = 26;
            lblAktivnostSazetak.TextAlign = ContentAlignment.MiddleLeft;
            lblAktivnostSazetak.ForeColor = UiTheme.LabelText;
            lblAktivnostSazetak.Font = UiTheme.Base(9.5f);
            lblAktivnostSazetak.Name = "lblAktivnostSazetak";

            dataGridAktivnost.AllowUserToAddRows = false;
            dataGridAktivnost.AllowUserToDeleteRows = false;
            dataGridAktivnost.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridAktivnost.Dock = DockStyle.Fill;
            dataGridAktivnost.MultiSelect = false;
            dataGridAktivnost.Name = "dataGridAktivnost";
            dataGridAktivnost.ReadOnly = true;
            dataGridAktivnost.RowHeadersVisible = false;
            dataGridAktivnost.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            UiTheme.StyleGrid(dataGridAktivnost);
            UiTheme.StyleEmptyState(lblEmptyAktivnost, "Još nema zabilježenih aktivnosti.");

            groupBoxAktivnost.Controls.Add(dataGridAktivnost);
            groupBoxAktivnost.Controls.Add(lblEmptyAktivnost);
            groupBoxAktivnost.Controls.Add(lblAktivnostSazetak);

            // ── Profilna slika ────────────────────────────────────
            picProfil.Location = new Point(24, 68);
            picProfil.Size = new Size(130, 130);
            picProfil.SizeMode = PictureBoxSizeMode.Zoom;
            picProfil.BorderStyle = BorderStyle.FixedSingle;
            picProfil.BackColor = UiTheme.PanelLight;
            picProfil.Name = "picProfil";

            btnOdaberiSliku.Location = new Point(172, 100);
            btnOdaberiSliku.Size = new Size(170, 36);
            btnOdaberiSliku.Text = "Odaberi sliku";
            btnOdaberiSliku.Name = "btnOdaberiSliku";
            btnOdaberiSliku.IconGlyph = "";
            UiTheme.StyleFlatButton(btnOdaberiSliku, UiTheme.Blue, 10f);
            btnOdaberiSliku.Click += btnOdaberiSliku_Click;

            btnUkloniSliku.Location = new Point(172, 146);
            btnUkloniSliku.Size = new Size(170, 36);
            btnUkloniSliku.Text = "Ukloni sliku";
            btnUkloniSliku.Name = "btnUkloniSliku";
            btnUkloniSliku.IconGlyph = "";
            UiTheme.StyleFlatButton(btnUkloniSliku, UiTheme.Red, 10f);
            btnUkloniSliku.Click += btnUkloniSliku_Click;

            btnPromijeniLozinku.Location = new Point(24, 424);
            btnPromijeniLozinku.Size = new Size(200, 36);
            btnPromijeniLozinku.Text = "Promijeni lozinku";
            btnPromijeniLozinku.Name = "btnPromijeniLozinku";
            btnPromijeniLozinku.IconGlyph = "";
            UiTheme.StyleFlatButton(btnPromijeniLozinku, UiTheme.Blue, 10f);
            btnPromijeniLozinku.Click += btnPromijeniLozinku_Click;

            btnNoviKorisnik.Location = new Point(236, 424);
            btnNoviKorisnik.Size = new Size(200, 36);
            btnNoviKorisnik.Text = "Novi korisnik";
            btnNoviKorisnik.Name = "btnNoviKorisnik";
            btnNoviKorisnik.IconGlyph = "";
            UiTheme.StyleFlatButton(btnNoviKorisnik, UiTheme.Green, 10f);
            btnNoviKorisnik.Click += btnNoviKorisnik_Click;

            btnKorisnici.Location = new Point(448, 424);
            btnKorisnici.Size = new Size(200, 36);
            btnKorisnici.Text = UiMessages.UsersButton;
            btnKorisnici.Name = "btnKorisnici";
            btnKorisnici.IconGlyph = "";
            UiTheme.StyleFlatButton(btnKorisnici, UiTheme.Blue, 10f);
            btnKorisnici.Click += btnKorisnici_Click;

            panelViewProfil.Dock = DockStyle.Fill;
            panelViewProfil.Name = "panelViewProfil";
            panelViewProfil.Visible = false;
            panelViewProfil.AutoScroll = true;
            panelViewProfil.Resize += (_, _) => FitAktivnostWidth();
            panelViewProfil.Controls.Add(groupBoxAktivnost);
            panelViewProfil.Controls.Add(btnPromijeniLozinku);
            panelViewProfil.Controls.Add(btnNoviKorisnik);
            panelViewProfil.Controls.Add(btnKorisnici);
            panelViewProfil.Controls.Add(picProfil);
            panelViewProfil.Controls.Add(btnOdaberiSliku);
            panelViewProfil.Controls.Add(btnUkloniSliku);
            panelViewProfil.Controls.Add(groupBoxProfil);
            panelViewProfil.Controls.Add(panelProfilHeader);

            // ── panelViewKlijenti (postojeći sadržaj, sad kao jedna sekcija) ─
            panelViewKlijenti.Dock = DockStyle.Fill;
            panelViewKlijenti.Name = "panelViewKlijenti";
            panelViewKlijenti.Controls.Add(splitMain);
            panelViewKlijenti.Controls.Add(panelSearch);
            panelViewKlijenti.Controls.Add(panelToolbar);

            // ── panelMainContent ───────────────────────────────────
            panelMainContent.Dock = DockStyle.Fill;
            panelMainContent.Name = "panelMainContent";
            panelMainContent.Controls.Add(panelViewProfil);
            panelMainContent.Controls.Add(panelViewAuditLog);
            panelMainContent.Controls.Add(panelViewStecaj);
            panelMainContent.Controls.Add(panelViewOtkazani);
            panelMainContent.Controls.Add(panelViewUdruzenja);
            panelMainContent.Controls.Add(panelViewRizik);
            panelMainContent.Controls.Add(panelViewPep);
            panelMainContent.Controls.Add(panelViewUbo);
            panelMainContent.Controls.Add(panelViewKyc);
            panelMainContent.Controls.Add(panelViewBezUgovora);
            panelMainContent.Controls.Add(panelViewKlijenti);
            panelMainContent.Controls.Add(panelViewDashboard);

            // ── panelSidebar ──────────────────────────────────────
            panelSidebar.Dock = DockStyle.Left;
            panelSidebar.Width = SidebarExpandedWidth;
            panelSidebar.Name = "panelSidebar";
            panelSidebar.BackColor = UiTheme.SidebarBackground;
            panelSidebar.Controls.Add(panelSidebarBottomDivider);
            panelSidebar.Controls.Add(btnNavProfil);
            panelSidebar.Controls.Add(panelSidebarLogoutDivider);
            panelSidebar.Controls.Add(btnNavLogout);
            panelSidebar.Controls.Add(btnNavAuditLog);
            panelSidebar.Controls.Add(btnNavStecaj);
            panelSidebar.Controls.Add(btnNavOtkazani);
            panelSidebar.Controls.Add(btnNavUdruzenja);
            panelSidebar.Controls.Add(btnNavBezUgovora);
            panelSidebar.Controls.Add(btnNavRizik);
            panelSidebar.Controls.Add(btnNavPep);
            panelSidebar.Controls.Add(btnNavUbo);
            panelSidebar.Controls.Add(btnNavKyc);
            panelSidebar.Controls.Add(btnNavKlijenti);
            panelSidebar.Controls.Add(btnNavDashboard);
            panelSidebar.Controls.Add(panelSidebarDivider);
            panelSidebar.Controls.Add(panelSidebarBrandDivider);
            panelSidebar.Controls.Add(panelSidebarBrand);
            panelSidebar.Controls.Add(panelToggleDivider);
            panelSidebar.Controls.Add(btnToggleSidebar);

            // ── panelSidebarDivider ───────────────────────────────
            panelSidebarDivider.Dock = DockStyle.Top;
            panelSidebarDivider.Height = 1;
            panelSidebarDivider.Name = "panelSidebarDivider";
            panelSidebarDivider.BackColor = UiTheme.SidebarDivider;

            // ── btnToggleSidebar ──────────────────────────────────
            btnToggleSidebar.Dock = DockStyle.Top;
            btnToggleSidebar.Height = 54;
            btnToggleSidebar.Name = "btnToggleSidebar";
            btnToggleSidebar.Text = "";
            btnToggleSidebar.IconGlyph = "\uE700";
            UiTheme.StyleSidebarButton(btnToggleSidebar);
            btnToggleSidebar.Click += btnToggleSidebar_Click;

            // ── panelToggleDivider ────────────────────────────────
            panelToggleDivider.Dock = DockStyle.Top;
            panelToggleDivider.Height = 1;
            panelToggleDivider.Name = "panelToggleDivider";
            panelToggleDivider.BackColor = UiTheme.SidebarDivider;

            // ── panelSidebarBrand ─────────────────────────────────
            panelSidebarBrand.Dock = DockStyle.Top;
            panelSidebarBrand.Height = 40;
            panelSidebarBrand.Name = "panelSidebarBrand";
            panelSidebarBrand.BackColor = UiTheme.SidebarBackground;
            panelSidebarBrand.Controls.Add(lblSidebarBrand);

            lblSidebarBrand.Dock = DockStyle.Fill;
            lblSidebarBrand.Name = "lblSidebarBrand";
            lblSidebarBrand.Text = "CONFIDIA BH";
            lblSidebarBrand.ForeColor = UiTheme.HeaderSubText;
            lblSidebarBrand.Font = UiTheme.Base(9.5f, FontStyle.Bold);
            lblSidebarBrand.TextAlign = ContentAlignment.MiddleCenter;
            lblSidebarBrand.AutoEllipsis = true;

            // ── panelSidebarBrandDivider ──────────────────────────
            panelSidebarBrandDivider.Dock = DockStyle.Top;
            panelSidebarBrandDivider.Height = 1;
            panelSidebarBrandDivider.Name = "panelSidebarBrandDivider";
            panelSidebarBrandDivider.BackColor = UiTheme.SidebarDivider;

            // -- btnNavDashboard --
            btnNavDashboard.Dock = DockStyle.Top;
            btnNavDashboard.Height = 46;
            btnNavDashboard.Name = "btnNavDashboard";
            btnNavDashboard.Text = "Početni ekran";
            btnNavDashboard.IconGlyph = "";
            UiTheme.StyleSidebarButton(btnNavDashboard, active: true);
            btnNavDashboard.Click += btnNavDashboard_Click;

            // -- btnNavKlijenti --
            btnNavKlijenti.Dock = DockStyle.Top;
            btnNavKlijenti.Height = 46;
            btnNavKlijenti.Name = "btnNavKlijenti";
            btnNavKlijenti.Text = "Klijenti";
            btnNavKlijenti.IconGlyph = "";
            UiTheme.StyleSidebarButton(btnNavKlijenti);
            btnNavKlijenti.Click += btnNavKlijenti_Click;

            // ── btnNavKyc ─────────────────────────────────────────
            btnNavKyc.Dock = DockStyle.Top;
            btnNavKyc.Height = 46;
            btnNavKyc.Name = "btnNavKyc";
            btnNavKyc.Text = "KYC evidencija";
            btnNavKyc.IconGlyph = "\uE77B";
            UiTheme.StyleSidebarButton(btnNavKyc);
            btnNavKyc.Click += btnNavKyc_Click;

            // ── btnNavUbo ─────────────────────────────────────────
            btnNavUbo.Dock = DockStyle.Top;
            btnNavUbo.Height = 46;
            btnNavUbo.Name = "btnNavUbo";
            btnNavUbo.Text = "UBO / Vlasništvo";
            btnNavUbo.IconGlyph = "";
            UiTheme.StyleSidebarButton(btnNavUbo);
            btnNavUbo.Click += btnNavUbo_Click;

            // ── btnNavPep ─────────────────────────────────────────
            btnNavPep.Dock = DockStyle.Top;
            btnNavPep.Height = 46;
            btnNavPep.Name = "btnNavPep";
            btnNavPep.Text = "PEP evidencija";
            btnNavPep.IconGlyph = "\uE779";
            UiTheme.StyleSidebarButton(btnNavPep);
            btnNavPep.Click += btnNavPep_Click;

            // ── btnNavRizik ───────────────────────────────────────
            btnNavRizik.Dock = DockStyle.Top;
            btnNavRizik.Height = 46;
            btnNavRizik.Name = "btnNavRizik";
            btnNavRizik.Text = "Evidencija procjena rizika";
            btnNavRizik.IconGlyph = "\uE730";
            UiTheme.StyleSidebarButton(btnNavRizik);
            btnNavRizik.Click += btnNavRizik_Click;

            // -- btnNavBezUgovora --
            btnNavBezUgovora.Dock = DockStyle.Top;
            btnNavBezUgovora.Height = 46;
            btnNavBezUgovora.Name = "btnNavBezUgovora";
            btnNavBezUgovora.Text = "Klijenti bez ugovora";
            btnNavBezUgovora.IconGlyph = "";
            UiTheme.StyleSidebarButton(btnNavBezUgovora);
            btnNavBezUgovora.Click += btnNavBezUgovora_Click;

            // ── btnNavOtkazani ────────────────────────────────────
            btnNavOtkazani.Dock = DockStyle.Top;
            btnNavOtkazani.Height = 46;
            btnNavOtkazani.Name = "btnNavOtkazani";
            btnNavOtkazani.Text = "Otkazani klijenti";
            btnNavOtkazani.IconGlyph = "";
            UiTheme.StyleSidebarButton(btnNavOtkazani);
            btnNavOtkazani.Click += btnNavOtkazani_Click;

            // ── btnNavUdruzenja ───────────────────────────────────
            btnNavUdruzenja.Dock = DockStyle.Top;
            btnNavUdruzenja.Height = 46;
            btnNavUdruzenja.Name = "btnNavUdruzenja";
            btnNavUdruzenja.Text = "Udruženja";
            btnNavUdruzenja.IconGlyph = "";
            UiTheme.StyleSidebarButton(btnNavUdruzenja);
            btnNavUdruzenja.Click += btnNavUdruzenja_Click;

            // ── btnNavStecaj ──────────────────────────────────────
            btnNavStecaj.Dock = DockStyle.Top;
            btnNavStecaj.Height = 46;
            btnNavStecaj.Name = "btnNavStecaj";
            btnNavStecaj.Text = "Klijenti u stečaju";
            btnNavStecaj.IconGlyph = "";
            UiTheme.StyleSidebarButton(btnNavStecaj);
            btnNavStecaj.Click += btnNavStecaj_Click;

            // ── btnNavAuditLog ────────────────────────────────────
            btnNavAuditLog.Dock = DockStyle.Top;
            btnNavAuditLog.Height = 46;
            btnNavAuditLog.Name = "btnNavAuditLog";
            btnNavAuditLog.Text = "Historija promjena";
            btnNavAuditLog.IconGlyph = "";
            UiTheme.StyleSidebarButton(btnNavAuditLog);
            btnNavAuditLog.Click += btnNavAuditLog_Click;

            // ── Profil / Odjava (dno sidebara) ────────────────────
            panelSidebarBottomDivider.Dock = DockStyle.Bottom;
            panelSidebarBottomDivider.Height = 1;
            panelSidebarBottomDivider.Name = "panelSidebarBottomDivider";
            panelSidebarBottomDivider.BackColor = UiTheme.SidebarDivider;

            panelSidebarLogoutDivider.Dock = DockStyle.Bottom;
            panelSidebarLogoutDivider.Height = 1;
            panelSidebarLogoutDivider.Name = "panelSidebarLogoutDivider";
            panelSidebarLogoutDivider.BackColor = UiTheme.SidebarDivider;

            btnNavProfil.Dock = DockStyle.Bottom;
            btnNavProfil.Height = 46;
            btnNavProfil.Name = "btnNavProfil";
            btnNavProfil.Text = "Profil";
            btnNavProfil.IconGlyph = "\uE168";
            UiTheme.StyleSidebarButton(btnNavProfil);
            btnNavProfil.Click += btnNavProfil_Click;

            btnNavLogout.Dock = DockStyle.Bottom;
            btnNavLogout.Height = 46;
            btnNavLogout.Name = "btnNavLogout";
            btnNavLogout.Text = "Odjava";
            btnNavLogout.IconGlyph = "";
            UiTheme.StyleSidebarButton(btnNavLogout);
            btnNavLogout.Click += btnNavLogout_Click;

            // ── Form1 ─────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UiTheme.FormBackgroundMain;
            ClientSize = new Size(1450, 844);
            Controls.Add(panelMainContent);
            Controls.Add(panelSidebar);
            Font = new Font("Segoe UI", 9F);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OwnerTrack — Upravljanje firmama i vlasnicima";
            WindowState = FormWindowState.Maximized;

            panelToolbar.ResumeLayout(false);
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridKlijenti).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridVlasnici).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridDirektori).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridBezUgovora).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridKyc).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridUbo).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridPep).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridRizik).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridOtkazani).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridUdruzenja).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridStecaj).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridAuditLog).EndInit();
            panelKycHeader.ResumeLayout(false);
            panelKycHeader.PerformLayout();
            panelUboHeader.ResumeLayout(false);
            panelUboHeader.PerformLayout();
            panelPepHeader.ResumeLayout(false);
            panelPepHeader.PerformLayout();
            panelRizikHeader.ResumeLayout(false);
            panelRizikHeader.PerformLayout();
            panelOtkazaniHeader.ResumeLayout(false);
            panelOtkazaniHeader.PerformLayout();
            panelUdruzenjaHeader.ResumeLayout(false);
            panelUdruzenjaHeader.PerformLayout();
            panelStecajHeader.ResumeLayout(false);
            panelStecajHeader.PerformLayout();
            panelAuditLogHeader.ResumeLayout(false);
            panelAuditLogHeader.PerformLayout();
            panelDashboardHeader.ResumeLayout(false);
            panelDashboardHeader.PerformLayout();
            panelBezUgovoraHeader.ResumeLayout(false);
            panelBezUgovoraHeader.PerformLayout();
            panelViewDashboard.ResumeLayout(false);
            panelViewBezUgovora.ResumeLayout(false);
            panelViewKyc.ResumeLayout(false);
            panelViewUbo.ResumeLayout(false);
            panelViewPep.ResumeLayout(false);
            panelViewRizik.ResumeLayout(false);
            panelViewOtkazani.ResumeLayout(false);
            panelViewUdruzenja.ResumeLayout(false);
            panelViewStecaj.ResumeLayout(false);
            panelViewAuditLog.ResumeLayout(false);
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            splitBottom.Panel1.ResumeLayout(false);
            splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitBottom).EndInit();
            splitBottom.ResumeLayout(false);
            panelVlasnici.ResumeLayout(false);
            panelVlasniciBtns.ResumeLayout(false);
            panelDirektori.ResumeLayout(false);
            panelDirektoriBtns.ResumeLayout(false);
            ResumeLayout(false);
        }

        private const int SidebarExpandedWidth = 216;
        private const int SidebarCollapsedWidth = 60;
        private System.Windows.Forms.Panel panelMainContent;
        private System.Windows.Forms.Panel panelViewKlijenti;
        private System.Windows.Forms.Panel panelSidebar;
        private IconButton btnToggleSidebar;
        private System.Windows.Forms.Panel panelToggleDivider;
        private System.Windows.Forms.Panel panelSidebarBrand;
        private System.Windows.Forms.Label lblSidebarBrand;
        private System.Windows.Forms.Panel panelSidebarBrandDivider;
        private IconButton btnNavDashboard;
        private System.Windows.Forms.Panel panelViewDashboard;
        private System.Windows.Forms.Panel panelDashboardHeader;
        private System.Windows.Forms.Label lblDashboardTitle;
        private System.Windows.Forms.Label lblConfidiaBrand;
        private StatTile tileDodajKlijenta;
        private StatTile tileOtkaziKlijenta;
        private StatTile tileExportPdf;
        private StatTile tileDjelatnosti;
        private StatTile tileOsvjezi;
        // Nevidljivo dugme — postojeći PdfExportPresenter/DialogHelper.ExecutePdfExport
        // obrazac treba Button (za busy-state tekst/Enabled tokom generisanja),
        // a klik dolazi sa StatTile kartice, ne pravog dugmeta.
        private Button btnDashboardExportPdfProxy;
        private System.Windows.Forms.TableLayoutPanel panelDashboardTiles;
        private StatTile tileAktivniKlijenti;
        private StatTile tileKyc;
        private StatTile tilePep;
        private StatTile tileRizik;
        private StatTile tileBezUgovora;
        private StatTile tileUpozorenja;
        private StatTile tileVlasnici;
        private StatTile tileArhivirani;
        private StatTile tileUdruzenja;
        private StatTile tileStecaj;
        private StatTile tileAuditLog;
        private IconButton btnNavKlijenti;
        private IconButton btnNavKyc;
        private IconButton btnNavUbo;
        private IconButton btnNavPep;
        private IconButton btnNavRizik;
        private IconButton btnNavBezUgovora;
        private System.Windows.Forms.Panel panelViewBezUgovora;
        private System.Windows.Forms.Panel panelBezUgovoraHeader;
        private System.Windows.Forms.Label lblBezUgovoraTitle;
        private IconButton btnBezUgovoraSacuvajPdf;
        private IconButton btnBezUgovoraExportPdf;
        public System.Windows.Forms.DataGridView dataGridBezUgovora;
        private System.Windows.Forms.Label lblEmptyBezUgovora;
        private IconButton btnNavOtkazani;
        private System.Windows.Forms.Panel panelViewOtkazani;
        private System.Windows.Forms.Panel panelOtkazaniHeader;
        private System.Windows.Forms.Label lblOtkazaniTitle;
        public System.Windows.Forms.DataGridView dataGridOtkazani;
        private System.Windows.Forms.Label lblEmptyOtkazani;
        private IconButton btnNavUdruzenja;
        private System.Windows.Forms.Panel panelViewUdruzenja;
        private System.Windows.Forms.Panel panelUdruzenjaHeader;
        private System.Windows.Forms.Label lblUdruzenjaTitle;
        private IconButton btnUdruzenjaSacuvajPdf;
        private IconButton btnUdruzenjaExportPdf;
        public System.Windows.Forms.DataGridView dataGridUdruzenja;
        private System.Windows.Forms.Label lblEmptyUdruzenja;
        private IconButton btnNavStecaj;
        private System.Windows.Forms.Panel panelViewStecaj;
        private System.Windows.Forms.Panel panelStecajHeader;
        private System.Windows.Forms.Label lblStecajTitle;
        private IconButton btnStecajSacuvajPdf;
        private IconButton btnStecajExportPdf;
        public System.Windows.Forms.DataGridView dataGridStecaj;
        private System.Windows.Forms.Label lblEmptyStecaj;
        private IconButton btnNavAuditLog;
        private IconButton btnNavProfil;
        private IconButton btnNavLogout;
        private System.Windows.Forms.Panel panelSidebarBottomDivider;
        private System.Windows.Forms.Panel panelSidebarLogoutDivider;
        private System.Windows.Forms.Panel panelViewProfil;
        private System.Windows.Forms.Panel panelProfilHeader;
        private System.Windows.Forms.Label lblProfilTitle;
        private System.Windows.Forms.GroupBox groupBoxProfil;
        private System.Windows.Forms.Label lblProfilKorisnickoIme;
        private System.Windows.Forms.Label lblProfilKorisnickoImeValue;
        private System.Windows.Forms.Label lblProfilPrikaznoIme;
        private System.Windows.Forms.Label lblProfilPrikaznoImeValue;
        private System.Windows.Forms.Label lblProfilStatus;
        private System.Windows.Forms.Label lblProfilStatusValue;
        private System.Windows.Forms.Label lblProfilZadnjaPrijava;
        private System.Windows.Forms.Label lblProfilZadnjaPrijavaValue;
        private System.Windows.Forms.GroupBox groupBoxAktivnost;
        private System.Windows.Forms.Label lblAktivnostSazetak;
        private System.Windows.Forms.DataGridView dataGridAktivnost;
        private System.Windows.Forms.Label lblEmptyAktivnost;
        private IconButton btnPromijeniLozinku;
        private IconButton btnNoviKorisnik;
        private IconButton btnKorisnici;
        private System.Windows.Forms.PictureBox picProfil;
        private IconButton btnOdaberiSliku;
        private IconButton btnUkloniSliku;
        private System.Windows.Forms.Panel panelViewAuditLog;
        private System.Windows.Forms.Panel panelAuditLogHeader;
        private System.Windows.Forms.Label lblAuditLogTitle;
        public System.Windows.Forms.DataGridView dataGridAuditLog;
        private System.Windows.Forms.Label lblEmptyAuditLog;
        private System.Windows.Forms.Panel panelSidebarDivider;
        private System.Windows.Forms.Panel panelViewKyc;
        private System.Windows.Forms.Panel panelKycHeader;
        private System.Windows.Forms.Label lblKycTitle;
        private IconButton btnKycSacuvajPdf;
        private IconButton btnKycExportPdf;
        public System.Windows.Forms.DataGridView dataGridKyc;
        private System.Windows.Forms.Label lblEmptyKyc;
        private System.Windows.Forms.Panel panelViewUbo;
        private System.Windows.Forms.Panel panelUboHeader;
        private System.Windows.Forms.Label lblUboTitle;
        private IconButton btnUboSacuvajPdf;
        private IconButton btnUboExportPdf;
        public System.Windows.Forms.DataGridView dataGridUbo;
        private System.Windows.Forms.Label lblEmptyUbo;
        private System.Windows.Forms.Panel panelViewPep;
        private System.Windows.Forms.Panel panelPepHeader;
        private System.Windows.Forms.Label lblPepTitle;
        private IconButton btnPepSacuvajPdf;
        private IconButton btnPepExportPdf;
        public System.Windows.Forms.DataGridView dataGridPep;
        private System.Windows.Forms.Label lblEmptyPep;
        private System.Windows.Forms.Panel panelViewRizik;
        private System.Windows.Forms.Panel panelRizikHeader;
        private System.Windows.Forms.Label lblRizikTitle;
        public System.Windows.Forms.DataGridView dataGridRizik;
        private System.Windows.Forms.Label lblEmptyRizik;
        private System.Windows.Forms.Panel panelToolbar;
        private IconButton btnDodajKlijent;
        private IconButton btnIzmijeniKlijent;
        private IconButton btnObrisiKlijent;
        private IconButton btnImportExcel;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearchKlijent;
        public System.Windows.Forms.TextBox txtSearchKlijent;
        public System.Windows.Forms.DataGridView dataGridKlijenti;
        public System.Windows.Forms.DataGridView dataGridVlasnici;
        public System.Windows.Forms.DataGridView dataGridDirektori;
        private System.Windows.Forms.Label lblEmptyKlijenti;
        private System.Windows.Forms.Label lblEmptyVlasnici;
        private System.Windows.Forms.Label lblEmptyDirektori;
        public IconButton btnDodajVlasnika;
        public IconButton btnIzmijeniVlasnika;
        public IconButton btnObrisiVlasnika;
        public IconButton btnDodajDirektora;
        public IconButton btnIzmijeniDirektora;
        public IconButton btnObrisiDirektora;
        private IconButton btnResetImport;
        private IconButton btnUpozorenja;
        private IconButton btnRizikSacuvajPdf;
        private IconButton btnRizikExportPdf;
        private SplitContainer splitBottom;
        private Panel panelVlasnici;
        private Panel panelVlasniciBtns;
        private Panel panelDirektori;
        private Panel panelDirektoriBtns;
        private System.Windows.Forms.Label lblFilterDjelatnost;
        public System.Windows.Forms.ComboBox cmbFilterDjelatnost;
        private System.Windows.Forms.Label lblFilterVelicina;
        public System.Windows.Forms.Label lblKlijentiCount;
        public System.Windows.Forms.ComboBox cmbFilterVelicina;
        private IconButton btnResetFilters;
        private IconButton btnSacuvajPdf;
        private IconButton btnExportTabelaPdf;
    }
}
