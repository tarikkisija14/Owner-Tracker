using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OwnerTrack.App
{
    public partial class FrmUpozorenja : Form
    {
        private const int DANA_UPOZORENJE = 60;

        private readonly OwnerTrackDbContext _db;
        private List<UpozorenjeDetalj> _svaUpozorenja = new();

        public FrmUpozorenja(OwnerTrackDbContext db)
        {
            _db = db;
            InitializeComponent();
            UcitajUpozorenja();
        }


        private void UcitajUpozorenja()
        {
            var danas = DateTime.Today;
            var granica = danas.AddDays(DANA_UPOZORENJE);

            var vlasnici = _db.Vlasnici
                .AsNoTracking()
                .Where(v => v.DatumValjanostiDokumenta != null
                         && v.DatumValjanostiDokumenta <= granica
                         && v.Status == StatusKonstante.Aktivan
                         && v.Klijent.Status != StatusKonstante.Arhiviran)
                .Include(v => v.Klijent)
                .Select(v => new UpozorenjeDetalj
                {
                    KlijentId = v.KlijentId,
                    NazivFirme = v.Klijent.Naziv,
                    ImePrezime = v.ImePrezime,
                    Tip = "Vlasnik",
                    DatumIsteka = v.DatumValjanostiDokumenta.Value
                })
                .ToList();

            var direktori = _db.Direktori
                .AsNoTracking()
                .Where(d => d.DatumValjanosti != null
                         && d.DatumValjanosti <= granica
                         && d.TipValjanosti == "VREMENSKI"
                         && d.Status == StatusKonstante.Aktivan
                         && d.Klijent.Status != StatusKonstante.Arhiviran)
                .Include(d => d.Klijent)
                .Select(d => new UpozorenjeDetalj
                {
                    KlijentId = d.KlijentId,
                    NazivFirme = d.Klijent.Naziv,
                    ImePrezime = d.ImePrezime,
                    Tip = "Direktor",
                    DatumIsteka = d.DatumValjanosti.Value
                })
                .ToList();

            _svaUpozorenja = vlasnici
                .Concat(direktori)
                .OrderBy(x => x.DatumIsteka)
                .ToList();


            int istekli = _svaUpozorenja.Count(x => x.DatumIsteka < danas);
            int kriticni = _svaUpozorenja.Count(x => x.DatumIsteka >= danas && x.DatumIsteka <= danas.AddDays(14));
            int ostali = _svaUpozorenja.Count(x => x.DatumIsteka > danas.AddDays(14));
            int firmi = _svaUpozorenja.Select(x => x.KlijentId).Distinct().Count();

            lblSumarij.Text =
                $"Ukupno {firmi} firma s upozorenjima   |   " +
                $" Isteklo: {istekli}   " +
                $" Kritično (≤14 dana): {kriticni}   " +
                $" Uskoro (15–60 dana): {ostali}";

            panelTop.BackColor = istekli > 0
                ? Color.FromArgb(160, 30, 30)
                : kriticni > 0
                    ? Color.FromArgb(180, 90, 20)
                    : Color.FromArgb(130, 110, 20);


            var poFirmama = _svaUpozorenja
                .GroupBy(x => new { x.KlijentId, x.NazivFirme })
                .Select(g => new
                {
                    KlijentId = g.Key.KlijentId,
                    Firma = g.Key.NazivFirme,
                    Upozorenja = g.Count(),
                    NajbliziDatum = g.Min(x => x.DatumIsteka),
                    DanaDoIsteka = (int)(g.Min(x => x.DatumIsteka) - danas).TotalDays
                })
                .OrderBy(x => x.NajbliziDatum)
                .ToList();


            gridFirme.SelectionChanged -= gridFirme_SelectionChanged;
            gridFirme.DataSource = poFirmama;
            gridFirme.ClearSelection();
            gridFirme.SelectionChanged += gridFirme_SelectionChanged;

            if (gridFirme.Columns.Count > 0)
            {
                if (gridFirme.Columns.Contains("KlijentId"))
                    gridFirme.Columns["KlijentId"].Visible = false;

                gridFirme.Columns["Firma"].HeaderText = "Naziv firme";
                gridFirme.Columns["Firma"].FillWeight = 50;
                gridFirme.Columns["Upozorenja"].HeaderText = "Br. upozorenja";
                gridFirme.Columns["Upozorenja"].FillWeight = 15;
                gridFirme.Columns["NajbliziDatum"].HeaderText = "Najbliži datum isteka";
                gridFirme.Columns["NajbliziDatum"].FillWeight = 20;
                gridFirme.Columns["NajbliziDatum"].DefaultCellStyle.Format = "dd.MM.yyyy";
                gridFirme.Columns["DanaDoIsteka"].HeaderText = "Dana do isteka";
                gridFirme.Columns["DanaDoIsteka"].FillWeight = 15;
            }
        }


        private void gridFirme_SelectionChanged(object sender, EventArgs e)
        {
            if (gridFirme.SelectedRows.Count == 0)
            {
                gridDetalji.DataSource = null;
                return;
            }

            dynamic row = gridFirme.SelectedRows[0].DataBoundItem;
            int klijentId = row.KlijentId;

            var detalji = _svaUpozorenja
                .Where(x => x.KlijentId == klijentId)
                .Select(x => new
                {
                    Tip = x.Tip,
                    ImePrezime = x.ImePrezime,
                    DatumIsteka = x.DatumIsteka,
                    DanaDoIsteka = (int)(x.DatumIsteka - DateTime.Today).TotalDays,
                    Status = x.DatumIsteka < DateTime.Today
                                    ? " ISTEKLO"
                                    : x.DatumIsteka <= DateTime.Today.AddDays(14)
                                        ? " Kritično"
                                        : " Uskoro"
                })
                .OrderBy(x => x.DatumIsteka)
                .ToList();

            gridDetalji.DataSource = detalji;
            gridDetalji.ClearSelection();

            if (gridDetalji.Columns.Count > 0)
            {
                gridDetalji.Columns["Tip"].HeaderText = "Tip";
                gridDetalji.Columns["Tip"].FillWeight = 15;
                gridDetalji.Columns["ImePrezime"].HeaderText = "Ime i prezime";
                gridDetalji.Columns["ImePrezime"].FillWeight = 35;
                gridDetalji.Columns["DatumIsteka"].HeaderText = "Datum isteka";
                gridDetalji.Columns["DatumIsteka"].FillWeight = 20;
                gridDetalji.Columns["DatumIsteka"].DefaultCellStyle.Format = "dd.MM.yyyy";
                gridDetalji.Columns["DanaDoIsteka"].HeaderText = "Dana do isteka";
                gridDetalji.Columns["DanaDoIsteka"].FillWeight = 15;
                gridDetalji.Columns["Status"].HeaderText = "Status";
                gridDetalji.Columns["Status"].FillWeight = 15;
            }
        }


        private void gridFirme_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || gridFirme.Rows[e.RowIndex].DataBoundItem == null) return;
            dynamic item = gridFirme.Rows[e.RowIndex].DataBoundItem;
            gridFirme.Rows[e.RowIndex].DefaultCellStyle.BackColor = BojaZaDane(item.DanaDoIsteka);
        }

        private void gridDetalji_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || gridDetalji.Rows[e.RowIndex].DataBoundItem == null) return;
            dynamic item = gridDetalji.Rows[e.RowIndex].DataBoundItem;
            gridDetalji.Rows[e.RowIndex].DefaultCellStyle.BackColor = BojaZaDane(item.DanaDoIsteka);
        }

        private static Color BojaZaDane(int dana) => dana < 0
            ? Color.FromArgb(220, 80, 80)  
            : dana <= 14
                ? Color.FromArgb(255, 200, 120) 
                : Color.FromArgb(255, 245, 150); 


        private void btnZatvori_Click(object sender, EventArgs e) => Close();
    }
}