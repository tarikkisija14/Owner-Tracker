using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

namespace OwnerTrack.App
{
    partial class FrmOznaciUpozorenje
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
            this.lblOpis = new System.Windows.Forms.Label();
            this.lblNapomena = new System.Windows.Forms.Label();
            this.txtNapomena = new System.Windows.Forms.TextBox();
            this.btnSpremi = new IconButton();
            this.btnOtkazi = new IconButton();

            this.SuspendLayout();

            var uiFont = UiTheme.Base(9.5f);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 280);
            this.Text = "Označi upozorenje kao pregledano";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = UiTheme.FormBackgroundDialog;
            this.Font = uiFont;
            this.Load += FrmOznaciUpozorenje_Load;

            this.lblOpis.Location = new System.Drawing.Point(18, 18);
            this.lblOpis.Size = new System.Drawing.Size(420, 70);
            UiTheme.StyleLabel(this.lblOpis);

            this.lblNapomena.Text = "Napomena (opcionalno):";
            this.lblNapomena.Location = new System.Drawing.Point(18, 100);
            UiTheme.StyleLabel(this.lblNapomena);

            this.txtNapomena.Location = new System.Drawing.Point(18, 122);
            this.txtNapomena.Size = new System.Drawing.Size(420, 90);
            this.txtNapomena.Multiline = true;
            UiTheme.StyleTextBox(this.txtNapomena);

            this.btnSpremi.Location = new System.Drawing.Point(118, 228);
            this.btnSpremi.Size = new System.Drawing.Size(150, 36);
            this.btnSpremi.Text = "Sačuvaj";
            this.btnSpremi.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnSpremi, UiTheme.Blue, 10f);
            this.btnSpremi.Click += btnSpremi_Click;

            this.btnOtkazi.Location = new System.Drawing.Point(280, 228);
            this.btnOtkazi.Size = new System.Drawing.Size(150, 36);
            this.btnOtkazi.Text = "Otkaži";
            this.btnOtkazi.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnOtkazi, UiTheme.Red, 10f);
            this.btnOtkazi.Click += btnOtkazi_Click;

            this.Controls.Add(this.lblOpis);
            this.Controls.Add(this.lblNapomena);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtNapomena, new System.Drawing.Point(18, 122), new System.Drawing.Size(420, 90)));
            this.Controls.Add(this.btnSpremi);
            this.Controls.Add(this.btnOtkazi);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblOpis;
        private System.Windows.Forms.Label lblNapomena;
        public System.Windows.Forms.TextBox txtNapomena;
        public IconButton btnSpremi;
        public IconButton btnOtkazi;
    }
}
