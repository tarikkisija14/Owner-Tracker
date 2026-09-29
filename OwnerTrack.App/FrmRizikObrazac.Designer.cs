using OwnerTrack.App.Constants;

namespace OwnerTrack.App
{
    partial class FrmRizikObrazac
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
            panelHeader = new Panel();
            lblHeaderNaziv = new Label();
            lblOdgovoreno = new Label();
            scrollPanel = new Panel();
            cardStranke = new Panel();
            cardPoslovniOdnos = new Panel();
            cardGeografski = new Panel();
            cardUkupno = new Panel();
            panelButtons = new Panel();
            btnSacuvaj = new Button();
            btnExportPdf = new Button();
            btnZatvori = new Button();

            SuspendLayout();

            // ── panelHeader ───────────────────────────────────────
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 56;
            panelHeader.BackColor = UiTheme.Navy;
            panelHeader.Name = "panelHeader";

            lblHeaderNaziv.Location = new Point(20, 14);
            lblHeaderNaziv.AutoSize = true;
            lblHeaderNaziv.ForeColor = Color.White;
            lblHeaderNaziv.Font = UiTheme.Base(13f, FontStyle.Bold);
            lblHeaderNaziv.Name = "lblHeaderNaziv";
            lblHeaderNaziv.Text = "Obrazac za procjenu rizika";
            panelHeader.Controls.Add(lblHeaderNaziv);

            lblOdgovoreno.AutoSize = true;
            lblOdgovoreno.ForeColor = Color.White;
            lblOdgovoreno.Font = UiTheme.Base(9.5f);
            lblOdgovoreno.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblOdgovoreno.Name = "lblOdgovoreno";
            lblOdgovoreno.Text = string.Empty;
            panelHeader.Controls.Add(lblOdgovoreno);
            panelHeader.Resize += (_, _) => PositionOdgovorenoLabel();

            // ── scrollPanel ───────────────────────────────────────
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.AutoScroll = true;
            scrollPanel.BackColor = UiTheme.FormBackgroundDialog;
            scrollPanel.Name = "scrollPanel";

            cardStranke.BackColor = Color.White;
            cardStranke.Width = 860;
            cardStranke.Name = "cardStranke";

            cardPoslovniOdnos.BackColor = Color.White;
            cardPoslovniOdnos.Width = 860;
            cardPoslovniOdnos.Name = "cardPoslovniOdnos";

            cardGeografski.BackColor = Color.White;
            cardGeografski.Width = 860;
            cardGeografski.Name = "cardGeografski";

            cardUkupno.BackColor = Color.White;
            cardUkupno.Width = 860;
            cardUkupno.Name = "cardUkupno";

            scrollPanel.Controls.Add(cardUkupno);
            scrollPanel.Controls.Add(cardGeografski);
            scrollPanel.Controls.Add(cardPoslovniOdnos);
            scrollPanel.Controls.Add(cardStranke);

            // ── panelButtons ──────────────────────────────────────
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Size = new Size(900, 56);
            panelButtons.BackColor = UiTheme.PanelLight;
            panelButtons.Name = "panelButtons";

            btnZatvori.Text = "Zatvori";
            btnZatvori.Location = new Point(778, 11);
            btnZatvori.Size = new Size(110, 34);
            btnZatvori.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnZatvori.Name = "btnZatvori";
            UiTheme.StyleFlatButton(btnZatvori, UiTheme.Red, 10f);
            btnZatvori.Click += btnZatvori_Click;

            btnExportPdf.Text = "Export PDF / Printaj";
            btnExportPdf.Location = new Point(598, 11);
            btnExportPdf.Size = new Size(170, 34);
            btnExportPdf.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportPdf.Name = "btnExportPdf";
            UiTheme.StyleFlatButton(btnExportPdf, UiTheme.Blue, 10f);
            btnExportPdf.Click += btnExportPdf_Click;

            btnSacuvaj.Text = "Sačuvaj";
            btnSacuvaj.Location = new Point(20, 11);
            btnSacuvaj.Size = new Size(120, 34);
            btnSacuvaj.Name = "btnSacuvaj";
            UiTheme.StyleFlatButton(btnSacuvaj, UiTheme.Green, 10f);
            btnSacuvaj.Click += btnSacuvaj_Click;

            panelButtons.Controls.Add(btnZatvori);
            panelButtons.Controls.Add(btnExportPdf);
            panelButtons.Controls.Add(btnSacuvaj);

            // ── FrmRizikObrazac ───────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UiTheme.FormBackgroundDialog;
            ClientSize = new Size(900, 720);
            MinimumSize = new Size(700, 500);
            StartPosition = FormStartPosition.CenterParent;
            Name = "FrmRizikObrazac";
            Text = "Obrazac za procjenu rizika";
            Controls.Add(scrollPanel);
            Controls.Add(panelButtons);
            Controls.Add(panelHeader);
            Load += FrmRizikObrazac_Load;

            ResumeLayout(false);
        }

        private Panel panelHeader;
        private Label lblHeaderNaziv;
        private Label lblOdgovoreno;
        private Panel scrollPanel;
        private Panel cardStranke;
        private Panel cardPoslovniOdnos;
        private Panel cardGeografski;
        private Panel cardUkupno;
        private Panel panelButtons;
        private Button btnSacuvaj;
        private Button btnExportPdf;
        private Button btnZatvori;
    }
}
