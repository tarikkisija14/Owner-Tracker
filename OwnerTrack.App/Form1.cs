using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Infrastructure;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class Form1 : Form
    {
        private OwnerTrackDbContext _db;
        private string _dbPath;     
        private string _connString;  

        
        private System.Windows.Forms.Timer _searchTimer;




        public Form1()
        {
            InitializeComponent();

            
            _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Firme.db");
            _connString = $"Data Source={_dbPath}";

           
            var schema = new OwnerTrack.Infrastructure.SchemaManager(_connString);
            schema.ApplyMigrations();

            _db = KreirajDbContext();

           
            _db.Database.EnsureCreated();

            
            _searchTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _searchTimer.Tick += (s, e) =>
            {
                _searchTimer.Stop();
                LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra());
            };

            LoadDjelatnostiFilter();
            LoadKlijenti();
            OsvjeziUpozerenjaBadge();
        }
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _db?.Dispose();
            _searchTimer?.Dispose();
            base.OnFormClosed(e);
        }

        
        private OwnerTrackDbContext KreirajDbContext()
        {
            var options = new DbContextOptionsBuilder<OwnerTrackDbContext>()
                .UseSqlite(_connString)
                .Options;
            return new OwnerTrackDbContext(options);
        }

        // ========== UČITAVANJE PODATAKA ==========

        private void LoadDjelatnostiFilter()
        {
            try
            {
                var djelatnosti = _db.Djelatnosti
                    .OrderBy(d => d.Naziv)
                    .ToList();

                cmbFilterDjelatnost.Items.Clear();
                cmbFilterDjelatnost.Items.Add(new { Sifra = "", Naziv = "-- Sve djelatnosti --" });

                foreach (var d in djelatnosti)
                {
                    cmbFilterDjelatnost.Items.Add(new { d.Sifra, Naziv = $"{d.Sifra} - {d.Naziv}" });
                }

                cmbFilterDjelatnost.DisplayMember = "Naziv";
                cmbFilterDjelatnost.ValueMember = "Sifra";
                cmbFilterDjelatnost.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju djelatnosti: {ex.Message}");
            }
        }

        private void LoadKlijenti(string filter = "", string sifraDjelatnosti = "")
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
                    )
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

                if (dataGridKlijenti.Columns.Count > 0)
                {
                    dataGridKlijenti.Columns["Id"].Width = 40;
                    dataGridKlijenti.Columns["Id"].HeaderText = "ID";

                    dataGridKlijenti.Columns["Naziv"].Width = 220;
                    dataGridKlijenti.Columns["Naziv"].HeaderText = "Naziv preduzeća";

                    dataGridKlijenti.Columns["IdBroj"].Width = 130;
                    dataGridKlijenti.Columns["IdBroj"].HeaderText = "ID broj";

                    dataGridKlijenti.Columns["Adresa"].Width = 200;
                    dataGridKlijenti.Columns["Adresa"].HeaderText = "Adresa";

                    dataGridKlijenti.Columns["SifraDjelatnosti"].Width = 70;
                    dataGridKlijenti.Columns["SifraDjelatnosti"].HeaderText = "Šifra";

                    dataGridKlijenti.Columns["Djelatnost"].Width = 220;
                    dataGridKlijenti.Columns["Djelatnost"].HeaderText = "Djelatnost";

                    dataGridKlijenti.Columns["DatumUspostaveOdnosa"].Width = 120;
                    dataGridKlijenti.Columns["DatumUspostaveOdnosa"].HeaderText = "Datum uspostave odnosa";
                    dataGridKlijenti.Columns["DatumUspostaveOdnosa"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    dataGridKlijenti.Columns["VrstaKlijenta"].Width = 110;
                    dataGridKlijenti.Columns["VrstaKlijenta"].HeaderText = "Vrsta klijenta";

                    dataGridKlijenti.Columns["DatumOsnivanjaFirme"].Width = 120;
                    dataGridKlijenti.Columns["DatumOsnivanjaFirme"].HeaderText = "Datum osnivanja";
                    dataGridKlijenti.Columns["DatumOsnivanjaFirme"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    dataGridKlijenti.Columns["Velicina"].Width = 80;
                    dataGridKlijenti.Columns["Velicina"].HeaderText = "Veličina";

                    // RIZICI
                    dataGridKlijenti.Columns["PepRizik"].Width = 70;
                    dataGridKlijenti.Columns["PepRizik"].HeaderText = "PEP rizik";

                    dataGridKlijenti.Columns["UboRizik"].Width = 70;
                    dataGridKlijenti.Columns["UboRizik"].HeaderText = "UBO rizik";

                    dataGridKlijenti.Columns["GotovinaRizik"].Width = 90;
                    dataGridKlijenti.Columns["GotovinaRizik"].HeaderText = "Gotovina rizik";

                    dataGridKlijenti.Columns["GeografskiRizik"].Width = 100;
                    dataGridKlijenti.Columns["GeografskiRizik"].HeaderText = "Geografski rizik";

                    dataGridKlijenti.Columns["UkupnaProcjena"].Width = 120;
                    dataGridKlijenti.Columns["UkupnaProcjena"].HeaderText = "Ukupna procjena";

                    dataGridKlijenti.Columns["DatumProcjeneRizika"].Width = 120;
                    dataGridKlijenti.Columns["DatumProcjeneRizika"].HeaderText = "Datum procjene rizika";
                    dataGridKlijenti.Columns["DatumProcjeneRizika"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    dataGridKlijenti.Columns["OvjeraCr"].Width = 150;
                    dataGridKlijenti.Columns["OvjeraCr"].HeaderText = "Ovjera/CR";

                    // UGOVOR
                    dataGridKlijenti.Columns["StatusUgovora"].Width = 110;
                    dataGridKlijenti.Columns["StatusUgovora"].HeaderText = "Status ugovora";

                    dataGridKlijenti.Columns["DatumPotpisaUgovora"].Width = 120;
                    dataGridKlijenti.Columns["DatumPotpisaUgovora"].HeaderText = "Datum potpisa ugovora";
                    dataGridKlijenti.Columns["DatumPotpisaUgovora"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    // BROJAČI
                    dataGridKlijenti.Columns["BrojVlasnika"].Width = 80;
                    dataGridKlijenti.Columns["BrojVlasnika"].HeaderText = "Vlasnici";

                    dataGridKlijenti.Columns["BrojDirektora"].Width = 80;
                    dataGridKlijenti.Columns["BrojDirektora"].HeaderText = "Direktori";

                    dataGridKlijenti.Columns["StatusKlijenta"].Width = 90;
                    dataGridKlijenti.Columns["StatusKlijenta"].HeaderText = "Status klijenta";

                    dataGridKlijenti.Columns["Napomena"].Width = 200;
                    dataGridKlijenti.Columns["Napomena"].HeaderText = "Napomena";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        private void LoadVlasnici(int klijentId)
        {
           
            var vlasnici = _db.Vlasnici
                .Where(v => v.KlijentId == klijentId)
                .AsNoTracking()
                .ToList();

            dataGridVlasnici.DataSource = vlasnici.Select(v => new
            {
                v.Id,
                v.ImePrezime,
                DatumValjanostiDokumenta = v.DatumValjanostiDokumenta,
                ProcenatVlasnistva = v.ProcetatVlasnistva,
                DatumUtvrdjivanja = v.DatumUtvrdjivanja,
                v.IzvorPodatka,
                StatusVlasnika = v.Status
            }).ToList();

            if (dataGridVlasnici.Rows.Count > 0)
            {

                if (dataGridVlasnici.Columns.Count > 0)
                {
                    dataGridVlasnici.Columns["Id"].Width = 40;
                    dataGridVlasnici.Columns["Id"].HeaderText = "ID";

                    dataGridVlasnici.Columns["ImePrezime"].Width = 180;
                    dataGridVlasnici.Columns["ImePrezime"].HeaderText = "Ime i prezime";

                    dataGridVlasnici.Columns["DatumValjanostiDokumenta"].Width = 140;
                    dataGridVlasnici.Columns["DatumValjanostiDokumenta"].HeaderText = "Datum važenja dok.";
                    dataGridVlasnici.Columns["DatumValjanostiDokumenta"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    dataGridVlasnici.Columns["ProcenatVlasnistva"].Width = 100;
                    dataGridVlasnici.Columns["ProcenatVlasnistva"].HeaderText = "% vlasništva";

                    dataGridVlasnici.Columns["DatumUtvrdjivanja"].Width = 130;
                    dataGridVlasnici.Columns["DatumUtvrdjivanja"].HeaderText = "Datum utvrđivanja";
                    dataGridVlasnici.Columns["DatumUtvrdjivanja"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    dataGridVlasnici.Columns["IzvorPodatka"].Width = 150;
                    dataGridVlasnici.Columns["IzvorPodatka"].HeaderText = "Izvor podatka";

                    dataGridVlasnici.Columns["StatusVlasnika"].Width = 90;
                    dataGridVlasnici.Columns["StatusVlasnika"].HeaderText = "Status";
                }

                dataGridVlasnici.ClearSelection();
            }
        }


        private void LoadDirektori(int klijentId)
        {
           
            var direktori = _db.Direktori
                .Where(d => d.KlijentId == klijentId)
                .AsNoTracking()
                .ToList();

            dataGridDirektori.DataSource = direktori.Select(d => new
            {
                d.Id,
                d.ImePrezime,
                DatumValjanostiDokumenta = d.DatumValjanosti,
                d.TipValjanosti,
                StatusDirektora = d.Status
            }).ToList();

            if (dataGridDirektori.Rows.Count > 0)
            {

                if (dataGridDirektori.Columns.Count > 0)
                {
                    dataGridDirektori.Columns["Id"].Width = 40;
                    dataGridDirektori.Columns["Id"].HeaderText = "ID";

                    dataGridDirektori.Columns["ImePrezime"].Width = 200;
                    dataGridDirektori.Columns["ImePrezime"].HeaderText = "Ime i prezime";

                    dataGridDirektori.Columns["DatumValjanostiDokumenta"].Width = 140;
                    dataGridDirektori.Columns["DatumValjanostiDokumenta"].HeaderText = "Datum važenja dok.";
                    dataGridDirektori.Columns["DatumValjanostiDokumenta"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    dataGridDirektori.Columns["TipValjanosti"].Width = 120;
                    dataGridDirektori.Columns["TipValjanosti"].HeaderText = "Tip valjanosti";

                    dataGridDirektori.Columns["StatusDirektora"].Width = 90;
                    dataGridDirektori.Columns["StatusDirektora"].HeaderText = "Status";
                }

                dataGridDirektori.ClearSelection();
            }
        }

        // ========== SELECTION ==========

        private void dataGridKlijenti_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count > 0)
            {
                int id = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
                LoadVlasnici(id);
                LoadDirektori(id);
            }
        }

        // ========== KLIJENTI - DODAJ, IZMIJENI, OBRIŠI ==========

        private void ApplyCurrentFilters()
        {
            LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra());
        }

        private void btnDodajKlijent_Click(object sender, EventArgs e)
        {
            FrmDodajKlijent forma = new FrmDodajKlijent(null, _db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadDjelatnostiFilter();
                ApplyCurrentFilters();
                OsvjeziUpozerenjaBadge();
            }
        }

        private void btnIzmijeniKlijent_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi firmu!");
                return;
            }

            int id = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
            FrmDodajKlijent forma = new FrmDodajKlijent(id, _db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadDjelatnostiFilter();
                ApplyCurrentFilters();
                OsvjeziUpozerenjaBadge();
            }
        }

        private void btnObrisiKlijent_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi firmu!");
                return;
            }

            if (MessageBox.Show("Obrisati firmu i sve podatke?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                int id = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
                var klijent = _db.Klijenti.Find(id);
                if (klijent != null)
                {
                    _db.Klijenti.Remove(klijent);
                    _db.SaveChanges();
                    MessageBox.Show("Obrisano!");
                    
                    LoadDjelatnostiFilter();
                    ApplyCurrentFilters();
                    OsvjeziUpozerenjaBadge();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        // ========== VLASNICI - DODAJ, IZMIJENI, OBRIŠI ==========

        private void btnDodajVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prvo odaberi firmu!");
                return;
            }

            int klijentId = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
            FrmDodajVlasnika forma = new FrmDodajVlasnika(klijentId, null, _db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadVlasnici(klijentId);
            }
        }

        private void btnIzmijeniVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridVlasnici.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi vlasnika!");
                return;
            }

            
            if (dataGridKlijenti.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi firmu!");
                return;
            }

            int vlasnikId = (int)dataGridVlasnici.SelectedRows[0].Cells["Id"].Value;
            int klijentId = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;

            FrmDodajVlasnika forma = new FrmDodajVlasnika(klijentId, vlasnikId, _db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadVlasnici(klijentId);
            }
        }

        private void btnObrisiVlasnika_Click(object sender, EventArgs e)
        {
            if (dataGridVlasnici.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi vlasnika!");
                return;
            }

            if (MessageBox.Show("Obrisati vlasnika?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                int vlasnikId = (int)dataGridVlasnici.SelectedRows[0].Cells["Id"].Value;
                var vlasnik = _db.Vlasnici.Find(vlasnikId);
                if (vlasnik != null)
                {
                    int klijentId = vlasnik.KlijentId;
                    _db.Vlasnici.Remove(vlasnik);
                    _db.SaveChanges();
                    MessageBox.Show("Obrisano!");
                    LoadVlasnici(klijentId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        // ========== DIREKTORI - DODAJ, IZMIJENI, OBRIŠI ==========

        private void btnDodajDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridKlijenti.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prvo odaberi firmu!");
                return;
            }

            int klijentId = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;
            FrmDodajDirektora forma = new FrmDodajDirektora(klijentId, null, _db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadDirektori(klijentId);
            }
        }

        private void btnIzmijeniDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridDirektori.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi direktora!");
                return;
            }

            
            if (dataGridKlijenti.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi firmu!");
                return;
            }

            int direktorId = (int)dataGridDirektori.SelectedRows[0].Cells["Id"].Value;
            int klijentId = (int)dataGridKlijenti.SelectedRows[0].Cells["Id"].Value;

            FrmDodajDirektora forma = new FrmDodajDirektora(klijentId, direktorId, _db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadDirektori(klijentId);
            }
        }

        private void btnObrisiDirektora_Click(object sender, EventArgs e)
        {
            if (dataGridDirektori.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberi direktora!");
                return;
            }

            if (MessageBox.Show("Obrisati direktora?", "Potvrda", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            try
            {
                int direktorId = (int)dataGridDirektori.SelectedRows[0].Cells["Id"].Value;
                var direktor = _db.Direktori.Find(direktorId);
                if (direktor != null)
                {
                    int klijentId = direktor.KlijentId;
                    _db.Direktori.Remove(direktor);
                    _db.SaveChanges();
                    MessageBox.Show("Obrisano!");
                    LoadDirektori(klijentId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }

        // ========== SEARCH ==========

        private string GetSelectedDjelatnostSifra()
        {
            if (cmbFilterDjelatnost.SelectedItem == null) return "";
            dynamic item = cmbFilterDjelatnost.SelectedItem;
            return item.Sifra ?? "";
        }

        private void txtSearchKlijent_TextChanged(object sender, EventArgs e)
        {
           
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void cmbFilterDjelatnost_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadKlijenti(txtSearchKlijent.Text, GetSelectedDjelatnostSifra());
        }

        private void btnResetFilters_Click(object sender, EventArgs e)
        {
            txtSearchKlijent.Text = "";
            cmbFilterDjelatnost.SelectedIndex = 0;
            LoadKlijenti();
        }

        // ========== EXCEL IMPORT ==========

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files (*.xlsx)|*.xlsx";

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                using (var progressForm = new Form())
                {
                    progressForm.Text = "Import u toku...";
                    progressForm.Width = 500;
                    progressForm.Height = 200;
                    progressForm.StartPosition = FormStartPosition.CenterParent;
                    progressForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    progressForm.MaximizeBox = false;
                    progressForm.MinimizeBox = false;

                    var progressBar = new ProgressBar
                    {
                        Location = new System.Drawing.Point(20, 20),
                        Width = 440,
                        Height = 30,
                        Style = ProgressBarStyle.Continuous
                    };

                    var lblStatus = new Label
                    {
                        Location = new System.Drawing.Point(20, 60),
                        Width = 440,
                        Height = 60,
                        Text = "Priprema..."
                    };

                    var btnCancel = new Button
                    {
                        Text = "Zatvori",
                        Location = new System.Drawing.Point(200, 130),
                        Enabled = false
                    };
                    btnCancel.Click += (s, args) => progressForm.Close();

                    progressForm.Controls.Add(progressBar);
                    progressForm.Controls.Add(lblStatus);
                    progressForm.Controls.Add(btnCancel);


                   
                    var progress = new Progress<ImportProgress>(p =>
                    {
                        if (progressForm.IsHandleCreated)
                            progressForm.Invoke((Action)(() => {
                                progressBar.Maximum = p.TotalRows;
                                progressBar.Value = p.ProcessedRows;
                                lblStatus.Text = $"Obrađeno: {p.ProcessedRows}/{p.TotalRows}\n" +
                                                $"Dodato: {p.SuccessCount} | Greške: {p.ErrorCount}\n" +
                                                $"{p.CurrentRow}";
                                progressForm.Refresh();
                            }));
                    });


                    progressForm.Shown += async (s, args) =>
                    {
                        try
                        {
                            
                            var result = await System.Threading.Tasks.Task.Run(() =>
                            {
                                var bgOptions = new DbContextOptionsBuilder<OwnerTrackDbContext>()
                                    .UseSqlite(_connString)
                                    .Options;
                                using var bgDb = new OwnerTrackDbContext(bgOptions);
                                var importService = new ExcelImportService(bgDb);
                                return importService.ImportFromExcel(dialog.FileName, progress);
                            });

                            btnCancel.Enabled = true;
                            lblStatus.Text = $"Import završen!\n" +
                                           $"Dodano: {result.SuccessCount}\n" +
                                           $"Greške: {result.ErrorCount}\n" +
                                           $"Vlasnici: {result.VlasnikCount}";

                            if (result.Errors.Count > 0)
                            {
                                string errors = string.Join("\n", result.Errors.Take(10));
                                MessageBox.Show($"Greške tokom importa:\n{errors}", "Upozorenje",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            LoadDjelatnostiFilter();
                            LoadKlijenti();
                            OsvjeziUpozerenjaBadge();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Greška: {ex.Message}", "Greška",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            progressForm.Close();
                        }
                    };

                    progressForm.ShowDialog(this);
                }

            }
        }

        private void btnResetImport_Click(object sender, EventArgs e)
        {
            var potvrda = MessageBox.Show(
               "Ovo će obrisati SVE podatke iz baze (klijente, vlasnike, direktore, ugovore, djelatnosti) " +
               "i zatim pokrenuti novi import iz Excel fajla.\n\nJesi li siguran?",
               "RESET BAZE",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning);

            if (potvrda != DialogResult.Yes) return;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
            dialog.Title = "Odaberi Excel fajl za reimport";

            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                ResetujBazu();
                MessageBox.Show("Baza uspješno resetovana! Pokrećem import...", "Reset OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PokreniImport(dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri resetu baze:\n{ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResetujBazu()
        {
            
            try
            {
                string backupPath = _dbPath + $".backup_{DateTime.Now:yyyyMMdd_HHmmss}";
                System.IO.File.Copy(_dbPath, backupPath, overwrite: true);
                System.Diagnostics.Debug.WriteLine($"[BACKUP] Kreiran backup: {backupPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BACKUP] Backup nije uspio: {ex.Message}");
                
            }

            using var transaction = _db.Database.BeginTransaction();
            try
            {
                _db.Database.ExecuteSqlRaw("DELETE FROM Ugovori");
                _db.Database.ExecuteSqlRaw("DELETE FROM Vlasnici");
                _db.Database.ExecuteSqlRaw("DELETE FROM Direktori");
                _db.Database.ExecuteSqlRaw("DELETE FROM Klijenti");
                _db.Database.ExecuteSqlRaw("DELETE FROM Djelatnosti");
                _db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name IN ('Ugovori','Vlasnici','Direktori','Klijenti','Djelatnosti')");

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

           
            _db.Dispose();
            _db = KreirajDbContext();
        }
        private void PokreniImport(string filePath)
        {
            using (var progressForm = new Form())
            {
                progressForm.Text = "Import u toku...";
                progressForm.Width = 500;
                progressForm.Height = 200;
                progressForm.StartPosition = FormStartPosition.CenterParent;
                progressForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                progressForm.MaximizeBox = false;
                progressForm.MinimizeBox = false;

                var progressBar = new ProgressBar
                {
                    Location = new System.Drawing.Point(20, 20),
                    Width = 440,
                    Height = 30,
                    Style = ProgressBarStyle.Continuous
                };

                var lblStatus = new Label
                {
                    Location = new System.Drawing.Point(20, 60),
                    Width = 440,
                    Height = 60,
                    Text = "Priprema..."
                };

                var btnCancel = new Button
                {
                    Text = "Zatvori",
                    Location = new System.Drawing.Point(200, 130),
                    Enabled = false
                };
                btnCancel.Click += (s, args) => progressForm.Close();

                progressForm.Controls.Add(progressBar);
                progressForm.Controls.Add(lblStatus);
                progressForm.Controls.Add(btnCancel);

                
                var progress = new Progress<ImportProgress>(p =>
                {
                    if (progressForm.IsHandleCreated)
                        progressForm.Invoke((Action)(() => {
                            progressBar.Maximum = p.TotalRows;
                            progressBar.Value = p.ProcessedRows;
                            lblStatus.Text = $"Obrađeno: {p.ProcessedRows}/{p.TotalRows}\n" +
                                             $"Dodato: {p.SuccessCount} | Greške: {p.ErrorCount}\n" +
                                             $"{p.CurrentRow}";
                            progressForm.Refresh();
                        }));
                });

                progressForm.Shown += async (s, args) =>
                {
                    try
                    {
                        
                        var result = await System.Threading.Tasks.Task.Run(() =>
                        {
                            var bgOptions = new DbContextOptionsBuilder<OwnerTrackDbContext>()
                                .UseSqlite(_connString)
                                .Options;
                            using var bgDb = new OwnerTrackDbContext(bgOptions);
                            var importService = new ExcelImportService(bgDb);
                            return importService.ImportFromExcel(filePath, progress);
                        });

                        btnCancel.Enabled = true;
                        lblStatus.Text = $"Import završen!\n" +
                                         $"Dodano: {result.SuccessCount}\n" +
                                         $"Greške: {result.ErrorCount}\n" +
                                         $"Vlasnici: {result.VlasnikCount}";

                        if (result.Errors.Count > 0)
                        {
                            string errors = string.Join("\n", result.Errors.Take(10));
                            MessageBox.Show($"Greške tokom importa:\n{errors}", "Upozorenje",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        LoadDjelatnostiFilter();
                        LoadKlijenti();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Greška: {ex.Message}", "Greška",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        progressForm.Close();
                    }
                };

                progressForm.ShowDialog(this);
            }
        }
        
        private void OsvjeziUpozerenjaBadge()
        {
            try
            {
                var danas = DateTime.Today;
                var granica = danas.AddDays(60);

                int count = _db.Vlasnici
                    .AsNoTracking()
                    .Count(v => v.DatumValjanostiDokumenta != null
                             && v.DatumValjanostiDokumenta <= granica
                             && v.Status == "AKTIVAN")
                    +
                    _db.Direktori
                    .AsNoTracking()
                    .Count(d => d.DatumValjanosti != null
                             && d.DatumValjanosti <= granica
                             && d.TipValjanosti == "VREMENSKI"
                             && d.Status == "AKTIVAN");

                if (count > 0)
                {
                    btnUpozorenja.Text = $"🔔 Upozorenja ({count})";
                    btnUpozorenja.BackColor = count > 0
                        ? (_db.Vlasnici.AsNoTracking()
                               .Any(v => v.DatumValjanostiDokumenta != null
                                      && v.DatumValjanostiDokumenta < danas
                                      && v.Status == "AKTIVAN")
                           || _db.Direktori.AsNoTracking()
                               .Any(d => d.DatumValjanosti != null
                                      && d.DatumValjanosti < danas
                                      && d.TipValjanosti == "VREMENSKI"
                                      && d.Status == "AKTIVAN")
                            ? Color.Firebrick          
                            : Color.FromArgb(220, 120, 20))  
                        : SystemColors.Control;
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BADGE] Greška: {ex.Message}");
            }
        }

        private void btnUpozorenja_Click(object sender, EventArgs e)
        {
            var frm = new FrmUpozorenja(_db);
            frm.ShowDialog(this);
        }
    }
}