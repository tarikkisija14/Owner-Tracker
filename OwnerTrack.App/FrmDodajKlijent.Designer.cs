using OwnerTrack.App.Constants;
using OwnerTrack.App.Controls;

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
            groupBoxPep = new GroupBox();
            lblPepImePrezime = new Label();
            txtPepImePrezime = new TextBox();
            lblPepFunkcija = new Label();
            txtPepFunkcija = new TextBox();
            lblPepPovezanost = new Label();
            txtPepPovezanost = new TextBox();
            lblPepDatumProvjere = new Label();
            dtPepDatumProvjere = new DateTimePicker();
            lblPepMjerePoduzete = new Label();
            txtPepMjerePoduzete = new TextBox();
            lblOpciIndikatoriRizika = new Label();
            txtOpciIndikatoriRizika = new TextBox();
            lblIndikatoriIdentifikacijeRizika = new Label();
            txtIndikatoriIdentifikacijeRizika = new TextBox();
            lblIndikatoriTransakcijaRizika = new Label();
            txtIndikatoriTransakcijaRizika = new TextBox();
            groupBoxUgovor = new GroupBox();
            lblVrstaUgovora = new Label();
            txtVrstaUgovora = new TextBox();
            lblStatusUgovora = new Label();
            cbStatusUgovora = new ComboBox();
            lblDatumUgovora = new Label();
            dtDatumUgovora = new DateTimePicker();
            panelButtons = new Panel();
            btnSpremi = new IconButton();
            btnOtkazi = new IconButton();
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
            groupBoxPep.SuspendLayout();
            groupBoxUgovor.SuspendLayout();
            panelButtons.SuspendLayout();
            groupBoxKontakti.SuspendLayout();
            scrollPanel.SuspendLayout();
            groupBoxNapomena.SuspendLayout();
            SuspendLayout();

            // ── Shared styles ─────────────────────────────────────
            var uiFont = UiTheme.Base(9.5f);

            void StyleGroupBox(GroupBox gb, string title) => UiTheme.StyleGroupBox(gb, title);
            void StyleLabel(Label l) => UiTheme.StyleLabel(l);
            void StyleTextBox(TextBox t) => UiTheme.StyleTextBox(t);
            void StyleComboBox(ComboBox c) => UiTheme.StyleComboBox(c);

            // ── groupBoxOsnovni ───────────────────────────────────
            groupBoxOsnovni.Controls.Add(lblNaziv);
            groupBoxOsnovni.Controls.Add(UiTheme.WrapWithFocusBorder(txtNaziv, new Point(110, 24), new Size(258, 24)));
            groupBoxOsnovni.Controls.Add(lblIdBroj);
            groupBoxOsnovni.Controls.Add(UiTheme.WrapWithFocusBorder(txtIdBroj, new Point(464, 24), new Size(184, 24)));
            groupBoxOsnovni.Controls.Add(lblAdresa);
            groupBoxOsnovni.Controls.Add(UiTheme.WrapWithFocusBorder(txtAdresa, new Point(110, 56), new Size(538, 24)));
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
            StyleGroupBox(groupBoxOsnovni, "📋 Osnovni podaci");

            lblNaziv.Location = new Point(9, 26);
            lblNaziv.Name = "lblNaziv";
            lblNaziv.TabIndex = 0;
            lblNaziv.Text = "Naziv firme:";
            StyleLabel(lblNaziv);

            txtNaziv.Location = new Point(110, 24);
            txtNaziv.MaxLength = 200;
            txtNaziv.Name = "txtNaziv";
            txtNaziv.Size = new Size(258, 24);
            txtNaziv.TabIndex = 1;
            StyleTextBox(txtNaziv);

            lblIdBroj.Location = new Point(376, 26);
            lblIdBroj.Name = "lblIdBroj";
            lblIdBroj.TabIndex = 2;
            lblIdBroj.Text = "ID Broj:";
            StyleLabel(lblIdBroj);

            txtIdBroj.Location = new Point(464, 24);
            txtIdBroj.MaxLength = 13;
            txtIdBroj.Name = "txtIdBroj";
            txtIdBroj.Size = new Size(184, 24);
            txtIdBroj.TabIndex = 3;
            StyleTextBox(txtIdBroj);

            lblAdresa.Location = new Point(9, 58);
            lblAdresa.Name = "lblAdresa";
            lblAdresa.TabIndex = 4;
            lblAdresa.Text = "Adresa:";
            StyleLabel(lblAdresa);

            txtAdresa.Location = new Point(110, 56);
            txtAdresa.Name = "txtAdresa";
            txtAdresa.Size = new Size(538, 24);
            txtAdresa.TabIndex = 5;
            StyleTextBox(txtAdresa);

            lblSifra.Location = new Point(9, 91);
            lblSifra.Name = "lblSifra";
            lblSifra.TabIndex = 6;
            lblSifra.Text = "Šifra djelatnosti:";
            StyleLabel(lblSifra);

            cbSifra.DropDownStyle = ComboBoxStyle.DropDownList;
            cbSifra.Location = new Point(110, 89);
            cbSifra.Name = "cbSifra";
            cbSifra.Size = new Size(258, 24);
            cbSifra.TabIndex = 7;
            StyleComboBox(cbSifra);

            lblDatumUspostave.Location = new Point(376, 91);
            lblDatumUspostave.Name = "lblDatumUspostave";
            lblDatumUspostave.TabIndex = 8;
            lblDatumUspostave.Text = "Datum uspostave:";
            StyleLabel(lblDatumUspostave);

            dtDatumUspostave.Location = new Point(490, 89);
            dtDatumUspostave.Name = "dtDatumUspostave";
            dtDatumUspostave.Size = new Size(158, 24);
            dtDatumUspostave.TabIndex = 9;
            UiTheme.StyleDateTimePicker(dtDatumUspostave, 9.5f);

            lblVrstaKlijenta.Location = new Point(9, 124);
            lblVrstaKlijenta.Name = "lblVrstaKlijenta";
            lblVrstaKlijenta.TabIndex = 10;
            lblVrstaKlijenta.Text = "Vrsta klijenta:";
            StyleLabel(lblVrstaKlijenta);

            cbVrstaKlijenta.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVrstaKlijenta.Location = new Point(110, 122);
            cbVrstaKlijenta.Name = "cbVrstaKlijenta";
            cbVrstaKlijenta.Size = new Size(158, 24);
            cbVrstaKlijenta.TabIndex = 11;
            StyleComboBox(cbVrstaKlijenta);

            lblDatumOsnivanja.Location = new Point(276, 124);
            lblDatumOsnivanja.Name = "lblDatumOsnivanja";
            lblDatumOsnivanja.TabIndex = 12;
            lblDatumOsnivanja.Text = "Datum osnivanja:";
            StyleLabel(lblDatumOsnivanja);

            dtDatumOsnivanja.Location = new Point(376, 122);
            dtDatumOsnivanja.Name = "dtDatumOsnivanja";
            dtDatumOsnivanja.Size = new Size(158, 24);
            dtDatumOsnivanja.TabIndex = 13;
            UiTheme.StyleDateTimePicker(dtDatumOsnivanja, 9.5f);

            lblVelicina.Location = new Point(9, 157);
            lblVelicina.Name = "lblVelicina";
            lblVelicina.TabIndex = 14;
            lblVelicina.Text = "Veličina:";
            StyleLabel(lblVelicina);

            cbVelicina.DropDownStyle = ComboBoxStyle.DropDownList;
            cbVelicina.Location = new Point(110, 155);
            cbVelicina.Name = "cbVelicina";
            cbVelicina.Size = new Size(158, 24);
            cbVelicina.TabIndex = 15;
            StyleComboBox(cbVelicina);

            lblStatus.Location = new Point(289, 157);
            lblStatus.Name = "lblStatus";
            lblStatus.TabIndex = 16;
            lblStatus.Text = "Status:";
            StyleLabel(lblStatus);

            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatus.Location = new Point(376, 155);
            cbStatus.Name = "cbStatus";
            cbStatus.Size = new Size(158, 24);
            cbStatus.TabIndex = 17;
            StyleComboBox(cbStatus);

            // ── groupBoxRizici ────────────────────────────────────
            groupBoxRizici.Controls.Add(lblPepRizik);
            groupBoxRizici.Controls.Add(cbPepRizik);
            groupBoxRizici.Controls.Add(lblUboRizik);
            groupBoxRizici.Controls.Add(cbUboRizik);
            groupBoxRizici.Controls.Add(lblGotovinaRizik);
            groupBoxRizici.Controls.Add(cbGotovinaRizik);
            groupBoxRizici.Controls.Add(lblGeografskiRizik);
            groupBoxRizici.Controls.Add(cbGeografskiRizik);
            groupBoxRizici.Controls.Add(lblUkupnaProcjena);
            groupBoxRizici.Controls.Add(UiTheme.WrapWithFocusBorder(txtUkupnaProcjena, new Point(120, 57), new Size(248, 24)));
            groupBoxRizici.Controls.Add(lblDatumProcjene);
            groupBoxRizici.Controls.Add(dtDatumProcjene);
            groupBoxRizici.Controls.Add(lblOvjeraCr);
            groupBoxRizici.Controls.Add(UiTheme.WrapWithFocusBorder(txtOvjeraCr, new Point(120, 90), new Size(248, 24)));
            groupBoxRizici.Location = new Point(9, 216);
            groupBoxRizici.Name = "groupBoxRizici";
            groupBoxRizici.Size = new Size(665, 131);
            groupBoxRizici.TabIndex = 1;
            groupBoxRizici.TabStop = false;
            StyleGroupBox(groupBoxRizici, "⚠️ Procjena rizika");

            lblPepRizik.Location = new Point(9, 26);
            lblPepRizik.Name = "lblPepRizik";
            lblPepRizik.TabIndex = 0;
            lblPepRizik.Text = "PEP:";
            StyleLabel(lblPepRizik);

            cbPepRizik.Location = new Point(55, 24);
            cbPepRizik.Name = "cbPepRizik";
            cbPepRizik.Size = new Size(75, 24);
            cbPepRizik.TabIndex = 1;
            StyleComboBox(cbPepRizik);

            lblUboRizik.Location = new Point(148, 26);
            lblUboRizik.Name = "lblUboRizik";
            lblUboRizik.TabIndex = 2;
            lblUboRizik.Text = "UBO:";
            StyleLabel(lblUboRizik);

            cbUboRizik.Location = new Point(195, 24);
            cbUboRizik.Name = "cbUboRizik";
            cbUboRizik.Size = new Size(75, 24);
            cbUboRizik.TabIndex = 3;
            StyleComboBox(cbUboRizik);

            lblGotovinaRizik.Location = new Point(288, 26);
            lblGotovinaRizik.Name = "lblGotovinaRizik";
            lblGotovinaRizik.TabIndex = 4;
            lblGotovinaRizik.Text = "Gotovina:";
            StyleLabel(lblGotovinaRizik);

            cbGotovinaRizik.Location = new Point(362, 24);
            cbGotovinaRizik.Name = "cbGotovinaRizik";
            cbGotovinaRizik.Size = new Size(75, 24);
            cbGotovinaRizik.TabIndex = 5;
            StyleComboBox(cbGotovinaRizik);

            lblGeografskiRizik.Location = new Point(456, 26);
            lblGeografskiRizik.Name = "lblGeografskiRizik";
            lblGeografskiRizik.TabIndex = 6;
            lblGeografskiRizik.Text = "Geografski:";
            StyleLabel(lblGeografskiRizik);

            cbGeografskiRizik.Location = new Point(540, 24);
            cbGeografskiRizik.Name = "cbGeografskiRizik";
            cbGeografskiRizik.Size = new Size(112, 24);
            cbGeografskiRizik.TabIndex = 7;
            StyleComboBox(cbGeografskiRizik);

            lblUkupnaProcjena.Location = new Point(9, 59);
            lblUkupnaProcjena.Name = "lblUkupnaProcjena";
            lblUkupnaProcjena.TabIndex = 8;
            lblUkupnaProcjena.Text = "Ukupna procjena:";
            StyleLabel(lblUkupnaProcjena);

            txtUkupnaProcjena.Location = new Point(120, 57);
            txtUkupnaProcjena.Name = "txtUkupnaProcjena";
            txtUkupnaProcjena.Size = new Size(248, 24);
            txtUkupnaProcjena.TabIndex = 9;
            StyleTextBox(txtUkupnaProcjena);

            lblDatumProcjene.Location = new Point(376, 59);
            lblDatumProcjene.Name = "lblDatumProcjene";
            lblDatumProcjene.TabIndex = 10;
            lblDatumProcjene.Text = "Datum procjene:";
            StyleLabel(lblDatumProcjene);

            dtDatumProcjene.Location = new Point(490, 57);
            dtDatumProcjene.Name = "dtDatumProcjene";
            dtDatumProcjene.Size = new Size(158, 24);
            dtDatumProcjene.TabIndex = 11;
            UiTheme.StyleDateTimePicker(dtDatumProcjene, 9.5f);

            lblOvjeraCr.Location = new Point(9, 92);
            lblOvjeraCr.Name = "lblOvjeraCr";
            lblOvjeraCr.TabIndex = 12;
            lblOvjeraCr.Text = "Ovjera/CR:";
            StyleLabel(lblOvjeraCr);

            txtOvjeraCr.Location = new Point(120, 90);
            txtOvjeraCr.Name = "txtOvjeraCr";
            txtOvjeraCr.Size = new Size(248, 24);
            txtOvjeraCr.TabIndex = 13;
            StyleTextBox(txtOvjeraCr);

            // ── groupBoxPep ───────────────────────────────────────
            groupBoxPep.Controls.Add(lblPepImePrezime);
            groupBoxPep.Controls.Add(UiTheme.WrapWithFocusBorder(txtPepImePrezime, new Point(150, 24), new Size(190, 24)));
            groupBoxPep.Controls.Add(lblPepFunkcija);
            groupBoxPep.Controls.Add(UiTheme.WrapWithFocusBorder(txtPepFunkcija, new Point(405, 24), new Size(251, 24)));
            groupBoxPep.Controls.Add(lblPepPovezanost);
            groupBoxPep.Controls.Add(UiTheme.WrapWithFocusBorder(txtPepPovezanost, new Point(170, 57), new Size(270, 24)));
            groupBoxPep.Controls.Add(lblPepDatumProvjere);
            groupBoxPep.Controls.Add(dtPepDatumProvjere);
            groupBoxPep.Controls.Add(lblPepMjerePoduzete);
            groupBoxPep.Controls.Add(UiTheme.WrapWithFocusBorder(txtPepMjerePoduzete, new Point(175, 90), new Size(480, 24)));
            groupBoxPep.Controls.Add(lblOpciIndikatoriRizika);
            groupBoxPep.Controls.Add(UiTheme.WrapWithFocusBorder(txtOpciIndikatoriRizika, new Point(115, 123), new Size(538, 24)));
            groupBoxPep.Controls.Add(lblIndikatoriIdentifikacijeRizika);
            groupBoxPep.Controls.Add(UiTheme.WrapWithFocusBorder(txtIndikatoriIdentifikacijeRizika, new Point(170, 156), new Size(483, 24)));
            groupBoxPep.Controls.Add(lblIndikatoriTransakcijaRizika);
            groupBoxPep.Controls.Add(UiTheme.WrapWithFocusBorder(txtIndikatoriTransakcijaRizika, new Point(165, 189), new Size(488, 24)));
            groupBoxPep.Location = new Point(9, 356);
            groupBoxPep.Name = "groupBoxPep";
            groupBoxPep.Size = new Size(665, 228);
            groupBoxPep.TabIndex = 2;
            groupBoxPep.TabStop = false;
            StyleGroupBox(groupBoxPep, "🔎 PEP i indikatori rizika");

            lblPepImePrezime.Location = new Point(9, 26);
            lblPepImePrezime.Name = "lblPepImePrezime";
            lblPepImePrezime.TabIndex = 0;
            lblPepImePrezime.Text = "Ime i prezime (PEP):";
            StyleLabel(lblPepImePrezime);

            txtPepImePrezime.Location = new Point(150, 24);
            txtPepImePrezime.Name = "txtPepImePrezime";
            txtPepImePrezime.Size = new Size(190, 24);
            txtPepImePrezime.TabIndex = 1;
            StyleTextBox(txtPepImePrezime);

            lblPepFunkcija.Location = new Point(350, 26);
            lblPepFunkcija.Name = "lblPepFunkcija";
            lblPepFunkcija.TabIndex = 2;
            lblPepFunkcija.Text = "Funkcija:";
            StyleLabel(lblPepFunkcija);

            txtPepFunkcija.Location = new Point(405, 24);
            txtPepFunkcija.Name = "txtPepFunkcija";
            txtPepFunkcija.Size = new Size(251, 24);
            txtPepFunkcija.TabIndex = 3;
            StyleTextBox(txtPepFunkcija);

            lblPepPovezanost.Location = new Point(9, 59);
            lblPepPovezanost.Name = "lblPepPovezanost";
            lblPepPovezanost.TabIndex = 4;
            lblPepPovezanost.Text = "Povezanost s klijentom:";
            StyleLabel(lblPepPovezanost);

            txtPepPovezanost.Location = new Point(170, 57);
            txtPepPovezanost.Name = "txtPepPovezanost";
            txtPepPovezanost.Size = new Size(270, 24);
            txtPepPovezanost.TabIndex = 5;
            StyleTextBox(txtPepPovezanost);

            lblPepDatumProvjere.Location = new Point(450, 59);
            lblPepDatumProvjere.Name = "lblPepDatumProvjere";
            lblPepDatumProvjere.TabIndex = 6;
            lblPepDatumProvjere.Text = "Datum provjere:";
            StyleLabel(lblPepDatumProvjere);

            dtPepDatumProvjere.Location = new Point(545, 57);
            dtPepDatumProvjere.Name = "dtPepDatumProvjere";
            dtPepDatumProvjere.Size = new Size(111, 24);
            dtPepDatumProvjere.TabIndex = 7;
            UiTheme.StyleDateTimePicker(dtPepDatumProvjere, 9.5f);

            lblPepMjerePoduzete.Location = new Point(9, 92);
            lblPepMjerePoduzete.Name = "lblPepMjerePoduzete";
            lblPepMjerePoduzete.TabIndex = 8;
            lblPepMjerePoduzete.Text = "Mjere koje su poduzete:";
            StyleLabel(lblPepMjerePoduzete);

            txtPepMjerePoduzete.Location = new Point(175, 90);
            txtPepMjerePoduzete.Name = "txtPepMjerePoduzete";
            txtPepMjerePoduzete.Size = new Size(480, 24);
            txtPepMjerePoduzete.TabIndex = 9;
            StyleTextBox(txtPepMjerePoduzete);

            lblOpciIndikatoriRizika.Location = new Point(9, 125);
            lblOpciIndikatoriRizika.Name = "lblOpciIndikatoriRizika";
            lblOpciIndikatoriRizika.TabIndex = 10;
            lblOpciIndikatoriRizika.Text = "Opći indikatori:";
            StyleLabel(lblOpciIndikatoriRizika);

            txtOpciIndikatoriRizika.Location = new Point(115, 123);
            txtOpciIndikatoriRizika.Name = "txtOpciIndikatoriRizika";
            txtOpciIndikatoriRizika.Size = new Size(538, 24);
            txtOpciIndikatoriRizika.TabIndex = 11;
            StyleTextBox(txtOpciIndikatoriRizika);

            lblIndikatoriIdentifikacijeRizika.Location = new Point(9, 158);
            lblIndikatoriIdentifikacijeRizika.Name = "lblIndikatoriIdentifikacijeRizika";
            lblIndikatoriIdentifikacijeRizika.TabIndex = 12;
            lblIndikatoriIdentifikacijeRizika.Text = "Indikatori identifikacije:";
            StyleLabel(lblIndikatoriIdentifikacijeRizika);

            txtIndikatoriIdentifikacijeRizika.Location = new Point(170, 156);
            txtIndikatoriIdentifikacijeRizika.Name = "txtIndikatoriIdentifikacijeRizika";
            txtIndikatoriIdentifikacijeRizika.Size = new Size(483, 24);
            txtIndikatoriIdentifikacijeRizika.TabIndex = 13;
            StyleTextBox(txtIndikatoriIdentifikacijeRizika);

            lblIndikatoriTransakcijaRizika.Location = new Point(9, 191);
            lblIndikatoriTransakcijaRizika.Name = "lblIndikatoriTransakcijaRizika";
            lblIndikatoriTransakcijaRizika.TabIndex = 14;
            lblIndikatoriTransakcijaRizika.Text = "Indikatori transakcija:";
            StyleLabel(lblIndikatoriTransakcijaRizika);

            txtIndikatoriTransakcijaRizika.Location = new Point(165, 189);
            txtIndikatoriTransakcijaRizika.Name = "txtIndikatoriTransakcijaRizika";
            txtIndikatoriTransakcijaRizika.Size = new Size(488, 24);
            txtIndikatoriTransakcijaRizika.TabIndex = 15;
            StyleTextBox(txtIndikatoriTransakcijaRizika);

            // ── groupBoxUgovor ────────────────────────────────────
            groupBoxUgovor.Controls.Add(lblVrstaUgovora);
            groupBoxUgovor.Controls.Add(UiTheme.WrapWithFocusBorder(txtVrstaUgovora, new Point(110, 28), new Size(172, 24)));
            groupBoxUgovor.Controls.Add(lblStatusUgovora);
            groupBoxUgovor.Controls.Add(cbStatusUgovora);
            groupBoxUgovor.Controls.Add(lblDatumUgovora);
            groupBoxUgovor.Controls.Add(dtDatumUgovora);
            groupBoxUgovor.Location = new Point(9, 594);
            groupBoxUgovor.Name = "groupBoxUgovor";
            groupBoxUgovor.Size = new Size(665, 108);
            groupBoxUgovor.TabIndex = 3;
            groupBoxUgovor.TabStop = false;
            StyleGroupBox(groupBoxUgovor, "📜 Ugovor");

            lblVrstaUgovora.Location = new Point(9, 30);
            lblVrstaUgovora.Name = "lblVrstaUgovora";
            lblVrstaUgovora.TabIndex = 0;
            lblVrstaUgovora.Text = "Vrsta ugovora:";
            StyleLabel(lblVrstaUgovora);

            txtVrstaUgovora.Location = new Point(110, 28);
            txtVrstaUgovora.Name = "txtVrstaUgovora";
            txtVrstaUgovora.Size = new Size(172, 24);
            txtVrstaUgovora.TabIndex = 1;
            StyleTextBox(txtVrstaUgovora);

            lblStatusUgovora.Location = new Point(289, 30);
            lblStatusUgovora.Name = "lblStatusUgovora";
            lblStatusUgovora.TabIndex = 2;
            lblStatusUgovora.Text = "Status ugovora:";
            StyleLabel(lblStatusUgovora);

            cbStatusUgovora.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStatusUgovora.Location = new Point(394, 28);
            cbStatusUgovora.Name = "cbStatusUgovora";
            cbStatusUgovora.Size = new Size(172, 24);
            cbStatusUgovora.TabIndex = 3;
            StyleComboBox(cbStatusUgovora);

            lblDatumUgovora.Location = new Point(9, 63);
            lblDatumUgovora.Name = "lblDatumUgovora";
            lblDatumUgovora.TabIndex = 4;
            lblDatumUgovora.Text = "Datum ugovora:";
            StyleLabel(lblDatumUgovora);

            dtDatumUgovora.Location = new Point(110, 61);
            dtDatumUgovora.Name = "dtDatumUgovora";
            dtDatumUgovora.Size = new Size(158, 24);
            dtDatumUgovora.TabIndex = 5;
            UiTheme.StyleDateTimePicker(dtDatumUgovora, 9.5f);

            // ── groupBoxKontakti ──────────────────────────────────
            groupBoxKontakti.Controls.Add(lblEmail);
            groupBoxKontakti.Controls.Add(UiTheme.WrapWithFocusBorder(txtEmail, new Point(60, 28), new Size(258, 24)));
            groupBoxKontakti.Controls.Add(lblTelefon);
            groupBoxKontakti.Controls.Add(UiTheme.WrapWithFocusBorder(txtTelefon, new Point(400, 28), new Size(250, 24)));
            groupBoxKontakti.Location = new Point(9, 712);
            groupBoxKontakti.Name = "groupBoxKontakti";
            groupBoxKontakti.Size = new Size(665, 108);
            groupBoxKontakti.TabIndex = 4;
            groupBoxKontakti.TabStop = false;
            StyleGroupBox(groupBoxKontakti, "📞 Kontakti");

            lblEmail.Location = new Point(9, 30);
            lblEmail.Name = "lblEmail";
            lblEmail.TabIndex = 0;
            lblEmail.Text = "Email:";
            StyleLabel(lblEmail);

            txtEmail.Location = new Point(60, 28);
            txtEmail.MaxLength = 255;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(258, 24);
            txtEmail.TabIndex = 1;
            StyleTextBox(txtEmail);

            lblTelefon.Location = new Point(340, 30);
            lblTelefon.Name = "lblTelefon";
            lblTelefon.TabIndex = 2;
            lblTelefon.Text = "Telefon:";
            StyleLabel(lblTelefon);

            txtTelefon.Location = new Point(400, 28);
            txtTelefon.MaxLength = 50;
            txtTelefon.Name = "txtTelefon";
            txtTelefon.Size = new Size(250, 24);
            txtTelefon.TabIndex = 3;
            StyleTextBox(txtTelefon);


            // ── groupBoxNapomena ──────────────────────────────────
            groupBoxNapomena.Controls.Add(lblNapomena);
            groupBoxNapomena.Controls.Add(UiTheme.WrapWithFocusBorder(txtNapomena, new Point(9, 44), new Size(639, 56)));
            groupBoxNapomena.Location = new Point(9, 830);
            groupBoxNapomena.Name = "groupBoxNapomena";
            groupBoxNapomena.Size = new Size(665, 112);
            groupBoxNapomena.TabIndex = 5;
            groupBoxNapomena.TabStop = false;
            StyleGroupBox(groupBoxNapomena, "📝 Napomena");

            lblNapomena.Location = new Point(9, 26);
            lblNapomena.Name = "lblNapomena";
            lblNapomena.TabIndex = 0;
            lblNapomena.Text = "Napomena:";
            StyleLabel(lblNapomena);

            txtNapomena.Location = new Point(9, 44);
            txtNapomena.Multiline = true;
            txtNapomena.AcceptsReturn = true;
            txtNapomena.Name = "txtNapomena";
            txtNapomena.ScrollBars = ScrollBars.Vertical;
            txtNapomena.Size = new Size(639, 56);
            txtNapomena.TabIndex = 1;
            StyleTextBox(txtNapomena);

            // ── scrollPanel ───────────────────────────────────────
            scrollPanel.AutoScroll = true;
            scrollPanel.Controls.Add(groupBoxOsnovni);
            scrollPanel.Controls.Add(groupBoxRizici);
            scrollPanel.Controls.Add(groupBoxPep);
            scrollPanel.Controls.Add(groupBoxUgovor);
            scrollPanel.Controls.Add(groupBoxKontakti);
            scrollPanel.Controls.Add(groupBoxNapomena);
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.Location = new Point(0, 0);
            scrollPanel.Name = "scrollPanel";
            scrollPanel.Padding = new Padding(0, 0, 0, 56);
            scrollPanel.Size = new Size(700, 724);
            scrollPanel.TabIndex = 0;
            scrollPanel.BackColor = UiTheme.FormBackgroundDialog;

            // ── panelButtons ──────────────────────────────────────
            panelButtons.BackColor = UiTheme.Navy;
            panelButtons.BorderStyle = BorderStyle.None;
            panelButtons.Controls.Add(btnSpremi);
            panelButtons.Controls.Add(btnOtkazi);
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Location = new Point(0, 647);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(700, 56);
            panelButtons.TabIndex = 1;

            btnSpremi.Location = new Point(420, 11);
            btnSpremi.Name = "btnSpremi";
            btnSpremi.Size = new Size(131, 36);
            btnSpremi.TabIndex = 0;
            btnSpremi.Text = "Spremi";
            btnSpremi.IconGlyph = "";
            UiTheme.StyleFlatButton(btnSpremi, UiTheme.Green, 10f);
            btnSpremi.Click += btnSpremi_Click;

            btnOtkazi.Location = new Point(560, 11);
            btnOtkazi.Name = "btnOtkazi";
            btnOtkazi.Size = new Size(131, 36);
            btnOtkazi.TabIndex = 1;
            btnOtkazi.Text = "Otkaži";
            btnOtkazi.IconGlyph = "";
            UiTheme.StyleFlatButton(btnOtkazi, UiTheme.Red, 10f);
            btnOtkazi.Click += btnOtkazi_Click;

            // ── FrmDodajKlijent ───────────────────────────────────
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = UiTheme.FormBackgroundDialog;
            ClientSize = new Size(700, 780);
            Controls.Add(scrollPanel);
            Controls.Add(panelButtons);
            Font = UiTheme.Base(9.5f);
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
            groupBoxPep.ResumeLayout(false);
            groupBoxPep.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBoxPep;
        private System.Windows.Forms.Label lblPepImePrezime;
        public System.Windows.Forms.TextBox txtPepImePrezime;
        private System.Windows.Forms.Label lblPepFunkcija;
        public System.Windows.Forms.TextBox txtPepFunkcija;
        private System.Windows.Forms.Label lblPepPovezanost;
        public System.Windows.Forms.TextBox txtPepPovezanost;
        private System.Windows.Forms.Label lblPepDatumProvjere;
        public System.Windows.Forms.DateTimePicker dtPepDatumProvjere;
        private System.Windows.Forms.Label lblPepMjerePoduzete;
        public System.Windows.Forms.TextBox txtPepMjerePoduzete;
        private System.Windows.Forms.Label lblOpciIndikatoriRizika;
        public System.Windows.Forms.TextBox txtOpciIndikatoriRizika;
        private System.Windows.Forms.Label lblIndikatoriIdentifikacijeRizika;
        public System.Windows.Forms.TextBox txtIndikatoriIdentifikacijeRizika;
        private System.Windows.Forms.Label lblIndikatoriTransakcijaRizika;
        public System.Windows.Forms.TextBox txtIndikatoriTransakcijaRizika;
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
        public IconButton btnSpremi;
        public IconButton btnOtkazi;
        private Panel scrollPanel;
    }
}