namespace OwnerTrack.App
{
    partial class FrmDodajKlijent
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
            // OSNOVNI PODACI
            this.groupBoxOsnovni = new System.Windows.Forms.GroupBox();
            this.lblNaziv = new System.Windows.Forms.Label();
            this.txtNaziv = new System.Windows.Forms.TextBox();
            this.lblIdBroj = new System.Windows.Forms.Label();
            this.txtIdBroj = new System.Windows.Forms.TextBox();
            this.lblAdresa = new System.Windows.Forms.Label();
            this.txtAdresa = new System.Windows.Forms.TextBox();
            this.lblSifra = new System.Windows.Forms.Label();
            this.cbSifra = new System.Windows.Forms.ComboBox();
            this.lblDatumUspostave = new System.Windows.Forms.Label();
            this.dtDatumUspostave = new System.Windows.Forms.DateTimePicker();
            this.lblVrstaKlijenta = new System.Windows.Forms.Label();
            this.cbVrstaKlijenta = new System.Windows.Forms.ComboBox();
            this.lblDatumOsnivanja = new System.Windows.Forms.Label();
            this.dtDatumOsnivanja = new System.Windows.Forms.DateTimePicker();
            this.lblVelicina = new System.Windows.Forms.Label();
            this.cbVelicina = new System.Windows.Forms.ComboBox();

            // RIZICI
            this.groupBoxRizici = new System.Windows.Forms.GroupBox();
            this.lblPepRizik = new System.Windows.Forms.Label();
            this.cbPepRizik = new System.Windows.Forms.ComboBox();
            this.lblUboRizik = new System.Windows.Forms.Label();
            this.cbUboRizik = new System.Windows.Forms.ComboBox();
            this.lblGotovinaRizik = new System.Windows.Forms.Label();
            this.cbGotovinaRizik = new System.Windows.Forms.ComboBox();
            this.lblGeografskiRizik = new System.Windows.Forms.Label();
            this.cbGeografskiRizik = new System.Windows.Forms.ComboBox();
            this.lblUkupnaProcjena = new System.Windows.Forms.Label();
            this.txtUkupnaProcjena = new System.Windows.Forms.TextBox();
            this.lblDatumProcjene = new System.Windows.Forms.Label();
            this.dtDatumProcjene = new System.Windows.Forms.DateTimePicker();
            this.lblOvjeraCr = new System.Windows.Forms.Label();
            this.txtOvjeraCr = new System.Windows.Forms.TextBox();

            // UGOVOR
            this.groupBoxUgovor = new System.Windows.Forms.GroupBox();
            this.lblStatusUgovora = new System.Windows.Forms.Label();
            this.cbStatusUgovora = new System.Windows.Forms.ComboBox();
            this.lblDatumUgovora = new System.Windows.Forms.Label();
            this.dtDatumUgovora = new System.Windows.Forms.DateTimePicker();

            // PANEL ZA BUTTONE
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnSpremi = new System.Windows.Forms.Button();
            this.btnOtkazi = new System.Windows.Forms.Button();
            this.groupBoxKontakti = new System.Windows.Forms.GroupBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblTelefon = new System.Windows.Forms.Label();
            this.txtTelefon = new System.Windows.Forms.TextBox();

            this.SuspendLayout();

            // ========== FORMA ==========
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 750);
            this.Text = "Dodaj firmu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Load += FrmDodajKlijent_Load;

            // ========== SCROLL PANEL ==========
            var scrollPanel = new System.Windows.Forms.Panel();
            scrollPanel.AutoScroll = true;
            scrollPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            scrollPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 60);

            // ========== OSNOVNI PODACI ==========
            this.groupBoxOsnovni.Text = "📋 Osnovni podaci";
            this.groupBoxOsnovni.Location = new System.Drawing.Point(10, 10);
            this.groupBoxOsnovni.Size = new System.Drawing.Size(760, 210);

            this.lblNaziv.Text = "Naziv firme:";
            this.lblNaziv.Location = new System.Drawing.Point(10, 25);
            this.lblNaziv.AutoSize = true;
            this.txtNaziv.Location = new System.Drawing.Point(120, 25);
            this.txtNaziv.Size = new System.Drawing.Size(300, 20);

            this.lblIdBroj.Text = "ID Broj:";
            this.lblIdBroj.Location = new System.Drawing.Point(430, 25);
            this.lblIdBroj.AutoSize = true;
            this.txtIdBroj.Location = new System.Drawing.Point(530, 25);
            this.txtIdBroj.Size = new System.Drawing.Size(210, 20);

            this.lblAdresa.Text = "Adresa:";
            this.lblAdresa.Location = new System.Drawing.Point(10, 60);
            this.lblAdresa.AutoSize = true;
            this.txtAdresa.Location = new System.Drawing.Point(120, 60);
            this.txtAdresa.Size = new System.Drawing.Size(620, 20);

            this.lblSifra.Text = "Šifra djelatnosti:";
            this.lblSifra.Location = new System.Drawing.Point(10, 95);
            this.lblSifra.AutoSize = true;
            this.cbSifra.Location = new System.Drawing.Point(120, 95);
            this.cbSifra.Size = new System.Drawing.Size(300, 21);
            this.cbSifra.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            this.lblDatumUspostave.Text = "Datum uspostave:";
            this.lblDatumUspostave.Location = new System.Drawing.Point(430, 95);
            this.lblDatumUspostave.AutoSize = true;
            this.dtDatumUspostave.Location = new System.Drawing.Point(560, 95);
            this.dtDatumUspostave.Size = new System.Drawing.Size(180, 20);

            this.lblVrstaKlijenta.Text = "Vrsta klijenta:";
            this.lblVrstaKlijenta.Location = new System.Drawing.Point(10, 130);
            this.lblVrstaKlijenta.AutoSize = true;
            this.cbVrstaKlijenta.Location = new System.Drawing.Point(120, 130);
            this.cbVrstaKlijenta.Size = new System.Drawing.Size(180, 21);

            this.lblDatumOsnivanja.Text = "Datum osnivanja:";
            this.lblDatumOsnivanja.Location = new System.Drawing.Point(310, 130);
            this.lblDatumOsnivanja.AutoSize = true;
            this.dtDatumOsnivanja.Location = new System.Drawing.Point(430, 130);
            this.dtDatumOsnivanja.Size = new System.Drawing.Size(180, 20);

            this.lblVelicina.Text = "Veličina:";
            this.lblVelicina.Location = new System.Drawing.Point(10, 165);
            this.lblVelicina.AutoSize = true;
            this.cbVelicina.Location = new System.Drawing.Point(120, 165);
            this.cbVelicina.Size = new System.Drawing.Size(180, 21);

            // STATUS
            this.lblStatus = new System.Windows.Forms.Label();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.lblStatus.Text = "Status:";
            this.lblStatus.Location = new System.Drawing.Point(330, 165);
            this.lblStatus.AutoSize = true;
            this.cbStatus.Location = new System.Drawing.Point(430, 165);
            this.cbStatus.Size = new System.Drawing.Size(180, 21);

            this.groupBoxOsnovni.Controls.Add(this.lblNaziv);
            this.groupBoxOsnovni.Controls.Add(this.txtNaziv);
            this.groupBoxOsnovni.Controls.Add(this.lblIdBroj);
            this.groupBoxOsnovni.Controls.Add(this.txtIdBroj);
            this.groupBoxOsnovni.Controls.Add(this.lblAdresa);
            this.groupBoxOsnovni.Controls.Add(this.txtAdresa);
            this.groupBoxOsnovni.Controls.Add(this.lblSifra);
            this.groupBoxOsnovni.Controls.Add(this.cbSifra);
            this.groupBoxOsnovni.Controls.Add(this.lblDatumUspostave);
            this.groupBoxOsnovni.Controls.Add(this.dtDatumUspostave);
            this.groupBoxOsnovni.Controls.Add(this.lblVrstaKlijenta);
            this.groupBoxOsnovni.Controls.Add(this.cbVrstaKlijenta);
            this.groupBoxOsnovni.Controls.Add(this.lblDatumOsnivanja);
            this.groupBoxOsnovni.Controls.Add(this.dtDatumOsnivanja);
            this.groupBoxOsnovni.Controls.Add(this.lblVelicina);
            this.groupBoxOsnovni.Controls.Add(this.cbVelicina);
            this.groupBoxOsnovni.Controls.Add(this.lblStatus);
            this.groupBoxOsnovni.Controls.Add(this.cbStatus);

            // ========== RIZICI ==========
            this.groupBoxRizici.Text = "⚠️ Procjena rizika";
            this.groupBoxRizici.Location = new System.Drawing.Point(10, 230);
            this.groupBoxRizici.Size = new System.Drawing.Size(760, 140);

            this.lblPepRizik.Text = "PEP:";
            this.lblPepRizik.Location = new System.Drawing.Point(10, 25);
            this.lblPepRizik.AutoSize = true;
            this.cbPepRizik.Location = new System.Drawing.Point(80, 25);
            this.cbPepRizik.Size = new System.Drawing.Size(80, 21);

            this.lblUboRizik.Text = "UBO:";
            this.lblUboRizik.Location = new System.Drawing.Point(180, 25);
            this.lblUboRizik.AutoSize = true;
            this.cbUboRizik.Location = new System.Drawing.Point(250, 25);
            this.cbUboRizik.Size = new System.Drawing.Size(80, 21);

            this.lblGotovinaRizik.Text = "Gotovina:";
            this.lblGotovinaRizik.Location = new System.Drawing.Point(350, 25);
            this.lblGotovinaRizik.AutoSize = true;
            this.cbGotovinaRizik.Location = new System.Drawing.Point(440, 25);
            this.cbGotovinaRizik.Size = new System.Drawing.Size(80, 21);

            this.lblGeografskiRizik.Text = "Geografski:";
            this.lblGeografskiRizik.Location = new System.Drawing.Point(570, 25);
            this.lblGeografskiRizik.AutoSize = true;
            this.cbGeografskiRizik.Location = new System.Drawing.Point(660, 25);
            this.cbGeografskiRizik.Size = new System.Drawing.Size(80, 21);

            this.lblUkupnaProcjena.Text = "Ukupna procjena:";
            this.lblUkupnaProcjena.Location = new System.Drawing.Point(10, 60);
            this.lblUkupnaProcjena.AutoSize = true;
            this.txtUkupnaProcjena.Location = new System.Drawing.Point(120, 60);
            this.txtUkupnaProcjena.Size = new System.Drawing.Size(300, 20);

            this.lblDatumProcjene.Text = "Datum procjene:";
            this.lblDatumProcjene.Location = new System.Drawing.Point(430, 60);
            this.lblDatumProcjene.AutoSize = true;
            this.dtDatumProcjene.Location = new System.Drawing.Point(560, 60);
            this.dtDatumProcjene.Size = new System.Drawing.Size(180, 20);

            this.lblOvjeraCr.Text = "Ovjera/CR:";
            this.lblOvjeraCr.Location = new System.Drawing.Point(10, 95);
            this.lblOvjeraCr.AutoSize = true;
            this.txtOvjeraCr.Location = new System.Drawing.Point(120, 95);
            this.txtOvjeraCr.Size = new System.Drawing.Size(300, 20);

            this.groupBoxRizici.Controls.Add(this.lblPepRizik);
            this.groupBoxRizici.Controls.Add(this.cbPepRizik);
            this.groupBoxRizici.Controls.Add(this.lblUboRizik);
            this.groupBoxRizici.Controls.Add(this.cbUboRizik);
            this.groupBoxRizici.Controls.Add(this.lblGotovinaRizik);
            this.groupBoxRizici.Controls.Add(this.cbGotovinaRizik);
            this.groupBoxRizici.Controls.Add(this.lblGeografskiRizik);
            this.groupBoxRizici.Controls.Add(this.cbGeografskiRizik);
            this.groupBoxRizici.Controls.Add(this.lblUkupnaProcjena);
            this.groupBoxRizici.Controls.Add(this.txtUkupnaProcjena);
            this.groupBoxRizici.Controls.Add(this.lblDatumProcjene);
            this.groupBoxRizici.Controls.Add(this.dtDatumProcjene);
            this.groupBoxRizici.Controls.Add(this.lblOvjeraCr);
            this.groupBoxRizici.Controls.Add(this.txtOvjeraCr);

            // ========== UGOVOR ==========
            this.groupBoxUgovor.Text = "📜 Ugovor";
            this.groupBoxUgovor.Location = new System.Drawing.Point(10, 380);
            this.groupBoxUgovor.Size = new System.Drawing.Size(760, 115);

            this.lblVrstaUgovora = new System.Windows.Forms.Label();
            this.txtVrstaUgovora = new System.Windows.Forms.TextBox();
            this.lblVrstaUgovora.Text = "Vrsta ugovora:";
            this.lblVrstaUgovora.Location = new System.Drawing.Point(10, 30);
            this.lblVrstaUgovora.AutoSize = true;
            this.txtVrstaUgovora.Location = new System.Drawing.Point(120, 30);
            this.txtVrstaUgovora.Size = new System.Drawing.Size(200, 20);

            this.lblStatusUgovora.Text = "Status ugovora:";
            this.lblStatusUgovora.Location = new System.Drawing.Point(330, 30);
            this.lblStatusUgovora.AutoSize = true;
            this.cbStatusUgovora.Location = new System.Drawing.Point(450, 30);
            this.cbStatusUgovora.Size = new System.Drawing.Size(200, 21);

            this.lblDatumUgovora.Text = "Datum ugovora:";
            this.lblDatumUgovora.Location = new System.Drawing.Point(10, 65);
            this.lblDatumUgovora.AutoSize = true;
            this.dtDatumUgovora.Location = new System.Drawing.Point(120, 65);
            this.dtDatumUgovora.Size = new System.Drawing.Size(180, 20);

            this.groupBoxUgovor.Controls.Add(this.lblVrstaUgovora);
            this.groupBoxUgovor.Controls.Add(this.txtVrstaUgovora);
            this.groupBoxUgovor.Controls.Add(this.lblStatusUgovora);
            this.groupBoxUgovor.Controls.Add(this.cbStatusUgovora);
            this.groupBoxUgovor.Controls.Add(this.lblDatumUgovora);
            this.groupBoxUgovor.Controls.Add(this.dtDatumUgovora);

            this.groupBoxKontakti.Text = "📞 Kontakti";
            this.groupBoxKontakti.Location = new System.Drawing.Point(10, 505);
            this.groupBoxKontakti.Size = new System.Drawing.Size(760, 115);

            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(10, 30);
            this.lblEmail.AutoSize = true;
            this.txtEmail.Location = new System.Drawing.Point(120, 30);
            this.txtEmail.Size = new System.Drawing.Size(300, 20);
            this.txtEmail.MaxLength = 255;

            this.lblTelefon.Text = "Telefon:";
            this.lblTelefon.Location = new System.Drawing.Point(430, 30);
            this.lblTelefon.AutoSize = true;
            this.txtTelefon.Location = new System.Drawing.Point(530, 30);
            this.txtTelefon.Size = new System.Drawing.Size(200, 20);
            this.txtTelefon.MaxLength = 50;

            this.groupBoxKontakti.Controls.Add(this.lblEmail);
            this.groupBoxKontakti.Controls.Add(this.txtEmail);
            this.groupBoxKontakti.Controls.Add(this.lblTelefon);
            this.groupBoxKontakti.Controls.Add(this.txtTelefon);


            // ========== NAPOMENA ==========
            this.groupBoxNapomena = new System.Windows.Forms.GroupBox();
            this.groupBoxNapomena.Text = "📝 Napomena";
            this.groupBoxNapomena.Location = new System.Drawing.Point(10, 630);

            this.groupBoxNapomena.Size = new System.Drawing.Size(760, 120);

            this.lblNapomena = new System.Windows.Forms.Label();
            this.txtNapomena = new System.Windows.Forms.TextBox();
            this.lblNapomena.Text = "Napomena:";
            this.lblNapomena.Location = new System.Drawing.Point(10, 25);
            this.lblNapomena.AutoSize = true;
            this.txtNapomena.Location = new System.Drawing.Point(10, 45);
            this.txtNapomena.Size = new System.Drawing.Size(730, 60);
            this.txtNapomena.Multiline = true;
            this.txtNapomena.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            this.groupBoxNapomena.Controls.Add(this.lblNapomena);
            this.groupBoxNapomena.Controls.Add(this.txtNapomena);

            // ========== PANEL ZA BUTTONE (FIKSIRAN NA DNU) ==========
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelButtons.Height = 60;
            this.panelButtons.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.btnSpremi.Location = new System.Drawing.Point(480, 15);
            this.btnSpremi.Size = new System.Drawing.Size(150, 35);
            this.btnSpremi.Text = "💾 Spremi";
            this.btnSpremi.UseVisualStyleBackColor = true;
            this.btnSpremi.BackColor = System.Drawing.Color.LightGreen;
            this.btnSpremi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSpremi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSpremi.Click += btnSpremi_Click;

            this.btnOtkazi.Location = new System.Drawing.Point(640, 15);
            this.btnOtkazi.Size = new System.Drawing.Size(150, 35);
            this.btnOtkazi.Text = "❌ Otkazi";
            this.btnOtkazi.UseVisualStyleBackColor = true;
            this.btnOtkazi.BackColor = System.Drawing.Color.LightCoral;
            this.btnOtkazi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOtkazi.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnOtkazi.Click += btnOtkazi_Click;

            this.panelButtons.Controls.Add(this.btnSpremi);
            this.panelButtons.Controls.Add(this.btnOtkazi);

            // ========== DODAVANJE NA SCROLL PANEL ==========
            scrollPanel.Controls.Add(this.groupBoxOsnovni);
            scrollPanel.Controls.Add(this.groupBoxRizici);
            scrollPanel.Controls.Add(this.groupBoxUgovor);
            scrollPanel.Controls.Add(this.groupBoxKontakti);
            scrollPanel.Controls.Add(this.groupBoxNapomena);


            // ========== DODAJ SVE NA FORMU ==========
            this.Controls.Add(scrollPanel);
            this.Controls.Add(this.panelButtons);

            this.ResumeLayout(false);
        }

        private System.Windows.Forms.GroupBox groupBoxOsnovni;
        private System.Windows.Forms.Label lblNaziv;
        public System.Windows.Forms.TextBox txtNaziv;
        private System.Windows.Forms.Label lblIdBroj;
        public System.Windows.Forms.TextBox txtIdBroj;
        private System.Windows.Forms.Label lblAdresa;
        public System.Windows.Forms.TextBox txtAdresa;
        private System.Windows.Forms.Label lblSifra;
        public System.Windows.Forms.ComboBox cbSifra;
        private System.Windows.Forms.Label lblDatumUspostave;
        public System.Windows.Forms.DateTimePicker dtDatumUspostave;
        private System.Windows.Forms.Label lblVrstaKlijenta;
        public System.Windows.Forms.ComboBox cbVrstaKlijenta;
        private System.Windows.Forms.Label lblDatumOsnivanja;
        public System.Windows.Forms.DateTimePicker dtDatumOsnivanja;
        private System.Windows.Forms.Label lblVelicina;
        public System.Windows.Forms.ComboBox cbVelicina;
        private System.Windows.Forms.Label lblStatus;
        public System.Windows.Forms.ComboBox cbStatus;
        private System.Windows.Forms.GroupBox groupBoxRizici;
        private System.Windows.Forms.Label lblPepRizik;
        public System.Windows.Forms.ComboBox cbPepRizik;
        private System.Windows.Forms.Label lblUboRizik;
        public System.Windows.Forms.ComboBox cbUboRizik;
        private System.Windows.Forms.Label lblGotovinaRizik;
        public System.Windows.Forms.ComboBox cbGotovinaRizik;
        private System.Windows.Forms.Label lblGeografskiRizik;
        public System.Windows.Forms.ComboBox cbGeografskiRizik;
        private System.Windows.Forms.Label lblUkupnaProcjena;
        public System.Windows.Forms.TextBox txtUkupnaProcjena;
        private System.Windows.Forms.Label lblDatumProcjene;
        public System.Windows.Forms.DateTimePicker dtDatumProcjene;
        private System.Windows.Forms.Label lblOvjeraCr;
        public System.Windows.Forms.TextBox txtOvjeraCr;
        private System.Windows.Forms.GroupBox groupBoxUgovor;
        private System.Windows.Forms.Label lblVrstaUgovora;
        public System.Windows.Forms.TextBox txtVrstaUgovora;
        private System.Windows.Forms.Label lblStatusUgovora;
        public System.Windows.Forms.ComboBox cbStatusUgovora;
        private System.Windows.Forms.Label lblDatumUgovora;
        public System.Windows.Forms.DateTimePicker dtDatumUgovora;
        private System.Windows.Forms.GroupBox groupBoxNapomena;
        private System.Windows.Forms.Label lblNapomena;
        public System.Windows.Forms.TextBox txtNapomena;
        private System.Windows.Forms.GroupBox groupBoxKontakti;
        private System.Windows.Forms.Label lblEmail;
        public System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblTelefon;
        public System.Windows.Forms.TextBox txtTelefon;
        private System.Windows.Forms.Panel panelButtons;
        public System.Windows.Forms.Button btnSpremi;
        public System.Windows.Forms.Button btnOtkazi;
    }
}