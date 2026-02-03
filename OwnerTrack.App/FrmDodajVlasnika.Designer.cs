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
            this.lblProcetat = new System.Windows.Forms.Label();
            this.txtProcetat = new System.Windows.Forms.TextBox();
            this.lblDatumUtvrdjivanja = new System.Windows.Forms.Label();
            this.dtDatumUtvrdjivanja = new System.Windows.Forms.DateTimePicker();
            this.lblIzvorPodatka = new System.Windows.Forms.Label();
            this.txtIzvorPodatka = new System.Windows.Forms.TextBox();
            this.btnSpremi = new System.Windows.Forms.Button();
            this.btnOtkazi = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // ========== FORMA ==========
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Text = "Dodaj vlasnika";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Load += FrmDodajVlasnika_Load;

            // ========== GROUPBOX ==========
            this.groupBox1.Text = "👤 Podaci vlasnika";
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Size = new System.Drawing.Size(560, 200);
            this.groupBox1.AutoSize = true;

            this.lblImePrezime.Text = "Ime i prezime:";
            this.lblImePrezime.Location = new System.Drawing.Point(10, 30);
            this.lblImePrezime.AutoSize = true;
            this.txtImePrezime.Location = new System.Drawing.Point(180, 30);
            this.txtImePrezime.Size = new System.Drawing.Size(350, 20);

            // POPRAVKA: Skraćen tekst labele i pomjeren TextBox
            this.lblProcetat.Text = "Vlasništvo (%):";
            this.lblProcetat.Location = new System.Drawing.Point(10, 70);
            this.lblProcetat.AutoSize = true;
            this.txtProcetat.Location = new System.Drawing.Point(180, 70);
            this.txtProcetat.Size = new System.Drawing.Size(100, 20);

            this.lblDatumUtvrdjivanja.Text = "Datum utvrđivanja:";
            this.lblDatumUtvrdjivanja.Location = new System.Drawing.Point(10, 110);
            this.lblDatumUtvrdjivanja.AutoSize = true;
            this.dtDatumUtvrdjivanja.Location = new System.Drawing.Point(180, 110);
            this.dtDatumUtvrdjivanja.Size = new System.Drawing.Size(200, 20);

            this.lblIzvorPodatka.Text = "Izvor podatka:";
            this.lblIzvorPodatka.Location = new System.Drawing.Point(10, 150);
            this.lblIzvorPodatka.AutoSize = true;
            this.txtIzvorPodatka.Location = new System.Drawing.Point(180, 150);
            this.txtIzvorPodatka.Size = new System.Drawing.Size(350, 20);

            this.groupBox1.Controls.Add(this.lblImePrezime);
            this.groupBox1.Controls.Add(this.txtImePrezime);
            this.groupBox1.Controls.Add(this.lblProcetat);
            this.groupBox1.Controls.Add(this.txtProcetat);
            this.groupBox1.Controls.Add(this.lblDatumUtvrdjivanja);
            this.groupBox1.Controls.Add(this.dtDatumUtvrdjivanja);
            this.groupBox1.Controls.Add(this.lblIzvorPodatka);
            this.groupBox1.Controls.Add(this.txtIzvorPodatka);

            // ========== DUGMICI ==========
            this.btnSpremi.Location = new System.Drawing.Point(240, 240);
            this.btnSpremi.Size = new System.Drawing.Size(150, 30);
            this.btnSpremi.Text = "💾 Dodaj";
            this.btnSpremi.UseVisualStyleBackColor = true;
            this.btnSpremi.Click += btnSpremi_Click;

            this.btnOtkazi.Location = new System.Drawing.Point(400, 240);
            this.btnOtkazi.Size = new System.Drawing.Size(150, 30);
            this.btnOtkazi.Text = "❌ Otkazi";
            this.btnOtkazi.UseVisualStyleBackColor = true;
            this.btnOtkazi.Click += btnOtkazi_Click;

            // DODAJ SVE
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSpremi);
            this.Controls.Add(this.btnOtkazi);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblImePrezime;
        public System.Windows.Forms.TextBox txtImePrezime;
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