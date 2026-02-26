using OwnerTrack.Infrastructure;
using OwnerTrack.Infrastructure.Models;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public class ImportHelper
    {
        private readonly string _connectionString;

        public ImportHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void PokreniImport(string filePath, Form owner, Action? onZavrsetak = null)
        {
            var cts = new CancellationTokenSource();
            bool importZavršen = false;

            using var frm = KreirajProgressFormu(
                out var progressBar,
                out var lblStatus,
                out var btnZatvori,
                out var btnOtkazi);

            
            frm.FormClosing += (s, args) =>
            {
                if (!importZavršen)
                {
                    args.Cancel = true;
                    MessageBox.Show(
                        "Import je u toku. Molimo sačekajte da se završi ili pritisnite Otkaži.",
                        "Import u toku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            
            btnOtkazi.Click += (s, args) =>
            {
                if (!importZavršen)
                {
                    btnOtkazi.Enabled = false;
                    btnOtkazi.Text = "Otkazivanje...";
                    cts.Cancel();
                }
            };

            
            var progress = new Progress<ImportProgress>(p =>
            {
                if (!frm.IsHandleCreated || frm.IsDisposed) return;
                frm.Invoke((Action)(() =>
                {
                    if (p.TotalRows > 0)
                    {
                        progressBar.Maximum = p.TotalRows;
                        progressBar.Value = Math.Min(p.ProcessedRows, p.TotalRows);
                    }
                    lblStatus.Text =
                        $"Obrađeno: {p.ProcessedRows}/{p.TotalRows}\n" +
                        $"Dodato: {p.SuccessCount}  |  Greške: {p.ErrorCount}\n" +
                        $"{p.CurrentRow}";
                    frm.Refresh();
                }));
            });

           
            frm.Shown += async (s, args) =>
            {
                try
                {
                    var svc = new ExcelImportService(_connectionString);
                    var result = await Task.Run(
                        () => svc.ImportFromExcel(filePath, progress, cts.Token),
                        cts.Token);

                    importZavršen = true;
                    btnZatvori.Enabled = true;
                    btnOtkazi.Enabled = false;

                    if (cts.IsCancellationRequested)
                    {
                        lblStatus.Text =
                            $"Import otkazan.\n" +
                            $"Dodano: {result.SuccessCount}  |  Preskočeno: {result.SkipCount}\n" +
                            $"Greške: {result.ErrorCount}";
                    }
                    else
                    {
                        lblStatus.Text =
                            $"Import završen!\n" +
                            $"Dodano: {result.SuccessCount}  |  Preskočeno (duplikati): {result.SkipCount}\n" +
                            $"Greške: {result.ErrorCount}  |  Vlasnici: {result.VlasnikCount}";
                    }

                    if (result.Errors.Count > 0)
                        MessageBox.Show(
                            $"Greške tokom importa:\n{string.Join("\n", result.Errors.Take(10))}",
                            "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (OperationCanceledException)
                {
                    importZavršen = true;
                    lblStatus.Text = "Import je otkazan od strane korisnika.";
                    btnZatvori.Enabled = true;
                    btnOtkazi.Enabled = false;
                }
                catch (Exception ex)
                {
                    importZavršen = true;
                    Program.LogException(ex);
                    MessageBox.Show($"Greška: {ex.Message}", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    frm.Close();
                    onZavrsetak?.Invoke();
                    return;
                }

                
                void ZatvoriHandler(object? bs, EventArgs be)
                {
                    btnZatvori.Click -= ZatvoriHandler;
                    frm.Close();
                    if (!cts.IsCancellationRequested)
                        onZavrsetak?.Invoke();
                }
                btnZatvori.Click += ZatvoriHandler;
            };

            frm.ShowDialog(owner);
            cts.Dispose();
        }

        private Form KreirajProgressFormu(
            out ProgressBar progressBar,
            out Label lblStatus,
            out Button btnZatvori,
            out Button btnOtkazi)
        {
            var frm = new Form
            {
                Text = "Import u toku...",
                Width = 500,
                Height = 250,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            progressBar = new ProgressBar
            {
                Location = new System.Drawing.Point(20, 20),
                Width = 440,
                Height = 30,
                Style = ProgressBarStyle.Continuous
            };

            lblStatus = new Label
            {
                Location = new System.Drawing.Point(20, 60),
                Width = 440,
                Height = 80,
                Text = "Priprema...",
                AutoSize = false
            };

            
            btnZatvori = new Button
            {
                Text = "Zatvori",
                Location = new System.Drawing.Point(290, 165),
                Width = 90,
                Enabled = false
            };

            
            btnOtkazi = new Button
            {
                Text = "Otkaži",
                Location = new System.Drawing.Point(390, 165),
                Width = 90,
                Enabled = true
            };

            frm.Controls.AddRange(new Control[] { progressBar, lblStatus, btnZatvori, btnOtkazi });
            return frm;
        }
    }
}