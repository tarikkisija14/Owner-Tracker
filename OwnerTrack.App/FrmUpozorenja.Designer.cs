using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

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
            panelTopBorder = new System.Windows.Forms.Panel();
            lblStatFirmi = new System.Windows.Forms.Label();
            lblStatIsteklo = new System.Windows.Forms.Label();
            lblStatKriticno = new System.Windows.Forms.Label();
            lblStatUskoro = new System.Windows.Forms.Label();
            btnZatvori = new IconButton();
            split = new System.Windows.Forms.SplitContainer();
            panelGornji = new System.Windows.Forms.Panel();
            gridFirme = new System.Windows.Forms.DataGridView();
            lblFirme = new System.Windows.Forms.Label();
            lblEmptyFirme = new System.Windows.Forms.Label();
            panelDonji = new System.Windows.Forms.Panel();
            gridDetalji = new System.Windows.Forms.DataGridView();
            lblDetalji = new System.Windows.Forms.Label();
            lblEmptyDetalji = new System.Windows.Forms.Label();
            btnOznaciPregledano = new IconButton();

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

            // ── panelTop (statistika kao "dashboard" kartice) ──────
            panelTop.Controls.Add(lblStatFirmi);
            panelTop.Controls.Add(lblStatIsteklo);
            panelTop.Controls.Add(lblStatKriticno);
            panelTop.Controls.Add(lblStatUskoro);
            panelTop.Controls.Add(btnZatvori);
            panelTop.Controls.Add(panelTopBorder);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Height = 84;
            panelTop.Name = "panelTop";
            panelTop.BackColor = UiTheme.Navy;

            // ── panelTopBorder (tanka linija razdvajanja od grida) ──
            panelTopBorder.Dock = System.Windows.Forms.DockStyle.Bottom;
            panelTopBorder.Height = 1;
            panelTopBorder.Name = "panelTopBorder";
            panelTopBorder.BackColor = UiTheme.GridBorder;

            // ── statistika kartice ────────────────────────────────
            ConfigureStatTile(lblStatFirmi, "lblStatFirmi", 24, "Ukupno firmi");
            ConfigureStatTile(lblStatIsteklo, "lblStatIsteklo", 190, "Isteklo");
            ConfigureStatTile(lblStatKriticno, "lblStatKriticno", 356, "Kritično (≤14 dana)");
            ConfigureStatTile(lblStatUskoro, "lblStatUskoro", 522, "Uskoro ističe");

            void ConfigureStatTile(System.Windows.Forms.Label numberLabel, string name, int x, string caption)
            {
                numberLabel.Location = new System.Drawing.Point(x, 16);
                numberLabel.Size = new System.Drawing.Size(150, 32);
                numberLabel.Name = name;
                numberLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
                numberLabel.ForeColor = System.Drawing.Color.White;
                numberLabel.Text = "0";
                numberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                var captionLabel = new System.Windows.Forms.Label
                {
                    Location = new System.Drawing.Point(x, 48),
                    Size = new System.Drawing.Size(150, 18),
                    Font = new System.Drawing.Font("Segoe UI", 8F),
                    ForeColor = UiTheme.HeaderSubText,
                    Text = caption,
                    TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                };
                panelTop.Controls.Add(captionLabel);
            }

            // ── btnZatvori ────────────────────────────────────────
            btnZatvori.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            btnZatvori.Location = new System.Drawing.Point(934, 24);
            btnZatvori.Size = new System.Drawing.Size(100, 36);
            btnZatvori.Name = "btnZatvori";
            btnZatvori.Text = "Zatvori";
            btnZatvori.IconGlyph = "";
            UiTheme.StyleFlatButton(btnZatvori, UiTheme.Red, 9.5f);
            btnZatvori.Click += btnZatvori_Click;

            // ── lblFirme ──────────────────────────────────────────
            lblFirme.BackColor = UiTheme.PanelLight;
            lblFirme.ForeColor = UiTheme.Navy;
            lblFirme.Dock = System.Windows.Forms.DockStyle.Top;
            lblFirme.Font = OwnerTrack.App.Constants.UiTheme.Base(11f, System.Drawing.FontStyle.Bold);
            lblFirme.Height = 30;
            lblFirme.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            lblFirme.Name = "lblFirme";
            lblFirme.Text = "Firme s upozorenjima — klikni red za detalje";
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
            gridFirme.CellMouseEnter += gridFirme_CellMouseEnter;
            gridFirme.CellMouseLeave += gridFirme_CellMouseLeave;
            UiTheme.StyleGrid(gridFirme, enableRowHover: false);

            // ── lblEmptyFirme ─────────────────────────────────────
            UiTheme.StyleEmptyState(lblEmptyFirme, "Nema upozorenja — svi dokumenti su ažurni.");

            // ── panelGornji ───────────────────────────────────────
            panelGornji.Controls.Add(gridFirme);
            panelGornji.Controls.Add(lblEmptyFirme);
            panelGornji.Controls.Add(lblFirme);
            panelGornji.Dock = System.Windows.Forms.DockStyle.Fill;
            panelGornji.Name = "panelGornji";
            panelGornji.BackColor = System.Drawing.Color.White;

            // ── lblDetalji ────────────────────────────────────────
            lblDetalji.BackColor = UiTheme.PanelLight;
            lblDetalji.ForeColor = UiTheme.Navy;
            lblDetalji.Dock = System.Windows.Forms.DockStyle.Top;
            lblDetalji.Font = OwnerTrack.App.Constants.UiTheme.Base(11f, System.Drawing.FontStyle.Bold);
            lblDetalji.Height = 30;
            lblDetalji.Padding = new System.Windows.Forms.Padding(14, 0, 0, 0);
            lblDetalji.Name = "lblDetalji";
            lblDetalji.Text = "Detalji za odabranu firmu";
            lblDetalji.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // ── btnOznaciPregledano ───────────────────────────────
            btnOznaciPregledano.Dock = System.Windows.Forms.DockStyle.Right;
            btnOznaciPregledano.Width = 210;
            btnOznaciPregledano.Name = "btnOznaciPregledano";
            btnOznaciPregledano.Text = "Označi kao pregledano";
            btnOznaciPregledano.IconGlyph = "";
            UiTheme.StyleAccentButton(btnOznaciPregledano, UiTheme.Blue, 9f);
            btnOznaciPregledano.Click += btnOznaciPregledano_Click;
            lblDetalji.Controls.Add(btnOznaciPregledano);

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
            gridDetalji.CellMouseEnter += gridDetalji_CellMouseEnter;
            gridDetalji.CellMouseLeave += gridDetalji_CellMouseLeave;
            UiTheme.StyleGrid(gridDetalji, enableRowHover: false);

            // ── lblEmptyDetalji ───────────────────────────────────
            UiTheme.StyleEmptyState(lblEmptyDetalji, "Izaberi firmu iz liste iznad za detalje.");

            // ── panelDonji ────────────────────────────────────────
            panelDonji.Controls.Add(gridDetalji);
            panelDonji.Controls.Add(lblEmptyDetalji);
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
            split.SplitterWidth = 6;
            split.BorderStyle = System.Windows.Forms.BorderStyle.None;
            split.BackColor = UiTheme.GridBorder;

            // ── Form ──────────────────────────────────────────────
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = UiTheme.FormBackgroundDialog;
            ClientSize = new System.Drawing.Size(1050, 680);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            Controls.Add(split);
            Controls.Add(panelTop);
            MinimumSize = new System.Drawing.Size(800, 500);
            Name = "FrmUpozorenja";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Upozorenja — dokumenti koji ističu";
            this.Load += new System.EventHandler(this.FrmUpozorenja_Load);

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
        private System.Windows.Forms.Panel panelTopBorder;
        private System.Windows.Forms.Label lblStatFirmi;
        private System.Windows.Forms.Label lblStatIsteklo;
        private System.Windows.Forms.Label lblStatKriticno;
        private System.Windows.Forms.Label lblStatUskoro;
        private IconButton btnZatvori;
        private System.Windows.Forms.SplitContainer split;
        private System.Windows.Forms.Panel panelGornji;
        private System.Windows.Forms.DataGridView gridFirme;
        private System.Windows.Forms.Label lblFirme;
        private System.Windows.Forms.Label lblEmptyFirme;
        private System.Windows.Forms.Panel panelDonji;
        private System.Windows.Forms.DataGridView gridDetalji;
        private System.Windows.Forms.Label lblDetalji;
        private System.Windows.Forms.Label lblEmptyDetalji;
        private IconButton btnOznaciPregledano;
    }
}