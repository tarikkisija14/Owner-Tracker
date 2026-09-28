using OwnerTrack.App.Constants;

namespace OwnerTrack.App
{
    partial class FrmKlijentProfil
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
            lblHeaderIdBroj = new Label();
            lblHeaderStatus = new Label();
            scrollPanel = new Panel();
            groupBoxOsnovni = new Panel();
            groupBoxRizik = new Panel();
            groupBoxUgovor = new Panel();
            groupBoxVlasnici = new Panel();
            gridVlasnici = new DataGridView();
            lblEmptyVlasnici = new Label();
            groupBoxDirektori = new Panel();
            gridDirektori = new DataGridView();
            lblEmptyDirektori = new Label();
            groupBoxHistorija = new Panel();
            gridHistorija = new DataGridView();
            lblEmptyHistorija = new Label();
            panelButtons = new Panel();
            btnZatvori = new Button();
            btnObrazacRizika = new Button();

            ((System.ComponentModel.ISupportInitialize)gridVlasnici).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridDirektori).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridHistorija).BeginInit();
            SuspendLayout();

            // ── panelHeader ───────────────────────────────────────
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 70;
            panelHeader.BackColor = UiTheme.Navy;
            panelHeader.Name = "panelHeader";

            lblHeaderNaziv.Location = new Point(20, 10);
            lblHeaderNaziv.AutoSize = true;
            lblHeaderNaziv.ForeColor = Color.White;
            lblHeaderNaziv.Font = UiTheme.Base(15f, FontStyle.Bold);
            lblHeaderNaziv.Name = "lblHeaderNaziv";
            lblHeaderNaziv.Text = "Naziv firme";

            lblHeaderIdBroj.Location = new Point(20, 40);
            lblHeaderIdBroj.AutoSize = true;
            lblHeaderIdBroj.ForeColor = UiTheme.HeaderSubText;
            lblHeaderIdBroj.Font = UiTheme.Base(9.5f);
            lblHeaderIdBroj.Name = "lblHeaderIdBroj";
            lblHeaderIdBroj.Text = "ID broj";

            lblHeaderStatus.Location = new Point(740, 22);
            lblHeaderStatus.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblHeaderStatus.AutoSize = true;
            lblHeaderStatus.ForeColor = Color.White;
            lblHeaderStatus.Font = UiTheme.Base(9f, FontStyle.Bold);
            lblHeaderStatus.Name = "lblHeaderStatus";
            lblHeaderStatus.Text = "AKTIVAN";
            lblHeaderStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblHeaderStatus.BackColor = UiTheme.Green;
            lblHeaderStatus.Padding = new Padding(10, 3, 10, 3);

            panelHeader.Controls.Add(lblHeaderStatus);
            panelHeader.Controls.Add(lblHeaderIdBroj);
            panelHeader.Controls.Add(lblHeaderNaziv);

            // ── scrollPanel ───────────────────────────────────────
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.AutoScroll = true;
            scrollPanel.BackColor = UiTheme.FormBackgroundDialog;
            scrollPanel.Name = "scrollPanel";

            // ── groupBoxOsnovni ───────────────────────────────────
            groupBoxOsnovni.BackColor = Color.White;
            groupBoxOsnovni.Width = 860;
            groupBoxOsnovni.Name = "groupBoxOsnovni";

            // ── groupBoxRizik ─────────────────────────────────────
            groupBoxRizik.BackColor = Color.White;
            groupBoxRizik.Width = 860;
            groupBoxRizik.Name = "groupBoxRizik";

            // ── groupBoxUgovor ────────────────────────────────────
            groupBoxUgovor.BackColor = Color.White;
            groupBoxUgovor.Width = 860;
            groupBoxUgovor.Name = "groupBoxUgovor";

            // ── groupBoxVlasnici ──────────────────────────────────
            groupBoxVlasnici.BackColor = Color.White;
            groupBoxVlasnici.Width = 860;
            groupBoxVlasnici.Height = 232;
            groupBoxVlasnici.Name = "groupBoxVlasnici";

            gridVlasnici.AllowUserToAddRows = false;
            gridVlasnici.AllowUserToDeleteRows = false;
            gridVlasnici.ReadOnly = true;
            gridVlasnici.MultiSelect = false;
            gridVlasnici.RowHeadersVisible = false;
            gridVlasnici.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridVlasnici.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridVlasnici.Location = new Point(12, 46);
            gridVlasnici.Size = new Size(836, 176);
            gridVlasnici.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            gridVlasnici.Name = "gridVlasnici";
            UiTheme.StyleGrid(gridVlasnici);
            UiTheme.StyleEmptyState(lblEmptyVlasnici, "Nema evidentiranih vlasnika.");
            groupBoxVlasnici.Controls.Add(gridVlasnici);
            groupBoxVlasnici.Controls.Add(lblEmptyVlasnici);

            // ── groupBoxDirektori ─────────────────────────────────
            groupBoxDirektori.BackColor = Color.White;
            groupBoxDirektori.Width = 860;
            groupBoxDirektori.Height = 232;
            groupBoxDirektori.Name = "groupBoxDirektori";

            gridDirektori.AllowUserToAddRows = false;
            gridDirektori.AllowUserToDeleteRows = false;
            gridDirektori.ReadOnly = true;
            gridDirektori.MultiSelect = false;
            gridDirektori.RowHeadersVisible = false;
            gridDirektori.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridDirektori.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridDirektori.Location = new Point(12, 46);
            gridDirektori.Size = new Size(836, 176);
            gridDirektori.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            gridDirektori.Name = "gridDirektori";
            UiTheme.StyleGrid(gridDirektori);
            UiTheme.StyleEmptyState(lblEmptyDirektori, "Nema evidentiranih direktora.");
            groupBoxDirektori.Controls.Add(gridDirektori);
            groupBoxDirektori.Controls.Add(lblEmptyDirektori);

            // ── groupBoxHistorija ─────────────────────────────────
            groupBoxHistorija.BackColor = Color.White;
            groupBoxHistorija.Width = 860;
            groupBoxHistorija.Height = 272;
            groupBoxHistorija.Name = "groupBoxHistorija";

            gridHistorija.AllowUserToAddRows = false;
            gridHistorija.AllowUserToDeleteRows = false;
            gridHistorija.ReadOnly = true;
            gridHistorija.MultiSelect = false;
            gridHistorija.RowHeadersVisible = false;
            gridHistorija.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gridHistorija.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridHistorija.Location = new Point(12, 46);
            gridHistorija.Size = new Size(836, 216);
            gridHistorija.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            gridHistorija.Name = "gridHistorija";
            UiTheme.StyleGrid(gridHistorija);
            UiTheme.StyleEmptyState(lblEmptyHistorija, "Nema evidentiranih promjena.");
            groupBoxHistorija.Controls.Add(gridHistorija);
            groupBoxHistorija.Controls.Add(lblEmptyHistorija);

            scrollPanel.Controls.Add(groupBoxHistorija);
            scrollPanel.Controls.Add(groupBoxDirektori);
            scrollPanel.Controls.Add(groupBoxVlasnici);
            scrollPanel.Controls.Add(groupBoxUgovor);
            scrollPanel.Controls.Add(groupBoxRizik);
            scrollPanel.Controls.Add(groupBoxOsnovni);

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

            btnObrazacRizika.Text = "Obrazac procjene rizika";
            btnObrazacRizika.Location = new Point(558, 11);
            btnObrazacRizika.Size = new Size(210, 34);
            btnObrazacRizika.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnObrazacRizika.Name = "btnObrazacRizika";
            UiTheme.StyleFlatButton(btnObrazacRizika, UiTheme.Blue, 10f);
            btnObrazacRizika.Click += btnObrazacRizika_Click;

            panelButtons.Controls.Add(btnZatvori);
            panelButtons.Controls.Add(btnObrazacRizika);

            // ── FrmKlijentProfil ──────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UiTheme.FormBackgroundDialog;
            ClientSize = new Size(900, 720);
            MinimumSize = new Size(700, 500);
            StartPosition = FormStartPosition.CenterParent;
            Name = "FrmKlijentProfil";
            Text = "Profil firme";
            Controls.Add(scrollPanel);
            Controls.Add(panelButtons);
            Controls.Add(panelHeader);
            Load += FrmKlijentProfil_Load;

            ((System.ComponentModel.ISupportInitialize)gridVlasnici).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridDirektori).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridHistorija).EndInit();
            ResumeLayout(false);
        }

        private Panel panelHeader;
        private Label lblHeaderNaziv;
        private Label lblHeaderIdBroj;
        private Label lblHeaderStatus;
        private Panel scrollPanel;
        private Panel groupBoxOsnovni;
        private Panel groupBoxRizik;
        private Panel groupBoxUgovor;
        private Panel groupBoxVlasnici;
        private DataGridView gridVlasnici;
        private Label lblEmptyVlasnici;
        private Panel groupBoxDirektori;
        private DataGridView gridDirektori;
        private Label lblEmptyDirektori;
        private Panel groupBoxHistorija;
        private DataGridView gridHistorija;
        private Label lblEmptyHistorija;
        private Panel panelButtons;
        private Button btnZatvori;
        private Button btnObrazacRizika;
    }
}
