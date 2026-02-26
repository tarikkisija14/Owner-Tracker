using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlSheet = DocumentFormat.OpenXml.Spreadsheet.Sheet;


namespace OwnerTrack.App
{
    public partial class Form1 : Form
    {
        private readonly System.Windows.Forms.Timer _searchTimer;
        private bool _koloneKonfigurisane = false;

        public Form1()
        {
            InitializeComponent();

            _searchTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();
                LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra(), GetSelectedVelicina());
            };

            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var schema = new SchemaManager(DbContextFactory.ConnectionString);
                schema.ApplyMigrations();

                LoadDjelatnostiFilter();
                LoadVelicinaFilter();
                LoadKlijenti();
                OsvjeziUpozerenjaBadge();

                Text = $"OwnerTrack v{Application.ProductVersion}";
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show(
                    $"Greška pri pokretanju aplikacije:\n\n{ex.Message}\n\n" +
                    $"Baza podataka: {DbContextFactory.DbPath}\n\n" +
                    $"Detalji su sačuvani u log fajlu:\n{Program.GetLogPath()}\n\n" +
                    "Ako problem ostane, kontaktiraj podršku.",
                    "Greška pokretanja",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _searchTimer?.Dispose();
            base.OnFormClosed(e);
        }

        // -------------------------------------------------------------------
        // LOAD METODE
        // -------------------------------------------------------------------

        private void LoadVelicinaFilter()
        {
            cmbFilterVelicina.Items.Clear();
            cmbFilterVelicina.Items.Add(new { Value = "", Display = "-- Sve --" });
            foreach (VelicinaFirme v in Enum.GetValues(typeof(VelicinaFirme)))
                cmbFilterVelicina.Items.Add(new { Value = v.ToString(), Display = v.ToString() });
            cmbFilterVelicina.DisplayMember = "Display";
            cmbFilterVelicina.ValueMember = "Value";
            cmbFilterVelicina.SelectedIndex = 0;
        }

        private void LoadDjelatnostiFilter()
        {
            try
            {
                using var db = DbContextFactory.Kreiraj();
                var djelatnosti = db.Djelatnosti.OrderBy(d => d.Naziv).ToList();
                cmbFilterDjelatnost.Items.Clear();
                cmbFilterDjelatnost.Items.Add(new { Sifra = "", Naziv = "-- Sve djelatnosti --" });
                foreach (var d in djelatnosti)
                    cmbFilterDjelatnost.Items.Add(new { d.Sifra, Naziv = $"{d.Sifra} - {d.Naziv}" });
                cmbFilterDjelatnost.DisplayMember = "Naziv";
                cmbFilterDjelatnost.ValueMember = "Sifra";
                cmbFilterDjelatnost.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška pri učitavanju djelatnosti: {ex.Message}");
            }
        }

        private void LoadKlijenti(string filter = "", string sifraDjelatnosti = "", string velicina = "")
        {
            try
            {
                using var db = DbContextFactory.Kreiraj();
                var klijenti = db.Klijenti
                    .AsNoTracking()
                    .Where(k =>
                        (string.IsNullOrWhiteSpace(filter) ||
                         k.Naziv.ToLower().Contains(filter.ToLower()) ||
                         k.IdBroj.ToLower().Contains(filter.ToLower()))
                        &&
                        (string.IsNullOrWhiteSpace(sifraDjelatnosti) ||
                         k.SifraDjelatnosti == sifraDjelatnosti)
                        &&
                        (string.IsNullOrWhiteSpace(velicina) ||
                         k.Velicina == velicina))
                    .Select(k => new KlijentViewModel
                    {
                        Id = k.Id,
                        Naziv = k.Naziv,
                        IdBroj = k.IdBroj,
                        Adresa = k.Adresa,
                        SifraDjelatnosti = k.SifraDjelatnosti,
                        Djelatnost = k.Djelatnost != null ? k.Djelatnost.Naziv : "",
                        DatumUspostaveOdnosa = k.DatumUspostave,
                        VrstaKlijenta = k.VrstaKlijenta,
                        DatumOsnivanjaFirme = k.DatumOsnivanja,
                        Velicina = k.Velicina,
                        PepRizik = k.PepRizik,
                        UboRizik = k.UboRizik,
                        GotovinaRizik = k.GotovinaRizik,
                        GeografskiRizik = k.GeografskiRizik,
                        UkupnaProcjena = k.UkupnaProcjena,
                        DatumProcjeneRizika = k.DatumProcjene,
                        OvjeraCr = k.OvjeraCr,
                        StatusUgovora = k.Ugovor != null ? k.Ugovor.StatusUgovora : "",
                        DatumPotpisaUgovora = k.Ugovor != null ? k.Ugovor.DatumUgovora : null,
                        BrojVlasnika = k.Vlasnici.Count(),
                        BrojDirektora = k.Direktori.Count(),
                        StatusKlijenta = k.Status,
                        Napomena = k.Napomena
                    })
                    .ToList();

                dataGridKlijenti.SelectionChanged -= dataGridKlijenti_SelectionChanged;
                dataGridKlijenti.DataSource = klijenti;
                dataGridKlijenti.ClearSelection();
                dataGridKlijenti.SelectionChanged += dataGridKlijenti_SelectionChanged;

                if (!_koloneKonfigurisane)
                {
                    KonfigurirajKolone();
                    _koloneKonfigurisane = true;
                }
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška pri učitavanju klijenata: {ex.Message}");
            }
        }

        private void KonfigurirajKolone()
        {
            PostaviKolone(dataGridKlijenti, new[]
            {
                ("Id", 40, "ID", (string?)null),
                ("Naziv", 220, "Naziv preduzeća", (string?)null),
                ("IdBroj", 130, "ID broj", (string?)null),
                ("Adresa", 200, "Adresa", (string?)null),
                ("SifraDjelatnosti", 70, "Šifra", (string?)null),
                ("Djelatnost", 220, "Djelatnost", (string?)null),
                ("DatumUspostaveOdnosa", 120, "Datum uspostave odnosa", "dd.MM.yyyy"),
                ("VrstaKlijenta", 110, "Vrsta klijenta", (string?)null),
                ("DatumOsnivanjaFirme", 120, "Datum osnivanja", "dd.MM.yyyy"),
                ("Velicina", 80, "Veličina", (string?)null),
                ("PepRizik", 70, "PEP", (string?)null),
                ("UboRizik", 70, "UBO", (string?)null),
                ("GotovinaRizik", 90, "Gotovina rizik", (string?)null),
                ("GeografskiRizik", 100, "Geografski rizik", (string?)null),
                ("UkupnaProcjena", 120, "Ukupna procjena", (string?)null),
                ("DatumProcjeneRizika", 120, "Datum procjene rizika", "dd.MM.yyyy"),
                ("OvjeraCr", 150, "Ovjera/CR", (string?)null),
                ("StatusUgovora", 110, "Status ugovora", (string?)null),
                ("DatumPotpisaUgovora", 120, "Datum potpisa ugovora", "dd.MM.yyyy"),
                ("BrojVlasnika", 80, "Vlasnici", (string?)null),
                ("BrojDirektora", 80, "Direktori", (string?)null),
                ("StatusKlijenta", 90, "Status klijenta", (string?)null),
                ("Napomena", 200, "Napomena", (string?)null),
            });
        }

        private void LoadVlasnici(int klijentId)
        {
            try
            {
                using var db = DbContextFactory.Kreiraj();
                var vlasnici = db.Vlasnici
                    .Where(v => v.KlijentId == klijentId)
                    .AsNoTracking()
                    .Select(v => new VlasnikViewModel
                    {
                        Id = v.Id,
                        ImePrezime = v.ImePrezime,
                        DatumValjanostiDokumenta = v.DatumValjanostiDokumenta,
                        ProcenatVlasnistva = v.ProcenatVlasnistva,
                        DatumUtvrdjivanja = v.DatumUtvrdjivanja,
                        IzvorPodatka = v.IzvorPodatka,
                        StatusVlasnika = v.Status
                    })
                    .ToList();

                dataGridVlasnici.DataSource = vlasnici;

                PostaviKolone(dataGridVlasnici, new[]
                {
                    ("Id", 40, "ID", (string?)null),
                    ("ImePrezime", 180, "Ime i prezime", (string?)null),
                    ("DatumValjanostiDokumenta", 140, "Datum važenja dok.", "dd.MM.yyyy"),
                    ("ProcenatVlasnistva", 100, "% vlasništva", (string?)null),
                    ("DatumUtvrdjivanja", 130, "Datum utvrđivanja", "dd.MM.yyyy"),
                    ("IzvorPodatka", 150, "Izvor podatka", (string?)null),
                    ("StatusVlasnika", 90, "Status", (string?)null),
                });
                dataGridVlasnici.ClearSelection();
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška pri učitavanju vlasnika: {ex.Message}");
            }
        }

        private void LoadDirektori(int klijentId)
        {
            try
            {
                using var db = DbContextFactory.Kreiraj();
                var direktori = db.Direktori
                    .Where(d => d.KlijentId == klijentId)
                    .AsNoTracking()
                    .Select(d => new DirektorViewModel
                    {
                        Id = d.Id,
                        ImePrezime = d.ImePrezime,
                        DatumValjanostiDokumenta = d.DatumValjanosti,
                        TipValjanosti = d.TipValjanosti,
                        StatusDirektora = d.Status
                    })
                    .ToList();

                dataGridDirektori.DataSource = direktori;

                PostaviKolone(dataGridDirektori, new[]
                {
                    ("Id", 40, "ID", (string?)null),
                    ("ImePrezime", 200, "Ime i prezime", (string?)null),
                    ("DatumValjanostiDokumenta", 140, "Datum važenja dok.", "dd.MM.yyyy"),
                    ("TipValjanosti", 120, "Tip valjanosti", (string?)null),
                    ("StatusDirektora", 90, "Status", (string?)null),
                });
                dataGridDirektori.ClearSelection();
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška pri učitavanju direktora: {ex.Message}");
            }
        }

        private static void PostaviKolone(DataGridView grid,
            (string Ime, int Sirina, string Zaglavlje, string? Format)[] kolone)
        {
            if (grid.Columns.Count == 0) return;
            foreach (var (ime, sirina, zaglavlje, format) in kolone)
            {
                if (!grid.Columns.Contains(ime)) continue;
                grid.Columns[ime].Width = sirina;
                grid.Columns[ime].HeaderText = zaglavlje;
                if (format != null)
                    grid.Columns[ime].DefaultCellStyle.Format = format;
            }
        }

        // -------------------------------------------------------------------
        // GRID EVENTI
        // -------------------------------------------------------------------

        private void dataGridKlijenti_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0) return;
            if (dataGridKlijenti.SelectedRows[0].Cells["Id"].Value is not int id) return;

            LoadVlasnici(id);
            LoadDirektori(id);
        }

        private void ApplyCurrentFilters() =>
            LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra(), GetSelectedVelicina());

        // -------------------------------------------------------------------
        // KLIJENTI — CRUD
        // -------------------------------------------------------------------

        private void btnDodajKlijent_Click(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajKlijent(null, db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadDjelatnostiFilter();
                ApplyCurrentFilters();
                OsvjeziUpozerenjaBadge();
            }
        }

        private void btnIzmijeniKlijent_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Odaberi firmu!"); return; }
            if (dataGridKlijenti.SelectedRows[0].Cells["Id"].Value is not int id) return;

            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajKlijent(id, db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadDjelatnostiFilter();
                ApplyCurrentFilters();
                OsvjeziUpozerenjaBadge();
            }
        }

        private void btnObrisiKlijent_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Odaberi firmu!"); return; }
            if (dataGridKlijenti.SelectedRows[0].Cells["Id"].Value is not int id) return;

            if (MessageBox.Show(
                    "Firma će biti arhivirana i neće biti vidljiva.\n" +
                    "Podaci ostaju u bazi radi revizijskog traga.\n\nNastavi?",
                    "Potvrda", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            try
            {
                using var db = DbContextFactory.Kreiraj();
                var audit = new AuditService(db);

                var klijent = db.Klijenti.Find(id);
                if (klijent == null) return;

                using var tx = db.Database.BeginTransaction();
                try
                {
                    klijent.Status = StatusKonstante.Arhiviran;
                    klijent.Obrisan = DateTime.Now;
                    audit.ZabiljeziBesSave(klijent, "Klijenti", id, $"Arhivirana firma: '{klijent.Naziv}'", "OBRISANO");
                    db.SaveChanges();
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }

                MessageBox.Show("Firma je arhivirana.");
                LoadDjelatnostiFilter();
                ApplyCurrentFilters();
                OsvjeziUpozerenjaBadge();
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // VLASNICI — CRUD
        // -------------------------------------------------------------------

        private void btnDodajVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Prvo odaberi firmu!"); return; }
            if (dataGridKlijenti.SelectedRows[0].Cells["Id"].Value is not int klijentId) return;

            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajVlasnika(klijentId, null, db);
            if (forma.ShowDialog() == DialogResult.OK) LoadVlasnici(klijentId);
        }

        private void btnIzmijeniVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridVlasnici.SelectedRows.Count == 0) { MessageBox.Show("Odaberi vlasnika!"); return; }
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Odaberi firmu!"); return; }
            if (dataGridVlasnici.SelectedRows[0].Cells["Id"].Value is not int vlasnikId) return;
            if (dataGridKlijenti.SelectedRows[0].Cells["Id"].Value is not int klijentId) return;

            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajVlasnika(klijentId, vlasnikId, db);
            if (forma.ShowDialog() == DialogResult.OK) LoadVlasnici(klijentId);
        }

        private void btnObrisiVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridVlasnici.SelectedRows.Count == 0) { MessageBox.Show("Odaberi vlasnika!"); return; }
            if (MessageBox.Show("Arhivirati vlasnika?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No) return;
            if (dataGridVlasnici.SelectedRows[0].Cells["Id"].Value is not int vlasnikId) return;

            try
            {
                using var db = DbContextFactory.Kreiraj();
                var audit = new AuditService(db);

                var vlasnik = db.Vlasnici.Find(vlasnikId);
                if (vlasnik == null) return;
                int klijentId = vlasnik.KlijentId;

                using var tx = db.Database.BeginTransaction();
                try
                {
                    vlasnik.Status = StatusKonstante.Arhiviran;
                    vlasnik.Obrisan = DateTime.Now;
                    audit.ZabiljeziBesSave(vlasnik, "Vlasnici", vlasnikId, $"Arhiviran: '{vlasnik.ImePrezime}'", "OBRISANO");
                    db.SaveChanges();
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }

                MessageBox.Show("Vlasnik arhiviran.");
                LoadVlasnici(klijentId);
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // DIREKTORI — CRUD
        // -------------------------------------------------------------------

        private void btnDodajDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Prvo odaberi firmu!"); return; }
            if (dataGridKlijenti.SelectedRows[0].Cells["Id"].Value is not int klijentId) return;

            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajDirektora(klijentId, null, db);
            if (forma.ShowDialog() == DialogResult.OK) LoadDirektori(klijentId);
        }

        private void btnIzmijeniDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridDirektori.SelectedRows.Count == 0) { MessageBox.Show("Odaberi direktora!"); return; }
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Odaberi firmu!"); return; }
            if (dataGridDirektori.SelectedRows[0].Cells["Id"].Value is not int direktorId) return;
            if (dataGridKlijenti.SelectedRows[0].Cells["Id"].Value is not int klijentId) return;

            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajDirektora(klijentId, direktorId, db);
            if (forma.ShowDialog() == DialogResult.OK) LoadDirektori(klijentId);
        }

        private void btnObrisiDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridDirektori.SelectedRows.Count == 0) { MessageBox.Show("Odaberi direktora!"); return; }
            if (MessageBox.Show("Arhivirati direktora?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No) return;
            if (dataGridDirektori.SelectedRows[0].Cells["Id"].Value is not int direktorId) return;

            try
            {
                using var db = DbContextFactory.Kreiraj();
                var audit = new AuditService(db);

                var direktor = db.Direktori.Find(direktorId);
                if (direktor == null) return;
                int klijentId = direktor.KlijentId;

                using var tx = db.Database.BeginTransaction();
                try
                {
                    direktor.Status = StatusKonstante.Arhiviran;
                    direktor.Obrisan = DateTime.Now;
                    audit.ZabiljeziBesSave(direktor, "Direktori", direktorId, $"Arhiviran: '{direktor.ImePrezime}'", "OBRISANO");
                    db.SaveChanges();
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }

                MessageBox.Show("Direktor arhiviran.");
                LoadDirektori(klijentId);
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // FILTERI
        // -------------------------------------------------------------------

        private string GetSelectedDjelatnostSifra()
        {
            if (cmbFilterDjelatnost.SelectedItem == null) return "";
            dynamic item = cmbFilterDjelatnost.SelectedItem;
            return item.Sifra ?? "";
        }

        private string GetSelectedVelicina()
        {
            if (cmbFilterVelicina.SelectedItem == null) return "";
            dynamic item = cmbFilterVelicina.SelectedItem;
            return item.Value ?? "";
        }

        private void txtSearchKlijent_TextChanged(object sender, EventArgs e)
        { _searchTimer.Stop(); _searchTimer.Start(); }

        private void cmbFilterDjelatnost_SelectedIndexChanged(object sender, EventArgs e) =>
            LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra(), GetSelectedVelicina());

        private void cmbFilterVelicina_SelectedIndexChanged(object sender, EventArgs e) =>
            LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra(), GetSelectedVelicina());

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            txtSearchKlijent.Text = "";
            cmbFilterDjelatnost.SelectedIndex = 0;
            cmbFilterVelicina.SelectedIndex = 0;
            LoadKlijenti();
        }

        // -------------------------------------------------------------------
        // IMPORT
        // -------------------------------------------------------------------

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx" };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            var helper = new ImportHelper(DbContextFactory.ConnectionString);
            helper.PokreniImport(dialog.FileName, this, () =>
            {
                LoadDjelatnostiFilter();
                LoadKlijenti();
                OsvjeziUpozerenjaBadge();
            });
        }

        private void btnResetImport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Ovo će obrisati SVE podatke i pokrenuti novi import.\n\nJesi li siguran?",
                    "RESET BAZE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            var dialog = new OpenFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Odaberi Excel fajl za reimport"
            };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            if (!File.Exists(dialog.FileName))
            {
                MessageBox.Show("Odabrani fajl nije pronađen.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (var testDoc = SpreadsheetDocument.Open(dialog.FileName, false))
                {
                    var wbPart = testDoc.WorkbookPart
                        ?? throw new Exception("Fajl nema validan WorkbookPart.");
                    _ = wbPart.Workbook.Sheets?.Cast<OpenXmlSheet>()
                        .FirstOrDefault(s => s.Name?.Value?.Contains("ZBIRNA") == true)
                        ?? throw new Exception("Fajl ne sadrži list sa 'ZBIRNA'. Provjeri da li je odabran ispravan Excel.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Odabrani fajl nije validan za import:\n\n{ex.Message}",
                    "Greška validacije", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var dbService = new DatabaseService(DbContextFactory.DbPath, DbContextFactory.ConnectionString);
                dbService.ResetirajBazu();

                var helper = new ImportHelper(DbContextFactory.ConnectionString);
                helper.PokreniImport(dialog.FileName, this, () =>
                {
                    LoadDjelatnostiFilter();
                    LoadKlijenti();
                    OsvjeziUpozerenjaBadge();
                });
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška pri resetu baze:\n{ex.Message}", "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------------------------------------------------------------------
        // UPOZORENJA
        // -------------------------------------------------------------------

        private void OsvjeziUpozerenjaBadge()
        {
            try
            {
                using var db = DbContextFactory.Kreiraj();
                var danas = DateTime.Today;
                var granica = danas.AddDays(60);

                bool imaIsteklih = db.Vlasnici.AsNoTracking()
                    .Any(v => v.DatumValjanostiDokumenta < danas
                           && v.Status == StatusKonstante.Aktivan
                           && v.Klijent.Status != StatusKonstante.Arhiviran)
                    || db.Direktori.AsNoTracking()
                    .Any(d => d.DatumValjanosti < danas
                           && d.TipValjanosti == "VREMENSKI"
                           && d.Status == StatusKonstante.Aktivan
                           && d.Klijent.Status != StatusKonstante.Arhiviran);

                int count = db.Vlasnici.AsNoTracking()
                    .Count(v => v.DatumValjanostiDokumenta <= granica
                             && v.Status == StatusKonstante.Aktivan
                             && v.Klijent.Status != StatusKonstante.Arhiviran)
                    + db.Direktori.AsNoTracking()
                    .Count(d => d.DatumValjanosti <= granica
                             && d.TipValjanosti == "VREMENSKI"
                             && d.Status == StatusKonstante.Aktivan
                             && d.Klijent.Status != StatusKonstante.Arhiviran);

                var targetStyle = count > 0
                    ? System.Drawing.FontStyle.Bold
                    : System.Drawing.FontStyle.Regular;

                if (btnUpozorenja.Font.Style != targetStyle)
                    btnUpozorenja.Font = new System.Drawing.Font(btnUpozorenja.Font, targetStyle);

                if (count > 0)
                {
                    btnUpozorenja.Text = $"🔔 Upozorenja ({count})";
                    btnUpozorenja.BackColor = imaIsteklih ? Color.Firebrick : Color.FromArgb(220, 120, 20);
                    btnUpozorenja.ForeColor = Color.White;
                }
                else
                {
                    btnUpozorenja.Text = "🔔 Upozorenja";
                    btnUpozorenja.BackColor = SystemColors.Control;
                    btnUpozorenja.ForeColor = SystemColors.ControlText;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BADGE] Greška: {ex.Message}");
                Program.LogException(ex);
            }
        }

        private void btnUpozorenja_Click(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Kreiraj();
            new FrmUpozorenja(db).ShowDialog(this);
        }

        // -------------------------------------------------------------------
        // PDF EXPORT — async da ne zamrzava UI
        // -------------------------------------------------------------------

        private async void btnExportTabelaPdf_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.Rows.Count == 0)
            { MessageBox.Show("Nema klijenata za export."); return; }

            var ids = dataGridKlijenti.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.DataBoundItem != null)
                .Select(r => r.Cells["Id"].Value is int id ? id : 0)
                .Where(id => id > 0)
                .ToList();

            using var dlg = new SaveFileDialog
            {
                Title = "Sačuvaj tabelu klijenata",
                Filter = "PDF dokument (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                FileName = $"Klijenti_tabela_{DateTime.Now:yyyyMMdd}.pdf",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            btnExportTabelaPdf.Enabled = false;
            btnExportTabelaPdf.Text = "Generišem...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string savedFileName = dlg.FileName;
                string path = await Task.Run(() =>
                {
                    using var db = DbContextFactory.Kreiraj();
                    var svc = new PdfExportService(db);
                    return svc.GenerirajTabeluKlijenata(ids, savedFileName);
                });

                if (MessageBox.Show($"PDF je sačuvan:\n{path}\n\nŽeliš li ga otvoriti?",
                        "PDF kreiran", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška pri generisanju PDF-a:\n\n{ex.Message}",
                    "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnExportTabelaPdf.Enabled = true;
                btnExportTabelaPdf.Text = "📋 Export tabele u PDF";
                Cursor = Cursors.Default;
            }
        }

        private async void btnSacuvajPdf_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0)
            { MessageBox.Show("Odaberi firmu iz liste prije eksporta."); return; }

            if (dataGridKlijenti.SelectedRows[0].DataBoundItem is not KlijentViewModel row)
            { MessageBox.Show("Nije moguće pročitati odabranu firmu."); return; }

            int klijentId = row.Id;
            string naziv = row.Naziv ?? "";

            using var dlg = new SaveFileDialog
            {
                Title = "Sačuvaj izvještaj firme",
                Filter = "PDF dokument (*.pdf)|*.pdf",
                DefaultExt = "pdf",
                FileName = SigurnoIme(naziv),
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            if (dlg.ShowDialog() != DialogResult.OK) return;

            btnSacuvajPdf.Enabled = false;
            btnSacuvajPdf.Text = "Generišem...";
            Cursor = Cursors.WaitCursor;

            try
            {
                string savedFileName = dlg.FileName;
                string path = await Task.Run(() =>
                {
                    using var db = DbContextFactory.Kreiraj();
                    var svc = new PdfExportService(db);
                    return svc.GenerirajPdf(klijentId, savedFileName);
                });

                if (MessageBox.Show($"PDF je sačuvan:\n{path}\n\nŽeliš li ga otvoriti?",
                        "PDF kreiran", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show($"Greška pri generisanju PDF-a:\n\n{ex.Message}",
                    "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSacuvajPdf.Enabled = true;
                btnSacuvajPdf.Text = "📄 Sačuvaj kao PDF";
                Cursor = Cursors.Default;
            }
        }

        private static string SigurnoIme(string naziv)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var safe = string.Concat(naziv.Select(c => invalid.Contains(c) ? '_' : c));
            return $"{safe.Trim('_', ' ')}_{DateTime.Now:yyyyMMdd}.pdf";
        }
    }
}