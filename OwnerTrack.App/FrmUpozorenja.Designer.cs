namespace OwnerTrack.App
{
    partial class FrmUpozorenja
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTop = new System.Windows.Forms.Panel();
            lblSumarij = new System.Windows.Forms.Label();
            btnZatvori = new System.Windows.Forms.Button();
            split = new System.Windows.Forms.SplitContainer();
            panelGornji = new System.Windows.Forms.Panel();
            gridFirme = new System.Windows.Forms.DataGridView();
            lblFirme = new System.Windows.Forms.Label();
            panelDonji = new System.Windows.Forms.Panel();
            gridDetalji = new System.Windows.Forms.DataGridView();
            lblDetalji = new System.Windows.Forms.Label();

            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)split).BeginInit();
            split.Panel1.SuspendLayout();
            split.Panel2.SuspendLayout();
            split.SuspendLayout();
            panelGornji.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridFirme).BeginInit();
            panelDonji.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridDetalji).BeginInit();
            SuspendLayout();

            // panelTop
            panelTop.Controls.Add(lblSumarij);
            panelTop.Controls.Add(btnZatvori);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Height = 40;
            panelTop.Name = "panelTop";
            panelTop.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            panelTop.BackColor = System.Drawing.Color.FromArgb(255, 248, 220);

            // lblSumarij
            lblSumarij.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSumarij.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblSumarij.Name = "lblSumarij";
            lblSumarij.Text = "Ucitavam...";
            lblSumarij.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // btnZatvori
            btnZatvori.Dock = System.Windows.Forms.DockStyle.Right;
            btnZatvori.Name = "btnZatvori";
            btnZatvori.Text = "Zatvori";
            btnZatvori.Width = 90;
            btnZatvori.UseVisualStyleBackColor = true;
            btnZatvori.Click += btnZatvori_Click;

            // gridFirme
            gridFirme.AllowUserToAddRows = false;
            gridFirme.AllowUserToDeleteRows = false;
            gridFirme.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            gridFirme.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gridFirme.Dock = System.Windows.Forms.DockStyle.Fill;
            gridFirme.MultiSelect = false;
            gridFirme.Name = "gridFirme";
            gridFirme.ReadOnly = true;
            gridFirme.RowHeadersVisible = false;
            gridFirme.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            gridFirme.SelectionChanged += gridFirme_SelectionChanged;
            gridFirme.CellFormatting += gridFirme_CellFormatting;

            // lblFirme
            lblFirme.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            lblFirme.Dock = System.Windows.Forms.DockStyle.Top;
            lblFirme.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblFirme.Height = 24;
            lblFirme.Name = "lblFirme";
            lblFirme.Text = "  Firme s upozorenjima (klikni za detalje):";
            lblFirme.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // panelGornji
            panelGornji.Controls.Add(gridFirme);
            panelGornji.Controls.Add(lblFirme);
            panelGornji.Dock = System.Windows.Forms.DockStyle.Fill;
            panelGornji.Name = "panelGornji";

            // gridDetalji
            gridDetalji.AllowUserToAddRows = false;
            gridDetalji.AllowUserToDeleteRows = false;
            gridDetalji.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            gridDetalji.BorderStyle = System.Windows.Forms.BorderStyle.None;
            gridDetalji.Dock = System.Windows.Forms.DockStyle.Fill;
            gridDetalji.MultiSelect = false;
            gridDetalji.Name = "gridDetalji";
            gridDetalji.ReadOnly = true;
            gridDetalji.RowHeadersVisible = false;
            gridDetalji.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            gridDetalji.CellFormatting += gridDetalji_CellFormatting;

            // lblDetalji
            lblDetalji.BackColor = System.Drawing.Color.FromArgb(230, 230, 230);
            lblDetalji.Dock = System.Windows.Forms.DockStyle.Top;
            lblDetalji.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDetalji.Height = 24;
            lblDetalji.Name = "lblDetalji";
            lblDetalji.Text = "  Detalji za odabranu firmu:";
            lblDetalji.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // panelDonji
            panelDonji.Controls.Add(gridDetalji);
            panelDonji.Controls.Add(lblDetalji);
            panelDonji.Dock = System.Windows.Forms.DockStyle.Fill;
            panelDonji.Name = "panelDonji";

            // split
            split.Dock = System.Windows.Forms.DockStyle.Fill;
            split.Name = "split";
            split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            split.Panel1.Controls.Add(panelGornji);
            split.Panel1MinSize = 80;
            split.Panel2.Controls.Add(panelDonji);
            split.Panel2MinSize = 80;
            split.SplitterDistance = 280;
            split.BorderStyle = System.Windows.Forms.BorderStyle.None;

            // Form
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1050, 680);
            Controls.Add(split);
            Controls.Add(panelTop);
            MinimumSize = new System.Drawing.Size(800, 500);
            Name = "FrmUpozorenja";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Upozorenja - dokumenti koji isticu";

            panelTop.ResumeLayout(false);
            split.Panel1.ResumeLayout(false);
            split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)split).EndInit();
            split.ResumeLayout(false);
            panelGornji.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridFirme).EndInit();
            panelDonji.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridDetalji).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblSumarij;
        private System.Windows.Forms.Button btnZatvori;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.Panel panelGornji;
        private System.Windows.Forms.DataGridView gridFirme;
        private System.Windows.Forms.Label lblFirme;
        private System.Windows.Forms.Panel panelDonji;
        private System.Windows.Forms.DataGridView gridDetalji;
        private System.Windows.Forms.Label lblDetalji;
    }
}