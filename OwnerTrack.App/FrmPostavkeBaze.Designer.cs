using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

namespace OwnerTrack.App
{
    partial class FrmPostavkeBaze
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
            this.lblTrenutno = new System.Windows.Forms.Label();
            this.lblTrenutnoValue = new System.Windows.Forms.Label();
            this.lblFolder = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.btnOdaberi = new IconButton();
            this.lblHint = new System.Windows.Forms.Label();
            this.btnPrimijeni = new IconButton();
            this.btnLokalna = new IconButton();
            this.btnZatvori = new IconButton();

            this.SuspendLayout();

            var uiFont = UiTheme.Base(9.5f);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 330);
            this.Text = UiMessages.DbSettingsTitle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = UiTheme.FormBackgroundDialog;
            this.Font = uiFont;
            this.Load += FrmPostavkeBaze_Load;

            // ── Trenutna baza ─────────────────────────────────────
            this.lblTrenutno.Text = "Trenutna baza:";
            this.lblTrenutno.Location = new System.Drawing.Point(18, 18);
            UiTheme.StyleLabel(this.lblTrenutno);

            this.lblTrenutnoValue.Location = new System.Drawing.Point(18, 40);
            this.lblTrenutnoValue.Size = new System.Drawing.Size(524, 44);
            this.lblTrenutnoValue.AutoSize = false;
            this.lblTrenutnoValue.ForeColor = UiTheme.Navy;
            this.lblTrenutnoValue.Font = UiTheme.Base(9.5f, System.Drawing.FontStyle.Bold);

            // ── Folder na serveru ─────────────────────────────────
            this.lblFolder.Text = "Folder s bazom na serveru:";
            this.lblFolder.Location = new System.Drawing.Point(18, 96);
            UiTheme.StyleLabel(this.lblFolder);

            this.txtFolder.Location = new System.Drawing.Point(18, 120);
            this.txtFolder.Size = new System.Drawing.Size(410, 28);
            UiTheme.StyleTextBox(this.txtFolder);

            this.btnOdaberi.Location = new System.Drawing.Point(438, 120);
            this.btnOdaberi.Size = new System.Drawing.Size(104, 28);
            this.btnOdaberi.Text = "Odaberi…";
            UiTheme.StyleFlatButton(this.btnOdaberi, UiTheme.PdfSaveAccent, 9f);
            this.btnOdaberi.Click += btnOdaberi_Click;

            this.lblHint.Location = new System.Drawing.Point(18, 160);
            this.lblHint.Size = new System.Drawing.Size(524, 70);
            this.lblHint.AutoSize = false;
            this.lblHint.ForeColor = UiTheme.MutedText;
            this.lblHint.Font = UiTheme.Base(9f);
            this.lblHint.Text =
                "Svi računari moraju koristiti isti folder. Ako u folderu već postoji baza, aplikacija je koristi. " +
                "Ako je nema, možeš u njega prebaciti postojeću lokalnu bazu sa ovog računara.";

            // ── Dugmad ────────────────────────────────────────────
            this.btnPrimijeni.Location = new System.Drawing.Point(18, 270);
            this.btnPrimijeni.Size = new System.Drawing.Size(160, 36);
            this.btnPrimijeni.Text = "Primijeni";
            this.btnPrimijeni.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnPrimijeni, UiTheme.Blue, 10f);
            this.btnPrimijeni.Click += btnPrimijeni_Click;

            this.btnLokalna.Location = new System.Drawing.Point(190, 270);
            this.btnLokalna.Size = new System.Drawing.Size(200, 36);
            this.btnLokalna.Text = "Vrati na lokalnu bazu";
            this.btnLokalna.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnLokalna, UiTheme.PdfSaveAccent, 10f);
            this.btnLokalna.Click += btnLokalna_Click;

            this.btnZatvori.Location = new System.Drawing.Point(402, 270);
            this.btnZatvori.Size = new System.Drawing.Size(140, 36);
            this.btnZatvori.Text = "Zatvori";
            this.btnZatvori.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnZatvori, UiTheme.Red, 10f);
            this.btnZatvori.Click += btnZatvori_Click;

            this.Controls.Add(this.lblTrenutno);
            this.Controls.Add(this.lblTrenutnoValue);
            this.Controls.Add(this.lblFolder);
            this.Controls.Add(UiTheme.WrapWithFocusBorder(this.txtFolder, new System.Drawing.Point(18, 120), new System.Drawing.Size(410, 28)));
            this.Controls.Add(this.btnOdaberi);
            this.Controls.Add(this.lblHint);
            this.Controls.Add(this.btnPrimijeni);
            this.Controls.Add(this.btnLokalna);
            this.Controls.Add(this.btnZatvori);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label lblTrenutno;
        private System.Windows.Forms.Label lblTrenutnoValue;
        private System.Windows.Forms.Label lblFolder;
        private System.Windows.Forms.TextBox txtFolder;
        private IconButton btnOdaberi;
        private System.Windows.Forms.Label lblHint;
        private IconButton btnPrimijeni;
        private IconButton btnLokalna;
        private IconButton btnZatvori;
    }
}
