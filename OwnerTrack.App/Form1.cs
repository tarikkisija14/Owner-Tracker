using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore;
using OpenXmlSheet = DocumentFormat.OpenXml.Spreadsheet.Sheet;
using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.App.ViewModels;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using System.Diagnostics;

namespace OwnerTrack.App
{
    public partial class Form1 : Form
    {
        private readonly System.Windows.Forms.Timer _searchTimer;

        public Form1()
        {
            InitializeComponent();

            _searchTimer = new System.Windows.Forms.Timer { Interval = UiConstants.SearchDebounceMs };
            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();
                LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra(), GetSelectedVelicina());
            };

            Load += Form1_Load;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                new SchemaManager(DbContextFactory.ConnectionString).ApplyMigrations();

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
                    "Greška pokretanja", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _searchTimer?.Dispose();
            base.OnFormClosed(e);
        }

        

        private void LoadVelicinaFilter()
        {
            cmbFilterVelicina.Items.Clear();
            cmbFilterVelicina.Items.Add(new { Value = UiConstants.FilterAllValue, Display = UiConstants.FilterAllDisplay });
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
                cmbFilterDjelatnost.Items.Add(new { Sifra = UiConstants.FilterAllValue, Naziv = UiConstants.FilterAllDjelatnostDisplay });
                foreach (var d in djelatnosti)
                    cmbFilterDjelatnost.Items.Add(new { d.Sifra, Naziv = $"{d.Sifra} - {d.Naziv}" });

                cmbFilterDjelatnost.DisplayMember = "Naziv";
                cmbFilterDjelatnost.ValueMember = "Sifra";
                cmbFilterDjelatnost.SelectedIndex = 0;
            }
            catch (Exception ex) { DialogHelper.LogirajIPokaziGresku(ex, "Greška pri učitavanju djelatnosti"); }
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

        private void ApplyCurrentFilters() =>
            LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra(), GetSelectedVelicina());

        private void txtSearchKlijent_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void cmbFilterDjelatnost_SelectedIndexChanged(object sender, EventArgs e) => ApplyCurrentFilters();
        private void cmbFilterVelicina_SelectedIndexChanged(object sender, EventArgs e) => ApplyCurrentFilters();

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            txtSearchKlijent.Text = "";
            cmbFilterDjelatnost.SelectedIndex = 0;
            cmbFilterVelicina.SelectedIndex = 0;
            LoadKlijenti();
        }

       

        private void LoadKlijenti(string filter = "", string sifraDjelatnosti = "", string velicina = "")
        {
            try
            {
                using var db = DbContextFactory.Kreiraj();

                var klijenti = db.Klijenti
                    .Where(k =>
                        (string.IsNullOrWhiteSpace(filter) ||
                         k.Naziv.ToLower().Contains(filter.ToLower()) ||
                         k.IdBroj.ToLower().Contains(filter.ToLower()))
                        && (string.IsNullOrWhiteSpace(sifraDjelatnosti) || k.SifraDjelatnosti == sifraDjelatnosti)
                        && (string.IsNullOrWhiteSpace(velicina) || k.Velicina == velicina))
                    .AsNoTracking()
                    .Select(k => new KlijentViewModel
                    {
                        Id = k.Id,
                        Naziv = k.Naziv,
                        IdBroj = k.IdBroj,
                        Adresa = k.Adresa,
                        SifraDjelatnosti = k.SifraDjelatnosti,
                        Djelatnost = k.Djelatnost != null ? k.Djelatnost.Naziv : "",
                        DatumUspostaveOdnosa = k.DatumUspostave,
                        VrstaKlijenta = k.VrstaKlijenta != null ? k.VrstaKlijenta.ToString() : null,
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
                        StatusKlijenta = k.Status.ToString(),
                        Napomena = k.Napomena,
                    })
                    .ToList();

                GridHelper.BindBezEventa(dataGridKlijenti, dataGridKlijenti_SelectionChanged, klijenti);
                GridHelper.PostaviKolone(dataGridKlijenti, GridColumns.Klijenti);
            }
            catch (Exception ex) { DialogHelper.LogirajIPokaziGresku(ex, "Greška pri učitavanju klijenata"); }
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
                        StatusVlasnika = v.Status.ToString(),
                    })
                    .ToList();

                dataGridVlasnici.DataSource = vlasnici;
                GridHelper.PostaviKolone(dataGridVlasnici, GridColumns.Vlasnici);
                dataGridVlasnici.ClearSelection();
            }
            catch (Exception ex) { DialogHelper.LogirajIPokaziGresku(ex, "Greška pri učitavanju vlasnika"); }
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
                        StatusDirektora = d.Status.ToString(),
                    })
                    .ToList();

                dataGridDirektori.DataSource = direktori;
                GridHelper.PostaviKolone(dataGridDirektori, GridColumns.Direktori);
                dataGridDirektori.ClearSelection();
            }
            catch (Exception ex) { DialogHelper.LogirajIPokaziGresku(ex, "Greška pri učitavanju direktora"); }
        }

        private void dataGridKlijenti_SelectionChanged(object sender, EventArgs e)
        {
            if (!GridHelper.PokupiOdabraniId(dataGridKlijenti, out int id)) return;
            LoadVlasnici(id);
            LoadDirektori(id);
        }

       

        private void btnDodajKlijent_Click(object sender, EventArgs e)
        {
            using var db = DbContextFactory.Kreiraj();
            if (new FrmDodajKlijent(null, db).ShowDialog() == DialogResult.OK)
                OsveziNakonIzmjene();
        }

        private void btnIzmijeniKlijent_Click(object sender, EventArgs e)
        {
            if (!GridHelper.PokupiOdabraniId(dataGridKlijenti, out int id, "Odaberi firmu!")) return;

            using var db = DbContextFactory.Kreiraj();
            if (new FrmDodajKlijent(id, db).ShowDialog() == DialogResult.OK)
                OsveziNakonIzmjene();
        }

        private void btnObrisiKlijent_Click(object sender, EventArgs e)
        {
            if (!GridHelper.PokupiOdabraniId(dataGridKlijenti, out int id, "Odaberi firmu!")) return;
            if (!DialogHelper.PotvrdiArhiviranje("Firma će biti arhivirana i neće biti vidljiva.")) return;

            IzvrsiArhiviranje(id, db =>
            {
                var k = db.Klijenti.Find(id);
                if (k == null) return;
                new AuditService(db).Arhiviraj(k, "Klijenti", id, $"Arhivirana firma: '{k.Naziv}'");
                db.SaveChanges();
            },
            nakonUspjeha: OsveziNakonIzmjene,
            successMessage: "Firma je arhivirana.");
        }

       

        private void btnDodajVlasnika_Click(object sender, EventArgs e)
        {
            if (!GridHelper.PokupiOdabraniId(dataGridKlijenti, out int klijentId, "Prvo odaberi firmu!")) return;

            using var db = DbContextFactory.Kreiraj();
            if (new FrmDodajVlasnika(klijentId, null, db).ShowDialog() == DialogResult.OK)
                LoadVlasnici(klijentId);
        }

        private void btnIzmijeniVlasnika_Click(object sender, EventArgs e)
        {
            if (!GridHelper.PokupiOdabraniId(dataGridVlasnici, out int vlasnikId, "Odaberi vlasnika!")) return;
            if (!GridHelper.PokupiOdabraniId(dataGridKlijenti, out int klijentId, "Odaberi firmu!")) return;

            using var db = DbContextFactory.Kreiraj();
            if (new FrmDodajVlasnika(klijentId, vlasnikId, db).ShowDialog() == DialogResult.OK)
                LoadVlasnici(klijentId);
        }

        private void btnObrisiVlasnika_Click(object sender, EventArgs e)
        {
            if (!GridHelper.PokupiOdabraniId(dataGridVlasnici, out int vlasnikId, "Odaberi vlasnika!")) return;
            if (!GridHelper.PokupiOdabraniId(dataGridKlijenti, out int klijentId, "Odaberi firmu!")) return;
            if (!DialogHelper.PotvrdiArhiviranje("Arhivirati vlasnika?")) return;

            IzvrsiArhiviranje(vlasnikId, db =>
            {
                var v = db.Vlasnici.Find(vlasnikId);
                if (v == null) return;
                new AuditService(db).Arhiviraj(v, "Vlasnici", vlasnikId, $"Arhiviran: '{v.ImePrezime}'");
                db.SaveChanges();
            },
            nakonUspjeha: () => LoadVlasnici(klijentId),
            successMessage: "Vlasnik arhiviran.");
        }

        

        private void btnDodajDirektora_Click(object sender, EventArgs e)
        {
            if (!GridHelper.PokupiOdabraniId(dataGridKlijenti, out int klijentId, "Prvo odaberi firmu!")) return;

            using var db = DbContextFactory.Kreiraj();
            if (new FrmDodajDirektora(klijentId, null, db).ShowDialog() == DialogResult.OK)
                LoadDirektori(klijentId);
        }

        private void btnIzmijeniDirektora_Click(object sender, EventArgs e)
        {
            if (!GridHelper.PokupiOdabraniId(dataGridDirektori, out int direktorId, "Odaberi direktora!")) return;
            if (!GridHelper.PokupiOdabraniId(dataGridKlijenti, out int klijentId, "Odaberi firmu!")) return;

            using var db = DbContextFactory.Kreiraj();
            if (new FrmDodajDirektora(klijentId, direktorId, db).ShowDialog() == DialogResult.OK)
                LoadDirektori(klijentId);
        }

        private void btnObrisiDirektora_Click(object sender, EventArgs e)
        {
            if (!GridHelper.PokupiOdabraniId(dataGridDirektori, out int direktorId, "Odaberi direktora!")) return;
            if (!GridHelper.PokupiOdabraniId(dataGridKlijenti, out int klijentId, "Odaberi firmu!")) return;
            if (!DialogHelper.PotvrdiArhiviranje("Arhivirati direktora?")) return;

            IzvrsiArhiviranje(direktorId, db =>
            {
                var d = db.Direktori.Find(direktorId);
                if (d == null) return;
                new AuditService(db).Arhiviraj(d, "Direktori", direktorId, $"Arhiviran: '{d.ImePrezime}'");
                db.SaveChanges();
            },
            nakonUspjeha: () => LoadDirektori(klijentId),
            successMessage: "Direktor arhiviran.");
        }

       

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            using var dialog = DialogHelper.KreirajOpenDialogExcel();
            if (dialog.ShowDialog() != DialogResult.OK) return;

            new ImportHelper(DbContextFactory.ConnectionString)
                .PokreniImport(dialog.FileName, this, OsveziNakonIzmjene);
        }

        private void btnResetImport_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Ovo će obrisati SVE podatke i pokrenuti novi import.\n\nJesi li siguran?",
                    "RESET BAZE", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            using var dialog = DialogHelper.KreirajOpenDialogExcel("Odaberi Excel fajl za reimport");
            if (dialog.ShowDialog() != DialogResult.OK) return;
            if (!File.Exists(dialog.FileName))
            {
                MessageBox.Show("Odabrani fajl nije pronađen.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!ValidirajExcelFajl(dialog.FileName)) return;

            try
            {
                var dbService = new DatabaseService(DbContextFactory.DbPath, DbContextFactory.ConnectionString);
                string backupPath = dbService.ResetirajBazu();

                new ImportHelper(DbContextFactory.ConnectionString).PokreniImport(
                    dialog.FileName, this,
                    onZavrsetak: OsveziNakonIzmjene,
                    onGreska: () => PonudiVracanjeBackupa(dbService, backupPath));
            }
            catch (Exception ex)
            {
                DialogHelper.LogirajIPokaziGresku(ex, "Greška pri resetu baze");
            }
        }

        private static bool ValidirajExcelFajl(string filePath)
        {
            try
            {
                using var testDoc = SpreadsheetDocument.Open(filePath, false);
                var wbPart = testDoc.WorkbookPart ?? throw new Exception("Fajl nema validan WorkbookPart.");
                _ = wbPart.Workbook.Sheets?.Cast<OpenXmlSheet>()
                      .FirstOrDefault(s => s.Name?.Value?.Contains(UiConstants.ExcelZirvanaKeyword) == true)
                    ?? throw new Exception($"Fajl ne sadrži list sa '{UiConstants.ExcelZirvanaKeyword}'.");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Odabrani fajl nije validan:\n\n{ex.Message}",
                    "Greška validacije", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void PonudiVracanjeBackupa(DatabaseService dbService, string backupPath)
        {
            if (string.IsNullOrEmpty(backupPath)) return;

            var odgovor = MessageBox.Show(
                $"Import nije uspio, a baza je već bila obrisana.\n\n" +
                $"Hoćeš li vratiti podatke iz backupa?\n\nBackup: {backupPath}",
                "Vraćanje podataka", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (odgovor != DialogResult.Yes)
            {
                MessageBox.Show(
                    $"Backup ostaje sačuvan na:\n{backupPath}\n\nMožeš ga ručno vratiti ako zatreba.",
                    "Backup sačuvan", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                dbService.VratiBackup(backupPath);
                OsveziNakonIzmjene();
                MessageBox.Show("Podaci su uspješno vraćeni iz backupa.", "Vraćanje uspješno",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Program.LogException(ex);
                MessageBox.Show(
                    $"Vraćanje nije uspjelo automatski.\n\n" +
                    $"Ručno kopiraj ovaj fajl:\n{backupPath}\n\ni preimenuj ga u 'Firme.db' na istoj lokaciji.",
                    "Ručno vraćanje", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void OsvjeziUpozerenjaBadge()
        {
            try
            {
                using var db = DbContextFactory.Kreiraj();
                var stats = new WarningQueryService(db).DohvatiStats();
                PostaviBadge(BadgeState.FromStats(stats.Count, stats.ImaIsteklih));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[BADGE] Greška: {ex.Message}");
                Program.LogException(ex);
            }
        }

        private void PostaviBadge(BadgeState state)
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
            using var db = DbContextFactory.Kreiraj();
            new FrmUpozorenja(db).ShowDialog(this);
        }

        

        private async void btnExportTabelaPdf_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.Rows.Count == 0) { MessageBox.Show("Nema klijenata za export."); return; }

            var ids = dataGridKlijenti.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.DataBoundItem != null)
                .Select(r => r.Cells["Id"].Value is int id ? id : 0)
                .Where(id => id > 0)
                .ToList();

            using var dlg = DialogHelper.KreirajSaveDialogPdf(
                "Sačuvaj tabelu klijenata",
                $"Klijenti_tabela_{DateTime.Now:yyyyMMdd}.pdf");
            if (dlg.ShowDialog() != DialogResult.OK) return;

            string savedPath = dlg.FileName;
            await DialogHelper.IzvrsiPdfExport(
                btnExportTabelaPdf, "📋 Export tabele u PDF",
                path =>
                {
                    using var db = DbContextFactory.Kreiraj();
                    return new PdfExportService(db).GenerirajTabeluKlijenata(ids, path);
                },
                savedPath);
        }

        private async void btnSacuvajPdf_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0)
            { MessageBox.Show("Odaberi firmu iz liste."); return; }

            if (dataGridKlijenti.SelectedRows[0].DataBoundItem is not KlijentViewModel row)
            { MessageBox.Show("Nije moguće pročitati odabranu firmu."); return; }

            using var dlg = DialogHelper.KreirajSaveDialogPdf(
                "Sačuvaj izvještaj firme",
                DialogHelper.SigurnoIme(row.Naziv ?? ""));
            if (dlg.ShowDialog() != DialogResult.OK) return;

            string savedPath = dlg.FileName;
            await DialogHelper.IzvrsiPdfExport(
                btnSacuvajPdf, "📄 Sačuvaj kao PDF",
                path =>
                {
                    using var db = DbContextFactory.Kreiraj();
                    return new PdfExportService(db).GenerirajPdf(row.Id, path);
                },
                savedPath);
        }

       

        private void OsveziNakonIzmjene()
        {
            LoadDjelatnostiFilter();
            LoadKlijenti();
            OsvjeziUpozerenjaBadge();
        }

        private void IzvrsiArhiviranje(
            int id,
            Action<OwnerTrackDbContext> akcija,
            Action? nakonUspjeha = null,
            string successMessage = "")
        {
            try
            {
                using var db = DbContextFactory.Kreiraj();
                TransactionHelper.Execute(db, akcija);
                if (!string.IsNullOrEmpty(successMessage))
                    MessageBox.Show(successMessage);
                nakonUspjeha?.Invoke();
            }
            catch (Exception ex) { DialogHelper.LogirajIPokaziGresku(ex); }
        }
    }
}