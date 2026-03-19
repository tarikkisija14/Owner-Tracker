using Microsoft.EntityFrameworkCore;
using OwnerTrack.Data.Entities;
using OwnerTrack.Data.Enums;
using OwnerTrack.Infrastructure.Database;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OwnerTrack.Infrastructure.Services
{
    public class PdfExportService
    {
        
        private const string ColourNavy = "#1a3a5c";
        private const string ColourNavyLight = "#e8f0f8";
        private const string ColourGrey = "#f5f5f5";
        private const string ColourBorder = "#c0c8d4";
        private const string ColourGreen = "#2e7d32";
        private const string ColourRed = "#c62828";
        private const string ColourOrange = "#e65100";
        private const string ColourText = "#212121";
        private const string ColourTextMuted = "#555555";
        private const string ColourFooter = "#888888";
        private const string ColourHeaderSub = "#b0c4de";
        private const string ColourWhite = "#ffffff";

        private readonly OwnerTrackDbContext _db;

        public PdfExportService(OwnerTrackDbContext db)
        {
            _db = db;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        
        public string GenerirajPdf(int klijentId, string outputPath)
        {
            var k = LoadKlijent(klijentId)
                ?? throw new InvalidOperationException($"Klijent ID={klijentId} nije pronađen.");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(18, Unit.Millimetre);
                    page.DefaultTextStyle(t => t.FontFamily(Fonts.Arial).FontSize(9).FontColor(ColourText));

                    page.Header().Element(c => BuildHeader(c, k));
                    page.Content().Element(c => BuildContent(c, k));
                    page.Footer().Element(BuildFooter);
                });
            }).GeneratePdf(outputPath);

            return outputPath;
        }

       
        public string GenerirajTabeluKlijenata(List<int> klijentIds, string outputPath)
        {
            
            var klijentiById = _db.Klijenti
                .AsNoTracking()
                .Include(x => x.Djelatnost)
                .Include(x => x.Vlasnici)
                .Include(x => x.Direktori)
                .Include(x => x.Ugovor)
                .Where(x => klijentIds.Contains(x.Id))
                .ToDictionary(k => k.Id);

            var klijenti = klijentIds
                .Where(id => klijentiById.ContainsKey(id))
                .Select(id => klijentiById[id])
                .ToList();

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(12, Unit.Millimetre);
                    page.DefaultTextStyle(t => t.FontFamily(Fonts.Arial).FontSize(7.5f).FontColor(ColourText));

                    page.Header().Element(c => BuildTabelaHeader(c, klijenti.Count));
                    page.Content().Element(c => BuildTabelaSadrzaj(c, klijenti));
                    page.Footer().Element(BuildFooter);
                });
            }).GeneratePdf(outputPath);

            return outputPath;
        }

      
        private void BuildHeader(IContainer c, Klijent k)
        {
            string statusColour = StatusColour(k.Status);

            c.Background(ColourNavy).Padding(14).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text(txt =>
                    {
                        txt.AlignCenter();
                        txt.Span(k.Naziv).FontSize(20).FontColor(ColourWhite).Bold();
                    });
                    col.Item().PaddingTop(4).Text(txt =>
                    {
                        txt.AlignCenter();
                        txt.Span($"ID: {k.IdBroj}   |   {Fmt(k.VrstaKlijenta)}").FontSize(10).FontColor(ColourHeaderSub);
                    });
                });

                row.ConstantItem(70).AlignMiddle().AlignRight()
                   .Background(statusColour).Padding(5)
                   .Text(txt =>
                   {
                       txt.AlignCenter();
                       txt.Span(Fmt(k.Status)).FontSize(9).FontColor(ColourWhite).Bold();
                   });
            });
        }

        private void BuildContent(IContainer c, Klijent k)
        {
            c.PaddingTop(10).Column(col =>
            {
                col.Spacing(10);

                col.Item().Element(x => SectionHeader(x, "OSNOVNI PODACI"));
                col.Item().Element(x => BuildOsnoviPodaci(x, k));

                col.Item().Element(x => SectionHeader(x, "PROCJENA RIZIKA"));
                col.Item().Element(x => BuildRizik(x, k));

                col.Item().Element(x => SectionHeader(x, "UGOVOR"));
                col.Item().Element(x => BuildUgovor(x, k.Ugovor));

                var vlasnici = k.Vlasnici.ToList();
                col.Item().Element(x => SectionHeader(x, $"VLASNICI  ({vlasnici.Count})"));
                col.Item().Element(x => BuildVlasnici(x, vlasnici));

                var direktori = k.Direktori.ToList();
                col.Item().Element(x => SectionHeader(x, $"DIREKTORI  ({direktori.Count})"));
                col.Item().Element(x => BuildDirektori(x, direktori));
            });
        }

        private void BuildOsnoviPodaci(IContainer c, Klijent k)
        {
            string djelatnost = string.IsNullOrWhiteSpace(k.SifraDjelatnosti)
                ? Fmt(k.Djelatnost?.Naziv)
                : $"{k.SifraDjelatnosti} – {Fmt(k.Djelatnost?.Naziv)}";

            var rows = new List<(string, string, string, string)>
            {
                ("Adresa:",         Fmt(k.Adresa),    "Datum osnivanja:", FmtDate(k.DatumOsnivanja)),
                ("Djelatnost:",     djelatnost,       "Datum uspostave:", FmtDate(k.DatumUspostave)),
                ("Veličina firme:", Fmt(k.Velicina),  "Ovjera / CR:",     Fmt(k.OvjeraCr)),
                ("Email:",          Fmt(k.Email),     "Telefon:",         Fmt(k.Telefon)),
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

                for (int i = 0; i < rows.Count; i++)
                {
                    var (l1, v1, l2, v2) = rows[i];
                    InfoRow(tbl, AlternatingBg(i), l1, v1, l2, v2);
                }

                if (!string.IsNullOrWhiteSpace(k.Napomena))
                {
                    string bg = AlternatingBg(rows.Count);
                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span("Napomena:").FontColor(ColourTextMuted));
                    tbl.Cell().ColumnSpan(3).Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(k.Napomena).Bold());
                }
            });
        }

        private void BuildRizik(IContainer c, Klijent k)
        {
            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                });

                var rows = new List<(string, string, string, string)>
                {
                    ("PEP:",             Fmt(k.PepRizik),       "UBO:",              Fmt(k.UboRizik)),
                    ("Gotovina rizik:",  Fmt(k.GotovinaRizik),  "Geografski rizik:", Fmt(k.GeografskiRizik)),
                    ("Ukupna procjena:", Fmt(k.UkupnaProcjena), "Datum procjene:",   FmtDate(k.DatumProcjene)),
                };

                for (int i = 0; i < rows.Count; i++)
                {
                    var (l1, v1, l2, v2) = rows[i];
                    string bg = AlternatingBg(i);
                    bool coloured = i < 2;

                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(l1).FontColor(ColourTextMuted));
                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(v1).Bold().FontColor(coloured ? DaNeColour(v1) : ColourText));
                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(l2).FontColor(ColourTextMuted));
                    tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
                       .Text(txt => txt.Span(v2).Bold().FontColor(coloured ? DaNeColour(v2) : ColourText));
                }
            });
        }

        private void BuildUgovor(IContainer c, Ugovor? u)
        {
            if (u == null)
            {
                c.PaddingHorizontal(8).PaddingVertical(5)
                 .Text(txt => txt.Span("Nema podataka o ugovoru.").FontColor(ColourFooter).Italic());
                return;
            }

            string statusTekst = u.StatusUgovora ?? "—";
            string statusColour = statusTekst == StatusUgovora.Potpisan ? ColourGreen
                                : statusTekst is StatusUgovora.Otkazan or StatusUgovora.Neaktivan ? ColourRed
                                : ColourOrange;

            c.Table(tbl =>
            {
                tbl.ColumnsDefinition(cd =>
                {
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                    cd.ConstantColumn(52, Unit.Millimetre);
                    cd.RelativeColumn();
                });

                tbl.Cell().Background(ColourWhite).PaddingHorizontal(8).PaddingVertical(5)
                   .Text(txt => txt.Span("Status ugovora:").FontColor(ColourTextMuted));
                tbl.Cell().Background(ColourWhite).PaddingHorizontal(8).PaddingVertical(5)
                   .Text(txt => txt.Span(Fmt(u.StatusUgovora)).Bold().FontColor(statusColour));
                tbl.Cell().Background(ColourWhite).PaddingHorizontal(8).PaddingVertical(5)
                   .Text(txt => txt.Span("Datum ugovora:").FontColor(ColourTextMuted));
                tbl.Cell().Background(ColourWhite).PaddingHorizontal(8).PaddingVertical(5)
                   .Text(txt => txt.Span(FmtDate(u.DatumUgovora)).Bold());

                if (!string.IsNullOrWhiteSpace(u.VrstaUgovora) || !string.IsNullOrWhiteSpace(u.Napomena))
                    InfoRow(tbl, ColourGrey, "Vrsta ugovora:", Fmt(u.VrstaUgovora), "Napomena:", Fmt(u.Napomena));
            });
        }

        private void BuildVlasnici(IContainer c, List<Vlasnik> vlasnici)
        {
            if (!vlasnici.Any())
            {
                c.PaddingHorizontal(8).PaddingVertical(5)
                 .Text(txt => txt.Span("Nema evidentiranih vlasnika.").FontColor(ColourFooter).Italic());
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

                foreach (var h in new[] { "#", "Ime i prezime", "% vlasn.", "Valjanost dok.", "Datum utvrđ.", "Izvor podatka", "Status" })
                    TableHeader(tbl, h);

                for (int i = 0; i < vlasnici.Count; i++)
                {
                    var v = vlasnici[i];
                    string bg = AlternatingBg(i);
                    string sc = v.Status == StatusEntiteta.AKTIVAN ? ColourGreen : ColourRed;
                    string pct = v.ProcenatVlasnistva > 0 ? $"{v.ProcenatVlasnistva:0.##} %" : "—";

                    TableCell(tbl, bg, (i + 1).ToString(), center: true);
                    TableCell(tbl, bg, Fmt(v.ImePrezime));
                    TableCell(tbl, bg, pct, center: true);
                    TableCell(tbl, bg, FmtDate(v.DatumValjanostiDokumenta), center: true);
                    TableCell(tbl, bg, FmtDate(v.DatumUtvrdjivanja), center: true);
                    TableCell(tbl, bg, Fmt(v.IzvorPodatka));
                    TableCell(tbl, bg, Fmt(v.Status), bold: true, colour: sc, center: true);
                }
            });
        }

        private void BuildDirektori(IContainer c, List<Direktor> direktori)
        {
            if (!direktori.Any())
            {
                c.PaddingHorizontal(8).PaddingVertical(5)
                 .Text(txt => txt.Span("Nema evidentiranih direktora.").FontColor(ColourFooter).Italic());
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

                foreach (var h in new[] { "#", "Ime i prezime", "JMBG", "Tip valjanosti", "Datum valjanosti", "Status" })
                    TableHeader(tbl, h);

                for (int i = 0; i < direktori.Count; i++)
                {
                    var d = direktori[i];
                    string bg = AlternatingBg(i);
                    string sc = d.Status == StatusEntiteta.AKTIVAN ? ColourGreen : ColourRed;
                    string datVal = d.TipValjanosti == TipValjanostiKonstante.Trajno
                        ? TipValjanostiKonstante.Trajno
                        : FmtDate(d.DatumValjanosti);

                    TableCell(tbl, bg, (i + 1).ToString(), center: true);
                    TableCell(tbl, bg, Fmt(d.ImePrezime));
                    TableCell(tbl, bg, Fmt(d.Jmbg), center: true);
                    TableCell(tbl, bg, Fmt(d.TipValjanosti), center: true);
                    TableCell(tbl, bg, datVal, center: true);
                    TableCell(tbl, bg, Fmt(d.Status), bold: true, colour: sc, center: true);
                }
            });
        }

       
        private void BuildTabelaHeader(IContainer c, int total)
        {
            c.Background(ColourNavy).Padding(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text(txt =>
                        txt.Span("Pregled klijenata").FontSize(16).FontColor(ColourWhite).Bold());
                    col.Item().PaddingTop(3).Text(txt =>
                        txt.Span($"Ukupno klijenata: {total}   |   Datum izvještaja: {DateTime.Now:dd.MM.yyyy.}")
                           .FontSize(9).FontColor(ColourHeaderSub));
                });
            });
        }

        private void BuildTabelaSadrzaj(IContainer c, List<Klijent> klijenti)
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

                foreach (var h in new[] { "#", "Naziv klijenta", "ID broj", "Djelatnost", "Veličina", "PEP", "UBO", "Procjena", "Ugovor", "Dat. uspostave", "Dat. osnivanja", "Status" })
                    TableHeader(tbl, h);

                for (int i = 0; i < klijenti.Count; i++)
                {
                    var k = klijenti[i];
                    string bg = AlternatingBg(i);
                    string sc = k.Status == StatusEntiteta.AKTIVAN ? ColourGreen
                               : k.Status == StatusEntiteta.ARHIVIRAN ? ColourOrange
                               : ColourRed;

                    string djelatnost = k.Djelatnost?.Naziv ?? k.SifraDjelatnosti ?? "—";
                    string statusUgovora = k.Ugovor?.StatusUgovora ?? "—";

                    TableCell(tbl, bg, (i + 1).ToString(), center: true, noWrap: true);
                    TableCell(tbl, bg, Fmt(k.Naziv));
                    TableCell(tbl, bg, Fmt(k.IdBroj), center: true, noWrap: true);
                    TableCell(tbl, bg, Fmt(djelatnost));
                    TableCell(tbl, bg, Fmt(k.Velicina), center: true, noWrap: true);
                    TableCell(tbl, bg, Fmt(k.PepRizik), bold: true, colour: DaNeColour(k.PepRizik), center: true, noWrap: true);
                    TableCell(tbl, bg, Fmt(k.UboRizik), bold: true, colour: DaNeColour(k.UboRizik), center: true, noWrap: true);
                    TableCell(tbl, bg, Fmt(k.UkupnaProcjena), center: true);
                    TableCell(tbl, bg, statusUgovora, center: true, noWrap: true);
                    TableCell(tbl, bg, FmtDate(k.DatumUspostave), center: true, noWrap: true);
                    TableCell(tbl, bg, FmtDate(k.DatumOsnivanja), center: true, noWrap: true);
                    TableCell(tbl, bg, Fmt(k.Status), bold: true, colour: sc, center: true, noWrap: true);
                }
            });
        }

       
        private static void SectionHeader(IContainer c, string text)
        {
            c.Background(ColourNavyLight)
             .PaddingHorizontal(10).PaddingVertical(7)
             .Text(txt => txt.Span(text).FontSize(10).FontColor(ColourNavy).Bold());
        }

        private static void BuildFooter(IContainer c)
        {
            c.PaddingTop(5).Row(row =>
            {
                row.RelativeItem().Text(txt =>
                    txt.Span($"Izvještaj generisan: {DateTime.Now:dd.MM.yyyy. u HH:mm}").FontSize(7).FontColor(ColourFooter));

                row.ConstantItem(60).AlignRight().Text(txt =>
                {
                    txt.Span("Stranica ").FontSize(7).FontColor(ColourFooter);
                    txt.CurrentPageNumber().FontSize(7).FontColor(ColourFooter);
                    txt.Span(" / ").FontSize(7).FontColor(ColourFooter);
                    txt.TotalPages().FontSize(7).FontColor(ColourFooter);
                });
            });
        }

        private static void InfoRow(TableDescriptor tbl, string bg,
                                    string label1, string value1, string label2, string value2)
        {
            tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
               .Text(txt => txt.Span(label1).FontColor(ColourTextMuted));
            tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
               .Text(txt => txt.Span(value1).Bold());
            tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
               .Text(txt => txt.Span(label2).FontColor(ColourTextMuted));
            tbl.Cell().Background(bg).PaddingHorizontal(8).PaddingVertical(5)
               .Text(txt => txt.Span(value2).Bold());
        }

        private static void TableHeader(TableDescriptor tbl, string text)
        {
            tbl.Cell().Background(ColourNavy).PaddingHorizontal(5).PaddingVertical(5)
               .Text(txt =>
               {
                   txt.AlignCenter();
                   txt.Span(text).FontSize(8).FontColor(ColourWhite).Bold();
               });
        }

        private static void TableCell(TableDescriptor tbl, string bg, string text,
                                      bool bold = false, string? colour = null,
                                      bool center = false, bool noWrap = false)
        {
            var container = tbl.Cell().Background(bg).PaddingHorizontal(5).PaddingVertical(4);
            IContainer cell = noWrap ? container.ShowOnce() : container;

            cell.Text(txt =>
            {
                var span = txt.Span(text).FontSize(8);
                if (bold) span.Bold();
                if (colour != null) span.FontColor(colour);
                if (center) txt.AlignCenter();
            });
        }

        

        private static string Fmt(string? val) => string.IsNullOrWhiteSpace(val) ? "—" : val.Trim();
        private static string Fmt(StatusEntiteta status) => status.ToString();
        private static string Fmt(VrstaKlijenta? vrsta) => vrsta.HasValue ? vrsta.Value.ToString() : "—";
        private static string FmtDate(DateTime? dt) => dt.HasValue ? dt.Value.ToString("dd.MM.yyyy.") : "—";
        private static string AlternatingBg(int rowIndex) => rowIndex % 2 == 0 ? ColourWhite : ColourGrey;
        private static string StatusColour(StatusEntiteta s) =>
            s == StatusEntiteta.AKTIVAN ? ColourGreen :
            s == StatusEntiteta.NEAKTIVAN ? ColourRed : ColourOrange;

        private static string DaNeColour(string? val) =>
            (val ?? "").Trim().ToUpperInvariant() switch
            {
                var s when s == DaNeKonstante.Da => ColourRed,
                var s when s == DaNeKonstante.Ne => ColourGreen,
                _ => ColourText,
            };

      

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