using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

namespace OwnerTrack.App
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.SplitContainer splitMain;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelToolbar = new Panel();
            btnResetImport = new IconButton();
            btnDodajKlijent = new IconButton();
            btnIzmijeniKlijent = new IconButton();
            btnObrisiKlijent = new IconButton();
            btnImportExcel = new IconButton();
            btnUpozorenja = new IconButton();
            btnExportTabelaPdf = new IconButton();
            btnSacuvajPdf = new IconButton();
            panelSearch = new Panel();
            lblSearchKlijent = new Label();
            txtSearchKlijent = new TextBox();
            lblFilterDjelatnost = new Label();
            cmbFilterDjelatnost = new ComboBox();
            lblFilterVelicina = new Label();
            cmbFilterVelicina = new ComboBox();
            btnResetFilters = new IconButton();
            lblKlijentiCount = new Label();
            dataGridKlijenti = new DataGridView();
            dataGridVlasnici = new DataGridView();
            dataGridDirektori = new DataGridView();
            lblEmptyKlijenti = new Label();
            lblEmptyVlasnici = new Label();
            lblEmptyDirektori = new Label();
            btnDodajVlasnika = new IconButton();
            btnIzmijeniVlasnika = new IconButton();
            btnObrisiVlasnika = new IconButton();
            btnDodajDirektora = new IconButton();
            btnIzmijeniDirektora = new IconButton();
            btnObrisiDirektora = new IconButton();
            splitMain = new SplitContainer();
            splitBottom = new SplitContainer();
            panelVlasnici = new Panel();
            panelVlasniciBtns = new Panel();
            panelDirektori = new Panel();
            panelDirektoriBtns = new Panel();
            panelToolbar.SuspendLayout();
            panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridKlijenti).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridVlasnici).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridDirektori).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitBottom).BeginInit();
            splitBottom.Panel1.SuspendLayout();
            splitBottom.Panel2.SuspendLayout();
            splitBottom.SuspendLayout();
            panelVlasnici.SuspendLayout();
            panelVlasniciBtns.SuspendLayout();
            panelDirektori.SuspendLayout();
            panelDirektoriBtns.SuspendLayout();
            SuspendLayout();

            // ── panelToolbar ──────────────────────────────────────
            panelToolbar.BackColor = UiTheme.Navy;
            panelToolbar.Controls.Add(btnResetImport);
            panelToolbar.Controls.Add(btnDodajKlijent);
            panelToolbar.Controls.Add(btnIzmijeniKlijent);
            panelToolbar.Controls.Add(btnObrisiKlijent);
            panelToolbar.Controls.Add(btnImportExcel);
            panelToolbar.Controls.Add(btnUpozorenja);
            panelToolbar.Controls.Add(btnExportTabelaPdf);
            panelToolbar.Controls.Add(btnSacuvajPdf);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Location = new Point(0, 0);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new Size(1450, 52);
            panelToolbar.TabIndex = 2;
            panelToolbar.Padding = new Padding(6, 0, 6, 0);

            // btnDodajKlijent
            btnDodajKlijent.Location = new Point(10, 8);
            btnDodajKlijent.Name = "btnDodajKlijent";
            btnDodajKlijent.Size = new Size(132, 36);
            btnDodajKlijent.TabIndex = 0;
            btnDodajKlijent.Text = "Dodaj firmu";
            btnDodajKlijent.IconGlyph = "";
            UiTheme.StyleAccentButton(btnDodajKlijent, UiTheme.Green);
            btnDodajKlijent.Click += btnDodajKlijent_Click;

            // btnIzmijeniKlijent
            btnIzmijeniKlijent.Location = new Point(154, 8);
            btnIzmijeniKlijent.Name = "btnIzmijeniKlijent";
            btnIzmijeniKlijent.Size = new Size(125, 36);
            btnIzmijeniKlijent.TabIndex = 1;
            btnIzmijeniKlijent.Text = "Izmijeni";
            btnIzmijeniKlijent.IconGlyph = "";
            UiTheme.StyleAccentButton(btnIzmijeniKlijent, UiTheme.Blue);
            btnIzmijeniKlijent.Click += btnIzmijeniKlijent_Click;

            // btnObrisiKlijent
            btnObrisiKlijent.Location = new Point(291, 8);
            btnObrisiKlijent.Name = "btnObrisiKlijent";
            btnObrisiKlijent.Size = new Size(125, 36);
            btnObrisiKlijent.TabIndex = 2;
            btnObrisiKlijent.Text = "Obriši";
            btnObrisiKlijent.IconGlyph = "";
            UiTheme.StyleAccentButton(btnObrisiKlijent, UiTheme.Red);
            btnObrisiKlijent.Click += btnObrisiKlijent_Click;

            // btnImportExcel
            btnImportExcel.Location = new Point(448, 8);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(155, 36);
            btnImportExcel.TabIndex = 3;
            btnImportExcel.Text = "Import Excel";
            btnImportExcel.IconGlyph = "";
            UiTheme.StyleAccentButton(btnImportExcel, Color.FromArgb(22, 141, 84));
            btnImportExcel.Click += btnImportExcel_Click;

            // btnResetImport
            btnResetImport.Location = new Point(615, 8);
            btnResetImport.Name = "btnResetImport";
            btnResetImport.Size = new Size(168, 36);
            btnResetImport.TabIndex = 4;
            btnResetImport.Text = "Resetuj i reimportuj";
            btnResetImport.IconGlyph = "";
            UiTheme.StyleAccentButton(btnResetImport, Color.FromArgb(150, 40, 40));
            btnResetImport.Click += btnResetImport_Click;

            // btnSacuvajPdf
            btnSacuvajPdf.Location = new Point(815, 8);
            btnSacuvajPdf.Name = "btnSacuvajPdf";
            btnSacuvajPdf.Size = new Size(168, 36);
            btnSacuvajPdf.TabIndex = 6;
            btnSacuvajPdf.Text = "Sačuvaj kao PDF";
            btnSacuvajPdf.IconGlyph = "";
            UiTheme.StyleAccentButton(btnSacuvajPdf, Color.FromArgb(70, 100, 160));
            btnSacuvajPdf.Click += btnSacuvajPdf_Click;

            // btnExportTabelaPdf
            btnExportTabelaPdf.Location = new Point(995, 8);
            btnExportTabelaPdf.Name = "btnExportTabelaPdf";
            btnExportTabelaPdf.Size = new Size(180, 36);
            btnExportTabelaPdf.TabIndex = 7;
            btnExportTabelaPdf.Text = "Export tabele u PDF";
            btnExportTabelaPdf.IconGlyph = "";
            UiTheme.StyleAccentButton(btnExportTabelaPdf, Color.FromArgb(41, 98, 155));
            btnExportTabelaPdf.Click += btnExportTabelaPdf_Click;

            // btnUpozorenja
            btnUpozorenja.Location = new Point(1207, 8);
            btnUpozorenja.Name = "btnUpozorenja";
            btnUpozorenja.Size = new Size(168, 36);
            btnUpozorenja.TabIndex = 5;
            btnUpozorenja.Text = "Upozorenja (0)";
            btnUpozorenja.IconGlyph = "";
            UiTheme.StyleAccentButton(btnUpozorenja, Color.FromArgb(200, 155, 10));
            btnUpozorenja.Click += btnUpozorenja_Click;

            // ── panelSearch ───────────────────────────────────────
            panelSearch.BackColor = UiTheme.PanelLight;
            panelSearch.BorderStyle = BorderStyle.None;
            panelSearch.Controls.Add(lblSearchKlijent);
            panelSearch.Controls.Add(txtSearchKlijent);
            panelSearch.Controls.Add(lblFilterDjelatnost);
            panelSearch.Controls.Add(cmbFilterDjelatnost);
            panelSearch.Controls.Add(lblFilterVelicina);
            panelSearch.Controls.Add(cmbFilterVelicina);
            panelSearch.Controls.Add(btnResetFilters);
            panelSearch.Controls.Add(lblKlijentiCount);
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Location = new Point(0, 52);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(1450, 44);
            panelSearch.TabIndex = 1;

            var searchFont = new Font("Segoe UI", 9F);

            lblSearchKlijent.AutoSize = true;
            lblSearchKlijent.Font = searchFont;
            lblSearchKlijent.ForeColor = Color.FromArgb(50, 60, 80);
            lblSearchKlijent.Location = new Point(12, 13);
            lblSearchKlijent.Name = "lblSearchKlijent";
            lblSearchKlijent.TabIndex = 0;
            lblSearchKlijent.Text = "🔍 Pretraži firmu po nazivu ili ID:";

            txtSearchKlijent.Location = new Point(195, 10);
            txtSearchKlijent.Name = "txtSearchKlijent";
            txtSearchKlijent.Size = new Size(230, 23);
            txtSearchKlijent.Font = searchFont;
            txtSearchKlijent.TabIndex = 1;
            txtSearchKlijent.TextChanged += txtSearchKlijent_TextChanged;

            lblFilterDjelatnost.AutoSize = true;
            lblFilterDjelatnost.Font = searchFont;
            lblFilterDjelatnost.ForeColor = Color.FromArgb(50, 60, 80);
            lblFilterDjelatnost.Location = new Point(468, 13);
            lblFilterDjelatnost.Name = "lblFilterDjelatnost";
            lblFilterDjelatnost.TabIndex = 2;
            lblFilterDjelatnost.Text = "🏭 Djelatnost:";

            cmbFilterDjelatnost.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterDjelatnost.Font = searchFont;
            cmbFilterDjelatnost.Location = new Point(560, 10);
            cmbFilterDjelatnost.Name = "cmbFilterDjelatnost";
            cmbFilterDjelatnost.Size = new Size(300, 23);
            cmbFilterDjelatnost.TabIndex = 3;
            cmbFilterDjelatnost.SelectedIndexChanged += cmbFilterDjelatnost_SelectedIndexChanged;

            lblFilterVelicina.AutoSize = true;
            lblFilterVelicina.Font = searchFont;
            lblFilterVelicina.ForeColor = Color.FromArgb(50, 60, 80);
            lblFilterVelicina.Location = new Point(876, 13);
            lblFilterVelicina.Name = "lblFilterVelicina";
            lblFilterVelicina.TabIndex = 5;
            lblFilterVelicina.Text = "📏 Veličina:";

            cmbFilterVelicina.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFilterVelicina.Font = searchFont;
            cmbFilterVelicina.Location = new Point(948, 10);
            cmbFilterVelicina.Name = "cmbFilterVelicina";
            cmbFilterVelicina.Size = new Size(130, 23);
            cmbFilterVelicina.TabIndex = 6;
            cmbFilterVelicina.SelectedIndexChanged += cmbFilterVelicina_SelectedIndexChanged;

            btnResetFilters.Location = new Point(1092, 7);
            btnResetFilters.Name = "btnResetFilters";
            btnResetFilters.Size = new Size(115, 30);
            btnResetFilters.TabIndex = 4;
            btnResetFilters.Text = "Resetuj";
            btnResetFilters.IconGlyph = "";
            UiTheme.StyleAccentButton(btnResetFilters, UiTheme.Blue, 8.5f);
            btnResetFilters.Click += btnResetFilters_Click;

            lblKlijentiCount.AutoSize = true;
            lblKlijentiCount.Font = searchFont;
            lblKlijentiCount.ForeColor = UiTheme.MutedText;
            lblKlijentiCount.Location = new Point(1230, 13);
            lblKlijentiCount.Name = "lblKlijentiCount";
            lblKlijentiCount.TabIndex = 8;
            lblKlijentiCount.Text = string.Empty;

            // ── dataGridKlijenti ──────────────────────────────────
            dataGridKlijenti.AllowUserToAddRows = false;
            dataGridKlijenti.AllowUserToDeleteRows = false;
            dataGridKlijenti.Dock = DockStyle.Fill;
            dataGridKlijenti.Location = new Point(0, 0);
            dataGridKlijenti.MultiSelect = false;
            dataGridKlijenti.Name = "dataGridKlijenti";
            dataGridKlijenti.ReadOnly = true;
            dataGridKlijenti.RowHeadersVisible = false;
            dataGridKlijenti.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridKlijenti.Size = new Size(1450, 536);
            dataGridKlijenti.TabIndex = 0;
            UiTheme.StyleGrid(dataGridKlijenti);
            dataGridKlijenti.SelectionChanged += dataGridKlijenti_SelectionChanged;

            // ── lblEmptyKlijenti ──────────────────────────────────
            UiTheme.StyleEmptyState(lblEmptyKlijenti, "Nema klijenata za prikaz.");

            // ── dataGridVlasnici ──────────────────────────────────
            dataGridVlasnici.AllowUserToAddRows = false;
            dataGridVlasnici.AllowUserToDeleteRows = false;
            dataGridVlasnici.Dock = DockStyle.Fill;
            dataGridVlasnici.Location = new Point(0, 38);
            dataGridVlasnici.MultiSelect = false;
            dataGridVlasnici.Name = "dataGridVlasnici";
            dataGridVlasnici.ReadOnly = true;
            dataGridVlasnici.RowHeadersVisible = false;
            dataGridVlasnici.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridVlasnici.Size = new Size(656, 177);
            dataGridVlasnici.TabIndex = 0;
            UiTheme.StyleGrid(dataGridVlasnici);

            // ── lblEmptyVlasnici ──────────────────────────────────
            UiTheme.StyleEmptyState(lblEmptyVlasnici, "Nema evidentiranih vlasnika.");

            // ── dataGridDirektori ─────────────────────────────────
            dataGridDirektori.AllowUserToAddRows = false;
            dataGridDirektori.AllowUserToDeleteRows = false;
            dataGridDirektori.Dock = DockStyle.Fill;
            dataGridDirektori.Location = new Point(0, 38);
            dataGridDirektori.MultiSelect = false;
            dataGridDirektori.Name = "dataGridDirektori";
            dataGridDirektori.ReadOnly = true;
            dataGridDirektori.RowHeadersVisible = false;
            dataGridDirektori.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridDirektori.Size = new Size(790, 177);
            dataGridDirektori.TabIndex = 0;
            UiTheme.StyleGrid(dataGridDirektori);

            // ── lblEmptyDirektori ─────────────────────────────────
            UiTheme.StyleEmptyState(lblEmptyDirektori, "Nema evidentiranih direktora.");

            // btnDodajVlasnika
            btnDodajVlasnika.Location = new Point(9, 5);
            btnDodajVlasnika.Name = "btnDodajVlasnika";
            btnDodajVlasnika.Size = new Size(138, 30);
            btnDodajVlasnika.TabIndex = 0;
            btnDodajVlasnika.Text = "Dodaj vlasnika";
            btnDodajVlasnika.IconGlyph = "";
            UiTheme.StyleAccentButton(btnDodajVlasnika, UiTheme.Green, 8.5f);
            btnDodajVlasnika.Click += btnDodajVlasnika_Click;

            btnIzmijeniVlasnika.Location = new Point(159, 5);
            btnIzmijeniVlasnika.Name = "btnIzmijeniVlasnika";
            btnIzmijeniVlasnika.Size = new Size(110, 30);
            btnIzmijeniVlasnika.TabIndex = 1;
            btnIzmijeniVlasnika.Text = "Izmijeni";
            btnIzmijeniVlasnika.IconGlyph = "";
            UiTheme.StyleAccentButton(btnIzmijeniVlasnika, UiTheme.Blue, 8.5f);
            btnIzmijeniVlasnika.Click += btnIzmijeniVlasnika_Click;

            btnObrisiVlasnika.Location = new Point(281, 5);
            btnObrisiVlasnika.Name = "btnObrisiVlasnika";
            btnObrisiVlasnika.Size = new Size(110, 30);
            btnObrisiVlasnika.TabIndex = 2;
            btnObrisiVlasnika.Text = "Obriši";
            btnObrisiVlasnika.IconGlyph = "";
            UiTheme.StyleAccentButton(btnObrisiVlasnika, UiTheme.Red, 8.5f);
            btnObrisiVlasnika.Click += btnObrisiVlasnika_Click;

            btnDodajDirektora.Location = new Point(9, 5);
            btnDodajDirektora.Name = "btnDodajDirektora";
            btnDodajDirektora.Size = new Size(158, 30);
            btnDodajDirektora.TabIndex = 0;
            btnDodajDirektora.Text = "Dodaj direktora";
            btnDodajDirektora.IconGlyph = "";
            UiTheme.StyleAccentButton(btnDodajDirektora, UiTheme.Green, 8.5f);
            btnDodajDirektora.Click += btnDodajDirektora_Click;

            btnIzmijeniDirektora.Location = new Point(179, 5);
            btnIzmijeniDirektora.Name = "btnIzmijeniDirektora";
            btnIzmijeniDirektora.Size = new Size(110, 30);
            btnIzmijeniDirektora.TabIndex = 1;
            btnIzmijeniDirektora.Text = "Izmijeni";
            btnIzmijeniDirektora.IconGlyph = "";
            UiTheme.StyleAccentButton(btnIzmijeniDirektora, UiTheme.Blue, 8.5f);
            btnIzmijeniDirektora.Click += btnIzmijeniDirektora_Click;

            btnObrisiDirektora.Location = new Point(301, 5);
            btnObrisiDirektora.Name = "btnObrisiDirektora";
            btnObrisiDirektora.Size = new Size(110, 30);
            btnObrisiDirektora.TabIndex = 2;
            btnObrisiDirektora.Text = "Obriši";
            btnObrisiDirektora.IconGlyph = "";
            UiTheme.StyleAccentButton(btnObrisiDirektora, UiTheme.Red, 8.5f);
            btnObrisiDirektora.Click += btnObrisiDirektora_Click;

            // ── panelVlasniciBtns ─────────────────────────────────
            panelVlasniciBtns.BackColor = UiTheme.PanelLight;
            panelVlasniciBtns.Controls.Add(btnDodajVlasnika);
            panelVlasniciBtns.Controls.Add(btnIzmijeniVlasnika);
            panelVlasniciBtns.Controls.Add(btnObrisiVlasnika);
            panelVlasniciBtns.Dock = DockStyle.Top;
            panelVlasniciBtns.Location = new Point(0, 0);
            panelVlasniciBtns.Name = "panelVlasniciBtns";
            panelVlasniciBtns.Size = new Size(656, 40);
            panelVlasniciBtns.TabIndex = 1;

            // ── panelVlasnici ─────────────────────────────────────
            panelVlasnici.Controls.Add(dataGridVlasnici);
            panelVlasnici.Controls.Add(lblEmptyVlasnici);
            panelVlasnici.Controls.Add(panelVlasniciBtns);
            panelVlasnici.Dock = DockStyle.Fill;
            panelVlasnici.Location = new Point(0, 0);
            panelVlasnici.Name = "panelVlasnici";
            panelVlasnici.Size = new Size(656, 215);
            panelVlasnici.TabIndex = 0;

            // ── panelDirektoriBtns ────────────────────────────────
            panelDirektoriBtns.BackColor = UiTheme.PanelLight;
            panelDirektoriBtns.Controls.Add(btnDodajDirektora);
            panelDirektoriBtns.Controls.Add(btnIzmijeniDirektora);
            panelDirektoriBtns.Controls.Add(btnObrisiDirektora);
            panelDirektoriBtns.Dock = DockStyle.Top;
            panelDirektoriBtns.Location = new Point(0, 0);
            panelDirektoriBtns.Name = "panelDirektoriBtns";
            panelDirektoriBtns.Size = new Size(790, 40);
            panelDirektoriBtns.TabIndex = 1;

            // ── panelDirektori ────────────────────────────────────
            panelDirektori.Controls.Add(dataGridDirektori);
            panelDirektori.Controls.Add(lblEmptyDirektori);
            panelDirektori.Controls.Add(panelDirektoriBtns);
            panelDirektori.Dock = DockStyle.Fill;
            panelDirektori.Location = new Point(0, 0);
            panelDirektori.Name = "panelDirektori";
            panelDirektori.Size = new Size(790, 215);
            panelDirektori.TabIndex = 0;

            // ── splitBottom ───────────────────────────────────────
            splitBottom.Dock = DockStyle.Fill;
            splitBottom.Location = new Point(0, 0);
            splitBottom.Name = "splitBottom";
            splitBottom.Panel1.Controls.Add(panelVlasnici);
            splitBottom.Panel1MinSize = 50;
            splitBottom.Panel2.Controls.Add(panelDirektori);
            splitBottom.Panel2MinSize = 50;
            splitBottom.Size = new Size(1450, 215);
            splitBottom.SplitterDistance = 656;
            splitBottom.TabIndex = 0;

            // ── splitMain ─────────────────────────────────────────
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 96);
            splitMain.Name = "splitMain";
            splitMain.Orientation = Orientation.Horizontal;
            splitMain.Panel1.Controls.Add(dataGridKlijenti);
            splitMain.Panel1.Controls.Add(lblEmptyKlijenti);
            splitMain.Panel1MinSize = 100;
            splitMain.Panel2.Controls.Add(splitBottom);
            splitMain.Panel2MinSize = 100;
            splitMain.Size = new Size(1450, 755);
            splitMain.SplitterDistance = 536;
            splitMain.TabIndex = 0;

            // ── Form1 ─────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UiTheme.FormBackgroundMain;
            ClientSize = new Size(1450, 844);
            Controls.Add(splitMain);
            Controls.Add(panelSearch);
            Controls.Add(panelToolbar);
            Font = new Font("Segoe UI", 9F);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OwnerTrack — Upravljanje firmama i vlasnicima";
            WindowState = FormWindowState.Maximized;

            panelToolbar.ResumeLayout(false);
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridKlijenti).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridVlasnici).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridDirektori).EndInit();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            splitBottom.Panel1.ResumeLayout(false);
            splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitBottom).EndInit();
            splitBottom.ResumeLayout(false);
            panelVlasnici.ResumeLayout(false);
            panelVlasniciBtns.ResumeLayout(false);
            panelDirektori.ResumeLayout(false);
            panelDirektoriBtns.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelToolbar;
        private IconButton btnDodajKlijent;
        private IconButton btnIzmijeniKlijent;
        private IconButton btnObrisiKlijent;
        private IconButton btnImportExcel;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearchKlijent;
        public System.Windows.Forms.TextBox txtSearchKlijent;
        public System.Windows.Forms.DataGridView dataGridKlijenti;
        public System.Windows.Forms.DataGridView dataGridVlasnici;
        public System.Windows.Forms.DataGridView dataGridDirektori;
        private System.Windows.Forms.Label lblEmptyKlijenti;
        private System.Windows.Forms.Label lblEmptyVlasnici;
        private System.Windows.Forms.Label lblEmptyDirektori;
        public IconButton btnDodajVlasnika;
        public IconButton btnIzmijeniVlasnika;
        public IconButton btnObrisiVlasnika;
        public IconButton btnDodajDirektora;
        public IconButton btnIzmijeniDirektora;
        public IconButton btnObrisiDirektora;
        private IconButton btnResetImport;
        private IconButton btnUpozorenja;
        private SplitContainer splitBottom;
        private Panel panelVlasnici;
        private Panel panelVlasniciBtns;
        private Panel panelDirektori;
        private Panel panelDirektoriBtns;
        private System.Windows.Forms.Label lblFilterDjelatnost;
        public System.Windows.Forms.ComboBox cmbFilterDjelatnost;
        private System.Windows.Forms.Label lblFilterVelicina;
        public System.Windows.Forms.Label lblKlijentiCount;
        public System.Windows.Forms.ComboBox cmbFilterVelicina;
        private IconButton btnResetFilters;
        private IconButton btnSacuvajPdf;
        private IconButton btnExportTabelaPdf;
    }
}