namespace OwnerTrack.App.Helpers
{
    
    internal static class ImportProgressFormFactory
    {
        public static Form Kreiraj(
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
                MinimizeBox = false,
            };

            progressBar = new ProgressBar
            {
                Location = new Point(20, 20),
                Width = 440,
                Height = 30,
                Style = ProgressBarStyle.Continuous,
            };

            lblStatus = new Label
            {
                Location = new Point(20, 60),
                Width = 440,
                Height = 80,
                Text = "Priprema...",
                AutoSize = false,
            };

            btnZatvori = new Button
            {
                Text = "Zatvori",
                Location = new Point(290, 165),
                Width = 90,
                Enabled = false,
            };

            btnOtkazi = new Button
            {
                Text = "Otkaži",
                Location = new Point(390, 165),
                Width = 90,
                Enabled = true,
            };

            frm.Controls.AddRange(new Control[] { progressBar, lblStatus, btnZatvori, btnOtkazi });
            return frm;
        }
    }
}