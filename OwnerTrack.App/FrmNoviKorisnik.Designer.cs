using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

namespace OwnerTrack.App
{
    partial class FrmNoviKorisnik
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
            this.lblKorisnickoIme = new System.Windows.Forms.Label();
            this.txtKorisnickoIme = new System.Windows.Forms.TextBox();
            this.lblPrikaznoIme = new System.Windows.Forms.Label();
            this.txtPrikaznoIme = new System.Windows.Forms.TextBox();
            this.lblLozinka = new System.Windows.Forms.Label();
            this.txtLozinka = new System.Windows.Forms.TextBox();
            this.lblPotvrda = new System.Windows.Forms.Label();
            this.txtPotvrda = new System.Windows.Forms.TextBox();
            this.btnSpremi = new IconButton();
            this.btnOtkazi = new IconButton();

            this.SuspendLayout();

            var uiFont = UiTheme.Base(9.5f);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 296);
            this.Text = UiMessages.NewUserTitle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = UiTheme.FormBackgroundDialog;
            this.Font = uiFont;
            this.Load += FrmNoviKorisnik_Load;

            this.lblKorisnickoIme.Text = "Korisničko ime:";
            this.lblKorisnickoIme.Location = new System.Drawing.Point(18, 28);
            UiTheme.StyleLabel(this.lblKorisnickoIme);
            this.txtKorisnickoIme.Location = new System.Drawing.Point(170, 26);
            this.txtKorisnickoIme.Size = new System.Drawing.Size(270, 26);
            UiTheme.StyleTextBox(this.txtKorisnickoIme);

            this.lblPrikaznoIme.Text = "Ime i prezime:";
            this.lblPrikaznoIme.Location = new System.Drawing.Point(18, 74);
            UiTheme.StyleLabel(this.lblPrikaznoIme);
            this.txtPrikaznoIme.Location = new System.Drawing.Point(170, 72);
            this.txtPrikaznoIme.Size = new System.Drawing.Size(270, 26);
            UiTheme.StyleTextBox(this.txtPrikaznoIme);

            this.lblLozinka.Text = "Lozinka:";
            this.lblLozinka.Location = new System.Drawing.Point(18, 120);
            UiTheme.StyleLabel(this.lblLozinka);
            this.txtLozinka.Location = new System.Drawing.Point(170, 118);
            this.txtLozinka.Size = new System.Drawing.Size(270, 26);
            this.txtLozinka.UseSystemPasswordChar = true;
            UiTheme.StyleTextBox(this.txtLozinka);

            this.lblPotvrda.Text = "Ponovi lozinku:";
            this.lblPotvrda.Location = new System.Drawing.Point(18, 166);
            UiTheme.StyleLabel(this.lblPotvrda);
            this.txtPotvrda.Location = new System.Drawing.Point(170, 164);
            this.txtPotvrda.Size = new System.Drawing.Size(270, 26);
            this.txtPotvrda.UseSystemPasswordChar = true;
            UiTheme.StyleTextBox(this.txtPotvrda);

            this.btnSpremi.Location = new System.Drawing.Point(118, 236);
            this.btnSpremi.Size = new System.Drawing.Size(150, 36);
            this.btnSpremi.Text = "Dodaj";
            this.btnSpremi.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnSpremi, UiTheme.Blue, 10f);
            this.btnSpremi.Click += btnSpremi_Click;

            this.btnOtkazi.Location = new System.Drawing.Point(280, 236);
            this.btnOtkazi.Size = new System.Drawing.Size(150, 36);
            this.btnOtkazi.Text = "Otkaži";
            this.btnOtkazi.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnOtkazi, UiTheme.Red, 10f);
            this.btnOtkazi.Click += btnOtkazi_Click;

            this.Controls.Add(this.lblKorisnickoIme);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtKorisnickoIme, new System.Drawing.Point(170, 26), new System.Drawing.Size(270, 26)));
            this.Controls.Add(this.lblPrikaznoIme);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtPrikaznoIme, new System.Drawing.Point(170, 72), new System.Drawing.Size(270, 26)));
            this.Controls.Add(this.lblLozinka);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtLozinka, new System.Drawing.Point(170, 118), new System.Drawing.Size(270, 26)));
            this.Controls.Add(this.lblPotvrda);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtPotvrda, new System.Drawing.Point(170, 164), new System.Drawing.Size(270, 26)));
            this.Controls.Add(this.btnSpremi);
            this.Controls.Add(this.btnOtkazi);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblKorisnickoIme;
        private System.Windows.Forms.TextBox txtKorisnickoIme;
        private System.Windows.Forms.Label lblPrikaznoIme;
        private System.Windows.Forms.TextBox txtPrikaznoIme;
        private System.Windows.Forms.Label lblLozinka;
        private System.Windows.Forms.TextBox txtLozinka;
        private System.Windows.Forms.Label lblPotvrda;
        private System.Windows.Forms.TextBox txtPotvrda;
        private IconButton btnSpremi;
        private IconButton btnOtkazi;
    }
}
