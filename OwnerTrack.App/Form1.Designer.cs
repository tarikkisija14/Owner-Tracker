namespace OwnerTrack.App
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer = new SplitContainer();
            dataGridKlijenti = new DataGridView();
            tabControl = new TabControl();
            tabKlijenti = new TabPage();
            scrollPaneKlijenti = new Panel();
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
            lblStatusUgovora = new Label();
            cbStatusUgovora = new ComboBox();
            lblDatumUgovora = new Label();
            dtDatumUgovora = new DateTimePicker();
            tabVlasnici = new TabPage();
            dataGridVlasnici = new DataGridView();
            groupBoxVlasnici = new GroupBox();
            lblVlasnikIme = new Label();
            txtVlasnikIme = new TextBox();
            lblProcenat = new Label();
            txtProcenat = new TextBox();
            lblDatumVlasnika = new Label();
            dtDatumVlasnika = new DateTimePicker();
            lblIzvorPodatka = new Label();
            txtIzvorPodatka = new TextBox();
            btnDodajVlasnika = new Button();
            btnObrisiVlasnika = new Button();
            tabDirektori = new TabPage();
            dataGridDirektori = new DataGridView();
            groupBoxDirektori = new GroupBox();
            lblDirektorIme = new Label();
            txtDirektorIme = new TextBox();
            lblDatumDirektora = new Label();
            dtDatumDirektora = new DateTimePicker();
            lblTipValjanosti = new Label();
            cbTipValjanosti = new ComboBox();
            btnDodajDirektora = new Button();
            btnObrisiDirektora = new Button();
            panelToolbar = new Panel();
            btnDodajKlijent = new Button();
            btnIzmijeniKlijent = new Button();
            btnObrisiKlijent = new Button();
            btnNovi = new Button();
            btnImportExcel = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridKlijenti).BeginInit();
            tabControl.SuspendLayout();
            tabKlijenti.SuspendLayout();
            scrollPaneKlijenti.SuspendLayout();
            groupBoxOsnovni.SuspendLayout();
            groupBoxRizici.SuspendLayout();
            groupBoxUgovor.SuspendLayout();
            tabVlasnici.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridVlasnici).BeginInit();
            groupBoxVlasnici.SuspendLayout();
            tabDirektori.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridDirektori).BeginInit();
            groupBoxDirektori.SuspendLayout();
            panelToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 47);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(dataGridKlijenti);
            splitContainer.Panel1MinSize = 300;
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(tabControl);
            splitContainer.Panel2MinSize = 300;
            splitContainer.Size = new Size(1225, 750);
            splitContainer.SplitterDistance = 921;
            splitContainer.TabIndex = 0;
            // 
            // dataGridKlijenti
            // 
            dataGridKlijenti.AllowUserToAddRows = false;
            dataGridKlijenti.AllowUserToDeleteRows = false;
            dataGridKlijenti.BackgroundColor = SystemColors.ControlLight;
            dataGridKlijenti.Dock = DockStyle.Fill;
            dataGridKlijenti.GridColor = SystemColors.ControlDarkDark;
            dataGridKlijenti.Location = new Point(0, 0);
            dataGridKlijenti.MultiSelect = false;
            dataGridKlijenti.Name = "dataGridKlijenti";
            dataGridKlijenti.ReadOnly = true;
            dataGridKlijenti.RowHeadersVisible = false;
            dataGridKlijenti.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridKlijenti.Size = new Size(921, 750);
            dataGridKlijenti.TabIndex = 0;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabKlijenti);
            tabControl.Controls.Add(tabVlasnici);
            tabControl.Controls.Add(tabDirektori);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(300, 750);
            tabControl.TabIndex = 0;
            // 
            // tabKlijenti
            // 
            tabKlijenti.Controls.Add(scrollPaneKlijenti);
            tabKlijenti.Location = new Point(4, 24);
            tabKlijenti.Name = "tabKlijenti";
            tabKlijenti.Size = new Size(292, 722);
            tabKlijenti.TabIndex = 0;
            tabKlijenti.Text = "🏢 Klijenti";
            // 
            // scrollPaneKlijenti
            // 
            scrollPaneKlijenti.AutoScroll = true;
            scrollPaneKlijenti.Controls.Add(groupBoxOsnovni);
            scrollPaneKlijenti.Controls.Add(groupBoxRizici);
            scrollPaneKlijenti.Controls.Add(groupBoxUgovor);
            scrollPaneKlijenti.Dock = DockStyle.Fill;
            scrollPaneKlijenti.Location = new Point(0, 0);
            scrollPaneKlijenti.Name = "scrollPaneKlijenti";
            scrollPaneKlijenti.Size = new Size(292, 722);
            scrollPaneKlijenti.TabIndex = 0;
            // 
            // groupBoxOsnovni
            // 
            groupBoxOsnovni.AutoSize = true;
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
            groupBoxOsnovni.Location = new Point(9, 9);
            groupBoxOsnovni.Name = "groupBoxOsnovni";
            groupBoxOsnovni.Size = new Size(663, 188);
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
            txtIdBroj.Name = "txtIdBroj";
            txtIdBroj.Size = new Size(132, 23);
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
            txtAdresa.Size = new Size(490, 23);
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
            dtDatumUspostave.Location = new Point(481, 89);
            dtDatumUspostave.Name = "dtDatumUspostave";
            dtDatumUspostave.Size = new Size(114, 23);
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
            cbVrstaKlijenta.Items.AddRange(new object[] { "PRAVNO LICE", "FIZIČKA OSOBA" });
            cbVrstaKlijenta.Location = new Point(105, 122);
            cbVrstaKlijenta.Name = "cbVrstaKlijenta";
            cbVrstaKlijenta.Size = new Size(176, 23);
            cbVrstaKlijenta.TabIndex = 11;
            // 
            // lblDatumOsnivanja
            // 
            lblDatumOsnivanja.AutoSize = true;
            lblDatumOsnivanja.Location = new Point(289, 122);
            lblDatumOsnivanja.Name = "lblDatumOsnivanja";
            lblDatumOsnivanja.Size = new Size(99, 15);
            lblDatumOsnivanja.TabIndex = 12;
            lblDatumOsnivanja.Text = "Datum osnivanja:";
            // 
            // dtDatumOsnivanja
            // 
            dtDatumOsnivanja.Location = new Point(394, 122);
            dtDatumOsnivanja.Name = "dtDatumOsnivanja";
            dtDatumOsnivanja.Size = new Size(114, 23);
            dtDatumOsnivanja.TabIndex = 13;
            // 
            // lblVelicina
            // 
            lblVelicina.AutoSize = true;
            lblVelicina.Location = new Point(516, 122);
            lblVelicina.Name = "lblVelicina";
            lblVelicina.Size = new Size(50, 15);
            lblVelicina.TabIndex = 14;
            lblVelicina.Text = "Veličina:";
            // 
            // cbVelicina
            // 
            cbVelicina.Items.AddRange(new object[] { "MIKRO", "MALO", "SREDNJE", "VELIKA" });
            cbVelicina.Location = new Point(569, 122);
            cbVelicina.Name = "cbVelicina";
            cbVelicina.Size = new Size(88, 23);
            cbVelicina.TabIndex = 15;
            // 
            // groupBoxRizici
            // 
            groupBoxRizici.AutoSize = true;
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
            groupBoxRizici.Location = new Point(9, 197);
            groupBoxRizici.Name = "groupBoxRizici";
            groupBoxRizici.Size = new Size(618, 129);
            groupBoxRizici.TabIndex = 1;
            groupBoxRizici.TabStop = false;
            groupBoxRizici.Text = "⚠️ Rizici";
            // 
            // lblPepRizik
            // 
            lblPepRizik.AutoSize = true;
            lblPepRizik.Location = new Point(9, 23);
            lblPepRizik.Name = "lblPepRizik";
            lblPepRizik.Size = new Size(0, 15);
            lblPepRizik.TabIndex = 0;
            // 
            // cbPepRizik
            // 
            cbPepRizik.Items.AddRange(new object[] { "", "DA", "NE" });
            cbPepRizik.Location = new Point(70, 23);
            cbPepRizik.Name = "cbPepRizik";
            cbPepRizik.Size = new Size(70, 23);
            cbPepRizik.TabIndex = 1;
            // 
            // lblUboRizik
            // 
            lblUboRizik.AutoSize = true;
            lblUboRizik.Location = new Point(149, 23);
            lblUboRizik.Name = "lblUboRizik";
            lblUboRizik.Size = new Size(0, 15);
            lblUboRizik.TabIndex = 2;
            // 
            // cbUboRizik
            // 
            cbUboRizik.Items.AddRange(new object[] { "", "DA", "NE" });
            cbUboRizik.Location = new Point(210, 23);
            cbUboRizik.Name = "cbUboRizik";
            cbUboRizik.Size = new Size(70, 23);
            cbUboRizik.TabIndex = 3;
            // 
            // lblGotovinaRizik
            // 
            lblGotovinaRizik.AutoSize = true;
            lblGotovinaRizik.Location = new Point(289, 23);
            lblGotovinaRizik.Name = "lblGotovinaRizik";
            lblGotovinaRizik.Size = new Size(0, 15);
            lblGotovinaRizik.TabIndex = 4;
            // 
            // cbGotovinaRizik
            // 
            cbGotovinaRizik.Items.AddRange(new object[] { "", "DA", "NE" });
            cbGotovinaRizik.Location = new Point(376, 23);
            cbGotovinaRizik.Name = "cbGotovinaRizik";
            cbGotovinaRizik.Size = new Size(70, 23);
            cbGotovinaRizik.TabIndex = 5;
            // 
            // lblGeografskiRizik
            // 
            lblGeografskiRizik.AutoSize = true;
            lblGeografskiRizik.Location = new Point(455, 23);
            lblGeografskiRizik.Name = "lblGeografskiRizik";
            lblGeografskiRizik.Size = new Size(0, 15);
            lblGeografskiRizik.TabIndex = 6;
            // 
            // cbGeografskiRizik
            // 
            cbGeografskiRizik.Items.AddRange(new object[] { "", "DA", "NE" });
            cbGeografskiRizik.Location = new Point(542, 23);
            cbGeografskiRizik.Name = "cbGeografskiRizik";
            cbGeografskiRizik.Size = new Size(70, 23);
            cbGeografskiRizik.TabIndex = 7;
            // 
            // lblUkupnaProcjena
            // 
            lblUkupnaProcjena.AutoSize = true;
            lblUkupnaProcjena.Location = new Point(9, 56);
            lblUkupnaProcjena.Name = "lblUkupnaProcjena";
            lblUkupnaProcjena.Size = new Size(0, 15);
            lblUkupnaProcjena.TabIndex = 8;
            // 
            // txtUkupnaProcjena
            // 
            txtUkupnaProcjena.Location = new Point(105, 56);
            txtUkupnaProcjena.Name = "txtUkupnaProcjena";
            txtUkupnaProcjena.Size = new Size(210, 23);
            txtUkupnaProcjena.TabIndex = 9;
            // 
            // lblDatumProcjene
            // 
            lblDatumProcjene.AutoSize = true;
            lblDatumProcjene.Location = new Point(324, 56);
            lblDatumProcjene.Name = "lblDatumProcjene";
            lblDatumProcjene.Size = new Size(0, 15);
            lblDatumProcjene.TabIndex = 10;
            // 
            // dtDatumProcjene
            // 
            dtDatumProcjene.Location = new Point(420, 56);
            dtDatumProcjene.Name = "dtDatumProcjene";
            dtDatumProcjene.Size = new Size(114, 23);
            dtDatumProcjene.TabIndex = 11;
            // 
            // lblOvjeraCr
            // 
            lblOvjeraCr.AutoSize = true;
            lblOvjeraCr.Location = new Point(9, 84);
            lblOvjeraCr.Name = "lblOvjeraCr";
            lblOvjeraCr.Size = new Size(0, 15);
            lblOvjeraCr.TabIndex = 12;
            // 
            // txtOvjeraCr
            // 
            txtOvjeraCr.Location = new Point(105, 84);
            txtOvjeraCr.Name = "txtOvjeraCr";
            txtOvjeraCr.Size = new Size(210, 23);
            txtOvjeraCr.TabIndex = 13;
            // 
            // groupBoxUgovor
            // 
            groupBoxUgovor.AutoSize = true;
            groupBoxUgovor.Controls.Add(lblStatusUgovora);
            groupBoxUgovor.Controls.Add(cbStatusUgovora);
            groupBoxUgovor.Controls.Add(lblDatumUgovora);
            groupBoxUgovor.Controls.Add(dtDatumUgovora);
            groupBoxUgovor.Location = new Point(9, 309);
            groupBoxUgovor.Name = "groupBoxUgovor";
            groupBoxUgovor.Size = new Size(612, 75);
            groupBoxUgovor.TabIndex = 2;
            groupBoxUgovor.TabStop = false;
            groupBoxUgovor.Text = "📜 Ugovor";
            // 
            // lblStatusUgovora
            // 
            lblStatusUgovora.AutoSize = true;
            lblStatusUgovora.Location = new Point(9, 28);
            lblStatusUgovora.Name = "lblStatusUgovora";
            lblStatusUgovora.Size = new Size(0, 15);
            lblStatusUgovora.TabIndex = 0;
            // 
            // cbStatusUgovora
            // 
            cbStatusUgovora.Items.AddRange(new object[] { "", "POTPISAN", "NEPOTPISAN", "ANEKS" });
            cbStatusUgovora.Location = new Point(105, 28);
            cbStatusUgovora.Name = "cbStatusUgovora";
            cbStatusUgovora.Size = new Size(176, 23);
            cbStatusUgovora.TabIndex = 1;
            // 
            // lblDatumUgovora
            // 
            lblDatumUgovora.AutoSize = true;
            lblDatumUgovora.Location = new Point(289, 28);
            lblDatumUgovora.Name = "lblDatumUgovora";
            lblDatumUgovora.Size = new Size(0, 15);
            lblDatumUgovora.TabIndex = 2;
            // 
            // dtDatumUgovora
            // 
            dtDatumUgovora.Location = new Point(394, 28);
            dtDatumUgovora.Name = "dtDatumUgovora";
            dtDatumUgovora.Size = new Size(114, 23);
            dtDatumUgovora.TabIndex = 3;
            // 
            // tabVlasnici
            // 
            tabVlasnici.Controls.Add(dataGridVlasnici);
            tabVlasnici.Controls.Add(groupBoxVlasnici);
            tabVlasnici.Location = new Point(4, 24);
            tabVlasnici.Name = "tabVlasnici";
            tabVlasnici.Size = new Size(14, 66);
            tabVlasnici.TabIndex = 1;
            tabVlasnici.Text = "👤 Vlasnici";
            // 
            // dataGridVlasnici
            // 
            dataGridVlasnici.AllowUserToAddRows = false;
            dataGridVlasnici.AllowUserToDeleteRows = false;
            dataGridVlasnici.Dock = DockStyle.Fill;
            dataGridVlasnici.Location = new Point(0, 139);
            dataGridVlasnici.MultiSelect = false;
            dataGridVlasnici.Name = "dataGridVlasnici";
            dataGridVlasnici.ReadOnly = true;
            dataGridVlasnici.RowHeadersVisible = false;
            dataGridVlasnici.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridVlasnici.Size = new Size(14, 0);
            dataGridVlasnici.TabIndex = 0;
            // 
            // groupBoxVlasnici
            // 
            groupBoxVlasnici.AutoSize = true;
            groupBoxVlasnici.Controls.Add(lblVlasnikIme);
            groupBoxVlasnici.Controls.Add(txtVlasnikIme);
            groupBoxVlasnici.Controls.Add(lblProcenat);
            groupBoxVlasnici.Controls.Add(txtProcenat);
            groupBoxVlasnici.Controls.Add(lblDatumVlasnika);
            groupBoxVlasnici.Controls.Add(dtDatumVlasnika);
            groupBoxVlasnici.Controls.Add(lblIzvorPodatka);
            groupBoxVlasnici.Controls.Add(txtIzvorPodatka);
            groupBoxVlasnici.Controls.Add(btnDodajVlasnika);
            groupBoxVlasnici.Controls.Add(btnObrisiVlasnika);
            groupBoxVlasnici.Dock = DockStyle.Top;
            groupBoxVlasnici.Location = new Point(0, 0);
            groupBoxVlasnici.Name = "groupBoxVlasnici";
            groupBoxVlasnici.Size = new Size(14, 139);
            groupBoxVlasnici.TabIndex = 1;
            groupBoxVlasnici.TabStop = false;
            groupBoxVlasnici.Text = "👤 Dodaj vlasnika";
            // 
            // lblVlasnikIme
            // 
            lblVlasnikIme.AutoSize = true;
            lblVlasnikIme.Location = new Point(9, 23);
            lblVlasnikIme.Name = "lblVlasnikIme";
            lblVlasnikIme.Size = new Size(0, 15);
            lblVlasnikIme.TabIndex = 0;
            // 
            // txtVlasnikIme
            // 
            txtVlasnikIme.Location = new Point(105, 23);
            txtVlasnikIme.Name = "txtVlasnikIme";
            txtVlasnikIme.Size = new Size(219, 23);
            txtVlasnikIme.TabIndex = 1;
            // 
            // lblProcenat
            // 
            lblProcenat.AutoSize = true;
            lblProcenat.Location = new Point(332, 23);
            lblProcenat.Name = "lblProcenat";
            lblProcenat.Size = new Size(0, 15);
            lblProcenat.TabIndex = 2;
            // 
            // txtProcenat
            // 
            txtProcenat.Location = new Point(420, 23);
            txtProcenat.Name = "txtProcenat";
            txtProcenat.Size = new Size(70, 23);
            txtProcenat.TabIndex = 3;
            // 
            // lblDatumVlasnika
            // 
            lblDatumVlasnika.AutoSize = true;
            lblDatumVlasnika.Location = new Point(499, 23);
            lblDatumVlasnika.Name = "lblDatumVlasnika";
            lblDatumVlasnika.Size = new Size(0, 15);
            lblDatumVlasnika.TabIndex = 4;
            // 
            // dtDatumVlasnika
            // 
            dtDatumVlasnika.Location = new Point(595, 23);
            dtDatumVlasnika.Name = "dtDatumVlasnika";
            dtDatumVlasnika.Size = new Size(106, 23);
            dtDatumVlasnika.TabIndex = 5;
            // 
            // lblIzvorPodatka
            // 
            lblIzvorPodatka.AutoSize = true;
            lblIzvorPodatka.Location = new Point(9, 56);
            lblIzvorPodatka.Name = "lblIzvorPodatka";
            lblIzvorPodatka.Size = new Size(0, 15);
            lblIzvorPodatka.TabIndex = 6;
            // 
            // txtIzvorPodatka
            // 
            txtIzvorPodatka.Location = new Point(105, 56);
            txtIzvorPodatka.Name = "txtIzvorPodatka";
            txtIzvorPodatka.Size = new Size(289, 23);
            txtIzvorPodatka.TabIndex = 7;
            // 
            // btnDodajVlasnika
            // 
            btnDodajVlasnika.Location = new Point(9, 89);
            btnDodajVlasnika.Name = "btnDodajVlasnika";
            btnDodajVlasnika.Size = new Size(131, 28);
            btnDodajVlasnika.TabIndex = 8;
            btnDodajVlasnika.Text = "➕ Dodaj vlasnika";
            btnDodajVlasnika.UseVisualStyleBackColor = true;
            // 
            // btnObrisiVlasnika
            // 
            btnObrisiVlasnika.Location = new Point(149, 89);
            btnObrisiVlasnika.Name = "btnObrisiVlasnika";
            btnObrisiVlasnika.Size = new Size(88, 28);
            btnObrisiVlasnika.TabIndex = 9;
            btnObrisiVlasnika.Text = "❌ Obriši";
            btnObrisiVlasnika.UseVisualStyleBackColor = true;
            // 
            // tabDirektori
            // 
            tabDirektori.Controls.Add(dataGridDirektori);
            tabDirektori.Controls.Add(groupBoxDirektori);
            tabDirektori.Location = new Point(4, 24);
            tabDirektori.Name = "tabDirektori";
            tabDirektori.Size = new Size(14, 66);
            tabDirektori.TabIndex = 2;
            tabDirektori.Text = "👔 Direktori";
            // 
            // dataGridDirektori
            // 
            dataGridDirektori.AllowUserToAddRows = false;
            dataGridDirektori.AllowUserToDeleteRows = false;
            dataGridDirektori.Dock = DockStyle.Fill;
            dataGridDirektori.Location = new Point(0, 106);
            dataGridDirektori.MultiSelect = false;
            dataGridDirektori.Name = "dataGridDirektori";
            dataGridDirektori.ReadOnly = true;
            dataGridDirektori.RowHeadersVisible = false;
            dataGridDirektori.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridDirektori.Size = new Size(14, 0);
            dataGridDirektori.TabIndex = 0;
            // 
            // groupBoxDirektori
            // 
            groupBoxDirektori.AutoSize = true;
            groupBoxDirektori.Controls.Add(lblDirektorIme);
            groupBoxDirektori.Controls.Add(txtDirektorIme);
            groupBoxDirektori.Controls.Add(lblDatumDirektora);
            groupBoxDirektori.Controls.Add(dtDatumDirektora);
            groupBoxDirektori.Controls.Add(lblTipValjanosti);
            groupBoxDirektori.Controls.Add(cbTipValjanosti);
            groupBoxDirektori.Controls.Add(btnDodajDirektora);
            groupBoxDirektori.Controls.Add(btnObrisiDirektora);
            groupBoxDirektori.Dock = DockStyle.Top;
            groupBoxDirektori.Location = new Point(0, 0);
            groupBoxDirektori.Name = "groupBoxDirektori";
            groupBoxDirektori.Size = new Size(14, 106);
            groupBoxDirektori.TabIndex = 1;
            groupBoxDirektori.TabStop = false;
            groupBoxDirektori.Text = "👔 Dodaj direktora";
            // 
            // lblDirektorIme
            // 
            lblDirektorIme.AutoSize = true;
            lblDirektorIme.Location = new Point(9, 23);
            lblDirektorIme.Name = "lblDirektorIme";
            lblDirektorIme.Size = new Size(0, 15);
            lblDirektorIme.TabIndex = 0;
            // 
            // txtDirektorIme
            // 
            txtDirektorIme.Location = new Point(105, 23);
            txtDirektorIme.Name = "txtDirektorIme";
            txtDirektorIme.Size = new Size(219, 23);
            txtDirektorIme.TabIndex = 1;
            // 
            // lblDatumDirektora
            // 
            lblDatumDirektora.AutoSize = true;
            lblDatumDirektora.Location = new Point(332, 23);
            lblDatumDirektora.Name = "lblDatumDirektora";
            lblDatumDirektora.Size = new Size(0, 15);
            lblDatumDirektora.TabIndex = 2;
            // 
            // dtDatumDirektora
            // 
            dtDatumDirektora.Location = new Point(420, 23);
            dtDatumDirektora.Name = "dtDatumDirektora";
            dtDatumDirektora.Size = new Size(114, 23);
            dtDatumDirektora.TabIndex = 3;
            // 
            // lblTipValjanosti
            // 
            lblTipValjanosti.AutoSize = true;
            lblTipValjanosti.Location = new Point(542, 23);
            lblTipValjanosti.Name = "lblTipValjanosti";
            lblTipValjanosti.Size = new Size(0, 15);
            lblTipValjanosti.TabIndex = 4;
            // 
            // cbTipValjanosti
            // 
            cbTipValjanosti.Items.AddRange(new object[] { "TRAJNO", "VREMENSKI" });
            cbTipValjanosti.Location = new Point(630, 23);
            cbTipValjanosti.Name = "cbTipValjanosti";
            cbTipValjanosti.Size = new Size(88, 23);
            cbTipValjanosti.TabIndex = 5;
            // 
            // btnDodajDirektora
            // 
            btnDodajDirektora.Location = new Point(9, 56);
            btnDodajDirektora.Name = "btnDodajDirektora";
            btnDodajDirektora.Size = new Size(131, 28);
            btnDodajDirektora.TabIndex = 6;
            btnDodajDirektora.Text = "➕ Dodaj direktora";
            btnDodajDirektora.UseVisualStyleBackColor = true;
            // 
            // btnObrisiDirektora
            // 
            btnObrisiDirektora.Location = new Point(149, 56);
            btnObrisiDirektora.Name = "btnObrisiDirektora";
            btnObrisiDirektora.Size = new Size(88, 28);
            btnObrisiDirektora.TabIndex = 7;
            btnObrisiDirektora.Text = "❌ Obriši";
            btnObrisiDirektora.UseVisualStyleBackColor = true;
            // 
            // panelToolbar
            // 
            panelToolbar.BackColor = Color.LightGray;
            panelToolbar.Controls.Add(btnDodajKlijent);
            panelToolbar.Controls.Add(btnIzmijeniKlijent);
            panelToolbar.Controls.Add(btnObrisiKlijent);
            panelToolbar.Controls.Add(btnNovi);
            panelToolbar.Controls.Add(btnImportExcel);
            panelToolbar.Dock = DockStyle.Top;
            panelToolbar.Location = new Point(0, 0);
            panelToolbar.Name = "panelToolbar";
            panelToolbar.Size = new Size(1225, 47);
            panelToolbar.TabIndex = 1;
            // 
            // btnDodajKlijent
            // 
            btnDodajKlijent.Location = new Point(9, 9);
            btnDodajKlijent.Name = "btnDodajKlijent";
            btnDodajKlijent.Size = new Size(105, 28);
            btnDodajKlijent.TabIndex = 0;
            btnDodajKlijent.Text = "➕ Dodaj firmu";
            btnDodajKlijent.UseVisualStyleBackColor = true;
            // 
            // btnIzmijeniKlijent
            // 
            btnIzmijeniKlijent.Location = new Point(122, 9);
            btnIzmijeniKlijent.Name = "btnIzmijeniKlijent";
            btnIzmijeniKlijent.Size = new Size(105, 28);
            btnIzmijeniKlijent.TabIndex = 1;
            btnIzmijeniKlijent.Text = "✏️ Izmijeni";
            btnIzmijeniKlijent.UseVisualStyleBackColor = true;
            // 
            // btnObrisiKlijent
            // 
            btnObrisiKlijent.Location = new Point(236, 9);
            btnObrisiKlijent.Name = "btnObrisiKlijent";
            btnObrisiKlijent.Size = new Size(105, 28);
            btnObrisiKlijent.TabIndex = 2;
            btnObrisiKlijent.Text = "❌ Obriši";
            btnObrisiKlijent.UseVisualStyleBackColor = true;
            // 
            // btnNovi
            // 
            btnNovi.Location = new Point(350, 9);
            btnNovi.Name = "btnNovi";
            btnNovi.Size = new Size(105, 28);
            btnNovi.TabIndex = 3;
            btnNovi.Text = "🔄 Novi";
            btnNovi.UseVisualStyleBackColor = true;
            // 
            // btnImportExcel
            // 
            btnImportExcel.BackColor = Color.LightGreen;
            btnImportExcel.Location = new Point(464, 9);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(131, 28);
            btnImportExcel.TabIndex = 4;
            btnImportExcel.Text = "📥 Import Excel";
            btnImportExcel.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1225, 797);
            Controls.Add(splitContainer);
            Controls.Add(panelToolbar);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OwnerTrack - Upravljanje firmama i vlasnicima";
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridKlijenti).EndInit();
            tabControl.ResumeLayout(false);
            tabKlijenti.ResumeLayout(false);
            scrollPaneKlijenti.ResumeLayout(false);
            scrollPaneKlijenti.PerformLayout();
            groupBoxOsnovni.ResumeLayout(false);
            groupBoxOsnovni.PerformLayout();
            groupBoxRizici.ResumeLayout(false);
            groupBoxRizici.PerformLayout();
            groupBoxUgovor.ResumeLayout(false);
            groupBoxUgovor.PerformLayout();
            tabVlasnici.ResumeLayout(false);
            tabVlasnici.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridVlasnici).EndInit();
            groupBoxVlasnici.ResumeLayout(false);
            groupBoxVlasnici.PerformLayout();
            tabDirektori.ResumeLayout(false);
            tabDirektori.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridDirektori).EndInit();
            groupBoxDirektori.ResumeLayout(false);
            groupBoxDirektori.PerformLayout();
            panelToolbar.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion private System.Windows.Forms.DataGridView dataGridKlijenti;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dataGridKlijenti;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabKlijenti;
        private System.Windows.Forms.TabPage tabVlasnici;
        private System.Windows.Forms.TabPage tabDirektori;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.Button btnDodajKlijent;
        private System.Windows.Forms.Button btnIzmijeniKlijent;
        private System.Windows.Forms.Button btnObrisiKlijent;
        private System.Windows.Forms.Button btnNovi;
        private System.Windows.Forms.Button btnImportExcel;
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
        private System.Windows.Forms.Label lblStatusUgovora;
        public System.Windows.Forms.ComboBox cbStatusUgovora;
        private System.Windows.Forms.Label lblDatumUgovora;
        public System.Windows.Forms.DateTimePicker dtDatumUgovora;
        private System.Windows.Forms.GroupBox groupBoxVlasnici;
        private System.Windows.Forms.Label lblVlasnikIme;
        public System.Windows.Forms.TextBox txtVlasnikIme;
        private System.Windows.Forms.Label lblProcenat;
        public System.Windows.Forms.TextBox txtProcenat;
        private System.Windows.Forms.Label lblDatumVlasnika;
        public System.Windows.Forms.DateTimePicker dtDatumVlasnika;
        private System.Windows.Forms.Label lblIzvorPodatka;
        public System.Windows.Forms.TextBox txtIzvorPodatka;
        public System.Windows.Forms.Button btnDodajVlasnika;
        public System.Windows.Forms.Button btnObrisiVlasnika;
        private System.Windows.Forms.DataGridView dataGridVlasnici;
        private System.Windows.Forms.GroupBox groupBoxDirektori;
        private System.Windows.Forms.Label lblDirektorIme;
        public System.Windows.Forms.TextBox txtDirektorIme;
        private System.Windows.Forms.Label lblDatumDirektora;
        public System.Windows.Forms.DateTimePicker dtDatumDirektora;
        private System.Windows.Forms.Label lblTipValjanosti;
        public System.Windows.Forms.ComboBox cbTipValjanosti;
        public System.Windows.Forms.Button btnDodajDirektora;
        public System.Windows.Forms.Button btnObrisiDirektora;
        private System.Windows.Forms.DataGridView dataGridDirektori;
        private System.Windows.Forms.Panel scrollPaneKlijenti;
    }
}