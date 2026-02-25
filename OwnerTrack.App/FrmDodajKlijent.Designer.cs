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
            groupBoxOsnovni = new GroupBox();
            lblNaziv = new Label();
            txtNaziv = new TextBox();
            lblIdBroj = new Label();
            txtIdBroj = new TextBox();
            lblAdresa = new Label();
            txtAdresa = new TextBox();
            lblSifra = new Label();
            cbSifra = new ComboBox();
            lblDatumUspostave = new Label();
            dtDatumUspostave = new DateTimePicker();
            lblVrstaKlijenta = new Label();
            cbVrstaKlijenta = new ComboBox();
            lblDatumOsnivanja = new Label();
            dtDatumOsnivanja = new DateTimePicker();
            lblVelicina = new Label();
            cbVelicina = new ComboBox();
            lblStatus = new Label();
            cbStatus = new ComboBox();
            groupBoxRizici = new GroupBox();
            lblPepRizik = new Label();
            cbPepRizik = new ComboBox();
            lblUboRizik = new Label();
            cbUboRizik = new ComboBox();
            lblGotovinaRizik = new Label();
            cbGotovinaRizik = new ComboBox();
            lblGeografskiRizik = new Label();
            cbGeografskiRizik = new ComboBox();
            lblUkupnaProcjena = new Label();
            txtUkupnaProcjena = new TextBox();
            lblDatumProcjene = new Label();
            dtDatumProcjene = new DateTimePicker();
            lblOvjeraCr = new Label();
            txtOvjeraCr = new TextBox();
            groupBoxUgovor = new GroupBox();
            lblVrstaUgovora = new Label();
            txtVrstaUgovora = new TextBox();
            lblStatusUgovora = new Label();
            cbStatusUgovora = new ComboBox();
            lblDatumUgovora = new Label();
            dtDatumUgovora = new DateTimePicker();
            panelButtons = new Panel();
            btnSpremi = new Button();
            btnOtkazi = new Button();
            groupBoxKontakti = new GroupBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblTelefon = new Label();
            txtTelefon = new TextBox();
            scrollPanel = new Panel();
            groupBoxNapomena = new GroupBox();
            lblNapomena = new Label();
            txtNapomena = new TextBox();
            groupBoxOsnovni.SuspendLayout();
            groupBoxRizici.SuspendLayout();
            groupBoxUgovor.SuspendLayout();
            panelButtons.SuspendLayout();
            groupBoxKontakti.SuspendLayout();
            scrollPanel.SuspendLayout();
            groupBoxNapomena.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxOsnovni
            // 
            groupBoxOsnovni.Controls.Add(lblNaziv);
            groupBoxOsnovni.Controls.Add(txtNaziv);
            groupBoxOsnovni.Controls.Add(lblIdBroj);
            groupBoxOsnovni.Controls.Add(txtIdBroj);
            groupBoxOsnovni.Controls.Add(lblAdresa);
            groupBoxOsnovni.Controls.Add(txtAdresa);
            groupBoxOsnovni.Controls.Add(lblSifra);
            groupBoxOsnovni.Controls.Add(cbSifra);
            groupBoxOsnovni.Controls.Add(lblDatumUspostave);
            groupBoxOsnovni.Controls.Add(dtDatumUspostave);
            groupBoxOsnovni.Controls.Add(lblVrstaKlijenta);
            groupBoxOsnovni.Controls.Add(cbVrstaKlijenta);
            groupBoxOsnovni.Controls.Add(lblDatumOsnivanja);
            groupBoxOsnovni.Controls.Add(dtDatumOsnivanja);
            groupBoxOsnovni.Controls.Add(lblVelicina);
            groupBoxOsnovni.Controls.Add(cbVelicina);
            groupBoxOsnovni.Controls.Add(lblStatus);
            groupBoxOsnovni.Controls.Add(cbStatus);
            groupBoxOsnovni.Location = new Point(9, 9);
            groupBoxOsnovni.Name = "groupBoxOsnovni";
            groupBoxOsnovni.Size = new Size(665, 197);
            groupBoxOsnovni.TabIndex = 0;
            groupBoxOsnovni.TabStop = false;
            groupBoxOsnovni.Text = "📋 Osnovni podaci";
            // 
            // lblNaziv
            // 
            lblNaziv.AutoSize = true;
            lblNaziv.Location = new Point(9, 23);
            lblNaziv.Name = "lblNaziv";
            lblNaziv.Size = new Size(70, 15);
            lblNaziv.TabIndex = 0;
            lblNaziv.Text = "Naziv firme:";
            // 
            // txtNaziv
            // 
            txtNaziv.Location = new Point(105, 23);
            txtNaziv.MaxLength = 200;
            txtNaziv.Name = "txtNaziv";
            txtNaziv.Size = new Size(263, 23);
            txtNaziv.TabIndex = 1;
            // 
            // lblIdBroj
            // 
            lblIdBroj.AutoSize = true;
            lblIdBroj.Location = new Point(376, 23);
            lblIdBroj.Name = "lblIdBroj";
            lblIdBroj.Size = new Size(45, 15);
            lblIdBroj.TabIndex = 2;
            lblIdBroj.Text = "ID Broj:";
            // 
            // txtIdBroj
            // 
            txtIdBroj.Location = new Point(464, 23);
            txtIdBroj.MaxLength = 13;
            txtIdBroj.Name = "txtIdBroj";
            txtIdBroj.Size = new Size(184, 23);
            txtIdBroj.TabIndex = 3;
            // 
            // lblAdresa
            // 
            lblAdresa.AutoSize = true;
            lblAdresa.Location = new Point(9, 56);
            lblAdresa.Name = "lblAdresa";
            lblAdresa.Size = new Size(46, 15);
            lblAdresa.TabIndex = 4;
            lblAdresa.Text = "Adresa:";
            // 
            // txtAdresa
            // 
            txtAdresa.Location = new Point(105, 56);
            txtAdresa.Name = "txtAdresa";
            txtAdresa.Size = new Size(543, 23);
            txtAdresa.TabIndex = 5;
            // 
            // lblSifra
            // 
            lblSifra.AutoSize = true;
            lblSifra.Location = new Point(9, 89);
            lblSifra.Name = "lblSifra";
            lblSifra.Size = new Size(91, 15);
            lblSifra.TabIndex = 6;
            lblSifra.Text = "Šifra djelatnosti:";
            // 
            // cbSifra
            // 
            cbSifra.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSifra.Location = new Point(105, 89);
            cbSifra.Name = "cbSifra";
            cbSifra.Size = new Size(263, 23);
            cbSifra.TabIndex = 7;
            // 
            // lblDatumUspostave
            // 
            lblDatumUspostave.AutoSize = true;
            lblDatumUspostave.Location = new Point(376, 89);
            lblDatumUspostave.Name = "lblDatumUspostave";
            lblDatumUspostave.Size = new Size(102, 15);
            lblDatumUspostave.TabIndex = 8;
            lblDatumUspostave.Text = "Datum uspostave:";
            // 
            // dtDatumUspostave
            // 
            dtDatumUspostave.Location = new Point(490, 89);
            dtDatumUspostave.Name = "dtDatumUspostave";
            dtDatumUspostave.Size = new Size(158, 23);
            dtDatumUspostave.TabIndex = 9;
            // 
            // lblVrstaKlijenta
            // 
            lblVrstaKlijenta.AutoSize = true;
            lblVrstaKlijenta.Location = new Point(9, 122);
            lblVrstaKlijenta.Name = "lblVrstaKlijenta";
            lblVrstaKlijenta.Size = new Size(77, 15);
            lblVrstaKlijenta.TabIndex = 10;
            lblVrstaKlijenta.Text = "Vrsta klijenta:";
            // 
            // cbVrstaKlijenta
            // 
            cbVrstaKlijenta.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVrstaKlijenta.Location = new Point(105, 122);
            cbVrstaKlijenta.Name = "cbVrstaKlijenta";
            cbVrstaKlijenta.Size = new Size(158, 23);
            cbVrstaKlijenta.TabIndex = 11;
            // 
            // lblDatumOsnivanja
            // 
            lblDatumOsnivanja.AutoSize = true;
            lblDatumOsnivanja.Location = new Point(271, 122);
            lblDatumOsnivanja.Name = "lblDatumOsnivanja";
            lblDatumOsnivanja.Size = new Size(99, 15);
            lblDatumOsnivanja.TabIndex = 12;
            lblDatumOsnivanja.Text = "Datum osnivanja:";
            // 
            // dtDatumOsnivanja
            // 
            dtDatumOsnivanja.Location = new Point(376, 122);
            dtDatumOsnivanja.Name = "dtDatumOsnivanja";
            dtDatumOsnivanja.Size = new Size(158, 23);
            dtDatumOsnivanja.TabIndex = 13;
            // 
            // lblVelicina
            // 
            lblVelicina.AutoSize = true;
            lblVelicina.Location = new Point(9, 155);
            lblVelicina.Name = "lblVelicina";
            lblVelicina.Size = new Size(50, 15);
            lblVelicina.TabIndex = 14;
            lblVelicina.Text = "Veličina:";
            // 
            // cbVelicina
            // 
            cbVelicina.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVelicina.Location = new Point(105, 155);
            cbVelicina.Name = "cbVelicina";
            cbVelicina.Size = new Size(158, 23);
            cbVelicina.TabIndex = 15;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(289, 155);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(42, 15);
            lblStatus.TabIndex = 16;
            lblStatus.Text = "Status:";
            // 
            // cbStatus
            // 
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.Location = new Point(376, 155);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(158, 23);
            cbStatus.TabIndex = 17;
            // 
            // groupBoxRizici
            // 
            groupBoxRizici.Controls.Add(lblPepRizik);
            groupBoxRizici.Controls.Add(cbPepRizik);
            groupBoxRizici.Controls.Add(lblUboRizik);
            groupBoxRizici.Controls.Add(cbUboRizik);
            groupBoxRizici.Controls.Add(lblGotovinaRizik);
            groupBoxRizici.Controls.Add(cbGotovinaRizik);
            groupBoxRizici.Controls.Add(lblGeografskiRizik);
            groupBoxRizici.Controls.Add(cbGeografskiRizik);
            groupBoxRizici.Controls.Add(lblUkupnaProcjena);
            groupBoxRizici.Controls.Add(txtUkupnaProcjena);
            groupBoxRizici.Controls.Add(lblDatumProcjene);
            groupBoxRizici.Controls.Add(dtDatumProcjene);
            groupBoxRizici.Controls.Add(lblOvjeraCr);
            groupBoxRizici.Controls.Add(txtOvjeraCr);
            groupBoxRizici.Location = new Point(9, 216);
            groupBoxRizici.Name = "groupBoxRizici";
            groupBoxRizici.Size = new Size(665, 131);
            groupBoxRizici.TabIndex = 1;
            groupBoxRizici.TabStop = false;
            groupBoxRizici.Text = "⚠️ Procjena rizika";
            // 
            // lblPepRizik
            // 
            lblPepRizik.AutoSize = true;
            lblPepRizik.Location = new Point(9, 23);
            lblPepRizik.Name = "lblPepRizik";
            lblPepRizik.Size = new Size(30, 15);
            lblPepRizik.TabIndex = 0;
            lblPepRizik.Text = "PEP:";
            // 
            // cbPepRizik
            // 
            cbPepRizik.Location = new Point(70, 23);
            cbPepRizik.Name = "cbPepRizik";
            cbPepRizik.Size = new Size(70, 23);
            cbPepRizik.TabIndex = 1;
            // 
            // lblUboRizik
            // 
            lblUboRizik.AutoSize = true;
            lblUboRizik.Location = new Point(158, 23);
            lblUboRizik.Name = "lblUboRizik";
            lblUboRizik.Size = new Size(34, 15);
            lblUboRizik.TabIndex = 2;
            lblUboRizik.Text = "UBO:";
            // 
            // cbUboRizik
            // 
            cbUboRizik.Location = new Point(219, 23);
            cbUboRizik.Name = "cbUboRizik";
            cbUboRizik.Size = new Size(70, 23);
            cbUboRizik.TabIndex = 3;
            // 
            // lblGotovinaRizik
            // 
            lblGotovinaRizik.AutoSize = true;
            lblGotovinaRizik.Location = new Point(306, 23);
            lblGotovinaRizik.Name = "lblGotovinaRizik";
            lblGotovinaRizik.Size = new Size(58, 15);
            lblGotovinaRizik.TabIndex = 4;
            lblGotovinaRizik.Text = "Gotovina:";
            // 
            // cbGotovinaRizik
            // 
            cbGotovinaRizik.Location = new Point(385, 23);
            cbGotovinaRizik.Name = "cbGotovinaRizik";
            cbGotovinaRizik.Size = new Size(70, 23);
            cbGotovinaRizik.TabIndex = 5;
            // 
            // lblGeografskiRizik
            // 
            lblGeografskiRizik.AutoSize = true;
            lblGeografskiRizik.Location = new Point(499, 23);
            lblGeografskiRizik.Name = "lblGeografskiRizik";
            lblGeografskiRizik.Size = new Size(66, 15);
            lblGeografskiRizik.TabIndex = 6;
            lblGeografskiRizik.Text = "Geografski:";
            // 
            // cbGeografskiRizik
            // 
            cbGeografskiRizik.Location = new Point(578, 23);
            cbGeografskiRizik.Name = "cbGeografskiRizik";
            cbGeografskiRizik.Size = new Size(70, 23);
            cbGeografskiRizik.TabIndex = 7;
            // 
            // lblUkupnaProcjena
            // 
            lblUkupnaProcjena.AutoSize = true;
            lblUkupnaProcjena.Location = new Point(9, 56);
            lblUkupnaProcjena.Name = "lblUkupnaProcjena";
            lblUkupnaProcjena.Size = new Size(100, 15);
            lblUkupnaProcjena.TabIndex = 8;
            lblUkupnaProcjena.Text = "Ukupna procjena:";
            // 
            // txtUkupnaProcjena
            // 
            txtUkupnaProcjena.Location = new Point(105, 56);
            txtUkupnaProcjena.Name = "txtUkupnaProcjena";
            txtUkupnaProcjena.Size = new Size(263, 23);
            txtUkupnaProcjena.TabIndex = 9;
            // 
            // lblDatumProcjene
            // 
            lblDatumProcjene.AutoSize = true;
            lblDatumProcjene.Location = new Point(376, 56);
            lblDatumProcjene.Name = "lblDatumProcjene";
            lblDatumProcjene.Size = new Size(95, 15);
            lblDatumProcjene.TabIndex = 10;
            lblDatumProcjene.Text = "Datum procjene:";
            // 
            // dtDatumProcjene
            // 
            dtDatumProcjene.Location = new Point(490, 56);
            dtDatumProcjene.Name = "dtDatumProcjene";
            dtDatumProcjene.Size = new Size(158, 23);
            dtDatumProcjene.TabIndex = 11;
            // 
            // lblOvjeraCr
            // 
            lblOvjeraCr.AutoSize = true;
            lblOvjeraCr.Location = new Point(9, 89);
            lblOvjeraCr.Name = "lblOvjeraCr";
            lblOvjeraCr.Size = new Size(64, 15);
            lblOvjeraCr.TabIndex = 12;
            lblOvjeraCr.Text = "Ovjera/CR:";
            // 
            // txtOvjeraCr
            // 
            txtOvjeraCr.Location = new Point(105, 89);
            txtOvjeraCr.Name = "txtOvjeraCr";
            txtOvjeraCr.Size = new Size(263, 23);
            txtOvjeraCr.TabIndex = 13;
            // 
            // groupBoxUgovor
            // 
            groupBoxUgovor.Controls.Add(lblVrstaUgovora);
            groupBoxUgovor.Controls.Add(txtVrstaUgovora);
            groupBoxUgovor.Controls.Add(lblStatusUgovora);
            groupBoxUgovor.Controls.Add(cbStatusUgovora);
            groupBoxUgovor.Controls.Add(lblDatumUgovora);
            groupBoxUgovor.Controls.Add(dtDatumUgovora);
            groupBoxUgovor.Location = new Point(9, 356);
            groupBoxUgovor.Name = "groupBoxUgovor";
            groupBoxUgovor.Size = new Size(665, 108);
            groupBoxUgovor.TabIndex = 2;
            groupBoxUgovor.TabStop = false;
            groupBoxUgovor.Text = "📜 Ugovor";
            // 
            // lblVrstaUgovora
            // 
            lblVrstaUgovora.AutoSize = true;
            lblVrstaUgovora.Location = new Point(9, 28);
            lblVrstaUgovora.Name = "lblVrstaUgovora";
            lblVrstaUgovora.Size = new Size(83, 15);
            lblVrstaUgovora.TabIndex = 0;
            lblVrstaUgovora.Text = "Vrsta ugovora:";
            // 
            // txtVrstaUgovora
            // 
            txtVrstaUgovora.Location = new Point(105, 28);
            txtVrstaUgovora.Name = "txtVrstaUgovora";
            txtVrstaUgovora.Size = new Size(176, 23);
            txtVrstaUgovora.TabIndex = 1;
            // 
            // lblStatusUgovora
            // 
            lblStatusUgovora.AutoSize = true;
            lblStatusUgovora.Location = new Point(289, 28);
            lblStatusUgovora.Name = "lblStatusUgovora";
            lblStatusUgovora.Size = new Size(89, 15);
            lblStatusUgovora.TabIndex = 2;
            lblStatusUgovora.Text = "Status ugovora:";
            // 
            // cbStatusUgovora
            // 
            cbStatusUgovora.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatusUgovora.Location = new Point(394, 28);
            cbStatusUgovora.Name = "cbStatusUgovora";
            cbStatusUgovora.Size = new Size(176, 23);
            cbStatusUgovora.TabIndex = 3;
            // 
            // lblDatumUgovora
            // 
            lblDatumUgovora.AutoSize = true;
            lblDatumUgovora.Location = new Point(9, 61);
            lblDatumUgovora.Name = "lblDatumUgovora";
            lblDatumUgovora.Size = new Size(93, 15);
            lblDatumUgovora.TabIndex = 4;
            lblDatumUgovora.Text = "Datum ugovora:";
            // 
            // dtDatumUgovora
            // 
            dtDatumUgovora.Location = new Point(105, 61);
            dtDatumUgovora.Name = "dtDatumUgovora";
            dtDatumUgovora.Size = new Size(158, 23);
            dtDatumUgovora.TabIndex = 5;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.WhiteSmoke;
            panelButtons.BorderStyle = BorderStyle.FixedSingle;
            panelButtons.Controls.Add(btnSpremi);
            panelButtons.Controls.Add(btnOtkazi);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 647);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(700, 56);
            panelButtons.TabIndex = 1;
            // 
            // btnSpremi
            // 
            btnSpremi.BackColor = Color.LightGreen;
            btnSpremi.FlatStyle = FlatStyle.Flat;
            btnSpremi.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSpremi.Location = new Point(420, 14);
            btnSpremi.Name = "btnSpremi";
            btnSpremi.Size = new Size(131, 33);
            btnSpremi.TabIndex = 0;
            btnSpremi.Text = "💾 Spremi";
            btnSpremi.UseVisualStyleBackColor = false;
            btnSpremi.Click += btnSpremi_Click;
            // 
            // btnOtkazi
            // 
            btnOtkazi.BackColor = Color.LightCoral;
            btnOtkazi.FlatStyle = FlatStyle.Flat;
            btnOtkazi.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnOtkazi.Location = new Point(560, 14);
            btnOtkazi.Name = "btnOtkazi";
            btnOtkazi.Size = new Size(131, 33);
            btnOtkazi.TabIndex = 1;
            btnOtkazi.Text = "❌ Otkazi";
            btnOtkazi.UseVisualStyleBackColor = false;
            btnOtkazi.Click += btnOtkazi_Click;
            // 
            // groupBoxKontakti
            // 
            groupBoxKontakti.Controls.Add(lblEmail);
            groupBoxKontakti.Controls.Add(txtEmail);
            groupBoxKontakti.Controls.Add(lblTelefon);
            groupBoxKontakti.Controls.Add(txtTelefon);
            groupBoxKontakti.Location = new Point(9, 473);
            groupBoxKontakti.Name = "groupBoxKontakti";
            groupBoxKontakti.Size = new Size(665, 108);
            groupBoxKontakti.TabIndex = 3;
            groupBoxKontakti.TabStop = false;
            groupBoxKontakti.Text = "📞 Kontakti";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(12, 28);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 0;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(57, 22);
            txtEmail.MaxLength = 255;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(263, 23);
            txtEmail.TabIndex = 1;
            // 
            // lblTelefon
            // 
            lblTelefon.AutoSize = true;
            lblTelefon.Location = new Point(394, 28);
            lblTelefon.Name = "lblTelefon";
            lblTelefon.Size = new Size(49, 15);
            lblTelefon.TabIndex = 2;
            lblTelefon.Text = "Telefon:";
            // 
            // txtTelefon
            // 
            txtTelefon.Location = new Point(464, 22);
            txtTelefon.MaxLength = 50;
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(176, 23);
            txtTelefon.TabIndex = 3;
            txtTelefon.TextChanged += txtTelefon_TextChanged;
            // 
            // scrollPanel
            // 
            scrollPanel.AutoScroll = true;
            scrollPanel.Controls.Add(groupBoxOsnovni);
            scrollPanel.Controls.Add(groupBoxRizici);
            scrollPanel.Controls.Add(groupBoxUgovor);
            scrollPanel.Controls.Add(groupBoxKontakti);
            scrollPanel.Controls.Add(groupBoxNapomena);
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.Location = new Point(0, 0);
            scrollPanel.Name = "scrollPanel";
            scrollPanel.Padding = new Padding(0, 0, 0, 56);
            scrollPanel.Size = new Size(700, 647);
            scrollPanel.TabIndex = 0;
            // 
            // groupBoxNapomena
            // 
            groupBoxNapomena.Controls.Add(lblNapomena);
            groupBoxNapomena.Controls.Add(txtNapomena);
            groupBoxNapomena.Location = new Point(9, 591);
            groupBoxNapomena.Name = "groupBoxNapomena";
            groupBoxNapomena.Size = new Size(665, 112);
            groupBoxNapomena.TabIndex = 4;
            groupBoxNapomena.TabStop = false;
            groupBoxNapomena.Text = "📝 Napomena";
            // 
            // lblNapomena
            // 
            lblNapomena.AutoSize = true;
            lblNapomena.Location = new Point(9, 23);
            lblNapomena.Name = "lblNapomena";
            lblNapomena.Size = new Size(69, 15);
            lblNapomena.TabIndex = 0;
            lblNapomena.Text = "Napomena:";
            // 
            // txtNapomena
            // 
            txtNapomena.Location = new Point(9, 42);
            txtNapomena.Multiline = true;
            txtNapomena.Name = "txtNapomena";
            txtNapomena.ScrollBars = ScrollBars.Vertical;
            txtNapomena.Size = new Size(639, 56);
            txtNapomena.TabIndex = 1;
            // 
            // FrmDodajKlijent
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 703);
            Controls.Add(scrollPanel);
            Controls.Add(panelButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDodajKlijent";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Dodaj firmu";
            Load += FrmDodajKlijent_Load;
            groupBoxOsnovni.ResumeLayout(false);
            groupBoxOsnovni.PerformLayout();
            groupBoxRizici.ResumeLayout(false);
            groupBoxRizici.PerformLayout();
            groupBoxUgovor.ResumeLayout(false);
            groupBoxUgovor.PerformLayout();
            panelButtons.ResumeLayout(false);
            groupBoxKontakti.ResumeLayout(false);
            groupBoxKontakti.PerformLayout();
            scrollPanel.ResumeLayout(false);
            groupBoxNapomena.ResumeLayout(false);
            groupBoxNapomena.PerformLayout();
            ResumeLayout(false);
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
        private Panel scrollPanel;
    }
}