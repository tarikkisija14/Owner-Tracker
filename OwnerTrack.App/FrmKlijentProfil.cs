using System.Drawing.Drawing2D;
using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using OwnerTrack.Infrastructure.ViewModels;

namespace OwnerTrack.App
{
    public partial class FrmKlijentProfil : Form
    {
        private KlijentProfilViewModel _profil;

        private const int CardWidth = 860;
        private const int CardHeaderHeight = 36;
        private const int FieldColGap = 20;
        private const int FieldColWidth = (CardWidth - 30 - FieldColGap) / 2;

        public FrmKlijentProfil(KlijentProfilViewModel profil)
        {
            _profil = profil ?? throw new ArgumentNullException(nameof(profil));
            InitializeComponent();
        }

        private void FrmKlijentProfil_Load(object sender, EventArgs e)
        {
            Populate();
            Shown += (_, _) => ResetScrollToTop();
        }

        // Populate() postavlja DataSource na tri DataGridView-a odozgo prema
        // dolje (Vlasnici, Direktori, pa Historija na kraju); svaki
        // DataGridView pri DataSource-u interno selektuje/fokusira svoju prvu
        // ćeliju. WinForms-ov AutoScroll na scrollPanel prati taj fokus i
        // scroll-uje se da ga prikaže TEK kad se forma stvarno prikaže
        // (paint/handle-creation ciklus), što je POSLIJE Load-a — zato
        // postavljanje scrolla u Load-u nije dovoljno i mora ići u Shown.
        // Fokus se prebacuje na panelHeader (ne na grid) da gridovi prestanu
        // "vući" scroll ka sebi kad forma dobije focus.
        private void ResetScrollToTop()
        {
            // Panel (panelHeader) ne prima fokus (nije "selectable"), pa
            // Focus() na njemu ne bi ništa uradio — ActiveControl = null miče
            // fokus sa grida bez potrebe da nešto drugo bude fokusirano.
            ActiveControl = null;
            scrollPanel.AutoScrollPosition = new Point(0, 0);
        }

        private void Populate()
        {
            Text = $"Profil firme — {_profil.Naziv}";
            lblHeaderNaziv.Text = _profil.Naziv;
            lblHeaderIdBroj.Text = _profil.IdBroj;

            bool arhiviran = _profil.StatusKlijenta == "ARHIVIRAN";
            lblHeaderStatus.Text = arhiviran ? "ARHIVIRAN" : "AKTIVAN";
            lblHeaderStatus.BackColor = arhiviran ? UiTheme.Red : UiTheme.Green;
            SizeAsPill(lblHeaderStatus);

            int y = 10;
            y = LayoutFieldCard(groupBoxOsnovni, y, "Osnovni podaci", new (string, string?, bool)[]
            {
                ("Adresa", _profil.Adresa, true),
                ("Djelatnost", _profil.Djelatnost, false),
                ("Vrsta klijenta", _profil.VrstaKlijenta, false),
                ("Datum uspostave", FormatDate(_profil.DatumUspostave), false),
                ("Datum osnivanja", FormatDate(_profil.DatumOsnivanja), false),
                ("Veličina", _profil.Velicina, false),
                ("Email", _profil.Email, false),
                ("Telefon", _profil.Telefon, false),
                ("Napomena", _profil.Napomena, true),
            });

            y = LayoutFieldCard(groupBoxRizik, y, "Procjena rizika / PEP", new (string, string?, bool)[]
            {
                ("PEP rizik", _profil.PepRizik, false),
                ("UBO rizik", _profil.UboRizik, false),
                ("Gotovina rizik", _profil.GotovinaRizik, false),
                ("Geografski rizik", _profil.GeografskiRizik, false),
                ("Ukupna procjena", _profil.UkupnaProcjena, false),
                ("Datum procjene", FormatDate(_profil.DatumProcjeneRizika), false),
                ("Ovjera CR", _profil.OvjeraCr, false),
                ("PEP ime i prezime", _profil.PepImePrezime, false),
                ("PEP funkcija", _profil.PepFunkcija, false),
                ("PEP povezanost", _profil.PepPovezanost, false),
                ("PEP datum provjere", FormatDate(_profil.PepDatumProvjere), false),
                ("PEP mjere poduzete", _profil.PepMjerePoduzete, true),
                ("Opći indikatori rizika", _profil.OpciIndikatoriRizika, true),
                ("Indikatori identifikacije rizika", _profil.IndikatoriIdentifikacijeRizika, true),
                ("Indikatori transakcija rizika", _profil.IndikatoriTransakcijaRizika, true),
            });

            y = LayoutFieldCard(groupBoxUgovor, y, "Ugovor", new (string, string?, bool)[]
            {
                ("Vrsta ugovora", _profil.VrstaUgovora, false),
                ("Status ugovora", _profil.StatusUgovora, false),
                ("Datum ugovora", FormatDate(_profil.DatumUgovora), false),
                ("Napomena", _profil.NapomenaUgovora, true),
            });

            decimal ukupnoVlasnistvo = _profil.Vlasnici
                .Where(v => v.Status == StatusConstants.Aktivan)
                .Sum(v => v.ProcenatVlasnistva);
            bool vlasnistvoOk = ukupnoVlasnistvo == 100m;
            string vlasnistvoText = $"Ukupno vlasništvo: {ukupnoVlasnistvo:0.##}%" + (vlasnistvoOk ? "" : " ⚠");
            Color vlasnistvoColor = vlasnistvoOk ? UiTheme.Navy : UiColors.SummaryCritical;

            y = LayoutGridCard(groupBoxVlasnici, y, "Vlasnici (aktivni i arhivirani)", vlasnistvoText, vlasnistvoColor);
            y = LayoutGridCard(groupBoxDirektori, y, "Direktori (aktivni i arhivirani)");

            // btnHistorijaTabela/btnHistorijaTimeline su Designer-deklarisani
            // (moraju preživjeti refresh), pa ih AttachHistorijaToggle mora
            // maknuti iz starog headera PRIJE nego RemoveExistingHeader taj
            // header (i sve što je u njemu ostalo) obriše — inače bi Dispose
            // na starom headeru obrisao i njih.
            btnHistorijaTabela.Parent?.Controls.Remove(btnHistorijaTabela);
            btnHistorijaTimeline.Parent?.Controls.Remove(btnHistorijaTimeline);
            RemoveExistingHeader(groupBoxHistorija);
            groupBoxHistorija.Location = new Point(10, y);
            var historijaHeader = AddCardHeader(groupBoxHistorija, "Historija promjena");
            AttachHistorijaToggle(historijaHeader);

            PopulateVlasniciGrid();
            PopulateDirektoriGrid();
            PopulateHistorijaGrid();
        }

        // btnHistorijaTabela/btnHistorijaTimeline su deklarisani u Designeru
        // (fiksne veličine), ali se pozicioniraju ovdje jer se card header
        // gradi dinamički u AddCardHeader — isto mjesto gdje se pravi rightText
        // labela na Vlasnici kartici.
        private void AttachHistorijaToggle(Panel header)
        {
            btnHistorijaTimeline.Location = new Point(header.Width - btnHistorijaTimeline.Width - 14, 6);
            btnHistorijaTabela.Location = new Point(btnHistorijaTimeline.Left - btnHistorijaTabela.Width - 6, 6);
            header.Controls.Add(btnHistorijaTabela);
            header.Controls.Add(btnHistorijaTimeline);
        }

        private void btnHistorijaTabela_Click(object sender, EventArgs e) => ShowHistorijaTabela();
        private void btnHistorijaTimeline_Click(object sender, EventArgs e) => ShowHistorijaTimeline();

        private void ShowHistorijaTabela()
        {
            gridHistorija.Visible = _profil.Historija.Count > 0;
            lblEmptyHistorija.Visible = _profil.Historija.Count == 0;
            timelineHistorija.Visible = false;
            UiTheme.StyleFlatButton(btnHistorijaTabela, UiTheme.Blue, 8f);
            UiTheme.StyleFlatButton(btnHistorijaTimeline, UiTheme.PanelLight, 8f);
            btnHistorijaTimeline.ForeColor = UiTheme.Navy;
        }

        private void ShowHistorijaTimeline()
        {
            timelineHistorija.SetEntries(_profil.Historija);
            timelineHistorija.Visible = true;
            gridHistorija.Visible = false;
            lblEmptyHistorija.Visible = false;
            UiTheme.StyleFlatButton(btnHistorijaTimeline, UiTheme.Blue, 8f);
            UiTheme.StyleFlatButton(btnHistorijaTabela, UiTheme.PanelLight, 8f);
            btnHistorijaTabela.ForeColor = UiTheme.Navy;
        }

        // ── Card layout ───────────────────────────────────────────────────

        private static Panel AddCardHeader(Panel card, string title, string? rightText = null, Color? rightColor = null)
        {
            var header = new Panel
            {
                Dock = DockStyle.Top,
                Height = CardHeaderHeight,
                BackColor = UiTheme.PanelLight,
            };
            var lbl = new Label
            {
                Text = title,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0),
                Font = UiTheme.Base(10f, FontStyle.Bold),
                ForeColor = UiTheme.Navy,
            };
            header.Controls.Add(lbl);

            if (!string.IsNullOrEmpty(rightText))
            {
                var lblRight = new Label
                {
                    Text = rightText,
                    Dock = DockStyle.Right,
                    Width = 220,
                    TextAlign = ContentAlignment.MiddleRight,
                    Padding = new Padding(0, 0, 14, 0),
                    Font = UiTheme.Base(9f, FontStyle.Bold),
                    ForeColor = rightColor ?? UiTheme.Navy,
                };
                header.Controls.Add(lblRight);
            }

            card.Controls.Add(header);
            return header;
        }

        private int LayoutFieldCard(Panel card, int y, string title, (string Label, string? Value, bool Wide)[] fields)
        {
            card.Controls.Clear();
            card.Location = new Point(10, y);
            card.Width = CardWidth;
            AddCardHeader(card, title);

            int rowY = CardHeaderHeight + 14;
            int col = 0;
            int rowHeight = 0;

            foreach (var f in fields)
            {
                int x = 15 + col * (FieldColWidth + FieldColGap);
                int width = f.Wide ? CardWidth - 30 : FieldColWidth;
                int usedHeight = AddFieldBlock(card, x, rowY, width, f.Label, f.Value, f.Wide);
                rowHeight = Math.Max(rowHeight, usedHeight);

                if (f.Wide)
                {
                    rowY += usedHeight + 16;
                    col = 0;
                    rowHeight = 0;
                }
                else
                {
                    col++;
                    if (col == 2)
                    {
                        col = 0;
                        rowY += rowHeight + 16;
                        rowHeight = 0;
                    }
                }
            }

            if (col != 0) rowY += rowHeight + 16;

            card.Height = rowY + 6;
            return y + card.Height + 16;
        }

        private static int AddFieldBlock(Panel card, int x, int y, int width, string label, string? value, bool wide)
        {
            var lbl = new Label
            {
                Text = label.ToUpperInvariant(),
                Location = new Point(x, y),
                AutoSize = true,
                Font = UiTheme.Base(7.5f, FontStyle.Bold),
                ForeColor = UiTheme.MutedText,
            };
            card.Controls.Add(lbl);

            int valueHeight = wide ? 42 : 20;
            var val = new Label
            {
                Text = string.IsNullOrWhiteSpace(value) ? "—" : value,
                Location = new Point(x, y + 16),
                Size = new Size(width, valueHeight),
                AutoSize = false,
                Font = UiTheme.Base(9.5f),
                ForeColor = UiTheme.LabelText,
            };
            card.Controls.Add(val);

            return 16 + valueHeight;
        }

        private static int LayoutGridCard(Panel card, int y, string title, string? rightText = null, Color? rightColor = null)
        {
            // Isti razlog kao Controls.Clear() u LayoutFieldCard: card ovdje
            // sadrži i DataGridView deklarisan u Designeru (koji mora ostati),
            // pa se briše samo prethodno dinamički dodani header umjesto cijelog
            // Controls — bez ovoga bi drugi poziv Populate() (npr. refresh
            // profila nakon spremanja obrasca za procjenu rizika) naslagao
            // dupli header panel svaki put.
            RemoveExistingHeader(card);
            card.Location = new Point(10, y);
            AddCardHeader(card, title, rightText, rightColor);
            return y + card.Height + 16;
        }

        private static void RemoveExistingHeader(Panel card)
        {
            var existingHeader = card.Controls.OfType<Panel>().FirstOrDefault(c => c.Dock == DockStyle.Top);
            if (existingHeader is null) return;

            card.Controls.Remove(existingHeader);
            existingHeader.Dispose();
        }

        private static void SizeAsPill(Label lbl)
        {
            lbl.AutoSize = false;
            var textSize = TextRenderer.MeasureText(lbl.Text, lbl.Font);
            lbl.Size = new Size(textSize.Width + 26, 24);

            var path = new GraphicsPath();
            int r = lbl.Height;
            path.AddArc(0, 0, r, r, 90, 180);
            path.AddArc(lbl.Width - r, 0, r, r, 270, 180);
            path.CloseFigure();
            lbl.Region = new Region(path);
        }

        // ── Grid-ovi ──────────────────────────────────────────────────────

        private void PopulateVlasniciGrid()
        {
            gridVlasnici.DataSource = _profil.Vlasnici;
            lblEmptyVlasnici.Visible = _profil.Vlasnici.Count == 0;
            if (gridVlasnici.Columns.Count == 0) return;

            GridHelper.ConfigureColumn(gridVlasnici, "ImePrezime", "Ime i prezime", 30);
            GridHelper.ConfigureColumn(gridVlasnici, "ProcenatVlasnistva", "% vlasništva", 15);
            GridHelper.ConfigureColumn(gridVlasnici, "DatumValjanostiDokumenta", "Valjanost dokumenta", 25, "dd.MM.yyyy");
            GridHelper.ConfigureColumn(gridVlasnici, "Status", "Status", 15);
            gridVlasnici.CellFormatting += (s, e) => ColorizeArchivedRow(gridVlasnici, e);
        }

        private void PopulateDirektoriGrid()
        {
            gridDirektori.DataSource = _profil.Direktori;
            lblEmptyDirektori.Visible = _profil.Direktori.Count == 0;
            if (gridDirektori.Columns.Count == 0) return;

            GridHelper.ConfigureColumn(gridDirektori, "ImePrezime", "Ime i prezime", 30);
            GridHelper.ConfigureColumn(gridDirektori, "DatumValjanosti", "Valjanost", 20, "dd.MM.yyyy");
            GridHelper.ConfigureColumn(gridDirektori, "TipValjanosti", "Tip valjanosti", 20);
            GridHelper.ConfigureColumn(gridDirektori, "Status", "Status", 15);
            gridDirektori.CellFormatting += (s, e) => ColorizeArchivedRow(gridDirektori, e);
        }

        private void PopulateHistorijaGrid()
        {
            gridHistorija.DataSource = _profil.Historija;
            lblEmptyHistorija.Visible = _profil.Historija.Count == 0;
            if (gridHistorija.Columns.Count == 0) return;

            GridHelper.ConfigureColumn(gridHistorija, "Vrijeme", "Vrijeme", 20, "dd.MM.yyyy HH:mm");
            GridHelper.ConfigureColumn(gridHistorija, "Tabela", "Tabela", 15);
            GridHelper.ConfigureColumn(gridHistorija, "Akcija", "Akcija", 15);
            GridHelper.ConfigureColumn(gridHistorija, "Opis", "Opis", 50);
        }

        private static void ColorizeArchivedRow(DataGridView grid, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || grid.Rows[e.RowIndex].DataBoundItem is null) return;
            dynamic item = grid.Rows[e.RowIndex].DataBoundItem;
            string? status = item.Status;
            if (status == "ARHIVIRAN")
                grid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = UiTheme.MutedText;
        }

        private void btnZatvori_Click(object sender, EventArgs e) => Close();

        private void btnObrazacRizika_Click(object sender, EventArgs e)
        {
            var frm = new FrmRizikObrazac(_profil.Id, _profil.Naziv ?? string.Empty);
            frm.ShowDialog(this);

            // Obrazac za procjenu rizika piše direktno u bazu (RizikObrazacJson,
            // Version, Azuriran) — ovaj profil je učitan kao snapshot prije
            // otvaranja tog obrasca, pa bez ovoga ostaje na starim podacima i
            // nakon uspješnog save-a. Refresh se radi samo kad je nešto stvarno
            // sačuvano (WasSaved), ne pri svakom zatvaranju obrasca.
            if (frm.WasSaved)
                RefreshProfile();
        }

        // Ponovo učitava profil iz baze kroz isti query servis koji je Form1
        // koristio da prvi put otvori ovu formu (Form1.OpenKlijentProfil), i
        // ponovo poziva postojeći Populate() — bez zatvaranja/ponovnog otvaranja
        // cijele forme i bez duple query logike.
        private void RefreshProfile()
        {
            try
            {
                using var db = DbContextFactory.Create();
                var refreshed = new KlijentProfilQueryService(db).GetProfile(_profil.Id);
                if (refreshed is null) return;

                _profil = refreshed;
                Populate();
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri osvježavanju profila firme");
            }
        }

        private static string FormatDate(DateTime? d) => d.HasValue ? d.Value.ToString("dd.MM.yyyy") : "—";
    }
}
