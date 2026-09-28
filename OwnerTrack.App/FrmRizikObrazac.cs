using OwnerTrack.App.Constants;
using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;
using OwnerTrack.Infrastructure.Services;

namespace OwnerTrack.App
{
    public partial class FrmRizikObrazac : Form
    {
        private const int CardHeaderHeight = 36;

        private readonly int _klijentId;
        private readonly string _nazivKlijenta;

        private readonly List<ComboBox> _combosStranke = new();
        private readonly List<ComboBox> _combosPoslovniOdnos = new();
        private readonly List<ComboBox> _combosGeografski = new();
        private ComboBox _comboProcjenaStranke = null!;
        private ComboBox _comboProcjenaPoslovnogOdnosa = null!;
        private ComboBox _comboProcjenaGeografskog = null!;
        private ComboBox _comboUkupnaProcjena = null!;
        private DateTimePicker _dtDatumProcjene = null!;
        private TextBox _txtOdobrio = null!;

        public FrmRizikObrazac(int klijentId, string nazivKlijenta)
        {
            _klijentId = klijentId;
            _nazivKlijenta = nazivKlijenta;
            InitializeComponent();
        }

        private void FrmRizikObrazac_Load(object sender, EventArgs e)
        {
            Text = $"Obrazac za procjenu rizika — {_nazivKlijenta}";
            lblHeaderNaziv.Text = $"Obrazac za procjenu rizika — {_nazivKlijenta}";

            BuildLayout();

            try
            {
                using var db = DbContextFactory.Create();
                var podaci = new RizikObrazacService(db).Load(_klijentId);
                Populate(podaci);
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju obrasca za procjenu rizika");
            }
        }

        // ── Dinamičko raspoređivanje ─────────────────────────────────────

        private void BuildLayout()
        {
            int y = 10;
            y = LayoutKriterijiCard(cardStranke, y, "Rizik stranke",
                RizikObrazacKriteriji.RizikStranke, _combosStranke, out _comboProcjenaStranke);
            y = LayoutKriterijiCard(cardPoslovniOdnos, y, "Rizik poslovnog odnosa",
                RizikObrazacKriteriji.RizikPoslovnogOdnosa, _combosPoslovniOdnos, out _comboProcjenaPoslovnogOdnosa);
            LayoutKriterijiCard(cardGeografski, y, "Geografski rizik",
                RizikObrazacKriteriji.GeografskiRizik, _combosGeografski, out _comboProcjenaGeografskog);
            LayoutUkupnoCard(cardUkupno, y + cardGeografski.Height + 16);
        }

        private static Panel AddCardHeader(Panel card, string title)
        {
            var header = new Panel { Dock = DockStyle.Top, Height = CardHeaderHeight, BackColor = UiTheme.PanelLight };
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
            card.Controls.Add(header);
            return header;
        }

        private int LayoutKriterijiCard(Panel card, int y, string title, string[] pitanja, List<ComboBox> combos, out ComboBox procjenaCombo)
        {
            card.Controls.Clear();
            combos.Clear();
            card.Location = new Point(10, y);
            card.Width = 860;
            AddCardHeader(card, title);

            int cy = CardHeaderHeight + 14;
            foreach (string pitanje in pitanja)
            {
                cy = AddKriterijRow(card, cy, pitanje, out var combo);
                combos.Add(combo);
            }

            var lblProcjena = new Label
            {
                Text = "PROCJENA:",
                Location = new Point(15, cy + 2),
                AutoSize = true,
                Font = UiTheme.Base(9f, FontStyle.Bold),
                ForeColor = UiTheme.Navy,
            };
            card.Controls.Add(lblProcjena);

            procjenaCombo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(120, cy),
                Size = new Size(150, 24),
            };
            procjenaCombo.Items.AddRange(new object[] { "", RizikObrazacKriteriji.Vise, RizikObrazacKriteriji.Nize });
            UiTheme.StyleComboBox(procjenaCombo);
            card.Controls.Add(procjenaCombo);

            cy += 32;
            card.Height = cy + 10;
            return y + card.Height + 16;
        }

        private static int AddKriterijRow(Panel card, int y, string pitanje, out ComboBox combo)
        {
            var lbl = new Label
            {
                Text = pitanje,
                Location = new Point(15, y),
                MaximumSize = new Size(650, 0),
                AutoSize = true,
                Font = UiTheme.Base(9f),
                ForeColor = UiTheme.LabelText,
            };
            card.Controls.Add(lbl);

            combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(690, y),
                Size = new Size(150, 24),
            };
            combo.Items.AddRange(new object[] { "", RizikObrazacKriteriji.Da, RizikObrazacKriteriji.Ne, RizikObrazacKriteriji.Np });
            UiTheme.StyleComboBox(combo);
            card.Controls.Add(combo);

            return y + Math.Max(lbl.Height, 24) + 10;
        }

        private void LayoutUkupnoCard(Panel card, int y)
        {
            card.Controls.Clear();
            card.Location = new Point(10, y);
            card.Width = 860;
            AddCardHeader(card, "Ukupna procjena rizika");

            int cy = CardHeaderHeight + 14;

            var lblProcjena = new Label
            {
                Text = "PROCJENA:",
                Location = new Point(15, cy + 2),
                AutoSize = true,
                Font = UiTheme.Base(9f, FontStyle.Bold),
                ForeColor = UiTheme.Navy,
            };
            card.Controls.Add(lblProcjena);

            _comboUkupnaProcjena = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(120, cy),
                Size = new Size(150, 24),
            };
            _comboUkupnaProcjena.Items.AddRange(new object[] { "", RizikObrazacKriteriji.Vise, RizikObrazacKriteriji.Nize });
            UiTheme.StyleComboBox(_comboUkupnaProcjena);
            card.Controls.Add(_comboUkupnaProcjena);

            cy += 40;

            var lblDatum = new Label
            {
                Text = "Datum procjene:",
                Location = new Point(15, cy + 4),
                AutoSize = true,
                Font = UiTheme.Base(9f),
            };
            card.Controls.Add(lblDatum);

            _dtDatumProcjene = new DateTimePicker
            {
                Location = new Point(150, cy),
                Size = new Size(150, 24),
                Format = DateTimePickerFormat.Short,
            };
            UiTheme.StyleDateTimePicker(_dtDatumProcjene);
            card.Controls.Add(_dtDatumProcjene);

            var lblOdobrio = new Label
            {
                Text = "Odobrio:",
                Location = new Point(330, cy + 4),
                AutoSize = true,
                Font = UiTheme.Base(9f),
            };
            card.Controls.Add(lblOdobrio);

            _txtOdobrio = new TextBox { Name = "txtOdobrio" };
            UiTheme.StyleTextBox(_txtOdobrio);
            card.Controls.Add(UiTheme.WrapWithFocusBorder(_txtOdobrio, new Point(410, cy), new Size(250, 24)));

            cy += 40;
            card.Height = cy + 10;
        }

        // ── Popunjavanje / čitanje ────────────────────────────────────────

        private void Populate(RizikObrazacPodaci p)
        {
            SetCombos(_combosStranke, p.RizikStrankeOdgovori);
            SetCombos(_combosPoslovniOdnos, p.RizikPoslovnogOdnosaOdgovori);
            SetCombos(_combosGeografski, p.GeografskiRizikOdgovori);

            _comboProcjenaStranke.Text = p.ProcjenaStranke ?? "";
            _comboProcjenaPoslovnogOdnosa.Text = p.ProcjenaPoslovnogOdnosa ?? "";
            _comboProcjenaGeografskog.Text = p.ProcjenaGeografskog ?? "";
            _comboUkupnaProcjena.Text = p.UkupnaProcjena ?? "";

            if (p.DatumProcjene.HasValue)
                _dtDatumProcjene.Value = p.DatumProcjene.Value;

            _txtOdobrio.Text = p.Odobrio ?? "";
        }

        private static void SetCombos(List<ComboBox> combos, List<string?> odgovori)
        {
            for (int i = 0; i < combos.Count; i++)
                combos[i].Text = i < odgovori.Count ? odgovori[i] ?? "" : "";
        }

        private RizikObrazacPodaci CollectPodaci() => new()
        {
            RizikStrankeOdgovori = ReadCombos(_combosStranke),
            ProcjenaStranke = NullIfEmpty(_comboProcjenaStranke.Text),
            RizikPoslovnogOdnosaOdgovori = ReadCombos(_combosPoslovniOdnos),
            ProcjenaPoslovnogOdnosa = NullIfEmpty(_comboProcjenaPoslovnogOdnosa.Text),
            GeografskiRizikOdgovori = ReadCombos(_combosGeografski),
            ProcjenaGeografskog = NullIfEmpty(_comboProcjenaGeografskog.Text),
            UkupnaProcjena = NullIfEmpty(_comboUkupnaProcjena.Text),
            DatumProcjene = _dtDatumProcjene.Value,
            Odobrio = NullIfEmpty(_txtOdobrio.Text),
        };

        private static List<string?> ReadCombos(List<ComboBox> combos) =>
            combos.Select(c => NullIfEmpty(c.Text)).ToList();

        private static string? NullIfEmpty(string? s) => string.IsNullOrWhiteSpace(s) ? null : s;

        // ── Dugmad ────────────────────────────────────────────────────────

        private void btnSacuvaj_Click(object sender, EventArgs e)
        {
            try
            {
                using var db = DbContextFactory.Create();
                new RizikObrazacService(db).Save(_klijentId, CollectPodaci());
                MessageBox.Show("Obrazac je sačuvan.", "Sačuvano", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri čuvanju obrasca za procjenu rizika");
            }
        }

        private async void btnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                using var db = DbContextFactory.Create();
                new RizikObrazacService(db).Save(_klijentId, CollectPodaci());
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri čuvanju obrasca za procjenu rizika");
                return;
            }

            using var dialog = DialogHelper.CreateSaveDialogPdf(
                "Sačuvaj obrazac za procjenu rizika",
                DialogHelper.BuildSafeFileName($"Procjena_rizika_{_nazivKlijenta}"));
            if (dialog.ShowDialog() != DialogResult.OK) return;

            string savedPath = dialog.FileName;
            await DialogHelper.ExecutePdfExport(
                btnExportPdf, btnExportPdf.Text,
                path =>
                {
                    using var db = DbContextFactory.Create();
                    return new PdfExportService(db).GenerateRizikObrazacPdf(_klijentId, path);
                },
                savedPath);
        }

        private void btnZatvori_Click(object sender, EventArgs e) => Close();
    }
}
