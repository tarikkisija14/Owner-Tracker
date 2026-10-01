using Microsoft.EntityFrameworkCore;
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
        private int _version;
        private bool _dirty;

        // Postavlja se na true tek nakon uspješnog Save-a (Sačuvaj ili Export
        // PDF, koji prvo sprema pa tek onda generiše fajl) — FrmKlijentProfil
        // ovo čita nakon ShowDialog da zna treba li osvježiti prikaz, umjesto
        // da uvijek refresha bez obzira je li se išta stvarno promijenilo u bazi.
        public bool WasSaved { get; private set; }

        private readonly List<ComboBox> _combosOpci = new();
        private readonly List<ComboBox> _combosIdentifikacija = new();
        private readonly List<ComboBox> _combosTransakcije = new();
        private readonly List<ComboBox> _combosGeografski = new();
        private ComboBox _comboProcjenaOpcih = null!;
        private ComboBox _comboProcjenaIdentifikacije = null!;
        private ComboBox _comboProcjenaTransakcija = null!;
        private ComboBox _comboProcjenaGeografskog = null!;
        private TextBox _txtOstalaZapazanja = null!;
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

            AcceptButton = btnSacuvaj;
            CancelButton = btnZatvori;

            BuildLayout();

            try
            {
                using var db = DbContextFactory.Create();
                var podaci = new RizikObrazacService(db).Load(_klijentId, out _version);
                Populate(podaci);
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri učitavanju obrasca za procjenu rizika");
            }

            UpdateOdgovorenoCounter();

            // Prati promjene tek OD OVDJE — vrijednosti upravo postavljene u
            // Populate() (učitane iz baze) ne smiju formu odmah označiti kao
            // "nesačuvanu". Isti obrazac kao FrmDodajKlijent/FrmDodajVlasnika/
            // FrmDodajDirektora (vidi FormHelper.AttachDirtyTracking).
            FormHelper.AttachDirtyTracking(this, () => _dirty = true);
            FormClosing += FrmRizikObrazac_FormClosing;
        }

        private void FrmRizikObrazac_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!_dirty) return;

            var answer = MessageBox.Show(
                UiMessages.UnsavedChangesSavePrompt,
                UiMessages.UnsavedChangesTitle,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

            switch (answer)
            {
                case DialogResult.Yes:
                    // Ako save ne uspije (uključujući DbUpdateConcurrencyException),
                    // TrySave već prikazuje postojeću error/concurrency poruku i
                    // vraća false — forma ostaje otvorena i dirty da korisnik ne
                    // izgubi promjene.
                    if (!TrySave())
                        e.Cancel = true;
                    break;

                case DialogResult.No:
                    // Odbaci nespremljene promjene — zatvori bez dodatnog savea.
                    break;

                case DialogResult.Cancel:
                default:
                    e.Cancel = true;
                    break;
            }
        }

        private void PositionOdgovorenoLabel()
        {
            lblOdgovoreno.Location = new Point(panelHeader.Width - lblOdgovoreno.Width - 20, 22);
        }

        // Broji koliko je od ukupno svih pitanja (sva četiri bloka) dobilo bilo
        // kakav odgovor (Da/Ne/N-P) — čisto informativno, ne utječe na
        // mogućnost snimanja obrasca.
        private void UpdateOdgovorenoCounter()
        {
            var sviCombosi = _combosOpci.Concat(_combosIdentifikacija).Concat(_combosTransakcije).Concat(_combosGeografski).ToList();
            int ukupno = sviCombosi.Count;
            int odgovoreno = sviCombosi.Count(c => !string.IsNullOrEmpty(c.Text));

            lblOdgovoreno.Text = $"{odgovoreno} / {ukupno} pitanja odgovoreno";
            PositionOdgovorenoLabel();
        }

        // ── Dinamičko raspoređivanje ─────────────────────────────────────

        private void BuildLayout()
        {
            int y = 10;
            y = LayoutKriterijiCard(cardOpci, y, "Opći indikatori",
                RizikObrazacKriteriji.OpciIndikatori, _combosOpci, out _comboProcjenaOpcih);
            y = LayoutKriterijiCard(cardIdentifikacija, y, "Indikatori vezani za identifikaciju klijenata",
                RizikObrazacKriteriji.IndikatoriIdentifikacije, _combosIdentifikacija, out _comboProcjenaIdentifikacije);
            y = LayoutKriterijiCard(cardTransakcije, y, "Indikatori vezani za transakcije",
                RizikObrazacKriteriji.IndikatoriTransakcija, _combosTransakcije, out _comboProcjenaTransakcija);
            y = LayoutKriterijiCard(cardGeografski, y, "Geografski rizik i ostali rizici",
                RizikObrazacKriteriji.GeografskiRizikOstali, _combosGeografski, out _comboProcjenaGeografskog,
                withOstalaZapazanja: true);
            LayoutUkupnoCard(cardUkupno, y);
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

        private int LayoutKriterijiCard(Panel card, int y, string title, string[] pitanja, List<ComboBox> combos, out ComboBox procjenaCombo,
            bool withOstalaZapazanja = false)
        {
            card.Controls.Clear();
            combos.Clear();
            card.Location = new Point(10, y);
            card.Width = 860;
            AddCardHeader(card, title);

            int cy = CardHeaderHeight + 14;
            for (int i = 0; i < pitanja.Length; i++)
            {
                cy = AddKriterijRow(card, cy, $"{i + 1}. {pitanja[i]}", out var combo);
                combos.Add(combo);
            }

            if (withOstalaZapazanja)
            {
                var lblZapazanja = new Label
                {
                    Text = $"{pitanja.Length + 1}. {RizikObrazacKriteriji.OstalaSumnjivaZapazanja}:",
                    Location = new Point(15, cy + 4),
                    AutoSize = true,
                    Font = UiTheme.Base(9f),
                    ForeColor = UiTheme.LabelText,
                };
                card.Controls.Add(lblZapazanja);

                _txtOstalaZapazanja = new TextBox { Name = "txtOstalaZapazanja" };
                UiTheme.StyleTextBox(_txtOstalaZapazanja);
                card.Controls.Add(UiTheme.WrapWithFocusBorder(_txtOstalaZapazanja, new Point(230, cy), new Size(610, 24)));
                cy += 34;
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

            // Svaki odgovor (Da/Ne/N/P) u ovom bloku automatski preračunava
            // PROCJENA combo tog istog bloka (vidi RecalculateProcjena) —
            // korisnik i dalje može ručno prepisati rezultat, ali dok god
            // mijenja odgovore, prijedlog se dinamički ažurira.
            var procjenaComboLocal = procjenaCombo;
            foreach (var combo in combos)
                combo.SelectedIndexChanged += (_, _) =>
                {
                    RecalculateProcjena(combos, procjenaComboLocal);
                    UpdateOdgovorenoCounter();
                };

            // Ručna izmjena procjene ovog bloka (ili automatski preračun iznad)
            // mora ažurirati i Ukupnu procjenu na dnu obrasca.
            procjenaCombo.SelectedIndexChanged += (_, _) => RecalculateUkupnaProcjena();

            cy += 32;
            card.Height = cy + 10;
            return y + card.Height + 16;
        }

        // Broji DA naspram NE u bloku (N/P i prazno se ignorišu); DA >= NE
        // (uz bar jedan odgovoren kriterij) daje VIŠE, inače NIŽE. Ništa se ne
        // postavlja dok bar jedan kriterij u bloku nije odgovoren, da prazan
        // obrazac ne ispadne automatski "NIŽE".
        // Po dogovoru s klijentom: ako je BAR JEDAN kriterij u bloku
        // odgovoren sa DA, cijeli blok je VIŠE — nije bitno koliko je NE
        // odgovora uz njega. NIŽE ide samo ako je bar jedan kriterij
        // odgovoren, a nijedan od odgovorenih nije DA (N/P i prazno se i
        // dalje ignorišu).
        private static void RecalculateProcjena(List<ComboBox> combos, ComboBox procjenaCombo)
        {
            int da = combos.Count(c => c.Text == RizikObrazacKriteriji.Da);
            int ne = combos.Count(c => c.Text == RizikObrazacKriteriji.Ne);

            if (da + ne == 0) return;

            procjenaCombo.Text = da > 0 ? RizikObrazacKriteriji.Vise : RizikObrazacKriteriji.Nize;
        }

        // Ukupna procjena je VIŠE čim je bilo koja od četiri pod-procjene VIŠE
        // (konzervativno — jedan faktor povišenog rizika je dovoljan), inače
        // NIŽE. Ništa se ne postavlja dok nijedna pod-procjena nije određena.
        private void RecalculateUkupnaProcjena()
        {
            var podprocjene = new[]
            {
                _comboProcjenaOpcih.Text, _comboProcjenaIdentifikacije.Text,
                _comboProcjenaTransakcija.Text, _comboProcjenaGeografskog.Text,
            };
            if (podprocjene.All(string.IsNullOrEmpty)) return;

            _comboUkupnaProcjena.Text = podprocjene.Contains(RizikObrazacKriteriji.Vise)
                ? RizikObrazacKriteriji.Vise
                : RizikObrazacKriteriji.Nize;
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
            SetCombos(_combosOpci, p.OpciIndikatoriOdgovori);
            SetCombos(_combosIdentifikacija, p.IdentifikacijaOdgovori);
            SetCombos(_combosTransakcije, p.TransakcijeOdgovori);
            SetCombos(_combosGeografski, p.GeografskiRizikOdgovori);

            _comboProcjenaOpcih.Text = p.ProcjenaOpcihIndikatora ?? "";
            _comboProcjenaIdentifikacije.Text = p.ProcjenaIdentifikacije ?? "";
            _comboProcjenaTransakcija.Text = p.ProcjenaTransakcija ?? "";
            _comboProcjenaGeografskog.Text = p.ProcjenaGeografskog ?? "";
            _txtOstalaZapazanja.Text = p.OstalaSumnjivaZapazanja ?? "";
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
            OpciIndikatoriOdgovori = ReadCombos(_combosOpci),
            ProcjenaOpcihIndikatora = NullIfEmpty(_comboProcjenaOpcih.Text),
            IdentifikacijaOdgovori = ReadCombos(_combosIdentifikacija),
            ProcjenaIdentifikacije = NullIfEmpty(_comboProcjenaIdentifikacije.Text),
            TransakcijeOdgovori = ReadCombos(_combosTransakcije),
            ProcjenaTransakcija = NullIfEmpty(_comboProcjenaTransakcija.Text),
            GeografskiRizikOdgovori = ReadCombos(_combosGeografski),
            OstalaSumnjivaZapazanja = NullIfEmpty(_txtOstalaZapazanja.Text),
            ProcjenaGeografskog = NullIfEmpty(_comboProcjenaGeografskog.Text),
            UkupnaProcjena = NullIfEmpty(_comboUkupnaProcjena.Text),
            DatumProcjene = _dtDatumProcjene.Value,
            Odobrio = NullIfEmpty(_txtOdobrio.Text),
        };

        private static List<string?> ReadCombos(List<ComboBox> combos) =>
            combos.Select(c => NullIfEmpty(c.Text)).ToList();

        private static string? NullIfEmpty(string? s) => string.IsNullOrWhiteSpace(s) ? null : s;

        // ── Dugmad ────────────────────────────────────────────────────────

        private void btnSacuvaj_Click(object sender, EventArgs e) => TrySave();

        // Zajednička save logika za btnSacuvaj i FormClosing (kad korisnik na
        // "nesačuvane promjene" upitu odabere Da) — isti save poziv, ista
        // obrada grešaka/concurrency-a, samo se rezultat (uspjeh/neuspjeh)
        // vraća pozivaocu umjesto da se ignoriše, jer FormClosing mora znati
        // smije li zatvoriti formu.
        private bool TrySave()
        {
            try
            {
                using var db = DbContextFactory.Create();
                _version = new RizikObrazacService(db).Save(_klijentId, CollectPodaci(), _version);
                _dirty = false;
                WasSaved = true;
                DialogHelper.ShowSaved("Obrazac je sačuvan.");
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                DialogHelper.ShowConcurrencyConflict();
                return false;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri čuvanju obrasca za procjenu rizika");
                return false;
            }
        }

        private async void btnExportPdf_Click(object sender, EventArgs e)
        {
            // Save-As dialog se prikazuje PRIJE snimanja u bazu — ako korisnik
            // klikne Cancel na dijalogu, trenutne izmjene na formi ne smiju
            // završiti upisane u bazu kao neželjeni nusprodukt neuspjelog
            // exporta. Baza se ažurira tek kad je korisnik stvarno potvrdio
            // export (odabrao lokaciju fajla), jer GenerateRizikObrazacPdf
            // čita podatke iz baze, pa moraju biti snimljeni prije generisanja
            // PDF-a — ali samo u tom slučaju, ne unaprijed.
            using var dialog = DialogHelper.CreateSaveDialogPdf(
                "Sačuvaj obrazac za procjenu rizika",
                DialogHelper.BuildSafeFileName($"Procjena_rizika_{_nazivKlijenta}"));
            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var db = DbContextFactory.Create();
                _version = new RizikObrazacService(db).Save(_klijentId, CollectPodaci(), _version);
                _dirty = false;
                WasSaved = true;
            }
            catch (DbUpdateConcurrencyException)
            {
                DialogHelper.ShowConcurrencyConflict();
                return;
            }
            catch (Exception ex)
            {
                DialogHelper.LogAndShowError(ex, "Greška pri čuvanju obrasca za procjenu rizika");
                return;
            }

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
