using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using OwnerTrack.Infrastructure.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OwnerTrack.Infrastructure.Services
{

    public class PdfExportService
    {


        private const float SinglePageMarginMm = 18f;
        private const float TablePageMarginMm = 12f;
        private const float SinglePageFontSize = 9f;
        private const float TablePageFontSize = 7.5f;

        private readonly OwnerTrackDbContext _db;

        public PdfExportService(OwnerTrackDbContext db)
        {
            _db = db;
            QuestPDF.Settings.License = LicenseType.Community;
        }


        public string GenerirajPdf(int klijentId, string outputPath)
        {
            var klijent = LoadKlijent(klijentId)
                ?? throw new InvalidOperationException($"Klijent ID={klijentId} nije pronađen.");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(SinglePageMarginMm, Unit.Millimetre);
                    page.DefaultTextStyle(t => t.FontFamily(Fonts.Arial).FontSize(SinglePageFontSize).FontColor(PdfColours.Text));

                    page.Header().Element(c => BuildKlijentHeader(c, klijent));
                    page.Content().Element(c => BuildKlijentContent(c, klijent));
                    page.Footer().Element(BuildFooter);
                });
            }).GeneratePdf(outputPath);

            return outputPath;
        }

        public string GenerirajTabeluKlijenata(List<int> klijentIds, string outputPath)
        {
           
            var byId = _db.Klijenti
                .AsNoTracking()
                .Include(x => x.Djelatnost)
                .Include(x => x.Vlasnici)
                .Include(x => x.Direktori)
                .Include(x => x.Ugovor)
                .Where(x => klijentIds.Contains(x.Id))
                .ToDictionary(k => k.Id);

            var klijenti = klijentIds
                .Where(id => byId.ContainsKey(id))
                .Select(id => byId[id])
                .ToList();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(TablePageMarginMm, Unit.Millimetre);
                    page.DefaultTextStyle(t => t.FontFamily(Fonts.Arial).FontSize(TablePageFontSize).FontColor(PdfColours.Text));

                    page.Header().Element(c => BuildTableHeader(c, klijenti.Count));
                    page.Content().Element(c => BuildTableContent(c, klijenti));
                    page.Footer().Element(BuildFooter);
                });
            }).GeneratePdf(outputPath);

            return outputPath;
        }


        private static void BuildKlijentHeader(IContainer c, Klijent k)
        {
            string statusColour = PdfRenderHelpers.EntityStatusColour(k.Status);

            c.Background(PdfColours.Navy).Padding(14).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text(txt =>
                    {
                        txt.AlignCenter();
                        txt.Span(k.Naziv).FontSize(20).FontColor(PdfColours.White).Bold();
                    });
                    col.Item().PaddingTop(4).Text(txt =>
                    {
                        txt.AlignCenter();
                        txt.Span($"ID: {k.IdBroj}   |   {PdfRenderHelpers.Fmt(k.VrstaKlijenta)}")
                           .FontSize(10).FontColor(PdfColours.HeaderSub);
                    });
                });

                row.ConstantItem(70).AlignMiddle().AlignRight()
                   .Background(statusColour).Padding(5)
                   .Text(txt =>
                   {
                       txt.AlignCenter();
                       txt.Span(PdfRenderHelpers.Fmt(k.Status)).FontSize(9).FontColor(PdfColours.White).Bold();
                   });
            });
        }

        private static void BuildKlijentContent(IContainer c, Klijent k)
        {
            c.PaddingTop(10).Column(col =>
            {
                col.Spacing(10);

                col.Item().Element(x => PdfRenderHelpers.RenderSectionHeader(x, "OSNOVNI PODACI"));
                col.Item().Element(x => BuildOsnovniPodaci(x, k));

                col.Item().Element(x => PdfRenderHelpers.RenderSectionHeader(x, "PROCJENA RIZIKA"));
                col.Item().Element(x => BuildRizik(x, k));

                col.Item().Element(x => PdfRenderHelpers.RenderSectionHeader(x, "UGOVOR"));
                col.Item().Element(x => BuildUgovor(x, k.Ugovor));

                var vlasnici = k.Vlasnici.ToList();
                col.Item().Element(x => PdfRenderHelpers.RenderSectionHeader(x, $"VLASNICI  ({vlasnici.Count})"));
                col.Item().Element(x => BuildVlasnici(x, vlasnici));

                var direktori = k.Direktori.ToList();
                col.Item().Element(x => PdfRenderHelpers.RenderSectionHeader(x, $"DIREKTORI  ({direktori.Count})"));
                col.Item().Element(x => BuildDirektori(x, direktori));
            });
        }

        private static void BuildOsnovniPodaci(IContainer c, Klijent k)
        {
            string djelatnost = string.IsNullOrWhiteSpace(k.SifraDjelatnosti)
                ? PdfRenderHelpers.Fmt(k.Djelatnost?.Naziv)
                : $"{k.SifraDjelatnosti} – {PdfRenderHelpers.Fmt(k.Djelatnost?.Naziv)}";

            var rows = new[]
            {
                ("Adresa:",         PdfRenderHelpers.Fmt(k.Adresa),   "Datum osnivanja:", PdfRenderHelpers.FmtDate(k.DatumOsnivanja)),
                ("Djelatnost:",     djelatnost,                        "Datum uspostave:", PdfRenderHelpers.FmtDate(k.DatumUspostave)),
                ("Veličina firme:", PdfRenderHelpers.Fmt(k.Velicina), "Ovjera / CR:",     PdfRenderHelpers.Fmt(k.OvjeraCr)),
                ("Email:",          PdfRenderHelpers.Fmt(k.Email),    "Telefon:",         PdfRenderHelpers.Fmt(k.Telefon)),
            };

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                });

                for (int i = 0; i < rows.Length; i++)
                {
                    var (l1, v1, l2, v2) = rows[i];
                    PdfRenderHelpers.RenderInfoRow(tbl, PdfRenderHelpers.AlternatingBackground(i), l1, v1, l2, v2);
                }

                if (!string.IsNullOrWhiteSpace(k.Napomena))
                {
                    string bg = PdfRenderHelpers.AlternatingBackground(rows.Length);
                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span("Napomena:").FontColor(PdfColours.TextMuted));
                    tbl.Cell().ColumnSpan(3).Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(k.Napomena).Bold());
                }
            });
        }

        private static void BuildRizik(IContainer c, Klijent k)
        {
            var rows = new[]
            {
                ("PEP:",             PdfRenderHelpers.Fmt(k.PepRizik),       "UBO:",              PdfRenderHelpers.Fmt(k.UboRizik)),
                ("Gotovina rizik:",  PdfRenderHelpers.Fmt(k.GotovinaRizik),  "Geografski rizik:", PdfRenderHelpers.Fmt(k.GeografskiRizik)),
                ("Ukupna procjena:", PdfRenderHelpers.Fmt(k.UkupnaProcjena), "Datum procjene:",   PdfRenderHelpers.FmtDate(k.DatumProcjene)),
            };

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                });

                for (int i = 0; i < rows.Length; i++)
                {
                    var (l1, v1, l2, v2) = rows[i];
                    string bg = PdfRenderHelpers.AlternatingBackground(i);
                    bool isRiskRow = i < 2;  // first two rows have colour-coded values

                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(l1).FontColor(PdfColours.TextMuted));
                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(v1).Bold().FontColor(isRiskRow ? PdfRenderHelpers.DaNeColour(v1) : PdfColours.Text));
                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(l2).FontColor(PdfColours.TextMuted));
                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(v2).Bold().FontColor(isRiskRow ? PdfRenderHelpers.DaNeColour(v2) : PdfColours.Text));
                }
            });
        }

        private static void BuildUgovor(IContainer c, Ugovor? u)
        {
            if (u is null)
            {
                PdfRenderHelpers.RenderEmptyNote(c, "Nema podataka o ugovoru.");
                return;
            }

            string statusColour = PdfRenderHelpers.ContractStatusColour(u.StatusUgovora);

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                });

                tbl.Cell().Background(PdfColours.White).PaddingHorizontal(8).PaddingVertical(5)
                   .Text(txt => txt.Span("Status ugovora:").FontColor(PdfColours.TextMuted));
                tbl.Cell().Background(PdfColours.White).PaddingHorizontal(8).PaddingVertical(5)
                   .Text(txt => txt.Span(PdfRenderHelpers.Fmt(u.StatusUgovora)).Bold().FontColor(statusColour));
                tbl.Cell().Background(PdfColours.White).PaddingHorizontal(8).PaddingVertical(5)
                   .Text(txt => txt.Span("Datum ugovora:").FontColor(PdfColours.TextMuted));
                tbl.Cell().Background(PdfColours.White).PaddingHorizontal(8).PaddingVertical(5)
                   .Text(txt => txt.Span(PdfRenderHelpers.FmtDate(u.DatumUgovora)).Bold());

                if (!string.IsNullOrWhiteSpace(u.VrstaUgovora) || !string.IsNullOrWhiteSpace(u.Napomena))
                    PdfRenderHelpers.RenderInfoRow(tbl, PdfColours.Grey,
                        "Vrsta ugovora:", PdfRenderHelpers.Fmt(u.VrstaUgovora),
                        "Napomena:", PdfRenderHelpers.Fmt(u.Napomena));
            });
        }

        private static void BuildVlasnici(IContainer c, List<Vlasnik> vlasnici)
        {
            if (!vlasnici.Any())
            {
                PdfRenderHelpers.RenderEmptyNote(c, "Nema evidentiranih vlasnika.");
                return;
            }

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(8, Unit.Millimetre);
                    cd.ConstantColumn(44, Unit.Millimetre);
                    cd.ConstantColumn(18, Unit.Millimetre);
                    cd.ConstantColumn(28, Unit.Millimetre);
                    cd.ConstantColumn(27, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(18, Unit.Millimetre);
                });

                foreach (string h in new[] { "#", "Ime i prezime", "% vlasn.", "Valjanost dok.", "Datum utvrđ.", "Izvor podatka", "Status" })
                    PdfRenderHelpers.RenderTableHeader(tbl, h);

                for (int i = 0; i < vlasnici.Count; i++)
                {
                    Vlasnik v = vlasnici[i];
                    string bg = PdfRenderHelpers.AlternatingBackground(i);
                    string sc = v.Status == StatusEntiteta.AKTIVAN ? PdfColours.Green : PdfColours.Red;
                    string pct = v.ProcenatVlasnistva > 0 ? $"{v.ProcenatVlasnistva:0.##} %" : "—";

                    PdfRenderHelpers.RenderTableCell(tbl, bg, (i + 1).ToString(), center: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(v.ImePrezime));
                    PdfRenderHelpers.RenderTableCell(tbl, bg, pct, center: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.FmtDate(v.DatumValjanostiDokumenta), center: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.FmtDate(v.DatumUtvrdjivanja), center: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(v.IzvorPodatka));
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(v.Status), bold: true, colour: sc, center: true);
                }
            });
        }

        private static void BuildDirektori(IContainer c, List<Direktor> direktori)
        {
            if (!direktori.Any())
            {
                PdfRenderHelpers.RenderEmptyNote(c, "Nema evidentiranih direktora.");
                return;
            }

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(8, Unit.Millimetre);
                    cd.ConstantColumn(44, Unit.Millimetre);
                    cd.ConstantColumn(28, Unit.Millimetre);
                    cd.ConstantColumn(30, Unit.Millimetre);
                    cd.ConstantColumn(27, Unit.Millimetre);
                    cd.ConstantColumn(18, Unit.Millimetre);
                });

                foreach (string h in new[] { "#", "Ime i prezime", "JMBG", "Tip valjanosti", "Datum valjanosti", "Status" })
                    PdfRenderHelpers.RenderTableHeader(tbl, h);

                for (int i = 0; i < direktori.Count; i++)
                {
                    Direktor d = direktori[i];
                    string bg = PdfRenderHelpers.AlternatingBackground(i);
                    string sc = d.Status == StatusEntiteta.AKTIVAN ? PdfColours.Green : PdfColours.Red;
                    string datVal = d.TipValjanosti == TipValjanostiKonstante.Trajno
                        ? TipValjanostiKonstante.Trajno
                        : PdfRenderHelpers.FmtDate(d.DatumValjanosti);

                    PdfRenderHelpers.RenderTableCell(tbl, bg, (i + 1).ToString(), center: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(d.ImePrezime));
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(d.Jmbg), center: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(d.TipValjanosti), center: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, datVal, center: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(d.Status), bold: true, colour: sc, center: true);
                }
            });
        }



        private static void BuildTableHeader(IContainer c, int total)
        {
            c.Background(PdfColours.Navy).Padding(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text(txt =>
                        txt.Span("Pregled klijenata").FontSize(16).FontColor(PdfColours.White).Bold());
                    col.Item().PaddingTop(3).Text(txt =>
                        txt.Span($"Ukupno klijenata: {total}   |   Datum izvještaja: {DateTime.Now:dd.MM.yyyy.}")
                           .FontSize(9).FontColor(PdfColours.HeaderSub));
                });
            });
        }

        private static void BuildTableContent(IContainer c, List<Klijent> klijenti)
        {
            c.PaddingTop(8).Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(10, Unit.Millimetre);
                    cd.RelativeColumn(2);
                    cd.ConstantColumn(27, Unit.Millimetre);
                    cd.RelativeColumn(1.5f);
                    cd.ConstantColumn(32, Unit.Millimetre);
                    cd.ConstantColumn(14, Unit.Millimetre);
                    cd.ConstantColumn(14, Unit.Millimetre);
                    cd.ConstantColumn(22, Unit.Millimetre);
                    cd.ConstantColumn(20, Unit.Millimetre);
                    cd.ConstantColumn(22, Unit.Millimetre);
                    cd.ConstantColumn(22, Unit.Millimetre);
                    cd.ConstantColumn(16, Unit.Millimetre);
                });

                foreach (string h in new[] { "#", "Naziv klijenta", "ID broj", "Djelatnost", "Veličina", "PEP", "UBO", "Procjena", "Ugovor", "Dat. uspostave", "Dat. osnivanja", "Status" })
                    PdfRenderHelpers.RenderTableHeader(tbl, h);

                for (int i = 0; i < klijenti.Count; i++)
                {
                    Klijent k = klijenti[i];
                    string bg = PdfRenderHelpers.AlternatingBackground(i);
                    string sc = PdfRenderHelpers.EntityStatusColour(k.Status);

                    string djelatnost = k.Djelatnost?.Naziv ?? k.SifraDjelatnosti ?? "—";
                    string statusUgovora = k.Ugovor?.StatusUgovora ?? "—";

                    PdfRenderHelpers.RenderTableCell(tbl, bg, (i + 1).ToString(), center: true, noWrap: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(k.Naziv));
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(k.IdBroj), center: true, noWrap: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(djelatnost));
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(k.Velicina), center: true, noWrap: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(k.PepRizik), bold: true, colour: PdfRenderHelpers.DaNeColour(k.PepRizik), center: true, noWrap: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(k.UboRizik), bold: true, colour: PdfRenderHelpers.DaNeColour(k.UboRizik), center: true, noWrap: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(k.UkupnaProcjena), center: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, statusUgovora, center: true, noWrap: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.FmtDate(k.DatumUspostave), center: true, noWrap: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.FmtDate(k.DatumOsnivanja), center: true, noWrap: true);
                    PdfRenderHelpers.RenderTableCell(tbl, bg, PdfRenderHelpers.Fmt(k.Status), bold: true, colour: sc, center: true, noWrap: true);
                }
            });
        }



        private static void BuildFooter(IContainer c)
        {
            c.PaddingTop(5).Row(row =>
            {
                row.RelativeItem().Text(txt =>
                    txt.Span($"Izvještaj generisan: {DateTime.Now:dd.MM.yyyy. u HH:mm}")
                       .FontSize(7).FontColor(PdfColours.Footer));

                row.ConstantItem(60).AlignRight().Text(txt =>
                {
                    txt.Span("Stranica ").FontSize(7).FontColor(PdfColours.Footer);
                    txt.CurrentPageNumber().FontSize(7).FontColor(PdfColours.Footer);
                    txt.Span(" / ").FontSize(7).FontColor(PdfColours.Footer);
                    txt.TotalPages().FontSize(7).FontColor(PdfColours.Footer);
                });
            });
        }



        private Klijent? LoadKlijent(int klijentId) =>
            _db.Klijenti
               .AsNoTracking()
               .Include(x => x.Djelatnost)
               .Include(x => x.Vlasnici)
               .Include(x => x.Direktori)
               .Include(x => x.Ugovor)
               .FirstOrDefault(x => x.Id == klijentId);
    }
}