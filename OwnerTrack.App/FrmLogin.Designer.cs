using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

namespace OwnerTrack.App
{
    partial class FrmLogin
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblHeaderNaziv = new System.Windows.Forms.Label();
            this.lblHeaderPodnaslov = new System.Windows.Forms.Label();
            this.lblKorisnik = new System.Windows.Forms.Label();
            this.txtKorisnik = new System.Windows.Forms.TextBox();
            this.lblLozinka = new System.Windows.Forms.Label();
            this.txtLozinka = new System.Windows.Forms.TextBox();
            this.chkZapamti = new System.Windows.Forms.CheckBox();
            this.chkPrikaziLozinku = new System.Windows.Forms.CheckBox();
            this.btnPrijava = new IconButton();
            this.btnIzlaz = new IconButton();
            this.lblBaza = new System.Windows.Forms.Label();

            this.SuspendLayout();

            var uiFont = UiTheme.Base(9.5f);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 350);
            this.Text = UiMessages.LoginTitle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = UiTheme.FormBackgroundDialog;
            this.Font = uiFont;
            this.Load += FrmLogin_Load;

            // ── panelHeader ───────────────────────────────────────
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 70;
            this.panelHeader.BackColor = UiTheme.Navy;

            this.lblHeaderNaziv.Location = new System.Drawing.Point(24, 10);
            this.lblHeaderNaziv.AutoSize = true;
            this.lblHeaderNaziv.ForeColor = System.Drawing.Color.White;
            this.lblHeaderNaziv.Font = UiTheme.Base(15f, System.Drawing.FontStyle.Bold);
            this.lblHeaderNaziv.Text = "OwnerTrack";

            this.lblHeaderPodnaslov.Location = new System.Drawing.Point(24, 40);
            this.lblHeaderPodnaslov.AutoSize = true;
            this.lblHeaderPodnaslov.ForeColor = UiTheme.HeaderSubText;
            this.lblHeaderPodnaslov.Font = UiTheme.Base(9.5f);
            this.lblHeaderPodnaslov.Text = "Confidia BH — prijava";

            this.panelHeader.Controls.Add(this.lblHeaderPodnaslov);
            this.panelHeader.Controls.Add(this.lblHeaderNaziv);

            // ── Korisničko ime ────────────────────────────────────
            this.lblKorisnik.Text = "Korisničko ime:";
            this.lblKorisnik.Location = new System.Drawing.Point(24, 90);
            UiTheme.StyleLabel(this.lblKorisnik);

            this.txtKorisnik.Location = new System.Drawing.Point(24, 112);
            this.txtKorisnik.Size = new System.Drawing.Size(372, 28);
            UiTheme.StyleTextBox(this.txtKorisnik);

            // ── Lozinka ───────────────────────────────────────────
            this.lblLozinka.Text = "Lozinka:";
            this.lblLozinka.Location = new System.Drawing.Point(24, 156);
            UiTheme.StyleLabel(this.lblLozinka);

            this.txtLozinka.Location = new System.Drawing.Point(24, 178);
            this.txtLozinka.Size = new System.Drawing.Size(372, 28);
            this.txtLozinka.UseSystemPasswordChar = true;
            UiTheme.StyleTextBox(this.txtLozinka);

            // ── Opcije ────────────────────────────────────────────
            this.chkZapamti.Text = "Sačuvaj me";
            this.chkZapamti.Location = new System.Drawing.Point(24, 216);
            this.chkZapamti.AutoSize = true;
            this.chkZapamti.ForeColor = UiTheme.LabelText;
            this.chkZapamti.Font = UiTheme.Base(9.5f);
            this.chkZapamti.Cursor = System.Windows.Forms.Cursors.Hand;

            this.chkPrikaziLozinku.Text = "Prikaži lozinku";
            this.chkPrikaziLozinku.Location = new System.Drawing.Point(262, 216);
            this.chkPrikaziLozinku.AutoSize = true;
            this.chkPrikaziLozinku.ForeColor = UiTheme.LabelText;
            this.chkPrikaziLozinku.Font = UiTheme.Base(9.5f);
            this.chkPrikaziLozinku.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkPrikaziLozinku.CheckedChanged += chkPrikaziLozinku_CheckedChanged;

            // ── Dugmad ────────────────────────────────────────────
            this.btnPrijava.Location = new System.Drawing.Point(24, 262);
            this.btnPrijava.Size = new System.Drawing.Size(180, 36);
            this.btnPrijava.Text = "Prijavi se";
            this.btnPrijava.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnPrijava, UiTheme.Blue, 10f);
            this.btnPrijava.Click += btnPrijava_Click;

            this.btnIzlaz.Location = new System.Drawing.Point(216, 262);
            this.btnIzlaz.Size = new System.Drawing.Size(180, 36);
            this.btnIzlaz.Text = "Izlaz";
            this.btnIzlaz.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnIzlaz, UiTheme.Red, 10f);
            this.btnIzlaz.Click += btnIzlaz_Click;

            // ── Baza (link na postavke baze) ─────────────────────
            this.lblBaza.Location = new System.Drawing.Point(24, 314);
            this.lblBaza.Size = new System.Drawing.Size(372, 20);
            this.lblBaza.AutoSize = false;
            this.lblBaza.AutoEllipsis = true;
            this.lblBaza.ForeColor = UiTheme.MutedText;
            this.lblBaza.Font = UiTheme.Base(8.5f);
            this.lblBaza.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblBaza.Click += lblBaza_Click;

            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.lblBaza);
            this.Controls.Add(this.lblKorisnik);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtKorisnik, new System.Drawing.Point(24, 112), new System.Drawing.Size(372, 28)));
            this.Controls.Add(this.lblLozinka);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtLozinka, new System.Drawing.Point(24, 178), new System.Drawing.Size(372, 28)));
            this.Controls.Add(this.chkZapamti);
            this.Controls.Add(this.chkPrikaziLozinku);
            this.Controls.Add(this.btnPrijava);
            this.Controls.Add(this.btnIzlaz);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblHeaderNaziv;
        private System.Windows.Forms.Label lblHeaderPodnaslov;
        private System.Windows.Forms.Label lblKorisnik;
        private System.Windows.Forms.TextBox txtKorisnik;
        private System.Windows.Forms.Label lblLozinka;
        private System.Windows.Forms.TextBox txtLozinka;
        private System.Windows.Forms.CheckBox chkZapamti;
        private System.Windows.Forms.CheckBox chkPrikaziLozinku;
        private IconButton btnPrijava;
        private IconButton btnIzlaz;
        private System.Windows.Forms.Label lblBaza;
    }
}
