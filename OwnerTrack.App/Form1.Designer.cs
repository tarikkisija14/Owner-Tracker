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
            this.components = new System.ComponentModel.Container();

            // TOOLBAR
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.btnDodajKlijent = new System.Windows.Forms.Button();
            this.btnIzmijeniKlijent = new System.Windows.Forms.Button();
            this.btnObrisiKlijent = new System.Windows.Forms.Button();
            this.btnImportExcel = new System.Windows.Forms.Button();

            // SEARCH
            this.panelSearch = new System.Windows.Forms.Panel();
            this.lblSearchKlijent = new System.Windows.Forms.Label();
            this.txtSearchKlijent = new System.Windows.Forms.TextBox();

            // DATAGRIDOVI
            this.dataGridKlijenti = new System.Windows.Forms.DataGridView();
            this.dataGridVlasnici = new System.Windows.Forms.DataGridView();
            this.dataGridDirektori = new System.Windows.Forms.DataGridView();

            // BUTTONS ZA VLASNIKE I DIREKTORE
            this.btnDodajVlasnika = new System.Windows.Forms.Button();
            this.btnIzmijeniVlasnika = new System.Windows.Forms.Button();
            this.btnObrisiVlasnika = new System.Windows.Forms.Button();
            this.btnDodajDirektora = new System.Windows.Forms.Button();
            this.btnIzmijeniDirektora = new System.Windows.Forms.Button();
            this.btnObrisiDirektora = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridKlijenti)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVlasnici)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDirektori)).BeginInit();
            this.SuspendLayout();

            // ========== FORMA ==========
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 900);
            this.Text = "OwnerTrack - Upravljanje firmama i vlasnicima";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            // ========== TOOLBAR ==========
            this.panelToolbar.BackColor = System.Drawing.Color.LightGray;
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Height = 50;

            this.btnDodajKlijent.Location = new System.Drawing.Point(10, 10);
            this.btnDodajKlijent.Size = new System.Drawing.Size(120, 30);
            this.btnDodajKlijent.Text = "➕ Dodaj firmu";
            this.btnDodajKlijent.UseVisualStyleBackColor = true;
            this.btnDodajKlijent.Click += btnDodajKlijent_Click;

            this.btnIzmijeniKlijent.Location = new System.Drawing.Point(140, 10);
            this.btnIzmijeniKlijent.Size = new System.Drawing.Size(120, 30);
            this.btnIzmijeniKlijent.Text = "✏️ Izmijeni";
            this.btnIzmijeniKlijent.UseVisualStyleBackColor = true;
            this.btnIzmijeniKlijent.Click += btnIzmijeniKlijent_Click;

            this.btnObrisiKlijent.Location = new System.Drawing.Point(270, 10);
            this.btnObrisiKlijent.Size = new System.Drawing.Size(120, 30);
            this.btnObrisiKlijent.Text = "❌ Obriši";
            this.btnObrisiKlijent.UseVisualStyleBackColor = true;
            this.btnObrisiKlijent.Click += btnObrisiKlijent_Click;

            this.btnImportExcel.Location = new System.Drawing.Point(400, 10);
            this.btnImportExcel.Size = new System.Drawing.Size(150, 30);
            this.btnImportExcel.Text = "📥 Import Excel";
            this.btnImportExcel.BackColor = System.Drawing.Color.LightGreen;
            this.btnImportExcel.UseVisualStyleBackColor = false;
            this.btnImportExcel.Click += btnImportExcel_Click;

            this.panelToolbar.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.btnDodajKlijent, this.btnIzmijeniKlijent, this.btnObrisiKlijent, this.btnImportExcel
            });

            // ========== SEARCH PANEL ==========
            this.panelSearch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Height = 45;
            this.panelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.lblSearchKlijent.AutoSize = true;
            this.lblSearchKlijent.Location = new System.Drawing.Point(10, 12);
            this.lblSearchKlijent.Text = "🔍 Pretraži firmu po nazivu ili ID:";

            this.txtSearchKlijent.Location = new System.Drawing.Point(300, 12);
            this.txtSearchKlijent.Size = new System.Drawing.Size(350, 20);
            this.txtSearchKlijent.TextChanged += txtSearchKlijent_TextChanged;

            this.panelSearch.Controls.AddRange(new System.Windows.Forms.Control[]
            {
                this.lblSearchKlijent, this.txtSearchKlijent
            });

            // ========== SPLIT CONTAINER ZA LISTE ==========
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitMain.Panel1MinSize = 100;
            this.splitMain.Panel2MinSize = 100;

            // KLIJENTI - TOP
            this.dataGridKlijenti.AllowUserToAddRows = false;
            this.dataGridKlijenti.AllowUserToDeleteRows = false;
            this.dataGridKlijenti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridKlijenti.ReadOnly = true;
            this.dataGridKlijenti.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridKlijenti.MultiSelect = false;
            this.dataGridKlijenti.RowHeadersVisible = false;
            this.dataGridKlijenti.SelectionChanged += dataGridKlijenti_SelectionChanged;

            this.splitMain.Panel1.Controls.Add(this.dataGridKlijenti);

            // VLASNICI I DIREKTORI - BOTTOM
            var splitBottom = new System.Windows.Forms.SplitContainer();
            splitBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            splitBottom.Panel1MinSize = 50;
            splitBottom.Panel2MinSize = 50;

            // VLASNICI
            var panelVlasnici = new System.Windows.Forms.Panel();
            panelVlasnici.Dock = System.Windows.Forms.DockStyle.Fill;

            var panelVlasniciBtns = new System.Windows.Forms.Panel();
            panelVlasniciBtns.Dock = System.Windows.Forms.DockStyle.Top;
            panelVlasniciBtns.Height = 40;
            panelVlasniciBtns.BackColor = System.Drawing.Color.LightGray;

            this.btnDodajVlasnika.Location = new System.Drawing.Point(10, 8);
            this.btnDodajVlasnika.Size = new System.Drawing.Size(130, 25);
            this.btnDodajVlasnika.Text = "➕ Dodaj vlasnika";
            this.btnDodajVlasnika.UseVisualStyleBackColor = true;
            this.btnDodajVlasnika.Click += btnDodajVlasnika_Click;

            this.btnIzmijeniVlasnika.Location = new System.Drawing.Point(150, 8);
            this.btnIzmijeniVlasnika.Size = new System.Drawing.Size(100, 25);
            this.btnIzmijeniVlasnika.Text = "✏️ Izmijeni";
            this.btnIzmijeniVlasnika.UseVisualStyleBackColor = true;
            this.btnIzmijeniVlasnika.Click += btnIzmijeniVlasnika_Click;

            this.btnObrisiVlasnika.Location = new System.Drawing.Point(260, 8);
            this.btnObrisiVlasnika.Size = new System.Drawing.Size(100, 25);
            this.btnObrisiVlasnika.Text = "❌ Obriši";
            this.btnObrisiVlasnika.UseVisualStyleBackColor = true;
            this.btnObrisiVlasnika.Click += btnObrisiVlasnika_Click;

            panelVlasniciBtns.Controls.Add(this.btnDodajVlasnika);
            panelVlasniciBtns.Controls.Add(this.btnIzmijeniVlasnika);
            panelVlasniciBtns.Controls.Add(this.btnObrisiVlasnika);

            // POPRAVKA: Dodao SelectionMode i MultiSelect za vlasnike
            this.dataGridVlasnici.AllowUserToAddRows = false;
            this.dataGridVlasnici.AllowUserToDeleteRows = false;
            this.dataGridVlasnici.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridVlasnici.ReadOnly = true;
            this.dataGridVlasnici.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridVlasnici.MultiSelect = false;
            this.dataGridVlasnici.RowHeadersVisible = false;

            panelVlasnici.Controls.Add(this.dataGridVlasnici);
            panelVlasnici.Controls.Add(panelVlasniciBtns);

            splitBottom.Panel1.Controls.Add(panelVlasnici);

            // DIREKTORI
            var panelDirektori = new System.Windows.Forms.Panel();
            panelDirektori.Dock = System.Windows.Forms.DockStyle.Fill;

            var panelDirektoriBtns = new System.Windows.Forms.Panel();
            panelDirektoriBtns.Dock = System.Windows.Forms.DockStyle.Top;
            panelDirektoriBtns.Height = 40;
            panelDirektoriBtns.BackColor = System.Drawing.Color.LightGray;

            this.btnDodajDirektora.Location = new System.Drawing.Point(10, 8);
            this.btnDodajDirektora.Size = new System.Drawing.Size(130, 25);
            this.btnDodajDirektora.Text = "➕ Dodaj direktora";
            this.btnDodajDirektora.UseVisualStyleBackColor = true;
            this.btnDodajDirektora.Click += btnDodajDirektora_Click;

            this.btnIzmijeniDirektora.Location = new System.Drawing.Point(150, 8);
            this.btnIzmijeniDirektora.Size = new System.Drawing.Size(100, 25);
            this.btnIzmijeniDirektora.Text = "✏️ Izmijeni";
            this.btnIzmijeniDirektora.UseVisualStyleBackColor = true;
            this.btnIzmijeniDirektora.Click += btnIzmijeniDirektora_Click;

            this.btnObrisiDirektora.Location = new System.Drawing.Point(260, 8);
            this.btnObrisiDirektora.Size = new System.Drawing.Size(100, 25);
            this.btnObrisiDirektora.Text = "❌ Obriši";
            this.btnObrisiDirektora.UseVisualStyleBackColor = true;
            this.btnObrisiDirektora.Click += btnObrisiDirektora_Click;

            panelDirektoriBtns.Controls.Add(this.btnDodajDirektora);
            panelDirektoriBtns.Controls.Add(this.btnIzmijeniDirektora);
            panelDirektoriBtns.Controls.Add(this.btnObrisiDirektora);

            // POPRAVKA: Dodao SelectionMode i MultiSelect za direktore
            this.dataGridDirektori.AllowUserToAddRows = false;
            this.dataGridDirektori.AllowUserToDeleteRows = false;
            this.dataGridDirektori.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridDirektori.ReadOnly = true;
            this.dataGridDirektori.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridDirektori.MultiSelect = false;
            this.dataGridDirektori.RowHeadersVisible = false;

            panelDirektori.Controls.Add(this.dataGridDirektori);
            panelDirektori.Controls.Add(panelDirektoriBtns);

            splitBottom.Panel2.Controls.Add(panelDirektori);

            this.splitMain.Panel2.Controls.Add(splitBottom);

            // DODAJ SVE NA FORMU
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.panelToolbar);

            ((System.ComponentModel.ISupportInitialize)(this.dataGridKlijenti)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridVlasnici)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDirektori)).EndInit();
            this.ResumeLayout(false);
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
    }
}