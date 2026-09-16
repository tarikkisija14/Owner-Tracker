using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

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
            this.btnSpremi = new IconButton();
            this.btnOtkazi = new IconButton();
            this.lblJmbg = new System.Windows.Forms.Label();
            this.txtJmbg = new System.Windows.Forms.TextBox();

            this.SuspendLayout();

            // ── Shared styles ─────────────────────────────────────
            var uiFont = UiTheme.Base(9.5f);

            // ── FORMA ─────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 310);
            this.Text = "Dodaj direktora";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = UiTheme.FormBackgroundDialog;
            this.Font = uiFont;
            this.Load += FrmDodajDirektora_Load;

            // ── GROUPBOX ──────────────────────────────────────────
            UiTheme.StyleGroupBox(this.groupBox1, "👔 Podaci direktora");
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Size = new System.Drawing.Size(562, 220);
            this.groupBox1.AutoSize = false;

            // Ime i prezime
            this.lblImePrezime.Text = "Ime i prezime:";
            this.lblImePrezime.Location = new System.Drawing.Point(12, 34);
            UiTheme.StyleLabel(this.lblImePrezime);

            this.txtImePrezime.Location = new System.Drawing.Point(155, 32);
            this.txtImePrezime.Size = new System.Drawing.Size(390, 24);
            UiTheme.StyleTextBox(this.txtImePrezime);

            // Tip valjanosti
            this.lblTipValjanosti.Text = "Tip valjanosti:";
            this.lblTipValjanosti.Location = new System.Drawing.Point(12, 74);
            UiTheme.StyleLabel(this.lblTipValjanosti);

            this.cbTipValjanosti.Location = new System.Drawing.Point(155, 72);
            this.cbTipValjanosti.Size = new System.Drawing.Size(170, 24);
            UiTheme.StyleComboBox(this.cbTipValjanosti);
            this.cbTipValjanosti.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTipValjanosti.SelectedIndexChanged += cbTipValjanosti_SelectedIndexChanged;

            // Datum važenja
            this.lblDatumValjanosti.Text = "Datum važenja:";
            this.lblDatumValjanosti.Location = new System.Drawing.Point(12, 114);
            UiTheme.StyleLabel(this.lblDatumValjanosti);

            this.dtDatumValjanosti.Location = new System.Drawing.Point(155, 112);
            this.dtDatumValjanosti.Size = new System.Drawing.Size(210, 24);
            this.dtDatumValjanosti.Font = uiFont;
            this.dtDatumValjanosti.Enabled = true;
            this.dtDatumValjanosti.ShowCheckBox = true;

            // JMBG
            this.lblJmbg.Text = "JMBG:";
            this.lblJmbg.Location = new System.Drawing.Point(12, 154);
            UiTheme.StyleLabel(this.lblJmbg);

            this.txtJmbg.Location = new System.Drawing.Point(155, 152);
            this.txtJmbg.Size = new System.Drawing.Size(210, 24);
            this.txtJmbg.MaxLength = 13;
            this.txtJmbg.Name = "txtJmbg";
            UiTheme.StyleTextBox(this.txtJmbg);

            this.groupBox1.Controls.Add(this.lblImePrezime);
            this.groupBox1.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtImePrezime, new System.Drawing.Point(155, 32), new System.Drawing.Size(390, 24)));
            this.groupBox1.Controls.Add(this.lblTipValjanosti);
            this.groupBox1.Controls.Add(this.cbTipValjanosti);
            this.groupBox1.Controls.Add(this.lblDatumValjanosti);
            this.groupBox1.Controls.Add(this.dtDatumValjanosti);
            this.groupBox1.Controls.Add(this.lblJmbg);
            this.groupBox1.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtJmbg, new System.Drawing.Point(155, 152), new System.Drawing.Size(210, 24)));

            // ── DUGMICI ───────────────────────────────────────────
            this.btnSpremi.Location = new System.Drawing.Point(218, 254);
            this.btnSpremi.Size = new System.Drawing.Size(160, 36);
            this.btnSpremi.Text = "Dodaj";
            this.btnSpremi.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnSpremi, UiTheme.Green, 10f);
            this.btnSpremi.Click += btnSpremi_Click;

            this.btnOtkazi.Location = new System.Drawing.Point(390, 254);
            this.btnOtkazi.Size = new System.Drawing.Size(160, 36);
            this.btnOtkazi.Text = "Otkaži";
            this.btnOtkazi.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnOtkazi, UiTheme.Red, 10f);
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
        public IconButton btnSpremi;
        public IconButton btnOtkazi;
        private System.Windows.Forms.Label lblJmbg;
        private System.Windows.Forms.TextBox txtJmbg;
    }
}