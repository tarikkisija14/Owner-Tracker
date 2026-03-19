using Microsoft.EntityFrameworkCore;
using OwnerTrack.App.Helpers;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Services;
using OwnerTrack.Infrastructure.Validators;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class FrmDodajKlijent : Form
    {
        private readonly OwnerTrackDbContext _db;
        private readonly AuditService _audit;
        private readonly int? _klijentId;

        public FrmDodajKlijent(int? klijentId, OwnerTrackDbContext db)
        {
            InitializeComponent();
            _klijentId = klijentId;
            _db = db;
            _audit = new AuditService(db);
        }

        
        private void FrmDodajKlijent_Load(object sender, EventArgs e)
        {
            PopuniComboe();
            LoadSifre();

            if (_klijentId.HasValue)
            {
                UcitajKlijenta(_klijentId.Value);
                Text = "Izmijeni firmu";
                btnSpremi.Text = "💾 Spremi izmjene";
            }
        }

        private void PopuniComboe()
        {
            FormHelper.PopuniEnumCombo<VrstaKlijenta>(cbVrstaKlijenta);
            FormHelper.PopuniEnumCombo<VelicinaFirme>(cbVelicina);
            FormHelper.PopuniEnumComboSPraznim<DaNe>(cbPepRizik);
            FormHelper.PopuniEnumComboSPraznim<DaNe>(cbUboRizik);
            FormHelper.PopuniEnumComboSPraznim<DaNe>(cbGotovinaRizik);
            FormHelper.PopuniEnumComboSPraznim<DaNe>(cbGeografskiRizik);
            FormHelper.PopuniEnumCombo<StatusEntiteta>(cbStatus);

            cbStatusUgovora.Items.Clear();
            cbStatusUgovora.Items.AddRange(StatusUgovora.Svi);
            cbStatusUgovora.Items.Insert(0, "");

            if (!_klijentId.HasValue)
                PostaviDefaultneVrijednosti();
        }

        private void PostaviDefaultneVrijednosti()
        {
            cbVrstaKlijenta.SelectedIndex = 0;
            cbVelicina.SelectedIndex = 0;
            cbPepRizik.SelectedIndex = 0;
            cbUboRizik.SelectedIndex = 0;
            cbGotovinaRizik.SelectedIndex = 0;
            cbGeografskiRizik.SelectedIndex = 0;
            cbStatusUgovora.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
        }

        private void LoadSifre()
        {
            var sifre = _db.Djelatnosti.OrderBy(d => d.Sifra).ToList();
            cbSifra.DataSource = sifre;
            cbSifra.DisplayMember = "Naziv";
            cbSifra.ValueMember = "Sifra";

            if (sifre.Count > 0 && !_klijentId.HasValue)
                cbSifra.SelectedIndex = 0;
        }

        private void UcitajKlijenta(int id)
        {
            var k = _db.Klijenti.Include(x => x.Ugovor).FirstOrDefault(x => x.Id == id);
            if (k == null) return;

            txtNaziv.Text = k.Naziv ?? string.Empty;
            txtIdBroj.Text = k.IdBroj ?? string.Empty;
            txtAdresa.Text = k.Adresa ?? string.Empty;
            txtEmail.Text = k.Email ?? string.Empty;
            txtTelefon.Text = k.Telefon ?? string.Empty;
            txtOvjeraCr.Text = k.OvjeraCr ?? string.Empty;
            txtUkupnaProcjena.Text = k.UkupnaProcjena ?? string.Empty;
            txtNapomena.Text = k.Napomena ?? string.Empty;

            if (!string.IsNullOrEmpty(k.SifraDjelatnosti))
                cbSifra.SelectedValue = k.SifraDjelatnosti;

            dtDatumUspostave.Value = k.DatumUspostave ?? DateTime.Now;
            dtDatumOsnivanja.Value = k.DatumOsnivanja ?? DateTime.Now;
            dtDatumProcjene.Value = k.DatumProcjene ?? DateTime.Now;

            FormHelper.SetCombo(cbVrstaKlijenta, k.VrstaKlijenta?.ToString());
            FormHelper.SetCombo(cbVelicina, k.Velicina);
            FormHelper.SetCombo(cbPepRizik, k.PepRizik);
            FormHelper.SetCombo(cbUboRizik, k.UboRizik);
            FormHelper.SetCombo(cbGotovinaRizik, k.GotovinaRizik);
            FormHelper.SetCombo(cbGeografskiRizik, k.GeografskiRizik);
            FormHelper.SetCombo(cbStatus, k.Status.ToString());

            if (k.Ugovor != null)
            {
                txtVrstaUgovora.Text = k.Ugovor.VrstaUgovora ?? string.Empty;
                dtDatumUgovora.Value = k.Ugovor.DatumUgovora ?? DateTime.Now;
                FormHelper.SetCombo(cbStatusUgovora, k.Ugovor.StatusUgovora);
            }
        }

       
        private bool ValidirajPolja(string naziv, string idBroj)
        {
            
            string? jibGreska = JibValidator.GreskaValidacije(idBroj);
            if (jibGreska != null)
            {
                MessageBox.Show(jibGreska, "Greška validacije", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdBroj.Focus();
                return false;
            }

            int trenutniId = _klijentId ?? 0;

            if (_db.Set<Klijent>().IgnoreQueryFilters()
                    .Any(k => k.IdBroj == idBroj && k.Id != trenutniId && k.Obrisan == null))
            {
                MessageBox.Show($"Klijent s ID brojem '{idBroj}' već postoji!", "Duplikat",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtIdBroj.Focus();
                return false;
            }

            if (_db.Set<Klijent>().IgnoreQueryFilters()
                    .Any(k => k.Naziv == naziv && k.Id != trenutniId && k.Obrisan == null))
            {
                MessageBox.Show($"Klijent s nazivom '{naziv}' već postoji!", "Duplikat",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNaziv.Focus();
                return false;
            }

            return true;
        }

        
        private void PrimijeniPoljaUKlijenta(Klijent k)
        {
            k.Adresa = txtAdresa.Text;
            k.SifraDjelatnosti = cbSifra.SelectedValue?.ToString() ?? string.Empty;
            k.DatumUspostave = dtDatumUspostave.Value;
            k.DatumOsnivanja = dtDatumOsnivanja.Value;
            k.Velicina = cbVelicina.Text;
            k.PepRizik = FormHelper.NullAkoJePrazno(cbPepRizik.Text);
            k.UboRizik = FormHelper.NullAkoJePrazno(cbUboRizik.Text);
            k.GotovinaRizik = FormHelper.NullAkoJePrazno(cbGotovinaRizik.Text);
            k.GeografskiRizik = FormHelper.NullAkoJePrazno(cbGeografskiRizik.Text);
            k.UkupnaProcjena = txtUkupnaProcjena.Text;
            k.DatumProcjene = dtDatumProcjene.Value;
            k.OvjeraCr = txtOvjeraCr.Text;
            k.Napomena = txtNapomena.Text;
            k.Email = FormHelper.NullAkoJePrazno(txtEmail.Text);
            k.Telefon = FormHelper.NullAkoJePrazno(txtTelefon.Text);
            k.VrstaKlijenta = Enum.TryParse<VrstaKlijenta>(cbVrstaKlijenta.Text, out var vk) ? vk : null;
            k.Status = Enum.TryParse<StatusEntiteta>(cbStatus.Text, out var se) ? se : StatusEntiteta.AKTIVAN;
        }

        private void PrimijeniPoljaUUgovor(Ugovor ugovor)
        {
            ugovor.VrstaUgovora = txtVrstaUgovora.Text;
            ugovor.StatusUgovora = cbStatusUgovora.Text;
            ugovor.DatumUgovora = dtDatumUgovora.Value;
        }

       
        private void btnSpremi_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNaziv.Text) || string.IsNullOrWhiteSpace(txtIdBroj.Text))
            {
                MessageBox.Show("Popuni obavezna polja: Naziv i ID Broj!");
                return;
            }

            string naziv = txtNaziv.Text.Trim();
            string idBroj = txtIdBroj.Text.Trim();

            if (!ValidirajPolja(naziv, idBroj)) return;

            try
            {
                if (_klijentId.HasValue)
                    SpremiIzmjenu(_klijentId.Value, naziv, idBroj);
                else
                    SpremiNovog(naziv, idBroj);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                DialogHelper.LogirajIPokaziGresku(ex, "Greška pri snimanju");
            }
        }

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

       
        private void SpremiIzmjenu(int id, string naziv, string idBroj)
        {
            var k = _db.Klijenti.Find(id);
            if (k == null) return;

            string stariNaziv = k.Naziv;
            k.Naziv = naziv;
            k.IdBroj = idBroj;
            k.Azuriran = DateTime.Now;
            PrimijeniPoljaUKlijenta(k);

            var ugovor = _db.Ugovori.FirstOrDefault(u => u.KlijentId == id);
            if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
            {
                if (ugovor == null) { ugovor = new Ugovor { KlijentId = id }; _db.Ugovori.Add(ugovor); }
                PrimijeniPoljaUUgovor(ugovor);
            }
            else if (ugovor != null)
            {
                _db.Ugovori.Remove(ugovor);
            }

            TransactionHelper.Execute(_db, db =>
            {
                db.SaveChanges();
                _audit.Izmijenjeno("Klijenti", id, $"'{stariNaziv}' → '{naziv}'");
                db.SaveChanges();
            });

            MessageBox.Show("Klijent ažuriran!");
        }

        private void SpremiNovog(string naziv, string idBroj)
        {
            var k = new Klijent { Naziv = naziv, IdBroj = idBroj, Kreiran = DateTime.Now };
            PrimijeniPoljaUKlijenta(k);

            TransactionHelper.Execute(_db, db =>
            {
                db.Klijenti.Add(k);
                db.SaveChanges();

                _audit.Dodano("Klijenti", k.Id, $"Novi klijent: '{naziv}' ({idBroj})");

                if (!string.IsNullOrWhiteSpace(cbStatusUgovora.Text))
                {
                    var ugovor = new Ugovor { KlijentId = k.Id };
                    PrimijeniPoljaUUgovor(ugovor);
                    db.Ugovori.Add(ugovor);
                }

                db.SaveChanges();
            });

            MessageBox.Show("Klijent dodan!");
        }
    }
}