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
            panelSearch = new Panel();
            lblSearchKlijent = new Label();
            txtSearchKlijent = new TextBox();
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
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = Color.LightGray;
            panelToolbar.Controls.Add(btnResetImport);
            panelToolbar.Controls.Add(btnDodajKlijent);
            panelToolbar.Controls.Add(btnIzmijeniKlijent);
            panelToolbar.Controls.Add(btnObrisiKlijent);
            panelToolbar.Controls.Add(btnImportExcel);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Location = new Point(0, 0);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new Size(1225, 47);
            panelToolbar.TabIndex = 2;
            // 
            // btnResetImport
            // 
            btnResetImport.BackColor = Color.IndianRed;
            btnResetImport.Location = new Point(487, 9);
            btnResetImport.Name = "btnResetImport";
            btnResetImport.Size = new Size(144, 28);
            btnResetImport.TabIndex = 4;
            btnResetImport.Text = "Resetuj i reimportuj";
            btnResetImport.UseVisualStyleBackColor = false;
            btnResetImport.Click += btnResetImport_Click;
            // 
            // btnDodajKlijent
            // 
            btnDodajKlijent.Location = new Point(9, 9);
            btnDodajKlijent.Name = "btnDodajKlijent";
            btnDodajKlijent.Size = new Size(105, 28);
            btnDodajKlijent.TabIndex = 0;
            btnDodajKlijent.Text = "➕ Dodaj firmu";
            btnDodajKlijent.UseVisualStyleBackColor = true;
            btnDodajKlijent.Click += btnDodajKlijent_Click;
            // 
            // btnIzmijeniKlijent
            // 
            btnIzmijeniKlijent.Location = new Point(122, 9);
            btnIzmijeniKlijent.Name = "btnIzmijeniKlijent";
            btnIzmijeniKlijent.Size = new Size(105, 28);
            btnIzmijeniKlijent.TabIndex = 1;
            btnIzmijeniKlijent.Text = "✏️ Izmijeni";
            btnIzmijeniKlijent.UseVisualStyleBackColor = true;
            btnIzmijeniKlijent.Click += btnIzmijeniKlijent_Click;
            // 
            // btnObrisiKlijent
            // 
            btnObrisiKlijent.Location = new Point(236, 9);
            btnObrisiKlijent.Name = "btnObrisiKlijent";
            btnObrisiKlijent.Size = new Size(105, 28);
            btnObrisiKlijent.TabIndex = 2;
            btnObrisiKlijent.Text = "❌ Obriši";
            btnObrisiKlijent.UseVisualStyleBackColor = true;
            btnObrisiKlijent.Click += btnObrisiKlijent_Click;
            // 
            // btnImportExcel
            // 
            btnImportExcel.BackColor = Color.LightGreen;
            btnImportExcel.Location = new Point(350, 9);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(131, 28);
            btnImportExcel.TabIndex = 3;
            btnImportExcel.Text = "📥 Import Excel";
            btnImportExcel.UseVisualStyleBackColor = false;
            btnImportExcel.Click += btnImportExcel_Click;
            // 
            // panelSearch
            // 
            panelSearch.BackColor = Color.WhiteSmoke;
            panelSearch.BorderStyle = BorderStyle.FixedSingle;
            panelSearch.Controls.Add(lblSearchKlijent);
            panelSearch.Controls.Add(txtSearchKlijent);
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Location = new Point(0, 47);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(1225, 42);
            panelSearch.TabIndex = 1;
            // 
            // lblSearchKlijent
            // 
            lblSearchKlijent.AutoSize = true;
            lblSearchKlijent.Location = new Point(9, 11);
            lblSearchKlijent.Name = "lblSearchKlijent";
            lblSearchKlijent.Size = new Size(176, 15);
            lblSearchKlijent.TabIndex = 0;
            lblSearchKlijent.Text = "🔍 Pretraži firmu po nazivu ili ID:";
            // 
            // txtSearchKlijent
            // 
            txtSearchKlijent.Location = new Point(262, 11);
            txtSearchKlijent.Name = "txtSearchKlijent";
            txtSearchKlijent.Size = new Size(307, 23);
            txtSearchKlijent.TabIndex = 1;
            txtSearchKlijent.TextChanged += txtSearchKlijent_TextChanged;
            // 
            // dataGridKlijenti
            // 
            dataGridKlijenti.AllowUserToAddRows = false;
            dataGridKlijenti.AllowUserToDeleteRows = false;
            dataGridKlijenti.Dock = DockStyle.Fill;
            dataGridKlijenti.Location = new Point(0, 0);
            dataGridKlijenti.MultiSelect = false;
            dataGridKlijenti.Name = "dataGridKlijenti";
            dataGridKlijenti.ReadOnly = true;
            dataGridKlijenti.RowHeadersVisible = false;
            dataGridKlijenti.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridKlijenti.Size = new Size(1225, 536);
            dataGridKlijenti.TabIndex = 0;
            dataGridKlijenti.SelectionChanged += dataGridKlijenti_SelectionChanged;
            // 
            // dataGridVlasnici
            // 
            dataGridVlasnici.AllowUserToAddRows = false;
            dataGridVlasnici.AllowUserToDeleteRows = false;
            dataGridVlasnici.Dock = DockStyle.Fill;
            dataGridVlasnici.Location = new Point(0, 38);
            dataGridVlasnici.MultiSelect = false;
            dataGridVlasnici.Name = "dataGridVlasnici";
            dataGridVlasnici.ReadOnly = true;
            dataGridVlasnici.RowHeadersVisible = false;
            dataGridVlasnici.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridVlasnici.Size = new Size(583, 177);
            dataGridVlasnici.TabIndex = 0;
            // 
            // dataGridDirektori
            // 
            dataGridDirektori.AllowUserToAddRows = false;
            dataGridDirektori.AllowUserToDeleteRows = false;
            dataGridDirektori.Dock = DockStyle.Fill;
            dataGridDirektori.Location = new Point(0, 38);
            dataGridDirektori.MultiSelect = false;
            dataGridDirektori.Name = "dataGridDirektori";
            dataGridDirektori.ReadOnly = true;
            dataGridDirektori.RowHeadersVisible = false;
            dataGridDirektori.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridDirektori.Size = new Size(638, 177);
            dataGridDirektori.TabIndex = 0;
            // 
            // btnDodajVlasnika
            // 
            btnDodajVlasnika.Location = new Point(9, 8);
            btnDodajVlasnika.Name = "btnDodajVlasnika";
            btnDodajVlasnika.Size = new Size(114, 23);
            btnDodajVlasnika.TabIndex = 0;
            btnDodajVlasnika.Text = "➕ Dodaj vlasnika";
            btnDodajVlasnika.UseVisualStyleBackColor = true;
            btnDodajVlasnika.Click += btnDodajVlasnika_Click;
            // 
            // btnIzmijeniVlasnika
            // 
            btnIzmijeniVlasnika.Location = new Point(131, 8);
            btnIzmijeniVlasnika.Name = "btnIzmijeniVlasnika";
            btnIzmijeniVlasnika.Size = new Size(88, 23);
            btnIzmijeniVlasnika.TabIndex = 1;
            btnIzmijeniVlasnika.Text = "✏️ Izmijeni";
            btnIzmijeniVlasnika.UseVisualStyleBackColor = true;
            btnIzmijeniVlasnika.Click += btnIzmijeniVlasnika_Click;
            // 
            // btnObrisiVlasnika
            // 
            btnObrisiVlasnika.Location = new Point(228, 8);
            btnObrisiVlasnika.Name = "btnObrisiVlasnika";
            btnObrisiVlasnika.Size = new Size(88, 23);
            btnObrisiVlasnika.TabIndex = 2;
            btnObrisiVlasnika.Text = "❌ Obriši";
            btnObrisiVlasnika.UseVisualStyleBackColor = true;
            btnObrisiVlasnika.Click += btnObrisiVlasnika_Click;
            // 
            // btnDodajDirektora
            // 
            btnDodajDirektora.Location = new Point(9, 8);
            btnDodajDirektora.Name = "btnDodajDirektora";
            btnDodajDirektora.Size = new Size(114, 23);
            btnDodajDirektora.TabIndex = 0;
            btnDodajDirektora.Text = "➕ Dodaj direktora";
            btnDodajDirektora.UseVisualStyleBackColor = true;
            btnDodajDirektora.Click += btnDodajDirektora_Click;
            // 
            // btnIzmijeniDirektora
            // 
            btnIzmijeniDirektora.Location = new Point(131, 8);
            btnIzmijeniDirektora.Name = "btnIzmijeniDirektora";
            btnIzmijeniDirektora.Size = new Size(88, 23);
            btnIzmijeniDirektora.TabIndex = 1;
            btnIzmijeniDirektora.Text = "✏️ Izmijeni";
            btnIzmijeniDirektora.UseVisualStyleBackColor = true;
            btnIzmijeniDirektora.Click += btnIzmijeniDirektora_Click;
            // 
            // btnObrisiDirektora
            // 
            btnObrisiDirektora.Location = new Point(228, 8);
            btnObrisiDirektora.Name = "btnObrisiDirektora";
            btnObrisiDirektora.Size = new Size(88, 23);
            btnObrisiDirektora.TabIndex = 2;
            btnObrisiDirektora.Text = "❌ Obriši";
            btnObrisiDirektora.UseVisualStyleBackColor = true;
            btnObrisiDirektora.Click += btnObrisiDirektora_Click;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 89);
            splitMain.Name = "splitMain";
            splitMain.Orientation = Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(dataGridKlijenti);
            splitMain.Panel1MinSize = 100;
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(splitBottom);
            splitMain.Panel2MinSize = 100;
            splitMain.Size = new Size(1225, 755);
            splitMain.SplitterDistance = 536;
            splitMain.TabIndex = 0;
            // 
            // splitBottom
            // 
            splitBottom.Dock = DockStyle.Fill;
            splitBottom.Location = new Point(0, 0);
            splitBottom.Name = "splitBottom";
            // 
            // splitBottom.Panel1
            // 
            splitBottom.Panel1.Controls.Add(panelVlasnici);
            splitBottom.Panel1MinSize = 50;
            // 
            // splitBottom.Panel2
            // 
            splitBottom.Panel2.Controls.Add(panelDirektori);
            splitBottom.Panel2MinSize = 50;
            splitBottom.Size = new Size(1225, 215);
            splitBottom.SplitterDistance = 583;
            splitBottom.TabIndex = 0;
            // 
            // panelVlasnici
            // 
            panelVlasnici.Controls.Add(dataGridVlasnici);
            panelVlasnici.Controls.Add(panelVlasniciBtns);
            panelVlasnici.Dock = DockStyle.Fill;
            panelVlasnici.Location = new Point(0, 0);
            panelVlasnici.Name = "panelVlasnici";
            panelVlasnici.Size = new Size(583, 215);
            panelVlasnici.TabIndex = 0;
            // 
            // panelVlasniciBtns
            // 
            panelVlasniciBtns.BackColor = Color.LightGray;
            panelVlasniciBtns.Controls.Add(btnDodajVlasnika);
            panelVlasniciBtns.Controls.Add(btnIzmijeniVlasnika);
            panelVlasniciBtns.Controls.Add(btnObrisiVlasnika);
            panelVlasniciBtns.Dock = DockStyle.Top;
            panelVlasniciBtns.Location = new Point(0, 0);
            panelVlasniciBtns.Name = "panelVlasniciBtns";
            panelVlasniciBtns.Size = new Size(583, 38);
            panelVlasniciBtns.TabIndex = 1;
            // 
            // panelDirektori
            // 
            panelDirektori.Controls.Add(dataGridDirektori);
            panelDirektori.Controls.Add(panelDirektoriBtns);
            panelDirektori.Dock = DockStyle.Fill;
            panelDirektori.Location = new Point(0, 0);
            panelDirektori.Name = "panelDirektori";
            panelDirektori.Size = new Size(638, 215);
            panelDirektori.TabIndex = 0;
            // 
            // panelDirektoriBtns
            // 
            panelDirektoriBtns.BackColor = Color.LightGray;
            panelDirektoriBtns.Controls.Add(btnDodajDirektora);
            panelDirektoriBtns.Controls.Add(btnIzmijeniDirektora);
            panelDirektoriBtns.Controls.Add(btnObrisiDirektora);
            panelDirektoriBtns.Dock = DockStyle.Top;
            panelDirektoriBtns.Location = new Point(0, 0);
            panelDirektoriBtns.Name = "panelDirektoriBtns";
            panelDirektoriBtns.Size = new Size(638, 38);
            panelDirektoriBtns.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1225, 844);
            Controls.Add(splitMain);
            Controls.Add(panelSearch);
            Controls.Add(panelToolbar);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OwnerTrack - Upravljanje firmama i vlasnicima";
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
        private SplitContainer splitBottom;
        private Panel panelVlasnici;
        private Panel panelVlasniciBtns;
        private Panel panelDirektori;
        private Panel panelDirektoriBtns;
    }
}