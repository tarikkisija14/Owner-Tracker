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
            btnResetImport = new Button();
            btnDodajKlijent = new Button();
            btnIzmijeniKlijent = new Button();
            btnObrisiKlijent = new Button();
            btnImportExcel = new Button();
            btnUpozorenja = new Button();
            btnExportTabelaPdf = new Button();
            btnSacuvajPdf = new Button();
            panelSearch = new Panel();
            lblSearchKlijent = new Label();
            txtSearchKlijent = new TextBox();
            lblFilterDjelatnost = new Label();
            cmbFilterDjelatnost = new ComboBox();
            lblFilterVelicina = new Label();
            cmbFilterVelicina = new ComboBox();
            btnResetFilters = new Button();
            dataGridKlijenti = new DataGridView();
            dataGridVlasnici = new DataGridView();
            dataGridDirektori = new DataGridView();
            btnDodajVlasnika = new Button();
            btnIzmijeniVlasnika = new Button();
            btnObrisiVlasnika = new Button();
            btnDodajDirektora = new Button();
            btnIzmijeniDirektora = new Button();
            btnObrisiDirektora = new Button();
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
            panelToolbar.BackColor = Color.FromArgb(28, 40, 65);
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

            // Helper: styled toolbar button
            static void StyleToolbarBtn(Button b, Color accent)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderColor = accent;
                b.FlatAppearance.BorderSize = 1;
                b.BackColor = accent;
                b.ForeColor = Color.White;
                b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                b.Cursor = Cursors.Hand;
                b.UseVisualStyleBackColor = false;
            }

            // btnDodajKlijent
            btnDodajKlijent.Location = new Point(10, 11);
            btnDodajKlijent.Name = "btnDodajKlijent";
            btnDodajKlijent.Size = new Size(112, 30);
            btnDodajKlijent.TabIndex = 0;
            btnDodajKlijent.Text = "➕ Dodaj firmu";
            StyleToolbarBtn(btnDodajKlijent, Color.FromArgb(39, 174, 96));
            btnDodajKlijent.Click += btnDodajKlijent_Click;

            // btnIzmijeniKlijent
            btnIzmijeniKlijent.Location = new Point(130, 11);
            btnIzmijeniKlijent.Name = "btnIzmijeniKlijent";
            btnIzmijeniKlijent.Size = new Size(105, 30);
            btnIzmijeniKlijent.TabIndex = 1;
            btnIzmijeniKlijent.Text = "✏️ Izmijeni";
            StyleToolbarBtn(btnIzmijeniKlijent, Color.FromArgb(52, 120, 200));
            btnIzmijeniKlijent.Click += btnIzmijeniKlijent_Click;

            // btnObrisiKlijent
            btnObrisiKlijent.Location = new Point(243, 11);
            btnObrisiKlijent.Name = "btnObrisiKlijent";
            btnObrisiKlijent.Size = new Size(105, 30);
            btnObrisiKlijent.TabIndex = 2;
            btnObrisiKlijent.Text = "🗑️ Obriši";
            StyleToolbarBtn(btnObrisiKlijent, Color.FromArgb(192, 57, 43));
            btnObrisiKlijent.Click += btnObrisiKlijent_Click;

            // btnImportExcel
            btnImportExcel.Location = new Point(360, 11);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(135, 30);
            btnImportExcel.TabIndex = 3;
            btnImportExcel.Text = "📥 Import Excel";
            StyleToolbarBtn(btnImportExcel, Color.FromArgb(22, 141, 84));
            btnImportExcel.Click += btnImportExcel_Click;

            // btnResetImport
            btnResetImport.Location = new Point(503, 11);
            btnResetImport.Name = "btnResetImport";
            btnResetImport.Size = new Size(148, 30);
            btnResetImport.TabIndex = 4;
            btnResetImport.Text = "🔁 Resetuj i reimportuj";
            StyleToolbarBtn(btnResetImport, Color.FromArgb(150, 40, 40));
            btnResetImport.Click += btnResetImport_Click;

            // btnSacuvajPdf
            btnSacuvajPdf.Location = new Point(663, 11);
            btnSacuvajPdf.Name = "btnSacuvajPdf";
            btnSacuvajPdf.Size = new Size(148, 30);
            btnSacuvajPdf.TabIndex = 6;
            btnSacuvajPdf.Text = "📄 Sačuvaj kao PDF";
            StyleToolbarBtn(btnSacuvajPdf, Color.FromArgb(70, 100, 160));
            btnSacuvajPdf.Click += btnSacuvajPdf_Click;

            // btnExportTabelaPdf
            btnExportTabelaPdf.Location = new Point(819, 11);
            btnExportTabelaPdf.Name = "btnExportTabelaPdf";
            btnExportTabelaPdf.Size = new Size(160, 30);
            btnExportTabelaPdf.TabIndex = 7;
            btnExportTabelaPdf.Text = "📋 Export tabele u PDF";
            StyleToolbarBtn(btnExportTabelaPdf, Color.FromArgb(41, 98, 155));
            btnExportTabelaPdf.Click += btnExportTabelaPdf_Click;

            // btnUpozorenja
            btnUpozorenja.Location = new Point(991, 11);
            btnUpozorenja.Name = "btnUpozorenja";
            btnUpozorenja.Size = new Size(148, 30);
            btnUpozorenja.TabIndex = 5;
            btnUpozorenja.Text = "⚠ Upozorenja (0)";
            StyleToolbarBtn(btnUpozorenja, Color.FromArgb(200, 155, 10));
            btnUpozorenja.Click += btnUpozorenja_Click;

            // ── panelSearch ───────────────────────────────────────
            panelSearch.BackColor = Color.FromArgb(240, 244, 250);
            panelSearch.BorderStyle = BorderStyle.None;
            panelSearch.Controls.Add(lblSearchKlijent);
            panelSearch.Controls.Add(txtSearchKlijent);
            panelSearch.Controls.Add(lblFilterDjelatnost);
            panelSearch.Controls.Add(cmbFilterDjelatnost);
            panelSearch.Controls.Add(lblFilterVelicina);
            panelSearch.Controls.Add(cmbFilterVelicina);
            panelSearch.Controls.Add(btnResetFilters);
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

            btnResetFilters.FlatStyle = FlatStyle.Flat;
            btnResetFilters.FlatAppearance.BorderColor = Color.FromArgb(150, 160, 180);
            btnResetFilters.Font = searchFont;
            btnResetFilters.BackColor = Color.FromArgb(220, 226, 238);
            btnResetFilters.ForeColor = Color.FromArgb(40, 50, 80);
            btnResetFilters.Cursor = Cursors.Hand;
            btnResetFilters.Location = new Point(1092, 9);
            btnResetFilters.Name = "btnResetFilters";
            btnResetFilters.Size = new Size(95, 26);
            btnResetFilters.TabIndex = 4;
            btnResetFilters.Text = "🔄 Resetuj";
            btnResetFilters.UseVisualStyleBackColor = false;
            btnResetFilters.Click += btnResetFilters_Click;

            // ── DataGridView helper ───────────────────────────────
            static void StyleGrid(DataGridView g)
            {
                g.BackgroundColor = Color.White;
                g.GridColor = Color.FromArgb(210, 218, 230);
                g.BorderStyle = BorderStyle.None;
                g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 40, 65);
                g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                g.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
                g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                g.ColumnHeadersHeight = 32;
                g.EnableHeadersVisualStyles = false;
                g.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
                g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 120, 200);
                g.DefaultCellStyle.SelectionForeColor = Color.White;
                g.DefaultCellStyle.Padding = new Padding(3, 0, 0, 0);
                g.RowTemplate.Height = 26;
                g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 249, 253);
            }

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
            StyleGrid(dataGridKlijenti);
            dataGridKlijenti.SelectionChanged += dataGridKlijenti_SelectionChanged;

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
            StyleGrid(dataGridVlasnici);

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
            StyleGrid(dataGridDirektori);

            // ── Sub-panel button helper ───────────────────────────
            static void StyleSubBtn(Button b, Color accent)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderColor = accent;
                b.FlatAppearance.BorderSize = 1;
                b.BackColor = accent;
                b.ForeColor = Color.White;
                b.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                b.Cursor = Cursors.Hand;
                b.UseVisualStyleBackColor = false;
            }

            // btnDodajVlasnika
            btnDodajVlasnika.Location = new Point(9, 8);
            btnDodajVlasnika.Name = "btnDodajVlasnika";
            btnDodajVlasnika.Size = new Size(118, 24);
            btnDodajVlasnika.TabIndex = 0;
            btnDodajVlasnika.Text = "➕ Dodaj vlasnika";
            StyleSubBtn(btnDodajVlasnika, Color.FromArgb(39, 174, 96));
            btnDodajVlasnika.Click += btnDodajVlasnika_Click;

            btnIzmijeniVlasnika.Location = new Point(135, 8);
            btnIzmijeniVlasnika.Name = "btnIzmijeniVlasnika";
            btnIzmijeniVlasnika.Size = new Size(90, 24);
            btnIzmijeniVlasnika.TabIndex = 1;
            btnIzmijeniVlasnika.Text = "✏️ Izmijeni";
            StyleSubBtn(btnIzmijeniVlasnika, Color.FromArgb(52, 120, 200));
            btnIzmijeniVlasnika.Click += btnIzmijeniVlasnika_Click;

            btnObrisiVlasnika.Location = new Point(233, 8);
            btnObrisiVlasnika.Name = "btnObrisiVlasnika";
            btnObrisiVlasnika.Size = new Size(90, 24);
            btnObrisiVlasnika.TabIndex = 2;
            btnObrisiVlasnika.Text = "🗑️ Obriši";
            StyleSubBtn(btnObrisiVlasnika, Color.FromArgb(192, 57, 43));
            btnObrisiVlasnika.Click += btnObrisiVlasnika_Click;

            btnDodajDirektora.Location = new Point(9, 8);
            btnDodajDirektora.Name = "btnDodajDirektora";
            btnDodajDirektora.Size = new Size(138, 24);
            btnDodajDirektora.TabIndex = 0;
            btnDodajDirektora.Text = "➕ Dodaj direktora";
            StyleSubBtn(btnDodajDirektora, Color.FromArgb(39, 174, 96));
            btnDodajDirektora.Click += btnDodajDirektora_Click;

            btnIzmijeniDirektora.Location = new Point(155, 8);
            btnIzmijeniDirektora.Name = "btnIzmijeniDirektora";
            btnIzmijeniDirektora.Size = new Size(90, 24);
            btnIzmijeniDirektora.TabIndex = 1;
            btnIzmijeniDirektora.Text = "✏️ Izmijeni";
            StyleSubBtn(btnIzmijeniDirektora, Color.FromArgb(52, 120, 200));
            btnIzmijeniDirektora.Click += btnIzmijeniDirektora_Click;

            btnObrisiDirektora.Location = new Point(253, 8);
            btnObrisiDirektora.Name = "btnObrisiDirektora";
            btnObrisiDirektora.Size = new Size(90, 24);
            btnObrisiDirektora.TabIndex = 2;
            btnObrisiDirektora.Text = "🗑️ Obriši";
            StyleSubBtn(btnObrisiDirektora, Color.FromArgb(192, 57, 43));
            btnObrisiDirektora.Click += btnObrisiDirektora_Click;

            // ── panelVlasniciBtns ─────────────────────────────────
            panelVlasniciBtns.BackColor = Color.FromArgb(240, 244, 250);
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
            panelVlasnici.Controls.Add(panelVlasniciBtns);
            panelVlasnici.Dock = DockStyle.Fill;
            panelVlasnici.Location = new Point(0, 0);
            panelVlasnici.Name = "panelVlasnici";
            panelVlasnici.Size = new Size(656, 215);
            panelVlasnici.TabIndex = 0;

            // ── panelDirektoriBtns ────────────────────────────────
            panelDirektoriBtns.BackColor = Color.FromArgb(240, 244, 250);
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
            splitMain.Panel1MinSize = 100;
            splitMain.Panel2.Controls.Add(splitBottom);
            splitMain.Panel2MinSize = 100;
            splitMain.Size = new Size(1450, 755);
            splitMain.SplitterDistance = 536;
            splitMain.TabIndex = 0;

            // ── Form1 ─────────────────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 250, 253);
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
        private System.Windows.Forms.Button btnDodajKlijent;
        private System.Windows.Forms.Button btnIzmijeniKlijent;
        private System.Windows.Forms.Button btnObrisiKlijent;
        private System.Windows.Forms.Button btnImportExcel;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label lblSearchKlijent;
        public System.Windows.Forms.TextBox txtSearchKlijent;
        public System.Windows.Forms.DataGridView dataGridKlijenti;
        public System.Windows.Forms.DataGridView dataGridVlasnici;
        public System.Windows.Forms.DataGridView dataGridDirektori;
        public System.Windows.Forms.Button btnDodajVlasnika;
        public System.Windows.Forms.Button btnIzmijeniVlasnika;
        public System.Windows.Forms.Button btnObrisiVlasnika;
        public System.Windows.Forms.Button btnDodajDirektora;
        public System.Windows.Forms.Button btnIzmijeniDirektora;
        public System.Windows.Forms.Button btnObrisiDirektora;
        private Button btnResetImport;
        private Button btnUpozorenja;
        private SplitContainer splitBottom;
        private Panel panelVlasnici;
        private Panel panelVlasniciBtns;
        private Panel panelDirektori;
        private Panel panelDirektoriBtns;
        private System.Windows.Forms.Label lblFilterDjelatnost;
        public System.Windows.Forms.ComboBox cmbFilterDjelatnost;
        private System.Windows.Forms.Label lblFilterVelicina;
        public System.Windows.Forms.ComboBox cmbFilterVelicina;
        private System.Windows.Forms.Button btnResetFilters;
        private System.Windows.Forms.Button btnSacuvajPdf;
        private System.Windows.Forms.Button btnExportTabelaPdf;
    }
}