using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

namespace OwnerTrack.App
{
    partial class FrmKorisnici
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
            this.dataGridKorisnici = new System.Windows.Forms.DataGridView();
            this.btnPromijeniStatus = new IconButton();
            this.btnZatvori = new IconButton();

            ((System.ComponentModel.ISupportInitialize)this.dataGridKorisnici).BeginInit();
            this.SuspendLayout();

            var uiFont = UiTheme.Base(9.5f);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 360);
            this.Text = UiMessages.UsersTitle;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = UiTheme.FormBackgroundDialog;
            this.Font = uiFont;
            this.Load += FrmKorisnici_Load;

            this.dataGridKorisnici.Location = new System.Drawing.Point(18, 18);
            this.dataGridKorisnici.Size = new System.Drawing.Size(584, 262);
            this.dataGridKorisnici.AllowUserToAddRows = false;
            this.dataGridKorisnici.AllowUserToDeleteRows = false;
            this.dataGridKorisnici.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridKorisnici.MultiSelect = false;
            this.dataGridKorisnici.ReadOnly = true;
            this.dataGridKorisnici.RowHeadersVisible = false;
            this.dataGridKorisnici.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            UiTheme.StyleGrid(this.dataGridKorisnici);
            this.dataGridKorisnici.SelectionChanged += dataGridKorisnici_SelectionChanged;
            this.dataGridKorisnici.CellContentClick += dataGridKorisnici_CellContentClick;

            this.btnPromijeniStatus.Location = new System.Drawing.Point(18, 304);
            this.btnPromijeniStatus.Size = new System.Drawing.Size(170, 36);
            this.btnPromijeniStatus.Text = "Deaktiviraj";
            this.btnPromijeniStatus.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnPromijeniStatus, UiTheme.Red, 10f);
            this.btnPromijeniStatus.Click += btnPromijeniStatus_Click;


            this.btnZatvori.Location = new System.Drawing.Point(200, 304);
            this.btnZatvori.Size = new System.Drawing.Size(150, 36);
            this.btnZatvori.Text = "Zatvori";
            this.btnZatvori.IconGlyph = "";
            UiTheme.StyleFlatButton(this.btnZatvori, UiTheme.Blue, 10f);
            this.btnZatvori.Click += btnZatvori_Click;

            this.Controls.Add(this.dataGridKorisnici);
            this.Controls.Add(this.btnPromijeniStatus);
            this.Controls.Add(this.btnZatvori);

            ((System.ComponentModel.ISupportInitialize)this.dataGridKorisnici).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dataGridKorisnici;
        private IconButton btnPromijeniStatus;
        private IconButton btnZatvori;
    }
}
