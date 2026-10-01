using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

namespace OwnerTrack.App
{
    partial class FrmPromjenaLozinke
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
            this.lblTrenutna = new System.Windows.Forms.Label();
            this.txtTrenutna = new System.Windows.Forms.TextBox();
            this.lblNova = new System.Windows.Forms.Label();
            this.txtNova = new System.Windows.Forms.TextBox();
            this.lblPotvrda = new System.Windows.Forms.Label();
            this.txtPotvrda = new System.Windows.Forms.TextBox();
            this.btnSpremi = new IconButton();
            this.btnOtkazi = new IconButton();

            this.SuspendLayout();

            var uiFont = UiTheme.Base(9.5f);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 250);
            this.Text = UiMessages.PasswordChangeTitle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = UiTheme.FormBackgroundDialog;
            this.Font = uiFont;
            this.Load += FrmPromjenaLozinke_Load;

            this.lblTrenutna.Text = "Trenutna lozinka:";
            this.lblTrenutna.Location = new System.Drawing.Point(18, 28);
            UiTheme.StyleLabel(this.lblTrenutna);
            this.txtTrenutna.Location = new System.Drawing.Point(170, 26);
            this.txtTrenutna.Size = new System.Drawing.Size(270, 26);
            this.txtTrenutna.UseSystemPasswordChar = true;
            UiTheme.StyleTextBox(this.txtTrenutna);

            this.lblNova.Text = "Nova lozinka:";
            this.lblNova.Location = new System.Drawing.Point(18, 74);
            UiTheme.StyleLabel(this.lblNova);
            this.txtNova.Location = new System.Drawing.Point(170, 72);
            this.txtNova.Size = new System.Drawing.Size(270, 26);
            this.txtNova.UseSystemPasswordChar = true;
            UiTheme.StyleTextBox(this.txtNova);

            this.lblPotvrda.Text = "Ponovi lozinku:";
            this.lblPotvrda.Location = new System.Drawing.Point(18, 120);
            UiTheme.StyleLabel(this.lblPotvrda);
            this.txtPotvrda.Location = new System.Drawing.Point(170, 118);
            this.txtPotvrda.Size = new System.Drawing.Size(270, 26);
            this.txtPotvrda.UseSystemPasswordChar = true;
            UiTheme.StyleTextBox(this.txtPotvrda);

            this.btnSpremi.Location = new System.Drawing.Point(118, 190);
            this.btnSpremi.Size = new System.Drawing.Size(150, 36);
            this.btnSpremi.Text = "Sačuvaj";
            this.btnSpremi.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnSpremi, UiTheme.Blue, 10f);
            this.btnSpremi.Click += btnSpremi_Click;

            this.btnOtkazi.Location = new System.Drawing.Point(280, 190);
            this.btnOtkazi.Size = new System.Drawing.Size(150, 36);
            this.btnOtkazi.Text = "Otkaži";
            this.btnOtkazi.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnOtkazi, UiTheme.Red, 10f);
            this.btnOtkazi.Click += btnOtkazi_Click;

            this.Controls.Add(this.lblTrenutna);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtTrenutna, new System.Drawing.Point(170, 26), new System.Drawing.Size(270, 26)));
            this.Controls.Add(this.lblNova);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtNova, new System.Drawing.Point(170, 72), new System.Drawing.Size(270, 26)));
            this.Controls.Add(this.lblPotvrda);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtPotvrda, new System.Drawing.Point(170, 118), new System.Drawing.Size(270, 26)));
            this.Controls.Add(this.btnSpremi);
            this.Controls.Add(this.btnOtkazi);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTrenutna;
        private System.Windows.Forms.TextBox txtTrenutna;
        private System.Windows.Forms.Label lblNova;
        private System.Windows.Forms.TextBox txtNova;
        private System.Windows.Forms.Label lblPotvrda;
        private System.Windows.Forms.TextBox txtPotvrda;
        private IconButton btnSpremi;
        private IconButton btnOtkazi;
    }
}
