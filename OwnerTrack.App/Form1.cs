using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class Form1 : Form
    {
        private OwnerTrackDbContext _db;
        private AuditService _audit;

        private readonly System.Windows.Forms.Timer _searchTimer;

        public Form1()
        {
            InitializeComponent();

            var schema = new SchemaManager(DbContextFactory.ConnectionString);
            schema.ApplyMigrations();

            _db = DbContextFactory.Kreiraj();
            _db.Database.EnsureCreated();
            _audit = new AuditService(_db);

            _searchTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();
                LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra(), GetSelectedVelicina());
            };

            LoadDjelatnostiFilter();
            LoadVelicinaFilter();
            LoadKlijenti();
            OsvjeziUpozerenjaBadge();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _db?.Dispose();
            _searchTimer?.Dispose();
            base.OnFormClosed(e);
        }

        /

        private void LoadVelicinaFilter()
        {
            cmbFilterVelicina.Items.Clear();
            cmbFilterVelicina.Items.Add(new { Value = "", Display = "-- Sve --" });
            foreach (var v in Enum.GetValues(typeof(VelicinaFirme)).Cast<VelicinaFirme>())
                cmbFilterVelicina.Items.Add(new { Value = v.ToString(), Display = v.ToString() });
            cmbFilterVelicina.DisplayMember = "Display";
            cmbFilterVelicina.ValueMember = "Value";
            cmbFilterVelicina.SelectedIndex = 0;
        }

        private void LoadDjelatnostiFilter()
        {
            try
            {
                var djelatnosti = _db.Djelatnosti.OrderBy(d => d.Naziv).ToList();
                cmbFilterDjelatnost.Items.Clear();
                cmbFilterDjelatnost.Items.Add(new { Sifra = "", Naziv = "-- Sve djelatnosti --" });
                foreach (var d in djelatnosti)
                    cmbFilterDjelatnost.Items.Add(new { d.Sifra, Naziv = $"{d.Sifra} - {d.Naziv}" });
                cmbFilterDjelatnost.DisplayMember = "Naziv";
                cmbFilterDjelatnost.ValueMember = "Sifra";
                cmbFilterDjelatnost.SelectedIndex = 0;
            }
            catch (Exception ex) { MessageBox.Show($"Greška pri učitavanju djelatnosti: {ex.Message}"); }
        }

        private void LoadKlijenti(string filter = "", string sifraDjelatnosti = "", string velicina = "")
        {
            try
            {
                var klijenti = _db.Klijenti
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
                    .Select(k => new
                    {
                        k.Id,
                        k.Naziv,
                        k.IdBroj,
                        k.Adresa,
                        SifraDjelatnosti = k.SifraDjelatnosti,
                        Djelatnost = k.Djelatnost != null ? k.Djelatnost.Naziv : "",
                        DatumUspostaveOdnosa = k.DatumUspostave,
                        k.VrstaKlijenta,
                        DatumOsnivanjaFirme = k.DatumOsnivanja,
                        k.Velicina,
                        k.PepRizik,
                        k.UboRizik,
                        k.GotovinaRizik,
                        k.GeografskiRizik,
                        k.UkupnaProcjena,
                        DatumProcjeneRizika = k.DatumProcjene,
                        k.OvjeraCr,
                        StatusUgovora = k.Ugovor != null ? k.Ugovor.StatusUgovora : "",
                        DatumPotpisaUgovora = k.Ugovor != null ? k.Ugovor.DatumUgovora : (DateTime?)null,
                        BrojVlasnika = k.Vlasnici.Count(),
                        BrojDirektora = k.Direktori.Count(),
                        StatusKlijenta = k.Status,
                        k.Napomena
                    })
                    .ToList();

                dataGridKlijenti.SelectionChanged -= dataGridKlijenti_SelectionChanged;
                dataGridKlijenti.DataSource = klijenti;
                dataGridKlijenti.ClearSelection();
                dataGridKlijenti.SelectionChanged += dataGridKlijenti_SelectionChanged;
                KonfigurirajKolone();
            }
            catch (Exception ex) { MessageBox.Show($"Greška: {ex.Message}"); }
        }

        private void KonfigurirajKolone()
        {
            if (dataGridKlijenti.Columns.Count == 0) return;
            void K(string n, int w, string h, string? f = null)
            {
                if (!dataGridKlijenti.Columns.Contains(n)) return;
                dataGridKlijenti.Columns[n].Width = w;
                dataGridKlijenti.Columns[n].HeaderText = h;
                if (f != null) dataGridKlijenti.Columns[n].DefaultCellStyle.Format = f;
            }
            K("Id", 40, "ID"); K("Naziv", 220, "Naziv preduzeća"); K("IdBroj", 130, "ID broj");
            K("Adresa", 200, "Adresa"); K("SifraDjelatnosti", 70, "Šifra"); K("Djelatnost", 220, "Djelatnost");
            K("DatumUspostaveOdnosa", 120, "Datum uspostave odnosa", "dd.MM.yyyy");
            K("VrstaKlijenta", 110, "Vrsta klijenta");
            K("DatumOsnivanjaFirme", 120, "Datum osnivanja", "dd.MM.yyyy");
            K("Velicina", 80, "Veličina"); K("PepRizik", 70, "PEP"); K("UboRizik", 70, "UBO");
            K("GotovinaRizik", 90, "Gotovina rizik"); K("GeografskiRizik", 100, "Geografski rizik");
            K("UkupnaProcjena", 120, "Ukupna procjena");
            K("DatumProcjeneRizika", 120, "Datum procjene rizika", "dd.MM.yyyy");
            K("OvjeraCr", 150, "Ovjera/CR"); K("StatusUgovora", 110, "Status ugovora");
            K("DatumPotpisaUgovora", 120, "Datum potpisa ugovora", "dd.MM.yyyy");
            K("BrojVlasnika", 80, "Vlasnici"); K("BrojDirektora", 80, "Direktori");
            K("StatusKlijenta", 90, "Status klijenta"); K("Napomena", 200, "Napomena");
        }

        private void LoadVlasnici(int klijentId)
        {
            var vlasnici = _db.Vlasnici.Where(v => v.KlijentId == klijentId).AsNoTracking().ToList();
            dataGridVlasnici.DataSource = vlasnici.Select(v => new
            {
                v.Id,
                v.ImePrezime,
                DatumValjanostiDokumenta = v.DatumValjanostiDokumenta,
                ProcenatVlasnistva = v.ProcenatVlasnistva,
                DatumUtvrdjivanja = v.DatumUtvrdjivanja,
                v.IzvorPodatka,
                StatusVlasnika = v.Status
            }).ToList();

            if (dataGridVlasnici.Rows.Count > 0 && dataGridVlasnici.Columns.Count > 0)
            {
                void K(string n, int w, string h, string? f = null)
                {
                    if (!dataGridVlasnici.Columns.Contains(n)) return;
                    dataGridVlasnici.Columns[n].Width = w;
                    dataGridVlasnici.Columns[n].HeaderText = h;
                    if (f != null) dataGridVlasnici.Columns[n].DefaultCellStyle.Format = f;
                }
                K("Id", 40, "ID"); K("ImePrezime", 180, "Ime i prezime");
                K("DatumValjanostiDokumenta", 140, "Datum važenja dok.", "dd.MM.yyyy");
                K("ProcenatVlasnistva", 100, "% vlasništva");
                K("DatumUtvrdjivanja", 130, "Datum utvrđivanja", "dd.MM.yyyy");
                K("IzvorPodatka", 150, "Izvor podatka"); K("StatusVlasnika", 90, "Status");
            }
            dataGridVlasnici.ClearSelection();
        }

        private void LoadDirektori(int klijentId)
        {
            var direktori = _db.Direktori.Where(d => d.KlijentId == klijentId).AsNoTracking().ToList();
            dataGridDirektori.DataSource = direktori.Select(d => new
            {
                d.Id,
                d.ImePrezime,
                DatumValjanostiDokumenta = d.DatumValjanosti,
                d.TipValjanosti,
                StatusDirektora = d.Status
            }).ToList();

            if (dataGridDirektori.Rows.Count > 0 && dataGridDirektori.Columns.Count > 0)
            {
                void K(string n, int w, string h, string? f = null)
                {
                    if (!dataGridDirektori.Columns.Contains(n)) return;
                    dataGridDirektori.Columns[n].Width = w;
                    dataGridDirektori.Columns[n].HeaderText = h;
                    if (f != null) dataGridDirektori.Columns[n].DefaultCellStyle.Format = f;
                }
                K("Id", 40, "ID"); K("ImePrezime", 200, "Ime i prezime");
                K("DatumValjanostiDokumenta", 140, "Datum važenja dok.", "dd.MM.yyyy");
                K("TipValjanosti", 120, "Tip valjanosti"); K("StatusDirektora", 90, "Status");
            }
            dataGridDirektori.ClearSelection();
        }

        

        private void dataGridKlijenti_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count > 0)
            {
                int id = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
                LoadVlasnici(id);
                LoadDirektori(id);
            }
        }

        private void ApplyCurrentFilters() =>
            LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra(), GetSelectedVelicina());

        

        private void btnDodajKlijent_Click(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajKlijent(null, db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadDjelatnostiFilter(); ApplyCurrentFilters(); OsvjeziUpozerenjaBadge();
            }
        }

        private void btnIzmijeniKlijent_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Odaberi firmu!"); return; }
            int id = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajKlijent(id, db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadDjelatnostiFilter(); ApplyCurrentFilters(); OsvjeziUpozerenjaBadge();
            }
        }

        private void btnObrisiKlijent_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Odaberi firmu!"); return; }

            if (MessageBox.Show(
                    "Firma će biti arhivirana i neće biti vidljiva.\n" +
                    "Podaci ostaju u bazi radi revizijskog traga.\n\nNastavi?",
                    "Potvrda", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            try
            {
                int id = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
                var klijent = _db.Klijenti.Find(id);
                if (klijent == null) return;

                _audit.SoftDelete(klijent, "Klijenti", id, $"Arhivirana firma: '{klijent.Naziv}'");
                klijent.Status = "ARHIVIRAN";
                _db.SaveChanges();

                MessageBox.Show("Firma je arhivirana.");
                LoadDjelatnostiFilter(); ApplyCurrentFilters(); OsvjeziUpozerenjaBadge();
            }
            catch (Exception ex) { MessageBox.Show($"Greška: {ex.Message}"); }
        }

        

        private void btnDodajVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Prvo odaberi firmu!"); return; }
            int klijentId = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajVlasnika(klijentId, null, db);
            if (forma.ShowDialog() == DialogResult.OK) LoadVlasnici(klijentId);
        }

        private void btnIzmijeniVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridVlasnici.SelectedRows.Count == 0) { MessageBox.Show("Odaberi vlasnika!"); return; }
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Odaberi firmu!"); return; }
            int vlasnikId = (int)dataGridVlasnici.SelectedRows[0].Cells["Id"].Value;
            int klijentId = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajVlasnika(klijentId, vlasnikId, db);
            if (forma.ShowDialog() == DialogResult.OK) LoadVlasnici(klijentId);
        }

        private void btnObrisiVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridVlasnici.SelectedRows.Count == 0) { MessageBox.Show("Odaberi vlasnika!"); return; }
            if (MessageBox.Show("Arhivirati vlasnika?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No) return;

            try
            {
                int vlasnikId = (int)dataGridVlasnici.SelectedRows[0].Cells["Id"].Value;
                var vlasnik = _db.Vlasnici.Find(vlasnikId);
                if (vlasnik == null) return;
                int klijentId = vlasnik.KlijentId;
                _audit.SoftDelete(vlasnik, "Vlasnici", vlasnikId, $"Arhiviran: '{vlasnik.ImePrezime}'");
                vlasnik.Status = "ARHIVIRAN";
                _db.SaveChanges();
                MessageBox.Show("Vlasnik arhiviran.");
                LoadVlasnici(klijentId);
            }
            catch (Exception ex) { MessageBox.Show($"Greška: {ex.Message}"); }
        }

        

        private void btnDodajDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Prvo odaberi firmu!"); return; }
            int klijentId = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajDirektora(klijentId, null, db);
            if (forma.ShowDialog() == DialogResult.OK) LoadDirektori(klijentId);
        }

        private void btnIzmijeniDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridDirektori.SelectedRows.Count == 0) { MessageBox.Show("Odaberi direktora!"); return; }
            if (dataGridKlijenti.SelectedRows.Count == 0) { MessageBox.Show("Odaberi firmu!"); return; }
            int direktorId = (int)dataGridDirektori.SelectedRows[0].Cells["Id"].Value;
            int klijentId = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
            using var db = DbContextFactory.Kreiraj();
            var forma = new FrmDodajDirektora(klijentId, direktorId, db);
            if (forma.ShowDialog() == DialogResult.OK) LoadDirektori(klijentId);
        }

        private void btnObrisiDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridDirektori.SelectedRows.Count == 0) { MessageBox.Show("Odaberi direktora!"); return; }
            if (MessageBox.Show("Arhivirati direktora?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No) return;

            try
            {
                int direktorId = (int)dataGridDirektori.SelectedRows[0].Cells["Id"].Value;
                var direktor = _db.Direktori.Find(direktorId);
                if (direktor == null) return;
                int klijentId = direktor.KlijentId;
                _audit.SoftDelete(direktor, "Direktori", direktorId, $"Arhiviran: '{direktor.ImePrezime}'");
                direktor.Status = "ARHIVIRAN";
                _db.SaveChanges();
                MessageBox.Show("Direktor arhiviran.");
                LoadDirektori(klijentId);
            }
            catch (Exception ex) { MessageBox.Show($"Greška: {ex.Message}"); }
        }

        

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

       

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx" };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            var helper = new ImportHelper(DbContextFactory.ConnectionString);
            helper.PokreniImport(dialog.FileName, this, () =>
            {
                LoadDjelatnostiFilter(); LoadKlijenti(); OsvjeziUpozerenjaBadge();
            });
        }

        private void btnResetImport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Ovo će obrisati SVE podatke i pokrenuti novi import.\n\nJesi li siguran?",
                    "RESET BAZE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            var dialog = new OpenFileDialog { Filter = "Excel Files (*.xlsx)|*.xlsx", Title = "Odaberi Excel fajl za reimport" };
            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                ResetujBazu();
                var helper = new ImportHelper(DbContextFactory.ConnectionString);
                helper.PokreniImport(dialog.FileName, this, () =>
                {
                    LoadDjelatnostiFilter(); LoadKlijenti(); OsvjeziUpozerenjaBadge();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri resetu baze:\n{ex.Message}", "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetujBazu()
        {
            string dbPath = DbContextFactory.DbPath;
            string backupPath = dbPath + $".backup_{DateTime.Now:yyyyMMdd_HHmmss}";

            try
            {
                File.Copy(dbPath, backupPath, overwrite: true);
            }
            catch (Exception ex)
            {
                if (MessageBox.Show(
                        $"Backup baze nije uspio:\n{ex.Message}\n\n" +
                        "Nastavi bez backupa? (Podaci mogu biti trajno izgubljeni!)",
                        "Backup nije uspio", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    == DialogResult.No)
                    throw new Exception("Reset otkazan — backup nije uspio.");
            }

            _db.Dispose();

            using var tmpDb = DbContextFactory.Kreiraj();
            using var tx = tmpDb.Database.BeginTransaction();
            try
            {
                tmpDb.Database.ExecuteSqlRaw("DELETE FROM AuditLogs");
                tmpDb.Database.ExecuteSqlRaw("DELETE FROM Ugovori");
                tmpDb.Database.ExecuteSqlRaw("DELETE FROM Vlasnici");
                tmpDb.Database.ExecuteSqlRaw("DELETE FROM Direktori");
                tmpDb.Database.ExecuteSqlRaw("DELETE FROM Klijenti");
                tmpDb.Database.ExecuteSqlRaw("DELETE FROM Djelatnosti");
                tmpDb.Database.ExecuteSqlRaw(
                    "DELETE FROM sqlite_sequence WHERE name IN " +
                    "('AuditLogs','Ugovori','Vlasnici','Direktori','Klijenti','Djelatnosti')");
                tx.Commit();
            }
            catch { tx.Rollback(); throw; }

            _db.Dispose();
            _db = DbContextFactory.Kreiraj();
            _db.Database.EnsureCreated();
            _audit = new AuditService(_db);
        }

       

        private void OsvjeziUpozerenjaBadge()
        {
            try
            {
                var danas = DateTime.Today;
                var granica = danas.AddDays(60);

                bool imaIsteklih = _db.Vlasnici.AsNoTracking()
                    .Any(v => v.DatumValjanostiDokumenta < danas && v.Status == "AKTIVAN")
                    || _db.Direktori.AsNoTracking()
                    .Any(d => d.DatumValjanosti < danas && d.TipValjanosti == "VREMENSKI" && d.Status == "AKTIVAN");

                int count = _db.Vlasnici.AsNoTracking()
                    .Count(v => v.DatumValjanostiDokumenta <= granica && v.Status == "AKTIVAN")
                    + _db.Direktori.AsNoTracking()
                    .Count(d => d.DatumValjanosti <= granica && d.TipValjanosti == "VREMENSKI" && d.Status == "AKTIVAN");

                if (count > 0)
                {
                    btnUpozorenja.Text = $"🔔 Upozorenja ({count})";
                    btnUpozorenja.BackColor = imaIsteklih ? Color.Firebrick : Color.FromArgb(220, 120, 20);
                    btnUpozorenja.ForeColor = Color.White;
                    btnUpozorenja.Font = new System.Drawing.Font(btnUpozorenja.Font, System.Drawing.FontStyle.Bold);
                }
                else
                {
                    btnUpozorenja.Text = "🔔 Upozorenja";
                    btnUpozorenja.BackColor = SystemColors.Control;
                    btnUpozorenja.ForeColor = SystemColors.ControlText;
                    btnUpozorenja.Font = new System.Drawing.Font(btnUpozorenja.Font, System.Drawing.FontStyle.Regular);
                }
            }
            catch (Exception ex) { Debug.WriteLine($"[BADGE] Greška: {ex.Message}"); }
        }

        private void btnUpozorenja_Click(object sender, EventArgs e) =>
            new FrmUpozorenja(_db).ShowDialog(this);

       

        private void btnExportTabelaPdf_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.Rows.Count == 0)
            { MessageBox.Show("Nema klijenata za export."); return; }

            
            var ids = dataGridKlijenti.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.DataBoundItem != null)
                .Select(r => (int)r.Cells["Id"].Value)
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
                var svc = new PdfExportService(_db);
                string path = svc.GenerirajTabeluKlijenata(ids, dlg.FileName);
                if (MessageBox.Show($"PDF je sačuvan:\n{path}\n\nŽeliš li ga otvoriti?",
                        "PDF kreiran", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
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

        private void btnSacuvajPdf_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0)
            { MessageBox.Show("Odaberi firmu iz liste prije eksporta."); return; }

            int klijentId; string naziv;
            try
            {
                dynamic row = dataGridKlijenti.SelectedRows[0].DataBoundItem;
                klijentId = row.Id; naziv = row.Naziv;
            }
            catch { MessageBox.Show("Nije moguće pročitati odabranu firmu."); return; }

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
            btnSacuvajPdf.Text = " Generišem...";
            Cursor = Cursors.WaitCursor;

            try
            {
                var svc = new PdfExportService(_db);
                string path = svc.GenerirajPdf(klijentId, dlg.FileName);
                if (MessageBox.Show($"PDF je sačuvan:\n{path}\n\nŽeliš li ga otvoriti?",
                        "PDF kreiran", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
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