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

        public Form1()
        {
            InitializeComponent();
            var options = new DbContextOptionsBuilder<OwnerTrackDbContext>()
              .UseSqlite(@"Data Source=C:\Users\tarik\Desktop\Job\Firme.db")
               .Options;

            _db = new OwnerTrackDbContext(options);
            LoadKlijenti();
        }

        // ========== UČITAVANJE PODATAKA ==========

        private void LoadKlijenti(string filter = "")
        {
            try
            {
                var query = _db.Klijenti
                    .Include(k => k.Djelatnost)
                    .Include(k => k.Vlasnici)
                    .Include(k => k.Direktori)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filter))
                {
                    query = query.Where(k => k.Naziv.Contains(filter) || k.IdBroj.Contains(filter));
                }

                var klijenti = query.ToList();

                dataGridKlijenti.DataSource = klijenti.Select(k => new
                {
                    k.Id,
                    k.Naziv,
                    k.IdBroj,
                    k.Adresa,
                    k.VrstaKlijenta,
                    k.Velicina,
                    Sifra = k.SifraDjelatnosti,
                    Djelatnost = k.Djelatnost?.Naziv ?? "",
                    BrojVlasnika = k.Vlasnici.Count,
                    BrojDirektora = k.Direktori.Count,
                    k.Status
                }).ToList();

                if (dataGridKlijenti.Columns.Count > 0)
                {
                    dataGridKlijenti.Columns["Id"].Width = 40;
                    dataGridKlijenti.Columns["Naziv"].Width = 220;
                    dataGridKlijenti.Columns["IdBroj"].Width = 130;
                    dataGridKlijenti.Columns["Adresa"].Width = 200;
                    dataGridKlijenti.Columns["BrojVlasnika"].Width = 80;
                    dataGridKlijenti.Columns["BrojDirektora"].Width = 80;
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
                .ToList();

            dataGridVlasnici.DataSource = vlasnici.Select(v => new
            {
                v.Id,
                v.ImePrezime,
                v.ProcetatVlasnistva,
                v.DatumUtvrdjivanja,
                v.IzvorPodatka,
                v.Status
            }).ToList();
        }

        private void LoadDirektori(int klijentId)
        {
            var direktori = _db.Direktori
                .Where(d => d.KlijentId == klijentId)
                .ToList();

            dataGridDirektori.DataSource = direktori.Select(d => new
            {
                d.Id,
                d.ImePrezime,
                d.DatumValjanosti,
                d.TipValjanosti,
                d.Status
            }).ToList();
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

        private void btnDodajKlijent_Click(object sender, EventArgs e)
        {
            FrmDodajKlijent forma = new FrmDodajKlijent(null, _db);
            if (forma.ShowDialog() == DialogResult.OK)
            {
                LoadKlijenti();
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
                LoadKlijenti();
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
                    LoadKlijenti();
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

        private void txtSearchKlijent_TextChanged(object sender, EventArgs e)
        {
            LoadKlijenti(txtSearchKlijent.Text);
        }

        // ========== EXCEL IMPORT ==========

        private void btnImportExcel_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel Files (*.xlsx)|*.xlsx";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Kreiraj progress dialog
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

                    // Progress reporter
                    var progress = new Progress<ImportProgress>(p =>
                    {
                        progressBar.Maximum = p.TotalRows;
                        progressBar.Value = p.ProcessedRows;
                        lblStatus.Text = $"Obrađeno: {p.ProcessedRows}/{p.TotalRows}\n" +
                                        $"Dodato: {p.SuccessCount} | Greške: {p.ErrorCount}\n" +
                                        $"{p.CurrentRow}";
                        progressForm.Refresh();
                    });

                    // Pokreni import u pozadini
                    progressForm.Shown += async (s, args) =>
                    {
                        try
                        {
                            var importService = new ExcelImportService(_db);
                            var result = await System.Threading.Tasks.Task.Run(() =>
                                importService.ImportFromExcel(dialog.FileName, progress));

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
        }
    }
}