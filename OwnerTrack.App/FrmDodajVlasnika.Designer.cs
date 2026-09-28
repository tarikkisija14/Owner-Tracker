using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

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
            this.btnSpremi = new IconButton();
            this.btnOtkazi = new IconButton();

            this.SuspendLayout();

            // ── Shared styles ─────────────────────────────────────
            var uiFont = UiTheme.Base(9.5f);

            // ── FORMA ─────────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 360);
            this.Text = "Dodaj vlasnika";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = UiTheme.FormBackgroundDialog;
            this.Font = uiFont;
            this.Load += FrmDodajVlasnika_Load;

            // ── GROUPBOX ──────────────────────────────────────────
            UiTheme.StyleGroupBox(this.groupBox1, "👤 Podaci vlasnika");
            this.groupBox1.Location = new System.Drawing.Point(18, 18);
            this.groupBox1.Size = new System.Drawing.Size(562, 270);

            // Ime i prezime
            this.lblImePrezime.Text = "Ime i prezime:";
            this.lblImePrezime.Location = new System.Drawing.Point(12, 34);
            UiTheme.StyleLabel(this.lblImePrezime);

            this.txtImePrezime.Location = new System.Drawing.Point(165, 32);
            this.txtImePrezime.Size = new System.Drawing.Size(375, 24);
            UiTheme.StyleTextBox(this.txtImePrezime);

            // Datum važenja dokumenta
            this.lblDatumValjanosti.Text = "Datum važenja dok.:";
            this.lblDatumValjanosti.Location = new System.Drawing.Point(12, 74);
            UiTheme.StyleLabel(this.lblDatumValjanosti);

            this.dtDatumValjanosti.Location = new System.Drawing.Point(165, 72);
            this.dtDatumValjanosti.Size = new System.Drawing.Size(210, 24);
            UiTheme.StyleDateTimePicker(this.dtDatumValjanosti, 9.5f);
            this.dtDatumValjanosti.ShowCheckBox = true;

            // Vlasništvo
            this.lblProcetat.Text = "Vlasništvo (%):";
            this.lblProcetat.Location = new System.Drawing.Point(12, 114);
            UiTheme.StyleLabel(this.lblProcetat);

            this.txtProcetat.Location = new System.Drawing.Point(165, 112);
            this.txtProcetat.Size = new System.Drawing.Size(110, 24);
            UiTheme.StyleTextBox(this.txtProcetat);

            // Datum utvrđivanja
            this.lblDatumUtvrdjivanja.Text = "Datum utvrđivanja:";
            this.lblDatumUtvrdjivanja.Location = new System.Drawing.Point(12, 154);
            UiTheme.StyleLabel(this.lblDatumUtvrdjivanja);

            this.dtDatumUtvrdjivanja.Location = new System.Drawing.Point(165, 152);
            this.dtDatumUtvrdjivanja.Size = new System.Drawing.Size(210, 24);
            UiTheme.StyleDateTimePicker(this.dtDatumUtvrdjivanja, 9.5f);

            // Izvor podatka
            this.lblIzvorPodatka.Text = "Izvor podatka:";
            this.lblIzvorPodatka.Location = new System.Drawing.Point(12, 194);
            UiTheme.StyleLabel(this.lblIzvorPodatka);

            this.txtIzvorPodatka.Location = new System.Drawing.Point(165, 192);
            this.txtIzvorPodatka.Size = new System.Drawing.Size(375, 24);
            UiTheme.StyleTextBox(this.txtIzvorPodatka);

            this.groupBox1.Controls.Add(this.lblImePrezime);
            this.groupBox1.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtImePrezime, new System.Drawing.Point(165, 32), new System.Drawing.Size(375, 24)));
            this.groupBox1.Controls.Add(this.lblDatumValjanosti);
            this.groupBox1.Controls.Add(this.dtDatumValjanosti);
            this.groupBox1.Controls.Add(this.lblProcetat);
            this.groupBox1.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtProcetat, new System.Drawing.Point(165, 112), new System.Drawing.Size(110, 24)));
            this.groupBox1.Controls.Add(this.lblDatumUtvrdjivanja);
            this.groupBox1.Controls.Add(this.dtDatumUtvrdjivanja);
            this.groupBox1.Controls.Add(this.lblIzvorPodatka);
            this.groupBox1.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtIzvorPodatka, new System.Drawing.Point(165, 192), new System.Drawing.Size(375, 24)));

            // ── DUGMICI ───────────────────────────────────────────
            this.btnSpremi.Location = new System.Drawing.Point(218, 305);
            this.btnSpremi.Size = new System.Drawing.Size(160, 36);
            this.btnSpremi.Text = "Dodaj";
            this.btnSpremi.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnSpremi, UiTheme.Green, 10f);
            this.btnSpremi.Click += btnSpremi_Click;

            this.btnOtkazi.Location = new System.Drawing.Point(390, 305);
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
        private System.Windows.Forms.Label lblProcetat;
        public System.Windows.Forms.TextBox txtProcetat;
        private System.Windows.Forms.Label lblDatumUtvrdjivanja;
        public System.Windows.Forms.DateTimePicker dtDatumUtvrdjivanja;
        private System.Windows.Forms.Label lblIzvorPodatka;
        public System.Windows.Forms.TextBox txtIzvorPodatka;
        public IconButton btnSpremi;
        public IconButton btnOtkazi;
    }
}