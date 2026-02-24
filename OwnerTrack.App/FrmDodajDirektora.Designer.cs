namespace OwnerTrack.App
{
    partial class FrmDodajDirektora
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
            this.lblTipValjanosti = new System.Windows.Forms.Label();
            this.cbTipValjanosti = new System.Windows.Forms.ComboBox();
            this.btnSpremi = new System.Windows.Forms.Button();
            this.btnOtkazi = new System.Windows.Forms.Button();
            this.lblJmbg = new System.Windows.Forms.Label();
            this.txtJmbg = new System.Windows.Forms.TextBox();



            this.SuspendLayout();

            // ========== FORMA ==========
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Text = "Dodaj direktora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Load += FrmDodajDirektora_Load;  // POPRAVKA: Dodao Load event

            // ========== GROUPBOX ==========
            this.groupBox1.Text = "👔 Podaci direktora";
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Size = new System.Drawing.Size(560, 200);
            this.groupBox1.AutoSize = true;

            this.lblImePrezime.Text = "Ime i prezime:";
            this.lblImePrezime.Location = new System.Drawing.Point(10, 30);
            this.lblImePrezime.AutoSize = true;
            this.txtImePrezime.Location = new System.Drawing.Point(150, 30);
            this.txtImePrezime.Size = new System.Drawing.Size(350, 20);

            this.lblTipValjanosti.Text = "Tip valjanosti:";
            this.lblTipValjanosti.Location = new System.Drawing.Point(10, 70);
            this.lblTipValjanosti.AutoSize = true;
            this.cbTipValjanosti.Location = new System.Drawing.Point(150, 70);
            this.cbTipValjanosti.Size = new System.Drawing.Size(150, 21);
            this.cbTipValjanosti.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTipValjanosti.SelectedIndexChanged += cbTipValjanosti_SelectedIndexChanged;

            this.lblDatumValjanosti.Text = "Datum važenja:";
            this.lblDatumValjanosti.Location = new System.Drawing.Point(10, 110);
            this.lblDatumValjanosti.AutoSize = true;
            this.dtDatumValjanosti.Location = new System.Drawing.Point(150, 110);
            this.dtDatumValjanosti.Size = new System.Drawing.Size(200, 20);
            this.dtDatumValjanosti.Enabled = true;


            this.lblJmbg.Text = "JMBG:";
            this.lblJmbg.Location = new System.Drawing.Point(10, 150);
            this.lblJmbg.AutoSize = true;

            this.txtJmbg.Location = new System.Drawing.Point(150, 150);
            this.txtJmbg.Size = new System.Drawing.Size(200, 20);
            this.txtJmbg.MaxLength = 13;
            this.txtJmbg.Name = "txtJmbg";

            this.groupBox1.Controls.Add(this.lblImePrezime);
            this.groupBox1.Controls.Add(this.txtImePrezime);
            this.groupBox1.Controls.Add(this.lblTipValjanosti);
            this.groupBox1.Controls.Add(this.cbTipValjanosti);
            this.groupBox1.Controls.Add(this.lblDatumValjanosti);
            this.groupBox1.Controls.Add(this.dtDatumValjanosti);
            this.groupBox1.Controls.Add(this.lblJmbg);
            this.groupBox1.Controls.Add(this.txtJmbg);

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
        private System.Windows.Forms.Label lblDatumValjanosti;
        public System.Windows.Forms.DateTimePicker dtDatumValjanosti;
        private System.Windows.Forms.Label lblTipValjanosti;
        public System.Windows.Forms.ComboBox cbTipValjanosti;
        public System.Windows.Forms.Button btnSpremi;
        public System.Windows.Forms.Button btnOtkazi;
        private System.Windows.Forms.Label lblJmbg;
        private System.Windows.Forms.TextBox txtJmbg;
    }
}