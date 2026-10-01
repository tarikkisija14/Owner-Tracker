using DocumentFormat.OpenXml.Packaging;
using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.App.Presenters;
using OwnerTrack.App.ViewModels;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;
using OwnerTrack.Infrastructure.Services;
using OwnerTrack.Infrastructure.ViewModels;
using OpenXmlSheet = DocumentFormat.OpenXml.Spreadsheet.Sheet;

namespace OwnerTrack.App
{
    public partial class Form1 : Form
    {
        private enum SidebarView { Dashboard, Klijenti, Kyc, Ubo, Pep, Rizik, BezUgovora, Otkazani, Udruzenja, Stecaj, AuditLog, Profil }

        private readonly UserSession _session;
        private readonly System.Windows.Forms.Timer _searchDebounceTimer;
        private readonly ArchivePresenter _archivePresenter;
        private readonly PdfExportPresenter _pdfPresenter;
        private readonly ToolTip _sidebarToolTip = new();
        private bool _sidebarExpanded = true;
        private SidebarView _currentView = SidebarView.Dashboard;

        // True kad je korisnik kliknuo Odjava — Program tada vraća prikaz Login forme
        // umjesto da završi aplikaciju.
        public bool LogoutRequested { get; private set; }

        public Form1(UserSession session)
        {
            if (!session.IsAuthenticated)
                throw new InvalidOperationException("Glavna forma se ne može otvoriti bez prijavljenog korisnika.");

            _session = session;
            InitializeComponent();

            _searchDebounceTimer = new System.Windows.Forms.Timer
            {
                Interval = UiConstants.SearchDebounceMs
            };
            _searchDebounceTimer.Tick += (_, _) =>
            {
                _searchDebounceTimer.Stop();
                ApplyCurrentFilters();
            };

            _archivePresenter = new ArchivePresenter();
            _pdfPresenter = new PdfExportPresenter();

            GridHelper.EnableClickToDeselect(dataGridKlijenti);
            GridHelper.EnableClickToDeselect(dataGridVlasnici);
            GridHelper.EnableClickToDeselect(dataGridDirektori);
            GridHelper.EnableClickToDeselect(dataGridKyc);
            GridHelper.EnableClickToDeselect(dataGridUbo);
            GridHelper.EnableClickToDeselect(dataGridPep);
            GridHelper.EnableClickToDeselect(dataGridRizik);
            GridHelper.EnableClickToDeselect(dataGridOtkazani);
            GridHelper.EnableColumnSort(dataGridKlijenti, StyleKlijentiGrid);
            GridHelper.EnableColumnSort(dataGridKyc, StyleKycGrid);
            GridHelper.EnableColumnSort(dataGridUbo, StyleUboGrid);
            GridHelper.EnableColumnSort(dataGridPep, StylePepGrid);
            GridHelper.EnableColumnSort(dataGridRizik, StyleRizikGrid);
            GridHelper.EnableColumnSort(dataGridOtkazani, StyleOtkazaniGrid);

            dataGridKlijenti.CellDoubleClick += (s, e) => OpenKlijentProfil(dataGridKlijenti);
            dataGridKyc.CellDoubleClick += (s, e) => OpenKlijentProfil(dataGridKyc);
            dataGridUbo.CellDoubleClick += (s, e) => OpenKlijentProfil(dataGridUbo);
            dataGridPep.CellDoubleClick += (s, e) => OpenKlijentProfil(dataGridPep);
            dataGridRizik.CellDoubleClick += (s, e) => OpenKlijentProfil(dataGridRizik);
            dataGridOtkazani.CellDoubleClick += (s, e) => OpenKlijentProfil(dataGridOtkazani);

            Load += Form1_Load;
        }

        private void OpenKlijentProfil(DataGridView grid)
        {
            if (!GridHelper.TryGetSelectedId(grid, out int klijentId)) return;

            try
            {
                using var db = DbContextFactory.Create();
                var profil = new KlijentProfilQueryService(db).GetProfile(klijentId);
                if (profil == null) return;
                new FrmKlijentProfil(profil).ShowDialog(this);
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju profila firme");
            }
        }

       

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Migracije se primjenjuju u Program.Main, prije prijave.
                LoadActivityCodeFilter();
                LoadSizeFilter();
                LoadClients();
                RefreshWarningsBadge();
                ShowView(SidebarView.Dashboard);

                // ProductVersion nosi i build metapodatke (npr. "1.0.0+49abc123") —
                // korisniku prikazujemo samo Major.Minor.Patch dio.
                string shortVersion = Application.ProductVersion.Split('+')[0];
                Text = $"OwnerTrack v{shortVersion} - Confidia BH";
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                MessageBox.Show(
                    string.Format(UiMessages.StartupErrorFormat,
                        ex.Message,
                        DbContextFactory.DbPath,
                        AppLogger.GetLogPath()),
                    UiMessages.StartupErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _searchDebounceTimer.Dispose();
            base.OnFormClosed(e);
        }

        

        private void LoadSizeFilter()
        {
            cmbFilterVelicina.Items.Clear();
            cmbFilterVelicina.Items.Add(new { Value = UiConstants.FilterAllValue, Display = UiConstants.FilterAllDisplay });

            foreach (VelicinaFirme v in Enum.GetValues(typeof(VelicinaFirme)))
                cmbFilterVelicina.Items.Add(new { Value = v.ToString(), Display = v.ToDisplay() });

            cmbFilterVelicina.DisplayMember = "Display";
            cmbFilterVelicina.ValueMember = "Value";
            cmbFilterVelicina.SelectedIndex = 0;
        }

        private void LoadActivityCodeFilter()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var activityCodes = db.Djelatnosti.OrderBy(d => d.Naziv).ToList();

                cmbFilterDjelatnost.Items.Clear();
                cmbFilterDjelatnost.Items.Add(new { Sifra = UiConstants.FilterAllValue, Naziv = UiConstants.FilterAllDjelatnostDisplay });

                foreach (var d in activityCodes)
                    cmbFilterDjelatnost.Items.Add(new { d.Sifra, Naziv = $"{d.Sifra} - {d.Naziv}" });

                cmbFilterDjelatnost.DisplayMember = "Naziv";
                cmbFilterDjelatnost.ValueMember = "Sifra";
                cmbFilterDjelatnost.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju djelatnosti");
            }
        }

        

        private string GetSelectedActivityCode()
        {
            if (cmbFilterDjelatnost.SelectedItem is null) return string.Empty;
            dynamic item = cmbFilterDjelatnost.SelectedItem;
            return item.Sifra ?? string.Empty;
        }

        private string GetSelectedSize()
        {
            if (cmbFilterVelicina.SelectedItem is null) return string.Empty;
            dynamic item = cmbFilterVelicina.SelectedItem;
            return item.Value ?? string.Empty;
        }

        private void ApplyCurrentFilters() =>
            LoadClients(txtSearchKlijent.Text, GetSelectedActivityCode(), GetSelectedSize());

        

        private void txtSearchKlijent_TextChanged(object sender, EventArgs e)
        {
            _searchDebounceTimer.Stop();
            _searchDebounceTimer.Start();
        }

        private void cmbFilterDjelatnost_SelectedIndexChanged(object sender, EventArgs e) => ApplyCurrentFilters();
        private void cmbFilterVelicina_SelectedIndexChanged(object sender, EventArgs e) => ApplyCurrentFilters();

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            txtSearchKlijent.Text = string.Empty;
            cmbFilterDjelatnost.SelectedIndex = 0;
            cmbFilterVelicina.SelectedIndex = 0;
            LoadClients();
        }

        

        private void LoadClients(string searchText = "", string sifraDjelatnosti = "", string velicina = "")
        {
            try
            {
                using var db = DbContextFactory.Create();
                var service = new KlijentQueryService(db);
                var clients = service.GetClients(searchText, sifraDjelatnosti, velicina);
                GridHelper.BindWithoutEvent(dataGridKlijenti, dataGridKlijenti_SelectionChanged, clients);
                StyleKlijentiGrid();
                lblEmptyKlijenti.Visible = clients.Count == 0;

                int total = service.GetTotalCount();
                lblKlijentiCount.Text = clients.Count == total
                    ? $"Prikazano: {total} firmi"
                    : $"Prikazano: {clients.Count} od {total} firmi";
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju klijenata");
            }
        }

        private void StyleKlijentiGrid()
        {
            GridHelper.ApplyColumns(dataGridKlijenti, GridColumns.Klijenti);
            GridHelper.FreezeColumns(dataGridKlijenti, "Id", "Naziv");
            GridHelper.AlignCenter(dataGridKlijenti,
                "DatumUspostaveOdnosa", "DatumOsnivanjaFirme", "DatumProcjeneRizika", "DatumPotpisaUgovora",
                "VrstaKlijenta", "BrojVlasnika", "BrojDirektora", "PepDatumProvjere");
            GridHelper.Emphasize(dataGridKlijenti, "Naziv", UiTheme.Base(9f, FontStyle.Bold));
        }

        private void LoadOwners(int klijentId)
        {
            try
            {
                using var db = DbContextFactory.Create();
                var owners = new KlijentQueryService(db).GetOwners(klijentId);
                dataGridVlasnici.DataSource = owners;
                GridHelper.ApplyColumns(dataGridVlasnici, GridColumns.Vlasnici);
                dataGridVlasnici.ClearSelection();
                lblEmptyVlasnici.Visible = owners.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju vlasnika");
            }
        }

        private void LoadDirectors(int klijentId)
        {
            try
            {
                using var db = DbContextFactory.Create();
                var directors = new KlijentQueryService(db).GetDirectors(klijentId);
                dataGridDirektori.DataSource = directors;
                GridHelper.ApplyColumns(dataGridDirektori, GridColumns.Direktori);
                dataGridDirektori.ClearSelection();
                lblEmptyDirektori.Visible = directors.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju direktora");
            }
        }

        private void dataGridKlijenti_SelectionChanged(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int id)) return;
            LoadOwners(id);
            LoadDirectors(id);
        }

        // ── Evidencija prikazi (KYC, UBO, PEP, procjena rizika) ─────

        private void LoadKyc()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var data = new EvidencijaQueryService(db).GetKycEvidencija();
                dataGridKyc.DataSource = data;
                StyleKycGrid();
                dataGridKyc.ClearSelection();
                lblEmptyKyc.Visible = data.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju KYC evidencije");
            }
        }

        private void StyleKycGrid()
        {
            GridHelper.ApplyColumns(dataGridKyc, GridColumns.Kyc);
            GridHelper.FreezeColumns(dataGridKyc, "Redni", "Naziv");
            GridHelper.AlignCenter(dataGridKyc, "Redni", "DatumUspostaveOdnosa", "VrstaKlijenta");
            if (dataGridKyc.Columns.Contains("Id")) dataGridKyc.Columns["Id"].Visible = false;
        }

        private void LoadUbo()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var data = new EvidencijaQueryService(db).GetUboEvidencija();
                dataGridUbo.DataSource = data;
                StyleUboGrid();
                dataGridUbo.ClearSelection();
                lblEmptyUbo.Visible = data.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju UBO evidencije");
            }
        }

        private void StyleUboGrid()
        {
            GridHelper.ApplyColumns(dataGridUbo, GridColumns.UboSveFirme);
            GridHelper.FreezeColumns(dataGridUbo, "Redni", "KlijentNaziv");
            GridHelper.AlignCenter(dataGridUbo, "Redni", "ProcenatVlasnistva", "DatumUtvrdjivanja");
            if (dataGridUbo.Columns.Contains("Id")) dataGridUbo.Columns["Id"].Visible = false;
        }

        private void LoadPep()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var data = new EvidencijaQueryService(db).GetPepEvidencija();
                dataGridPep.DataSource = data;
                StylePepGrid();
                dataGridPep.ClearSelection();
                lblEmptyPep.Visible = data.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju PEP evidencije");
            }
        }

        private void StylePepGrid()
        {
            GridHelper.ApplyColumns(dataGridPep, GridColumns.Pep);
            GridHelper.FreezeColumns(dataGridPep, "Redni", "NazivKlijenta");
            GridHelper.AlignCenter(dataGridPep, "Redni", "PepDatumProvjere");
            if (dataGridPep.Columns.Contains("Id")) dataGridPep.Columns["Id"].Visible = false;
        }

        private void LoadRizik()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var data = new EvidencijaQueryService(db).GetRizikEvidencija();
                dataGridRizik.DataSource = data;
                StyleRizikGrid();
                dataGridRizik.ClearSelection();
                lblEmptyRizik.Visible = data.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju procjene rizika");
            }
        }

        private void StyleRizikGrid()
        {
            GridHelper.ApplyColumns(dataGridRizik, GridColumns.RizikProcjena);
            GridHelper.FreezeColumns(dataGridRizik, "Redni", "Naziv");
            GridHelper.AlignCenter(dataGridRizik, "Redni", "DatumProcjeneRizika", "DatumUgovora", "VrstaKlijenta");
            if (dataGridRizik.Columns.Contains("Id")) dataGridRizik.Columns["Id"].Visible = false;
        }

        private void LoadBezUgovoraKlijenata()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var clients = new KlijentQueryService(db).GetClientsWithoutContract();
                dataGridBezUgovora.DataSource = clients;
                StyleBezUgovoraGrid();
                dataGridBezUgovora.ClearSelection();
                lblEmptyBezUgovora.Visible = clients.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju klijenata bez ugovora");
            }
        }

        private void StyleBezUgovoraGrid()
        {
            GridHelper.ApplyColumns(dataGridBezUgovora, GridColumns.Klijenti);
            GridHelper.FreezeColumns(dataGridBezUgovora, "Id", "Naziv");
            GridHelper.AlignCenter(dataGridBezUgovora,
                "DatumUspostaveOdnosa", "DatumOsnivanjaFirme", "DatumProcjeneRizika", "DatumPotpisaUgovora",
                "VrstaKlijenta", "BrojVlasnika", "BrojDirektora", "PepDatumProvjere");
            GridHelper.Emphasize(dataGridBezUgovora, "Naziv", UiTheme.Base(9f, FontStyle.Bold));
        }

        private async void btnBezUgovoraSacuvajPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericSingleRowAsync(dataGridBezUgovora, btnBezUgovoraSacuvajPdf, "Klijenti bez ugovora", "BezUgovora", GridColumns.KlijentiPdf);

        private async void btnBezUgovoraExportPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericTableAsync(dataGridBezUgovora, btnBezUgovoraExportPdf, "Klijenti bez ugovora", "BezUgovora_tabela", GridColumns.KlijentiPdf);

        // ── Početni ekran ──────────────────────────────────────────

        private void LoadDashboard()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var dashboard = new DashboardQueryService(db);
                var warningStats = new WarningQueryService(db).GetStats();

                tileAktivniKlijenti.Value = dashboard.GetActiveClientCount().ToString();
                tileKyc.Value = dashboard.GetActiveClientCount().ToString();
                tileVlasnici.Value = dashboard.GetOwnerCount().ToString();
                tilePep.Value = dashboard.GetPepClientCount().ToString();
                tileRizik.Value = dashboard.GetActiveClientCount().ToString();
                tileBezUgovora.Value = dashboard.GetClientsWithoutContractCount().ToString();
                tileUpozorenja.Value = warningStats.Count.ToString();
                tileArhivirani.Value = dashboard.GetArchivedClientCount().ToString();
                tileUdruzenja.Value = dashboard.GetUdruzenjaCount().ToString();
                tileStecaj.Value = dashboard.GetStecajCount().ToString();
                tileAuditLog.Value = dashboard.GetAuditLogCount().ToString();
                tileDjelatnosti.Value = dashboard.GetActivityCodeCount().ToString();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju početnog ekrana");
            }
        }

        private void tileAktivniKlijenti_TileClick(object sender, EventArgs e) => ShowView(SidebarView.Klijenti);
        private void tileKyc_TileClick(object sender, EventArgs e) => ShowView(SidebarView.Kyc);
        private void tileVlasnici_TileClick(object sender, EventArgs e) => ShowView(SidebarView.Ubo);
        private void tilePep_TileClick(object sender, EventArgs e) => ShowView(SidebarView.Pep);
        private void tileRizik_TileClick(object sender, EventArgs e) => ShowView(SidebarView.Rizik);
        private void tileBezUgovora_TileClick(object sender, EventArgs e) => ShowView(SidebarView.BezUgovora);
        private void tileArhivirani_TileClick(object sender, EventArgs e) => ShowView(SidebarView.Otkazani);
        private void tileUdruzenja_TileClick(object sender, EventArgs e) => ShowView(SidebarView.Udruzenja);
        private void tileStecaj_TileClick(object sender, EventArgs e) => ShowView(SidebarView.Stecaj);
        private void tileAuditLog_TileClick(object sender, EventArgs e) => ShowView(SidebarView.AuditLog);
        private void tileDjelatnosti_TileClick(object sender, EventArgs e) => ShowView(SidebarView.Klijenti);
        private void tileOsvjezi_TileClick(object sender, EventArgs e) => LoadDashboard();

        private void tileUpozorenja_TileClick(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Create();
            new FrmUpozorenja(db).ShowDialog(this);
            LoadDashboard();
            RefreshWarningsBadge();
        }

        private void tileDodajKlijenta_TileClick(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Create();
            if (new FrmDodajKlijent(klijentId: null, db).ShowDialog() == DialogResult.OK)
            {
                RefreshAfterChange();
                LoadDashboard();
            }
        }

        private void tileOtkaziKlijenta_TileClick(object sender, EventArgs e)
        {
            ShowView(SidebarView.Klijenti);
            MessageBox.Show(UiMessages.SelectFirmOnKlijentiScreen);
        }

        private async void tileExportPdf_TileClick(object sender, EventArgs e)
        {
            try
            {
                using var db = DbContextFactory.Create();
                var dashboard = new DashboardQueryService(db);
                var warningStats = new WarningQueryService(db).GetStats();

                var data = new ComplianceSummaryData
                {
                    AktivniKlijenti = dashboard.GetActiveClientCount(),
                    KlijentiPoRiziku = dashboard.GetClientCountByRisk(),
                    PepKlijenti = dashboard.GetPepClientCount(),
                    KlijentiBezUgovora = dashboard.GetClientsWithoutContractCount(),
                    UpozorenjaUkupno = warningStats.Count,
                    UpozorenjaImaIsteklih = warningStats.HasExpired,
                };

                await _pdfPresenter.ExportComplianceSummaryAsync(btnDashboardExportPdfProxy, data);
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri generisanju compliance izvještaja");
            }
        }

        private void LoadOtkazaniKlijenti()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var clients = new KlijentQueryService(db).GetArchivedClients();
                dataGridOtkazani.DataSource = clients;
                StyleOtkazaniGrid();
                dataGridOtkazani.ClearSelection();
                lblEmptyOtkazani.Visible = clients.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju otkazanih klijenata");
            }
        }

        private void StyleOtkazaniGrid()
        {
            GridHelper.ApplyColumns(dataGridOtkazani, GridColumns.Klijenti);
            GridHelper.FreezeColumns(dataGridOtkazani, "Id", "Naziv");
            GridHelper.AlignCenter(dataGridOtkazani,
                "DatumUspostaveOdnosa", "DatumOsnivanjaFirme", "DatumProcjeneRizika", "DatumPotpisaUgovora",
                "VrstaKlijenta", "BrojVlasnika", "BrojDirektora", "PepDatumProvjere");
            GridHelper.Emphasize(dataGridOtkazani, "Naziv", UiTheme.Base(9f, FontStyle.Bold));
        }

        private void LoadUdruzenja()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var clients = new KlijentQueryService(db).GetUdruzenjaClients();
                dataGridUdruzenja.DataSource = clients;
                StyleUdruzenjaGrid();
                dataGridUdruzenja.ClearSelection();
                lblEmptyUdruzenja.Visible = clients.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju udruženja");
            }
        }

        private void StyleUdruzenjaGrid()
        {
            GridHelper.ApplyColumns(dataGridUdruzenja, GridColumns.Klijenti);
            GridHelper.FreezeColumns(dataGridUdruzenja, "Id", "Naziv");
            GridHelper.AlignCenter(dataGridUdruzenja,
                "DatumUspostaveOdnosa", "DatumOsnivanjaFirme", "DatumProcjeneRizika", "DatumPotpisaUgovora",
                "VrstaKlijenta", "BrojVlasnika", "BrojDirektora", "PepDatumProvjere");
            GridHelper.Emphasize(dataGridUdruzenja, "Naziv", UiTheme.Base(9f, FontStyle.Bold));
        }

        private void LoadStecajKlijenti()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var clients = new KlijentQueryService(db).GetStecajClients();
                dataGridStecaj.DataSource = clients;
                StyleStecajGrid();
                dataGridStecaj.ClearSelection();
                lblEmptyStecaj.Visible = clients.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju klijenata u stečaju");
            }
        }

        private void StyleStecajGrid()
        {
            GridHelper.ApplyColumns(dataGridStecaj, GridColumns.Klijenti);
            GridHelper.FreezeColumns(dataGridStecaj, "Id", "Naziv");
            GridHelper.AlignCenter(dataGridStecaj,
                "DatumUspostaveOdnosa", "DatumOsnivanjaFirme", "DatumProcjeneRizika", "DatumPotpisaUgovora",
                "VrstaKlijenta", "BrojVlasnika", "BrojDirektora", "PepDatumProvjere");
            GridHelper.Emphasize(dataGridStecaj, "Naziv", UiTheme.Base(9f, FontStyle.Bold));
        }

        private void LoadAuditLog()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var data = new EvidencijaQueryService(db).GetAuditLogEvidencija();
                dataGridAuditLog.DataSource = data;
                StyleAuditLogGrid();
                dataGridAuditLog.ClearSelection();
                lblEmptyAuditLog.Visible = data.Count == 0;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju audit loga");
            }
        }

        private void StyleAuditLogGrid()
        {
            // AutoSizeColumnsMode.Fill (postavljen u Designeru) razvlači kolone
            // preko cijele širine grida umjesto GridColumns.AuditLog fiksnih
            // Width vrijednosti, pa se ovdje isti omjer širina (140/120/80/110/400)
            // prenosi kroz FillWeight — isti obrazac kao ostali gridovi u
            // aplikaciji koji koriste Fill (npr. FrmKlijentProfil).
            foreach (var (ime, sirina, zaglavlje, format) in GridColumns.AuditLog)
                GridHelper.ConfigureColumn(dataGridAuditLog, ime, zaglavlje, sirina, format);

            GridHelper.FreezeColumns(dataGridAuditLog, "Vrijeme");
            GridHelper.AlignCenter(dataGridAuditLog, "Vrijeme", "EntitetId", "Akcija");
        }

        private void ShowView(SidebarView view)
        {
            _currentView = view;

            panelViewDashboard.Visible = view == SidebarView.Dashboard;
            panelViewKlijenti.Visible = view == SidebarView.Klijenti;
            panelViewKyc.Visible = view == SidebarView.Kyc;
            panelViewUbo.Visible = view == SidebarView.Ubo;
            panelViewPep.Visible = view == SidebarView.Pep;
            panelViewRizik.Visible = view == SidebarView.Rizik;
            panelViewBezUgovora.Visible = view == SidebarView.BezUgovora;
            panelViewOtkazani.Visible = view == SidebarView.Otkazani;
            panelViewUdruzenja.Visible = view == SidebarView.Udruzenja;
            panelViewStecaj.Visible = view == SidebarView.Stecaj;
            panelViewAuditLog.Visible = view == SidebarView.AuditLog;
            panelViewProfil.Visible = view == SidebarView.Profil;

            UiTheme.StyleSidebarButton(btnNavDashboard, active: view == SidebarView.Dashboard);
            UiTheme.StyleSidebarButton(btnNavKlijenti, active: view == SidebarView.Klijenti);
            UiTheme.StyleSidebarButton(btnNavKyc, active: view == SidebarView.Kyc);
            UiTheme.StyleSidebarButton(btnNavUbo, active: view == SidebarView.Ubo);
            UiTheme.StyleSidebarButton(btnNavPep, active: view == SidebarView.Pep);
            UiTheme.StyleSidebarButton(btnNavRizik, active: view == SidebarView.Rizik);
            UiTheme.StyleSidebarButton(btnNavBezUgovora, active: view == SidebarView.BezUgovora);
            UiTheme.StyleSidebarButton(btnNavOtkazani, active: view == SidebarView.Otkazani);
            UiTheme.StyleSidebarButton(btnNavUdruzenja, active: view == SidebarView.Udruzenja);
            UiTheme.StyleSidebarButton(btnNavStecaj, active: view == SidebarView.Stecaj);
            UiTheme.StyleSidebarButton(btnNavAuditLog, active: view == SidebarView.AuditLog);
            UiTheme.StyleSidebarButton(btnNavProfil, active: view == SidebarView.Profil);

            switch (view)
            {
                case SidebarView.Dashboard: LoadDashboard(); break;
                case SidebarView.Kyc: LoadKyc(); break;
                case SidebarView.Ubo: LoadUbo(); break;
                case SidebarView.Pep: LoadPep(); break;
                case SidebarView.Rizik: LoadRizik(); break;
                case SidebarView.BezUgovora: LoadBezUgovoraKlijenata(); break;
                case SidebarView.Otkazani: LoadOtkazaniKlijenti(); break;
                case SidebarView.Udruzenja: LoadUdruzenja(); break;
                case SidebarView.Stecaj: LoadStecajKlijenti(); break;
                case SidebarView.AuditLog: LoadAuditLog(); break;
                case SidebarView.Profil: LoadProfil(); break;
            }
        }

        // ── Profil / odjava ────────────────────────────────────────

        private void LoadProfil()
        {
            var user = _session.CurrentUser;
            if (user is null) return;

            lblProfilKorisnickoImeValue.Text = user.Username;
            lblProfilPrikaznoImeValue.Text = user.DisplayName;
            lblProfilStatusValue.Text = user.IsActive
                ? UiMessages.ProfileActiveStatus
                : UiMessages.ProfileInactiveStatus;
            lblProfilStatusValue.ForeColor = user.IsActive ? UiTheme.Green : UiTheme.Red;

            try
            {
                using var db = DbContextFactory.Create();
                ShowProfilPhoto(new AuthService(db).GetPhoto(user.Id));
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju profilne slike");
            }
        }

        private void ShowProfilPhoto(byte[]? photo)
        {
            var old = picProfil.Image;
            picProfil.Image = photo is { Length: > 0 }
                ? ProfileImageHelper.FromBytes(photo)
                : ProfileImageHelper.CreatePlaceholder(picProfil.Width);
            old?.Dispose();
        }

        private void btnOdaberiSliku_Click(object sender, EventArgs e)
        {
            var user = _session.CurrentUser;
            if (user is null) return;

            using var dialog = DialogHelper.CreateOpenDialogImage();
            if (dialog.ShowDialog() != DialogResult.OK) return;

            byte[] photo;
            try
            {
                photo = ProfileImageHelper.LoadAndNormalize(dialog.FileName);
            }
            catch (Exception ex) when (ex is ArgumentException or OutOfMemoryException or IOException)
            {
                MessageBox.Show(UiMessages.PhotoInvalid);
                return;
            }

            try
            {
                using var db = DbContextFactory.Create();
                new AuthService(db).SetPhoto(user.Id, photo);
                ShowProfilPhoto(photo);
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri spremanju slike");
            }
        }

        private void btnUkloniSliku_Click(object sender, EventArgs e)
        {
            var user = _session.CurrentUser;
            if (user is null) return;

            try
            {
                using var db = DbContextFactory.Create();
                new AuthService(db).SetPhoto(user.Id, null);
                ShowProfilPhoto(null);
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri uklanjanju slike");
            }
        }

        private void btnPromijeniLozinku_Click(object sender, EventArgs e)
        {
            var user = _session.CurrentUser;
            if (user is null) return;

            using var form = new FrmPromjenaLozinke(user.Id);
            form.ShowDialog(this);
        }

        private void btnNoviKorisnik_Click(object sender, EventArgs e)
        {
            using var form = new FrmNoviKorisnik();
            form.ShowDialog(this);
        }

        private void btnNavProfil_Click(object sender, EventArgs e) => ShowView(SidebarView.Profil);

        private void btnNavLogout_Click(object sender, EventArgs e)
        {
            if (!DialogHelper.ConfirmLogout()) return;

            _session.SignOut();
            LogoutRequested = true;
            Close();
        }

        private async void btnKycSacuvajPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericSingleRowAsync(dataGridKyc, btnKycSacuvajPdf, "KYC evidencija", "KYC");

        private async void btnKycExportPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericTableAsync(dataGridKyc, btnKycExportPdf, "KYC evidencija", "KYC_tabela");

        private async void btnUboSacuvajPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericSingleRowAsync(dataGridUbo, btnUboSacuvajPdf, "UBO / Vlasništvo", "UBO");

        private async void btnUboExportPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericTableAsync(dataGridUbo, btnUboExportPdf, "UBO / Vlasništvo", "UBO_tabela");

        private async void btnPepSacuvajPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericSingleRowAsync(dataGridPep, btnPepSacuvajPdf, "PEP evidencija", "PEP");

        private async void btnPepExportPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericTableAsync(dataGridPep, btnPepExportPdf, "PEP evidencija", "PEP_tabela");

        private async void btnRizikSacuvajPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericSingleRowAsync(dataGridRizik, btnRizikSacuvajPdf, "Procjena rizika", "Rizik");

        private async void btnRizikExportPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericTableAsync(dataGridRizik, btnRizikExportPdf, "Procjena rizika", "Rizik_tabela");

        private async void btnUdruzenjaSacuvajPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericSingleRowAsync(dataGridUdruzenja, btnUdruzenjaSacuvajPdf, "Udruženja", "Udruzenje", GridColumns.KlijentiPdf);

        private async void btnUdruzenjaExportPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericTableAsync(dataGridUdruzenja, btnUdruzenjaExportPdf, "Udruženja", "Udruzenja_tabela", GridColumns.KlijentiPdf);

        private async void btnStecajSacuvajPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericSingleRowAsync(dataGridStecaj, btnStecajSacuvajPdf, "Klijenti u stečaju", "Stecaj", GridColumns.KlijentiPdf);

        private async void btnStecajExportPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportGenericTableAsync(dataGridStecaj, btnStecajExportPdf, "Klijenti u stečaju", "Stecaj_tabela", GridColumns.KlijentiPdf);

        private void btnNavDashboard_Click(object sender, EventArgs e) => ShowView(SidebarView.Dashboard);
        private void btnNavKlijenti_Click(object sender, EventArgs e) => ShowView(SidebarView.Klijenti);
        private void btnNavKyc_Click(object sender, EventArgs e) => ShowView(SidebarView.Kyc);
        private void btnNavUbo_Click(object sender, EventArgs e) => ShowView(SidebarView.Ubo);
        private void btnNavBezUgovora_Click(object sender, EventArgs e) => ShowView(SidebarView.BezUgovora);
        private void btnNavOtkazani_Click(object sender, EventArgs e) => ShowView(SidebarView.Otkazani);
        private void btnNavUdruzenja_Click(object sender, EventArgs e) => ShowView(SidebarView.Udruzenja);
        private void btnNavStecaj_Click(object sender, EventArgs e) => ShowView(SidebarView.Stecaj);
        private void btnNavAuditLog_Click(object sender, EventArgs e) => ShowView(SidebarView.AuditLog);
        private void btnNavPep_Click(object sender, EventArgs e) => ShowView(SidebarView.Pep);
        private void btnNavRizik_Click(object sender, EventArgs e) => ShowView(SidebarView.Rizik);

        

        private void btnDodajKlijent_Click(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Create();
            if (new FrmDodajKlijent(klijentId: null, db).ShowDialog() == DialogResult.OK)
                RefreshAfterChange();
        }

        private void btnIzmijeniKlijent_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int id, UiMessages.SelectFirm)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajKlijent(id, db).ShowDialog() == DialogResult.OK)
                RefreshAfterChange();
        }

        private void btnObrisiKlijent_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int id, UiMessages.SelectFirm)) return;
            if (!DialogHelper.ConfirmArchive(UiMessages.ArchiveKlijentPrompt)) return;

            _archivePresenter.ArchiveKlijent(id, onSuccess: RefreshAfterChange);
        }

        

        private void btnDodajVlasnika_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirmFirst)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajVlasnika(klijentId, vlasnikId: null, db).ShowDialog() == DialogResult.OK)
                LoadOwners(klijentId);
        }

        private void btnIzmijeniVlasnika_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridVlasnici, out int vlasnikId, UiMessages.SelectVlasnik)) return;
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirm)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajVlasnika(klijentId, vlasnikId, db).ShowDialog() == DialogResult.OK)
                LoadOwners(klijentId);
        }

        private void btnObrisiVlasnika_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridVlasnici, out int vlasnikId, UiMessages.SelectVlasnik)) return;
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirm)) return;
            if (!DialogHelper.ConfirmArchive(UiMessages.ArchiveVlasnikPrompt)) return;

            _archivePresenter.ArchiveVlasnik(vlasnikId, onSuccess: () => LoadOwners(klijentId));
        }

       

        private void btnDodajDirektora_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirmFirst)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajDirektora(klijentId, direktorId: null, db).ShowDialog() == DialogResult.OK)
                LoadDirectors(klijentId);
        }

        private void btnIzmijeniDirektora_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridDirektori, out int direktorId, UiMessages.SelectDirektor)) return;
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirm)) return;

            using var db = DbContextFactory.Create();
            if (new FrmDodajDirektora(klijentId, direktorId, db).ShowDialog() == DialogResult.OK)
                LoadDirectors(klijentId);
        }

        private void btnObrisiDirektora_Click(object sender, EventArgs e)
        {
            if (!GridHelper.TryGetSelectedId(dataGridDirektori, out int direktorId, UiMessages.SelectDirektor)) return;
            if (!GridHelper.TryGetSelectedId(dataGridKlijenti, out int klijentId, UiMessages.SelectFirm)) return;
            if (!DialogHelper.ConfirmArchive(UiMessages.ArchiveDirektorPrompt)) return;

            _archivePresenter.ArchiveDirektor(direktorId, onSuccess: () => LoadDirectors(klijentId));
        }

        

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            using var dialog = DialogHelper.CreateOpenDialogExcel();
            if (dialog.ShowDialog() != DialogResult.OK) return;

            new ImportHelper(DbContextFactory.ConnectionString)
                .RunImport(dialog.FileName, this, RefreshAfterChange);
        }

        private void btnResetImport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    UiConstants.ResetImportConfirmMessage,
                    UiConstants.ResetImportTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            using var dialog = DialogHelper.CreateOpenDialogExcel(UiMessages.ResetImportDialogTitle);
            if (dialog.ShowDialog() != DialogResult.OK) return;

            if (!File.Exists(dialog.FileName))
            {
                MessageBox.Show(
                    UiMessages.ResetImportFileNotFound,
                    UiMessages.ResetImportFileNotFoundTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidateExcelFile(dialog.FileName)) return;

            try
            {
                var dbService = new DatabaseService(DbContextFactory.DbPath, DbContextFactory.ConnectionString);
                string backupPath = dbService.ResetDatabase();

                new ImportHelper(DbContextFactory.ConnectionString).RunImport(
                    dialog.FileName, this,
                    onCompleted: RefreshAfterChange,
                    onError: () => OfferBackupRestore(dbService, backupPath));
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri resetu baze");
            }
        }

        private static bool ValidateExcelFile(string filePath)
        {
            try
            {
                using var doc = SpreadsheetDocument.Open(filePath, isEditable: false);
                var workbookPart = doc.WorkbookPart
                    ?? throw new InvalidOperationException("Fajl nema validan WorkbookPart.");

                _ = workbookPart.Workbook.Sheets
                      ?.Cast<OpenXmlSheet>()
                      .FirstOrDefault(s => s.Name?.Value?.Contains(UiConstants.ExcelZirvanaKeyword) == true)
                    ?? throw new InvalidOperationException(
                        $"Fajl ne sadrži list sa '{UiConstants.ExcelZirvanaKeyword}'.");

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(UiMessages.ExcelValidationErrorFormat, ex.Message),
                    UiMessages.ExcelValidationErrorTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void OfferBackupRestore(DatabaseService dbService, string backupPath)
        {
            if (string.IsNullOrEmpty(backupPath)) return;

            var answer = MessageBox.Show(
                string.Format(UiMessages.BackupRestorePromptFormat, backupPath),
                UiMessages.BackupRestoreTitle,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (answer != DialogResult.Yes)
            {
                MessageBox.Show(
                    string.Format(UiMessages.BackupKeptFormat, backupPath),
                    UiMessages.BackupKeptTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                dbService.RestoreBackup(backupPath);
                RefreshAfterChange();
                MessageBox.Show(
                    UiMessages.BackupRestoredSuccess,
                    UiMessages.BackupRestoredSuccessTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppLogger.LogException(ex);
                MessageBox.Show(
                    string.Format(UiMessages.BackupRestoreFailedFormat, backupPath),
                    UiMessages.BackupRestoreFailedTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void RefreshWarningsBadge()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var stats = new WarningQueryService(db).GetStats();
                ApplyBadgeState(BadgeState.FromStats(stats.Count, stats.HasExpired));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BADGE] Greška: {ex.Message}");
                AppLogger.LogException(ex);
            }
        }

        private void ApplyBadgeState(BadgeState state)
        {
            btnUpozorenja.Text = state.Label;
            btnUpozorenja.BackColor = state.BackColor;
            btnUpozorenja.ForeColor = state.ForeColor;

            var oldFont = btnUpozorenja.Font;
            if (oldFont.Style != state.FontStyle)
            {
                btnUpozorenja.Font = new Font(oldFont.FontFamily, oldFont.Size, state.FontStyle);
                oldFont.Dispose();
            }
        }

        private void btnUpozorenja_Click(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Create();
            new FrmUpozorenja(db).ShowDialog(this);
        }

        

        private async void btnExportTabelaPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportTableAsync(dataGridKlijenti, btnExportTabelaPdf);

        private async void btnSacuvajPdf_Click(object sender, EventArgs e) =>
            await _pdfPresenter.ExportSingleClientAsync(dataGridKlijenti, btnSacuvajPdf);

        

        private void RefreshAfterChange()
        {
            LoadActivityCodeFilter();
            LoadClients();
            RefreshWarningsBadge();
            if (_currentView == SidebarView.Dashboard) LoadDashboard();
        }

        // ── Sidebar ───────────────────────────────────────────────

        private void btnToggleSidebar_Click(object sender, EventArgs e)
        {
            _sidebarExpanded = !_sidebarExpanded;
            panelSidebar.Width = _sidebarExpanded ? SidebarExpandedWidth : SidebarCollapsedWidth;

            lblSidebarBrand.Text = _sidebarExpanded ? "CONFIDIA BH" : "C";
            _sidebarToolTip.SetToolTip(lblSidebarBrand, _sidebarExpanded ? string.Empty : "Confidia BH");

            SetNavButtonLabel(btnNavDashboard, "Početni ekran");
            SetNavButtonLabel(btnNavKlijenti, "Klijenti");
            SetNavButtonLabel(btnNavKyc, "KYC evidencija");
            SetNavButtonLabel(btnNavUbo, "UBO / Vlasništvo");
            SetNavButtonLabel(btnNavPep, "PEP evidencija");
            SetNavButtonLabel(btnNavRizik, "Procjena rizika");
            SetNavButtonLabel(btnNavBezUgovora, "Klijenti bez ugovora");
            SetNavButtonLabel(btnNavOtkazani, "Otkazani klijenti");
            SetNavButtonLabel(btnNavUdruzenja, "Udruženja");
            SetNavButtonLabel(btnNavStecaj, "Klijenti u stečaju");
            SetNavButtonLabel(btnNavAuditLog, "Historija promjena");
            SetNavButtonLabel(btnNavProfil, "Profil");
            SetNavButtonLabel(btnNavLogout, "Odjava");
        }

        // Kada je sidebar collapsed, dugmad prikazuju samo ikonu — naziv
        // stavke se u tom slučaju prebacuje u tooltip da ostane dostupan na
        // hover. Kada je expanded, naziv je već vidljiv kao tekst dugmeta,
        // pa tooltip nije potreban.
        private void SetNavButtonLabel(Control button, string label)
        {
            button.Text = _sidebarExpanded ? label : string.Empty;
            _sidebarToolTip.SetToolTip(button, _sidebarExpanded ? string.Empty : label);
        }
    }
}