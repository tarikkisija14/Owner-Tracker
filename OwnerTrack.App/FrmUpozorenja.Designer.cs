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

            // ── Shared grid style ─────────────────────────────────
            static void StyleGrid(System.Windows.Forms.DataGridView g)
            {
                g.BackgroundColor = System.Drawing.Color.White;
                g.GridColor = System.Drawing.Color.FromArgb(210, 218, 230);
                g.BorderStyle = System.Windows.Forms.BorderStyle.None;
                g.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
                g.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(28, 40, 65);
                g.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
                g.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
                g.ColumnHeadersDefaultCellStyle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
                g.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
                g.ColumnHeadersHeight = 32;
                g.EnableHeadersVisualStyles = false;
                g.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
                g.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(52, 120, 200);
                g.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                g.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
                g.RowTemplate.Height = 27;
            }

            // ── panelTop ──────────────────────────────────────────
            panelTop.Controls.Add(lblSumarij);
            panelTop.Controls.Add(btnZatvori);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Height = 46;
            panelTop.Name = "panelTop";
            panelTop.Padding = new System.Windows.Forms.Padding(12, 0, 8, 0);
            panelTop.BackColor = System.Drawing.Color.FromArgb(28, 40, 65);

            // ── lblSumarij ────────────────────────────────────────
            lblSumarij.Dock = System.Windows.Forms.DockStyle.Fill;
            lblSumarij.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblSumarij.ForeColor = System.Drawing.Color.White;
            lblSumarij.Name = "lblSumarij";
            lblSumarij.Text = "Učitavam...";
            lblSumarij.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── btnZatvori ────────────────────────────────────────
            btnZatvori.Dock = System.Windows.Forms.DockStyle.Right;
            btnZatvori.Name = "btnZatvori";
            btnZatvori.Text = "✕  Zatvori";
            btnZatvori.Width = 110;
            btnZatvori.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnZatvori.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            btnZatvori.ForeColor = System.Drawing.Color.White;
            btnZatvori.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnZatvori.FlatAppearance.BorderSize = 0;
            btnZatvori.Cursor = System.Windows.Forms.Cursors.Hand;
            btnZatvori.UseVisualStyleBackColor = false;
            btnZatvori.Click += btnZatvori_Click;

            // ── lblFirme ──────────────────────────────────────────
            lblFirme.BackColor = System.Drawing.Color.FromArgb(240, 244, 250);
            lblFirme.ForeColor = System.Drawing.Color.FromArgb(28, 40, 65);
            lblFirme.Dock = System.Windows.Forms.DockStyle.Top;
            lblFirme.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblFirme.Height = 28;
            lblFirme.Name = "lblFirme";
            lblFirme.Text = "   ⚠  Firme s upozorenjima — klikni red za detalje:";
            lblFirme.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── gridFirme ─────────────────────────────────────────
            gridFirme.AllowUserToAddRows = false;
            gridFirme.AllowUserToDeleteRows = false;
            gridFirme.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            gridFirme.Dock = System.Windows.Forms.DockStyle.Fill;
            gridFirme.MultiSelect = false;
            gridFirme.Name = "gridFirme";
            gridFirme.ReadOnly = true;
            gridFirme.RowHeadersVisible = false;
            gridFirme.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            gridFirme.SelectionChanged += gridFirme_SelectionChanged;
            gridFirme.CellFormatting += gridFirme_CellFormatting;
            StyleGrid(gridFirme);

            // ── panelGornji ───────────────────────────────────────
            panelGornji.Controls.Add(gridFirme);
            panelGornji.Controls.Add(lblFirme);
            panelGornji.Dock = System.Windows.Forms.DockStyle.Fill;
            panelGornji.Name = "panelGornji";
            panelGornji.BackColor = System.Drawing.Color.White;

            // ── lblDetalji ────────────────────────────────────────
            lblDetalji.BackColor = System.Drawing.Color.FromArgb(240, 244, 250);
            lblDetalji.ForeColor = System.Drawing.Color.FromArgb(28, 40, 65);
            lblDetalji.Dock = System.Windows.Forms.DockStyle.Top;
            lblDetalji.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblDetalji.Height = 28;
            lblDetalji.Name = "lblDetalji";
            lblDetalji.Text = "   📋  Detalji za odabranu firmu:";
            lblDetalji.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── gridDetalji ───────────────────────────────────────
            gridDetalji.AllowUserToAddRows = false;
            gridDetalji.AllowUserToDeleteRows = false;
            gridDetalji.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            gridDetalji.Dock = System.Windows.Forms.DockStyle.Fill;
            gridDetalji.MultiSelect = false;
            gridDetalji.Name = "gridDetalji";
            gridDetalji.ReadOnly = true;
            gridDetalji.RowHeadersVisible = false;
            gridDetalji.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            gridDetalji.CellFormatting += gridDetalji_CellFormatting;
            StyleGrid(gridDetalji);

            // ── panelDonji ────────────────────────────────────────
            panelDonji.Controls.Add(gridDetalji);
            panelDonji.Controls.Add(lblDetalji);
            panelDonji.Dock = System.Windows.Forms.DockStyle.Fill;
            panelDonji.Name = "panelDonji";
            panelDonji.BackColor = System.Drawing.Color.White;

            // ── split ─────────────────────────────────────────────
            split.Dock = System.Windows.Forms.DockStyle.Fill;
            split.Name = "split";
            split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            split.Panel1.Controls.Add(panelGornji);
            split.Panel1MinSize = 80;
            split.Panel2.Controls.Add(panelDonji);
            split.Panel2MinSize = 80;
            split.SplitterDistance = 280;
            split.BorderStyle = System.Windows.Forms.BorderStyle.None;
            split.BackColor = System.Drawing.Color.FromArgb(200, 210, 225);

            // ── Form ──────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            ClientSize = new System.Drawing.Size(1050, 680);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            Controls.Add(split);
            Controls.Add(panelTop);
            MinimumSize = new System.Drawing.Size(800, 500);
            Name = "FrmUpozorenja";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Upozorenja — dokumenti koji ističu";

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