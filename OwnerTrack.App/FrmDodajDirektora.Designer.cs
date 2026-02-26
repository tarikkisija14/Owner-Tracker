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

            // ── Shared styles ─────────────────────────────────────
            var uiFont = new System.Drawing.Font("Segoe UI", 9.5F);
            var labelColor = System.Drawing.Color.FromArgb(45, 55, 75);
            var accentBlue = System.Drawing.Color.FromArgb(28, 40, 65);
            var inputBg = System.Drawing.Color.FromArgb(250, 252, 255);

            // ── FORMA ─────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 310);
            this.Text = "Dodaj direktora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            this.Font = uiFont;
            this.Load += FrmDodajDirektora_Load;

            // ── GROUPBOX ──────────────────────────────────────────
            this.groupBox1.Text = "👔 Podaci direktora";
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Size = new System.Drawing.Size(562, 220);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = accentBlue;
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.AutoSize = false;

            // Ime i prezime
            this.lblImePrezime.Text = "Ime i prezime:";
            this.lblImePrezime.Location = new System.Drawing.Point(12, 34);
            this.lblImePrezime.AutoSize = true;
            this.lblImePrezime.Font = uiFont;
            this.lblImePrezime.ForeColor = labelColor;

            this.txtImePrezime.Location = new System.Drawing.Point(155, 32);
            this.txtImePrezime.Size = new System.Drawing.Size(390, 24);
            this.txtImePrezime.Font = uiFont;
            this.txtImePrezime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtImePrezime.BackColor = inputBg;

            // Tip valjanosti
            this.lblTipValjanosti.Text = "Tip valjanosti:";
            this.lblTipValjanosti.Location = new System.Drawing.Point(12, 74);
            this.lblTipValjanosti.AutoSize = true;
            this.lblTipValjanosti.Font = uiFont;
            this.lblTipValjanosti.ForeColor = labelColor;

            this.cbTipValjanosti.Location = new System.Drawing.Point(155, 72);
            this.cbTipValjanosti.Size = new System.Drawing.Size(170, 24);
            this.cbTipValjanosti.Font = uiFont;
            this.cbTipValjanosti.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTipValjanosti.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbTipValjanosti.SelectedIndexChanged += cbTipValjanosti_SelectedIndexChanged;

            // Datum važenja
            this.lblDatumValjanosti.Text = "Datum važenja:";
            this.lblDatumValjanosti.Location = new System.Drawing.Point(12, 114);
            this.lblDatumValjanosti.AutoSize = true;
            this.lblDatumValjanosti.Font = uiFont;
            this.lblDatumValjanosti.ForeColor = labelColor;

            this.dtDatumValjanosti.Location = new System.Drawing.Point(155, 112);
            this.dtDatumValjanosti.Size = new System.Drawing.Size(210, 24);
            this.dtDatumValjanosti.Font = uiFont;
            this.dtDatumValjanosti.Enabled = true;

            // JMBG
            this.lblJmbg.Text = "JMBG:";
            this.lblJmbg.Location = new System.Drawing.Point(12, 154);
            this.lblJmbg.AutoSize = true;
            this.lblJmbg.Font = uiFont;
            this.lblJmbg.ForeColor = labelColor;

            this.txtJmbg.Location = new System.Drawing.Point(155, 152);
            this.txtJmbg.Size = new System.Drawing.Size(210, 24);
            this.txtJmbg.MaxLength = 13;
            this.txtJmbg.Name = "txtJmbg";
            this.txtJmbg.Font = uiFont;
            this.txtJmbg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJmbg.BackColor = inputBg;

            this.groupBox1.Controls.Add(this.lblImePrezime);
            this.groupBox1.Controls.Add(this.txtImePrezime);
            this.groupBox1.Controls.Add(this.lblTipValjanosti);
            this.groupBox1.Controls.Add(this.cbTipValjanosti);
            this.groupBox1.Controls.Add(this.lblDatumValjanosti);
            this.groupBox1.Controls.Add(this.dtDatumValjanosti);
            this.groupBox1.Controls.Add(this.lblJmbg);
            this.groupBox1.Controls.Add(this.txtJmbg);

            // ── DUGMICI ───────────────────────────────────────────
            this.btnSpremi.Location = new System.Drawing.Point(218, 254);
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

            this.btnOtkazi.Location = new System.Drawing.Point(390, 254);
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
        private System.Windows.Forms.Label lblTipValjanosti;
        public System.Windows.Forms.ComboBox cbTipValjanosti;
        public System.Windows.Forms.Button btnSpremi;
        public System.Windows.Forms.Button btnOtkazi;
        private System.Windows.Forms.Label lblJmbg;
        private System.Windows.Forms.TextBox txtJmbg;
    }
}