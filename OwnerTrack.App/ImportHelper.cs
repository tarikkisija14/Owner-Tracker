using OwnerTrack.Infrastructure;
using System;
using System.Drawing;
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
            using var frm = KreirajProgressFormu(
                out var progressBar, out var lblStatus, out var btnZatvori);

            var progress = new Progress<ImportProgress>(p =>
            {
                if (!frm.IsHandleCreated) return;
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
                    var result = await System.Threading.Tasks.Task.Run(
                        () => svc.ImportFromExcel(filePath, progress));

                    btnZatvori.Enabled = true;
                    lblStatus.Text =
                        $"Import završen!\n" +
                        $"Dodano: {result.SuccessCount}\n" +
                        $"Greške: {result.ErrorCount}  |  Vlasnici: {result.VlasnikCount}";

                    if (result.Errors.Count > 0)
                        MessageBox.Show(
                            $"Greške tokom importa:\n{string.Join("\n", result.Errors.Take(10))}",
                            "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    onZavrsetak?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška: {ex.Message}", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    frm.Close();
                }
            };

            frm.ShowDialog(owner);
        }

        private Form KreirajProgressFormu(
            out ProgressBar progressBar, out Label lblStatus, out Button btnZatvori)
        {
            var frm = new Form
            {
                Text = "Import u toku...",
                Width = 500,
                Height = 220,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            progressBar = new ProgressBar
            {
                Location = new Point(20, 20),
                Width = 440,
                Height = 30,
                Style = ProgressBarStyle.Continuous
            };

            lblStatus = new Label
            {
                Location = new Point(20, 60),
                Width = 440,
                Height = 80,
                Text = "Priprema...",
                AutoSize = false
            };

            btnZatvori = new Button
            {
                Text = "Zatvori",
                Location = new Point(200, 150),
                Enabled = false
            };
            btnZatvori.Click += (s, e) => frm.Close();

            frm.Controls.AddRange(new Control[] { progressBar, lblStatus, btnZatvori });
            return frm;
        }
    }
}