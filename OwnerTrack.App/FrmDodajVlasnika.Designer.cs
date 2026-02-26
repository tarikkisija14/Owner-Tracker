namespace OwnerTrack.App
{
    partial class FrmDodajVlasnika
    {
        private System.ComponentModel.IContainer components = null;

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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblImePrezime = new System.Windows.Forms.Label();
            this.txtImePrezime = new System.Windows.Forms.TextBox();
            this.lblDatumValjanosti = new System.Windows.Forms.Label();
            this.dtDatumValjanosti = new System.Windows.Forms.DateTimePicker();
            this.lblProcetat = new System.Windows.Forms.Label();
            this.txtProcetat = new System.Windows.Forms.TextBox();
            this.lblDatumUtvrdjivanja = new System.Windows.Forms.Label();
            this.dtDatumUtvrdjivanja = new System.Windows.Forms.DateTimePicker();
            this.lblIzvorPodatka = new System.Windows.Forms.Label();
            this.txtIzvorPodatka = new System.Windows.Forms.TextBox();
            this.btnSpremi = new System.Windows.Forms.Button();
            this.btnOtkazi = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // ── Shared styles ─────────────────────────────────────
            var uiFont = new System.Drawing.Font("Segoe UI", 9.5F);
            var labelColor = System.Drawing.Color.FromArgb(45, 55, 75);
            var accentBlue = System.Drawing.Color.FromArgb(28, 40, 65);
            var inputBg = System.Drawing.Color.FromArgb(250, 252, 255);

            // ── FORMA ─────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 360);
            this.Text = "Dodaj vlasnika";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.Font = uiFont;
            this.Load += FrmDodajVlasnika_Load;

            // ── GROUPBOX ──────────────────────────────────────────
            this.groupBox1.Text = "👤 Podaci vlasnika";
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Size = new System.Drawing.Size(562, 270);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = accentBlue;
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);

            // Ime i prezime
            this.lblImePrezime.Text = "Ime i prezime:";
            this.lblImePrezime.Location = new System.Drawing.Point(12, 34);
            this.lblImePrezime.AutoSize = true;
            this.lblImePrezime.Font = uiFont;
            this.lblImePrezime.ForeColor = labelColor;

            this.txtImePrezime.Location = new System.Drawing.Point(165, 32);
            this.txtImePrezime.Size = new System.Drawing.Size(375, 24);
            this.txtImePrezime.Font = uiFont;
            this.txtImePrezime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtImePrezime.BackColor = inputBg;

            // Datum važenja dokumenta
            this.lblDatumValjanosti.Text = "Datum važenja dok.:";
            this.lblDatumValjanosti.Location = new System.Drawing.Point(12, 74);
            this.lblDatumValjanosti.AutoSize = true;
            this.lblDatumValjanosti.Font = uiFont;
            this.lblDatumValjanosti.ForeColor = labelColor;

            this.dtDatumValjanosti.Location = new System.Drawing.Point(165, 72);
            this.dtDatumValjanosti.Size = new System.Drawing.Size(210, 24);
            this.dtDatumValjanosti.Font = uiFont;

            // Vlasništvo
            this.lblProcetat.Text = "Vlasništvo (%):";
            this.lblProcetat.Location = new System.Drawing.Point(12, 114);
            this.lblProcetat.AutoSize = true;
            this.lblProcetat.Font = uiFont;
            this.lblProcetat.ForeColor = labelColor;

            this.txtProcetat.Location = new System.Drawing.Point(165, 112);
            this.txtProcetat.Size = new System.Drawing.Size(110, 24);
            this.txtProcetat.Font = uiFont;
            this.txtProcetat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtProcetat.BackColor = inputBg;

            // Datum utvrđivanja
            this.lblDatumUtvrdjivanja.Text = "Datum utvrđivanja:";
            this.lblDatumUtvrdjivanja.Location = new System.Drawing.Point(12, 154);
            this.lblDatumUtvrdjivanja.AutoSize = true;
            this.lblDatumUtvrdjivanja.Font = uiFont;
            this.lblDatumUtvrdjivanja.ForeColor = labelColor;

            this.dtDatumUtvrdjivanja.Location = new System.Drawing.Point(165, 152);
            this.dtDatumUtvrdjivanja.Size = new System.Drawing.Size(210, 24);
            this.dtDatumUtvrdjivanja.Font = uiFont;

            // Izvor podatka
            this.lblIzvorPodatka.Text = "Izvor podatka:";
            this.lblIzvorPodatka.Location = new System.Drawing.Point(12, 194);
            this.lblIzvorPodatka.AutoSize = true;
            this.lblIzvorPodatka.Font = uiFont;
            this.lblIzvorPodatka.ForeColor = labelColor;

            this.txtIzvorPodatka.Location = new System.Drawing.Point(165, 192);
            this.txtIzvorPodatka.Size = new System.Drawing.Size(375, 24);
            this.txtIzvorPodatka.Font = uiFont;
            this.txtIzvorPodatka.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtIzvorPodatka.BackColor = inputBg;

            this.groupBox1.Controls.Add(this.lblImePrezime);
            this.groupBox1.Controls.Add(this.txtImePrezime);
            this.groupBox1.Controls.Add(this.lblDatumValjanosti);
            this.groupBox1.Controls.Add(this.dtDatumValjanosti);
            this.groupBox1.Controls.Add(this.lblProcetat);
            this.groupBox1.Controls.Add(this.txtProcetat);
            this.groupBox1.Controls.Add(this.lblDatumUtvrdjivanja);
            this.groupBox1.Controls.Add(this.dtDatumUtvrdjivanja);
            this.groupBox1.Controls.Add(this.lblIzvorPodatka);
            this.groupBox1.Controls.Add(this.txtIzvorPodatka);

            // ── DUGMICI ───────────────────────────────────────────
            this.btnSpremi.Location = new System.Drawing.Point(218, 305);
            this.btnSpremi.Size = new System.Drawing.Size(160, 36);
            this.btnSpremi.Text = "💾 Dodaj";
            this.btnSpremi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSpremi.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.btnSpremi.ForeColor = System.Drawing.Color.White;
            this.btnSpremi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSpremi.FlatAppearance.BorderSize = 0;
            this.btnSpremi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSpremi.UseVisualStyleBackColor = false;
            this.btnSpremi.Click += btnSpremi_Click;

            this.btnOtkazi.Location = new System.Drawing.Point(390, 305);
            this.btnOtkazi.Size = new System.Drawing.Size(160, 36);
            this.btnOtkazi.Text = "❌ Otkaži";
            this.btnOtkazi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOtkazi.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.btnOtkazi.ForeColor = System.Drawing.Color.White;
            this.btnOtkazi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOtkazi.FlatAppearance.BorderSize = 0;
            this.btnOtkazi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOtkazi.UseVisualStyleBackColor = false;
            this.btnOtkazi.Click += btnOtkazi_Click;

            // ── DODAJ SVE ─────────────────────────────────────────
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSpremi);
            this.Controls.Add(this.btnOtkazi);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblImePrezime;
        public System.Windows.Forms.TextBox txtImePrezime;
        private System.Windows.Forms.Label lblDatumValjanosti;
        public System.Windows.Forms.DateTimePicker dtDatumValjanosti;
        private System.Windows.Forms.Label lblProcetat;
        public System.Windows.Forms.TextBox txtProcetat;
        private System.Windows.Forms.Label lblDatumUtvrdjivanja;
        public System.Windows.Forms.DateTimePicker dtDatumUtvrdjivanja;
        private System.Windows.Forms.Label lblIzvorPodatka;
        public System.Windows.Forms.TextBox txtIzvorPodatka;
        public System.Windows.Forms.Button btnSpremi;
        public System.Windows.Forms.Button btnOtkazi;
    }
}